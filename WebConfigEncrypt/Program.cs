// Program.cs

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
using System.Diagnostics;
using System.Windows.Forms;

namespace edu.cwru.weatherhead.WebConfigEncrypt
{
    static class Program
    {
        [STAThread]
        static void Main(params string[] args) 
        {
            // launch GUI
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new WebConfigEncryptForm());
        }

        internal static string RunTask(string argumentsFormatString, params object[] args)
        {
            return RunTask(String.Format(argumentsFormatString, args));
        }

        internal static string RunTask(string arguments)
        {
            Process process = new Process();
            process.StartInfo.FileName = ConfigurationManager.AppSettings["AspNetRegIisPath"] ?? @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\aspnet_regiis";
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            // Read the output
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }

        internal static bool IsSuccess(string output)
        {
            return !String.IsNullOrEmpty(output) && output.Contains("Succeeded!");
        }

        internal static DialogResult ShowMessageBox(IWin32Window owner, string output)
        {
            if(IsSuccess(output))
                return MessageBox.Show(owner, output, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (!String.IsNullOrEmpty(output))
                return MessageBox.Show(owner, output, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                return MessageBox.Show(owner, "No error message available.", "Unknown Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
