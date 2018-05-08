// ConfigCrawler.cs

// Copyright (C) 2018 Kinsey Roberts (@kinzdesign), Weatherhead School of Management (@wsomweb)

// MIT License

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Xml;

namespace edu.cwru.weatherhead.WebConfigEncrypt
{
    public class ConfigCrawler
    {
        #region constants

        const string WEB_CONFIG = "web.config";

        #endregion

        #region instance properties

        BackgroundWorker _worker;

        HashSet<string> _enqueued = new HashSet<string>(); 

        Queue<string> _toProcess = new Queue<string>();

        List<ConfigSectionEventArgs> _foundSections = new List<ConfigSectionEventArgs>();

        #endregion

        #region constructors

        public ConfigCrawler()
        {
            _worker = new BackgroundWorker();
            _worker.WorkerReportsProgress = true;
            _worker.WorkerSupportsCancellation = true;
            _worker.DoWork += _worker_DoWork;
            _worker.ProgressChanged += _worker_ProgressChanged;
            _worker.RunWorkerCompleted += _worker_RunWorkerCompleted;
        }

        #endregion

        #region BackgroundWorker public methods

        public void BeginCrawl()
        {
            _worker.RunWorkerAsync();
        }

        public void CancelCrawl()
        {
            _worker.CancelAsync();
        }

        #endregion

        #region BackgroundWorker events

        private void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // add webroot paths
            foreach (var item in Settings.Webroots)
            {
                EnqueueDirectory(item);
            }
            // process directories until queue is empty
            int directoriesProcessed = 0;
            int filesProcessed = 0;
            int sectionsFound = 0;
            while (_toProcess.Count > 0)
            {
                // update every 25 directories
                if (directoriesProcessed % 25 == 0)
                    _worker.ReportProgress(0, new CrawlProgressEventArgs(_worker.CancellationPending, directoriesProcessed, filesProcessed, sectionsFound));
                // process next directory
                ProcessDirectory(_toProcess.Dequeue(), ref directoriesProcessed, ref filesProcessed, ref sectionsFound);
            }
            e.Result = new CrawlProgressEventArgs(_worker.CancellationPending, directoriesProcessed, filesProcessed, sectionsFound);
        }

        private void _worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // raise public event as needed
            if (e.UserState is ConfigSectionEventArgs)
            {
                if (SectionFound != null)

                    SectionFound(sender, (ConfigSectionEventArgs)e.UserState);
            }
            else if (e.UserState is CrawlProgressEventArgs)
            {
                if (ProgressUpdated != null)
                    ProgressUpdated(sender, (CrawlProgressEventArgs)e.UserState);
            }
        }

        private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (ScanCompleted != null)
                ScanCompleted(sender, e);
        }

        #endregion

        #region public events

        public event ConfigSectionEventHandler SectionFound;

        public event RunWorkerCompletedEventHandler ScanCompleted;

        public event CrawlProgressEventHandler ProgressUpdated;

        #endregion

        #region config helper functions

        private void AddRangeFromConfig(HashSet<string> set, string key)
        {
            // get config value for key
            string value = ConfigurationManager.AppSettings[key];
            // if it has contents
            if (!String.IsNullOrEmpty(value))
            {
                // split on comma, pipe, or semicolon
                foreach (string s in value.Split(',', '|', ';'))
                {
                    // add each chunk to the set
                    set.Add(s.Trim());
                }
            }
        }

        #endregion

        #region crawl helper functions

        private void EnqueueDirectory(string path)
        {
            EnqueueDirectory(new DirectoryInfo(path));
        }

        private void EnqueueDirectory(DirectoryInfo directory)
        {
            // check to make sure it's not an excluded folder
            if (Settings.ExcludeDirectories.Contains(directory.Name))
                return;
            // ensure the folder has not already been enqueued or processed
            if (!_enqueued.Contains(directory.FullName))
            {
                // add to list to process
                _toProcess.Enqueue(directory.FullName);
            }
        }

        private void ProcessDirectory(string path, ref int directoriesProcessed, ref int filesProcessed, ref int sectionsFound)
        {
            // obey cancellation requests
            if (_worker.CancellationPending)
                return;
            // load directory info
            DirectoryInfo directory = new DirectoryInfo(path);
            if (directory == null || !directory.Exists)
                return;
            bool recurse = true;
            // process web.config
            ProcessConfigFile(directory.FullName, ref recurse, ref directoriesProcessed, ref filesProcessed, ref sectionsFound);
            // enqueue child directories
            if(recurse)
                foreach (var item in directory.EnumerateDirectories())
                    EnqueueDirectory(item);
        }

        private void ProcessConfigFile(string directory, ref bool recurse, ref int directoriesProcessed, ref int filesProcessed, ref int sectionsFound)
        {
            directoriesProcessed++;
            // ensure web.config exists
            string path = String.Concat(directory, @"\", WEB_CONFIG);
            if (!File.Exists(path))
                return;
            filesProcessed++;

            // load as XML
            XmlDocument config = new XmlDocument();
            config.Load(path);
            // check for WebConfigEncrypt.Exclude 
            var excludeNode = config.SelectSingleNode("/configuration/appSettings/add[@key = 'WebConfigEncrypt.Exclude' and @value]");
            if(excludeNode != null)
            {
                // don't recurse children
                recurse = false;
                // if value starts with self, don't even process this file
                string exclude = excludeNode.Attributes["value"].Value;
                if (exclude.ToLower().StartsWith("self"))
                    return;
            }
            // search for sections of interest
            foreach (string section in Settings.IncludeSections)
            {
                XmlNode node = config.SelectSingleNode("/configuration/" + section);
                if (node != null)
                {
                    sectionsFound++;
                    ProcessSection(path, section, node);
                }
            }
        }

        private void ProcessSection(string path, string section, XmlNode node)
        {
            // see whether section is encrypted
            bool isEncrypted = node.FirstChild != null && String.Equals(node.FirstChild.Name, "EncryptedData", StringComparison.InvariantCultureIgnoreCase);
            ConfigSectionEventArgs args = new ConfigSectionEventArgs(path, section, isEncrypted);
            // add to list
            _foundSections.Add(args);
            // raise event
            _worker.ReportProgress(0, args);
        }

        #endregion
    }
}
