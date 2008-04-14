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
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FreeTrain.Controls;
using FreeTrain.Framework.Graphics;
using FreeTrain.Util;

namespace FreeTrain.Framework
{
    /// <summary>
    /// AboutDialog
    /// </summary>
    public partial class AboutDialog : System.Windows.Forms.Form
    {
        /// <summary>
        /// 
        /// </summary>
        public AboutDialog()
        {
            InitializeComponent();
            browser.Navigate(ResourceUtil.FindSystemResource("about.html"));
        }

        private void okButton_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            UrlInvoker.OpenUrl(((LinkLabel)sender).Text);
        }

    }
}
