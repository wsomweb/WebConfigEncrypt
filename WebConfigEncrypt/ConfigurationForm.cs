// ConfigurationForm.cs

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
using System.Windows.Forms;

namespace edu.cwru.weatherhead.WebConfigEncrypt
{
    public partial class ConfigurationForm : Form
    {
        public ConfigurationForm()
        {
            InitializeComponent();
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            BindValues(txtWebroots, Settings.Webroots);
            BindValues(txtExcludes, Settings.ExcludeDirectories);
            BindValues(txtSections, Settings.IncludeSections);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            // tell user to make a copy of the file
            string message = "The permissions on the applicationHost.config file's default location " +
                @"(C:\Windows\System32\inetsrv\config\) make it impossible to be read from code. " +
                "Please make a copy of the file elsewhere and load from the copy.";
            MessageBox.Show(this, message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // display the open file prompt
            OpenFileDialog open = new OpenFileDialog();
            open.DefaultExt = ".config";
            open.FileName = "applicationHost.config";
            open.Title = "Open applicationHost.config";
            // if file selected
            if(open.ShowDialog(this) == DialogResult.OK)
            {
                // get webroots from file
                var webroots = Settings.GetWebrootsFromAppHost(open.FileName);
                // set value if found
                if (webroots != null && webroots.Length > 0)
                {
                    Settings.Webroots = webroots;
                    BindValues(txtWebroots, Settings.Webroots);
                }
                else
                {
                    MessageBox.Show(this, "Could not load webroots from " + open.FileName, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void BindValues(TextBox text, IEnumerable<string> values)
        {
            if (values == null)
            {
                text.Text = "";
            }
            else
            {
                text.Text = String.Join(Environment.NewLine, values);
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Settings.SaveSettings(
                GetDelimitedString(txtWebroots),
                GetDelimitedString(txtExcludes),
                GetDelimitedString(txtSections));
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private string GetDelimitedString(TextBox textBox)
        {
            return textBox.Text.Replace(Environment.NewLine, ",");
        }

        private void btnKeys_Click(object sender, EventArgs e)
        {
            (new KeyContainerForm()).ShowDialog(this);
        }
    }
}
