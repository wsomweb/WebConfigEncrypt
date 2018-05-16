// ConfigSection.cs

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
using System.Windows.Forms;
using System.Xml;

namespace edu.cwru.weatherhead.WebConfigEncrypt
{
    public class ConfigSection : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region instance properties

        public string ConfigFilePath { get; private set; }

        public string SectionName { get; private set; }

        private bool _isEncrypted;


        public bool IsEncrypted
        {
            get
            {
                return _isEncrypted;
            }
            private set
            {
                _isEncrypted = value;
                OnPropertyChanged("IsEncrypted");
                OnPropertyChanged("ToggleButtonText");
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

        }

        #endregion

        #region computed properties

        public string ToggleButtonText
        {
            get
            {
                return IsEncrypted ? "Decrypt" : "Encrypt";
            }
        }

        #endregion

        #region constructor

        internal ConfigSection(string configFilePath, string sectionName, bool isEncrypted)
        {
            ConfigFilePath = configFilePath;
            SectionName = sectionName;
            IsEncrypted = isEncrypted;
        }

        #endregion

        #region ToggleEncryption

        public string ToggleEncryption(IWin32Window window)
        {
            string output = "";
            string directory = ConfigFilePath.Substring(0, ConfigFilePath.Length - 11);
            if (IsEncrypted)
            {
                // decrypt
                output = Program.RunTask("-pdf \"{0}\" \"{1}\"", SectionName, directory);
                if (Program.IsSuccess(output))
                {
                    IsEncrypted = false;
                }
            }
            else
            {
                // encrypt
                string keyProvider = GetKeyProvider(window);
                if (!String.IsNullOrEmpty(keyProvider))
                {
                    output = Program.RunTask("-pef \"{0}\" \"{1}\" -prov \"{2}\"", SectionName, directory, keyProvider);
                    if (Program.IsSuccess(output))
                    {
                        IsEncrypted = true;
                    }
                }
            }
            Program.ShowMessageBox(window, output);
            return output;
        }

        string GetKeyProvider(IWin32Window window)
        {
            // get key providers from config file
            XmlDocument xmld = new XmlDocument();
            xmld.Load(ConfigFilePath);
            XmlNodeList providers = xmld.SelectNodes("configuration/configProtectedData/providers/add[@name]");
            // if none found, show error
            if (providers == null || providers.Count == 0)
            {
                MessageBox.Show(window, "No providers found in configProtectedData section of " + ConfigFilePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            // if one found, return it
            if (providers.Count == 1)
            {
                return providers[0].Attributes["name"].Value;
            }
            // handle multiple key providers
            List<string> providerKeys = new List<string>();
            foreach (XmlNode item in providers)
            {
                providerKeys.Add(item.Attributes["name"].Value);
            }
            return ChooseKeyProviderForm.GetKeyProvider(window, ConfigFilePath, SectionName, providerKeys);
        }

        #endregion
    }

    public class ConfigSectionList : BindingList<ConfigSection>
    {

    }
}
