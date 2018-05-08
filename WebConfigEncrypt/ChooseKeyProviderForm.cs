// ChooseKeyProviderForm.cs

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
    public partial class ChooseKeyProviderForm : Form
    {
        public ChooseKeyProviderForm(string configFilePath, string section, IEnumerable<string> providers)
        {
            InitializeComponent();
            // config label text
            label1.Text = String.Format("Multiple key providers were found in {0}.\r\nPlease choose which provider to use when encrypting {1}.", configFilePath, section);
            // load drop-down
            foreach (string provider in providers)
                cbProviders.Items.Add(provider);
        }


        public static string GetKeyProvider(IWin32Window window, string configFilePath, string section, IEnumerable<string> providers)
        {
            // build window
            var form = new ChooseKeyProviderForm(configFilePath, section, providers);
            // show window
            if (form.ShowDialog(window) == DialogResult.OK)
                return form.cbProviders.SelectedItem.ToString();
            return null;
        }

        private void cbProviders_SelectedIndexChanged(object sender, EventArgs e)
        {
            // only allow OK button when a provider is selected
            btnOk.Enabled = !String.IsNullOrEmpty(cbProviders.SelectedItem.ToString());
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
