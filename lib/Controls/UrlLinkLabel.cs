#region LICENSE
/*
 * Copyright (C) 2007 - 2008 FreeTrain Team (http://freetrain.sourceforge.net)
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
#endregion LICENSE

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
//using Microsoft.Win32;

namespace FreeTrain.controls
{
    /// <summary>
    /// LinkLabel control that launchs a browser and
    /// navigates to a specified URL.
    /// </summary>
    public class UrlLinkLabel : LinkLabel
    {
        private string targetUrl;

        /// <summary>
        /// 
        /// </summary>
        [
            Description("URL to be opened")
        ]
        public string TargetUrl
        {
            get { return targetUrl; }
            set { targetUrl = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public UrlLinkLabel() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLinkClicked(LinkLabelLinkClickedEventArgs e)
        {
            base.OnLinkClicked(e);

            // Because shell execuse doesn't work
            // we have to specify executing module directory
            ProcessStartInfo info = new ProcessStartInfo();
            // get default browser (exe) path
            //TODO
            //RegistryKey rkey = Registry.ClassesRoot.OpenSubKey(@"http\shell\open\command");
            String val = "";// rkey.GetValue("").ToString();
            Debug.WriteLine(val);
            if (val.StartsWith("\""))
            {
                int n = val.IndexOf("\"", 1);
                info.FileName = val.Substring(1, n - 1);
                info.Arguments = val.Substring(n + 1);
            }
            else
            {
                string[] a = val.Split(new char[] { ' ' });
                info.FileName = a[0];
                info.Arguments = val.Substring(a[0].Length + 1);
            }
            // Specifies working directory is necessary to run browser successfuly.
            info.WorkingDirectory = Path.GetDirectoryName(info.FileName);

            info.Arguments += targetUrl;
            Debug.WriteLine(info.FileName);
            Debug.WriteLine(info.WorkingDirectory);
            Debug.WriteLine(info.Arguments);
            System.Diagnostics.Process.Start(info);

            // Following code doesn't work. I don't know why...
            //System.Diagnostics.Process.Start(targetUrl);
        }
    }
}
