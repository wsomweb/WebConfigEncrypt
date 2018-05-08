// Settings.cs

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
using System.Configuration;
using System.Linq;
using System.Xml;

namespace edu.cwru.weatherhead.WebConfigEncrypt
{
    internal static class Settings
    {
        #region webroots

        private const string WEBROOTS_KEY = "Webroots";
        private static string[] _webroots = null;
        public static string[] Webroots
        {
            get
            {
                // lazy-load from app.config
                if (_webroots == null)
                {
                    _webroots = GetSettingAsArray(WEBROOTS_KEY);
                }
                return _webroots;
            }
            set
            {
                // update locally and in app.config
                _webroots = value;
                SetSetting(WEBROOTS_KEY, value);
            }
        }

        public static bool HasWebroots
        {
            get
            {
                // ensured lazy-loaded with first getter, so don't need to call getter the second time
                return Webroots != null && _webroots.Length > 0;
            }
        }

        #region load from applicationHost.config

        internal static string[] GetWebrootsFromAppHost(string applicationHostPath)
        {
            if (applicationHostPath == null)
            {
                throw new ArgumentNullException(nameof(applicationHostPath));
            }
            try
            {
                // load applicationHost.config as an XML document
                XmlDocument appHost = new XmlDocument();
                appHost.Load(applicationHostPath);
                // create temp list to hold webroots
                List<string> webroots = new List<string>();
                // use XPath to select all virtualDirectory nodes with a physicalPath attribute
                foreach (XmlNode node in appHost.SelectNodes("//virtualDirectory[@physicalPath]"))
                {
                    // enqueue each physicalPath for processing
                    string path = node.Attributes["physicalPath"].Value.Trim();
                    // strip trailing slash
                    if (path.EndsWith(@"\"))
                    {
                        path = path.Substring(0, path.Length - 1);
                    }
                    if (!webroots.Contains(path))
                    {
                        webroots.Add(path);
                    }
                }
                webroots.Sort();
                return webroots.ToArray();
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #endregion

        #region ExcludeDirectories

        private const string EXCLUDE_DIRS_KEY = "ExcludeDirectories";
        private static HashSet<string> _excludeDirectories = null;
        public static HashSet<string> ExcludeDirectories
        {
            get
            {
                // lazy-load from app.config
                if (_excludeDirectories == null)
                {
                    _excludeDirectories = GetSettingAsHashSet(EXCLUDE_DIRS_KEY);
                }
                return _excludeDirectories;
            }
            set
            {
                // update locally and in app.config
                _excludeDirectories = value;
                SetSetting(EXCLUDE_DIRS_KEY, value);
            }
        }

        #endregion

        #region sections

        private const string INCLUDE_SECTIONS_KEY = "IncludeSections";
        private static HashSet<string> _includeSections = null;
        public static HashSet<string> IncludeSections
        {
            get
            {
                // lazy-load from app.config
                if (_includeSections == null)
                {
                    _includeSections = GetSettingAsHashSet(INCLUDE_SECTIONS_KEY);
                }
                return _includeSections;
            }
            set
            {
                // update locally and in app.config
                _includeSections = value;
                SetSetting(INCLUDE_SECTIONS_KEY, value);
            }
        }

        public static bool HasSections
        {
            get
            {
                // ensured lazy-loaded with first getter, so don't need to call getter the second time
                return IncludeSections != null && _includeSections.Count > 0;
            }
        }

        #endregion

        public static void SaveSettings(string webroots, string excludeDirectories, string includeSections)
        {
            // save to app.config
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            SetSetting(settings, WEBROOTS_KEY, webroots);
            SetSetting(settings, EXCLUDE_DIRS_KEY, excludeDirectories);
            SetSetting(settings, INCLUDE_SECTIONS_KEY, includeSections);
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            // clear local caches to force re-load
            _webroots = null;
            _excludeDirectories = null;
            _includeSections = null;
        }


        #region Helper Functions

        private static string[] GetSettingAsArray(string key)
        {
            string value = ConfigurationManager.AppSettings[key];
            if (!String.IsNullOrEmpty(value))
            {
                List<string> values = new List<string>();
                foreach (var item in value.Split(new char[] { ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    values.Add(item.Trim());
                }
                return values.ToArray();
            }
            return null;
        }

        private static HashSet<string> GetSettingAsHashSet(string key)
        {
            string[] values = GetSettingAsArray(key);
            if (values != null && values.Length > 0)
            {
                return new HashSet<string>(values);
            }
            return new HashSet<string>();
        }

        private static void SetSetting(string key, IEnumerable<string> values)
        {
            if (values == null || values.Count() < 1)
            {
                SetSetting(key, "");
            }
            else
            {
                SetSetting(key, String.Join(",", values));
            }
        }

        private static void SetSetting(string key, string value)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            SetSetting(settings, key, value);
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }

        private static void SetSetting(KeyValueConfigurationCollection settings, string key, string value)
        {
            if (settings[key] == null)
            {
                settings.Add(key, value);
            }
            else
            {
                settings[key].Value = value;
            }
        }

        #endregion
    }
}
