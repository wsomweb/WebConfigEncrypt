// WebConfigEncryptForm.cs

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
using System.ComponentModel;
using System.Media;
using System.Windows.Forms;

namespace edu.cwru.weatherhead.WebConfigEncrypt
{
    public partial class WebConfigEncryptForm : Form
    {
        private ConfigCrawler _crawler;
        private ConfigSectionList _sections;

        #region Constructor

        public WebConfigEncryptForm()
        {
            InitializeComponent();
            // bind config sections
            _sections = new ConfigSectionList();
            configSectionListBindingSource1.DataSource = _sections;
        }

        #endregion

        #region Form events

        private void WebConfigEncryptForm_Shown(object sender, System.EventArgs e)
        {
            // ensure webroots present
            if (!Settings.HasWebroots)
            {
                MessageBox.Show(this, "Please select at least one webroot to scan on the next page.", "No webroots configured", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // if not, prompt user
                ShowConfigWindow();
                // if still not, close program
                if(!Settings.HasWebroots)
                {
                    MessageBox.Show(this, "No webroots configured, exiting program.", "No webroots configured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }
            }
            BeginCrawl();
        }

        #endregion

        #region Crawler Functions & Events

        private void BeginCrawl()
        {
            _crawler = new ConfigCrawler();
            _crawler.ProgressUpdated += _crawler_ProgressUpdated;
            _crawler.SectionFound += _crawler_SectionFound;
            _crawler.ScanCompleted += _crawler_ScanComplete;
            _crawler.BeginCrawl();
        }

        public void CancelCrawl()
        {
            if (_crawler != null)
            {
                _crawler.CancelCrawl();
            }
            _sections = new ConfigSectionList();
            configSectionListBindingSource1.DataSource = _sections;
        }

        private void _crawler_ProgressUpdated(object sender, CrawlProgressEventArgs args)
        {
            try
            {
                toolStripStatusLabel1.Text = string.Format("Scan progress: {0} directories processed, {1} files processed, {2} sections found",
                    args.DirectoriesProcessed, args.FilesProcessed, args.SectionsFound);
                Application.DoEvents();
            }
            catch
            {
                // fail gracefully
            }
        }

        private void _crawler_SectionFound(object sender, ConfigSectionEventArgs args)
        {
            _sections.Add(args.ConfigSection);
        }

        private void _crawler_ScanComplete(object sender, RunWorkerCompletedEventArgs args)
        {
            try
            {
                CrawlProgressEventArgs stats = args.Result as CrawlProgressEventArgs;
                if (stats == null)
                {
                    SystemSounds.Beep.Play();
                    toolStripStatusLabel1.Text = "Scan completed!";
                }
                else if (stats.WasCanceled)
                {
                    toolStripStatusLabel1.Text = "Scan canceled!";
                }
                else
                {
                    SystemSounds.Beep.Play();
                    toolStripStatusLabel1.Text = string.Format("Scan completed! {0} directories processed, {1} files processed, {2} sections found",
                        stats.DirectoriesProcessed, stats.FilesProcessed, stats.SectionsFound);
                }
            }
            catch
            {
                // fail gracefully
            }
        }

        #endregion

        #region Grid events

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // if button column was clicked
            if(e.ColumnIndex == dataGridView1.Columns["Encryption"].Index)
            {
                // toggle the encryption state
                string output;
                bool success = _sections[e.RowIndex].ToggleEncryption(this, out output);
                // show message box
                if (success)
                    MessageBox.Show(this, output, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (!String.IsNullOrEmpty(output))
                    MessageBox.Show(this, output, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Menu

        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CancelCrawl();
            ShowConfigWindow();
            BeginCrawl();
        }

        private DialogResult ShowConfigWindow()
        {
            ConfigurationForm form = new ConfigurationForm();
            return form.ShowDialog(this);
        }

        private void cancelScanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CancelCrawl();
        }

        #endregion
    }
}
