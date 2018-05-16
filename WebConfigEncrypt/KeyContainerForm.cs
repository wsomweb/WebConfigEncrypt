// KeyContainerForm.cs

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
using System.Configuration;
using System.Windows.Forms;
using System.Xml;

namespace edu.cwru.weatherhead.WebConfigEncrypt
{
    public partial class KeyContainerForm : Form
    {
        public KeyContainerForm()
        {
            InitializeComponent();
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            string name = GetStringForm.Prompt(this, "Create RSA Key Container", "Enter a name for the new key container.");
            if(!String.IsNullOrEmpty(name))
            {
                string output = Program.RunTask("-pc \"{0}\" -exp", name);
                Program.ShowMessageBox(this, output);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            // get container to export
            string container = ChooseKeyProviderForm.GetKeyProvider(this, "Choose an RSA Key Container to Export", CryptoAPI.GetKeyContainerNames());
            if (String.IsNullOrEmpty(container))
                return;

            // get file name
            SaveFileDialog save = new SaveFileDialog();
            save.Title = "Choose a File for RSA Key Container Export";
            save.Filter = "XML File (.xml)|*.xml";
            save.OverwritePrompt = true;
            save.ShowHelp = false;
            if (save.ShowDialog(this) != DialogResult.OK)
                return;
            string fileName = save.FileName;
            if (String.IsNullOrEmpty(fileName))
                return;

            // export
            string output = Program.RunTask("-px \"{0}\" \"{1}\" -pri", container, fileName);
            Program.ShowMessageBox(this, output);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            // get file name
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Choose an RSA Key Container File to Import";
            open.Filter = "XML File (.xml)|*.xml";
            open.ShowHelp = false;
            if (open.ShowDialog(this) != DialogResult.OK)
                return;
            string fileName = open.FileName;
            if (String.IsNullOrEmpty(fileName))
                return;

            // get container to import
            string container = GetStringForm.Prompt(this, "Enter RSA Key Container Name", "This must be the same name you used when creating the container.");
            if (String.IsNullOrEmpty(container))
                return;

            // import
            string output = Program.RunTask("-pi \"{0}\" \"{1}\"", container, fileName);
            Program.ShowMessageBox(this, output);
        }

        private void btnPermission_Click(object sender, EventArgs e)
        {
            // get container to grant on
            string container = ChooseKeyProviderForm.GetKeyProvider(this, "Choose an RSA Key Container to Grant Permissions For", CryptoAPI.GetKeyContainerNames());
            if (String.IsNullOrEmpty(container))
                return;

            // get user name to grant to
            string user = GetStringForm.Prompt(this, "Enter User Name", "Enter the Windows user to grant access to. (Usually 'NT AUTHORITY\\NETWORK SERVICE' in IIS versions before 7.5 and 'APPPOOL\\YourAppPoolName' in IIS versions 7.5 and later.)");
            if (String.IsNullOrEmpty(user))
                return;

            // grant
            string output = Program.RunTask("-pa \"{0}\" \"{1}\"", container, user);
            Program.ShowMessageBox(this, output);
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            // get file name
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Choose a web.config File to add RSA Key Container to";
            open.Filter = "config File (.config)|*.config";
            open.ShowHelp = false;
            if (open.ShowDialog(this) != DialogResult.OK)
                return;
            string fileName = open.FileName;
            if (String.IsNullOrEmpty(fileName))
                return;

            // get container to grant on
            string container = ChooseKeyProviderForm.GetKeyProvider(this, "Choose an RSA Key Container to Add", CryptoAPI.GetKeyContainerNames());
            if (String.IsNullOrEmpty(container))
                return;

            // open config file
            try
            {
                // load file
                XmlDocument xmld = new XmlDocument();
                xmld.Load(fileName);

                // ensure not already present
                string xpath = String.Format("/configuration/configProtectedData/providers/add[@keyContainerName='{0}']", container);
                XmlNode node = xmld.SelectSingleNode(xpath);
                if(node != null)
                {
                    MessageBox.Show(this, String.Format("RSA Key Container {0} was already configured in {1}.", container, fileName),
                        "Already Configured", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // create node
                node = xmld.CreateNode(XmlNodeType.Element, "add", "");
                AppendAttribute(node, "name", container + "Provider");
                AppendAttribute(node, "type", String.Format(
                    "System.Configuration.RsaProtectedConfigurationProvider, System.Configuration, Version={0}, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, processorArchitecture=MSIL",
                    ConfigurationManager.AppSettings["SystemConfigurationVersion"] ?? "4.0.0.0"));
                AppendAttribute(node, "keyContainerName", container);
                AppendAttribute(node, "useMachineContainer", "true");
                GetKeyProvidersNode(xmld).AppendChild(node);
                // save updated XML
                xmld.Save(fileName);
                // update user
                MessageBox.Show(this, String.Format("RSA Key Container {0} configured in {1}.", container, fileName),
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // show error
                MessageBox.Show(this, ex.Message, "An Error Occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        XmlNode GetKeyProvidersNode(XmlDocument xmld)
        {
            // if already present, return it
            XmlNode node = xmld.SelectSingleNode("/configuration/configProtectedData/providers");
            if (node != null)
                return node;
            // create parent node if not present
            node = xmld.SelectSingleNode("/configuration/configProtectedData");
            if(node == null)
            {
                node = xmld.DocumentElement.AppendChild(xmld.CreateNode(XmlNodeType.Element, "configProtectedData", ""));
            }
            // create and return node
            node = node.AppendChild(xmld.CreateNode(XmlNodeType.Element, "providers", ""));
            return node;
        }

        void AppendAttribute(XmlNode node, string name, string value)
        {
            XmlAttribute xmlAttribute = node.OwnerDocument.CreateAttribute(name);
            xmlAttribute.Value = value;
            node.Attributes.Append(xmlAttribute);
        }
    }
}
