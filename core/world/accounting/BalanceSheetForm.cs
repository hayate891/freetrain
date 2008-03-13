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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using AxSHDocVw;
using MsHtmlHost;
using freetrain.controls;
using freetrain.framework;
using freetrain.framework.plugin;

namespace freetrain.world.accounting
{
    /// <summary>
    /// Displays the balance sheet.
    /// </summary>
    public class BalanceSheetForm : Form
    {
        #region singleton instance
        /// <summary>
        /// 
        /// </summary>
        public static void create()
        {
            if (theInstance == null)
            {
                theInstance = new BalanceSheetForm();
                theInstance.Show();
            }
            theInstance.BringToFront();
        }

        private static Form theInstance = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            theInstance = null;
        }
        #endregion


        private System.ComponentModel.Container components = null;
        private freetrain.controls.WebBrowser webBrowser;

        private BalanceSheetForm()
        {
            //			this.MdiParent = MainWindow.mainWindow;
            InitializeComponent();

            //            object flags = 0;
            //            object targetFrame = String.Empty;
            //            object postData = String.Empty;
            //            object headers = String.Empty;
            //            webBrowser.Navigate("about:hello", ref flags, ref targetFrame, ref postData, ref headers);

            webBrowser.navigate("about:blank");
            webBrowser.docHostUIHandler = new DocHostUIHandlerImpl(this);
            webBrowser.navigate(ResourceUtil.findSystemResource("balanceSheet.html"));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(BalanceSheetForm));
            this.webBrowser = new freetrain.controls.WebBrowser();
            ((System.ComponentModel.ISupportInitialize)(this.webBrowser)).BeginInit();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Enabled = true;
            this.webBrowser.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("webBrowser.OcxState")));
            this.webBrowser.Size = new System.Drawing.Size(592, 206);
            this.webBrowser.TabIndex = 0;
            // 
            // BalanceSheetForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
            this.ClientSize = new System.Drawing.Size(592, 206);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.webBrowser});
            this.Name = "BalanceSheetForm";
            this.Text = "Balance Sheet";
            //! this.Text = "バランスシート";
            ((System.ComponentModel.ISupportInitialize)(this.webBrowser)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
    }
}
