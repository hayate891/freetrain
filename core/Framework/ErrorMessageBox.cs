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
using FreeTrain.Controls;

namespace FreeTrain.Framework
{
    /// <summary>
    /// Shows an exception to the user
    /// (and ask for the forgiveness :-)
    /// </summary>
    public sealed class ErrorMessageBox : System.Windows.Forms.Form
    {
        /// <summary>
        /// Displays a dialog box and returns after the dialog is closed.
        /// </summary>
        /// <param name="owner">can be null.</param>
        /// <param name="caption"></param>
        /// <param name="e"></param>
        public static void ShowDialog(IWin32Window owner, string caption, Exception e)
        {
            using (Form f = new ErrorMessageBox(caption, e))
            {
                f.ShowDialog(owner);
            }
        }

        private System.Windows.Forms.Label msg;
        private UrlLinkLabel linkLabel1;

        private readonly Exception exception;

        private ErrorMessageBox(string caption, Exception e)
        {
            this.exception = e;
            this.Text = caption;

            InitializeComponent();

            base.Icon = SystemIcons.Error;
            icon.Image = SystemIcons.Error.ToBitmap();

            detail.Text = e.Message + "\n" + e.StackTrace;

            while (true)
            {
                e = e.InnerException;
                if (e == null) break;

                detail.Text = detail.Text + "\n" + e.Message + "\n" + e.StackTrace;
            }

            detail.Select(0, 0);
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
        private System.Windows.Forms.PictureBox icon;
        private System.Windows.Forms.TextBox detail;
        private System.Windows.Forms.Button okButton;
        private System.ComponentModel.Container components = null;

        private void InitializeComponent()
        {
            this.icon = new System.Windows.Forms.PictureBox();
            this.detail = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.msg = new System.Windows.Forms.Label();
            this.linkLabel1 = new FreeTrain.Controls.UrlLinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.icon)).BeginInit();
            this.SuspendLayout();
            // 
            // icon
            // 
            this.icon.Location = new System.Drawing.Point(16, 0);
            this.icon.Name = "icon";
            this.icon.Size = new System.Drawing.Size(48, 52);
            this.icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.icon.TabIndex = 0;
            this.icon.TabStop = false;
            // 
            // detail
            // 
            this.detail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.detail.Location = new System.Drawing.Point(16, 52);
            this.detail.Multiline = true;
            this.detail.Name = "detail";
            this.detail.ReadOnly = true;
            this.detail.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.detail.Size = new System.Drawing.Size(368, 89);
            this.detail.TabIndex = 2;
            this.detail.Text = "detail";
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.okButton.Location = new System.Drawing.Point(304, 145);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(80, 26);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "&OK";
            // 
            // msg
            // 
            this.msg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.msg.Location = new System.Drawing.Point(72, 9);
            this.msg.Name = "msg";
            this.msg.Size = new System.Drawing.Size(312, 17);
            this.msg.TabIndex = 1;
            this.msg.Text = "An error has occurred";
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel1.Location = new System.Drawing.Point(72, 26);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(312, 17);
            this.linkLabel1.TabIndex = 4;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.TargetUrl = "http://freetrain.sourceforge.net/";
            this.linkLabel1.Text = "Report a bug";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ErrorMessageBox
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(400, 174);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.detail);
            this.Controls.Add(this.msg);
            this.Controls.Add(this.icon);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ErrorMessageBox";
            this.Text = "Error";
            ((System.ComponentModel.ISupportInitialize)(this.icon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
    }
}
