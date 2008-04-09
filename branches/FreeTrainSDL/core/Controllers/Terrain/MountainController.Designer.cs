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
using FreeTrain.Views.Map;
using FreeTrain.Util;
using FreeTrain.World;
using FreeTrain.World.Terrain;
using FreeTrain.Framework;

namespace FreeTrain.Controllers.Terrain
{
    /// <summary>
    /// Manipulates mountains
    /// </summary>
    public partial class MountainController : AbstractControllerImpl
    {
        #region Windows Form Designer generated code
        private System.Windows.Forms.PictureBox preview;
        private System.Windows.Forms.RadioButton buttonUp;
        private System.Windows.Forms.RadioButton buttonDown;
        private System.ComponentModel.Container components = null;

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonUp = new System.Windows.Forms.RadioButton();
            this.buttonDown = new System.Windows.Forms.RadioButton();
            this.preview = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.selSize = new FreeTrain.Controls.IndexSelector();
            ((System.ComponentModel.ISupportInitialize)(this.preview)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonUp
            // 
            this.buttonUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonUp.Appearance = System.Windows.Forms.Appearance.Button;
            this.buttonUp.Checked = true;
            this.buttonUp.Location = new System.Drawing.Point(4, 258);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(56, 26);
            this.buttonUp.TabIndex = 2;
            this.buttonUp.TabStop = true;
            this.buttonUp.Text = "Raise";
            this.buttonUp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonDown
            // 
            this.buttonDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDown.Appearance = System.Windows.Forms.Appearance.Button;
            this.buttonDown.Location = new System.Drawing.Point(153, 258);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(56, 26);
            this.buttonDown.TabIndex = 4;
            this.buttonDown.Text = "Lower";
            this.buttonDown.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // preview
            // 
            this.preview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.preview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.preview.InitialImage = null;
            this.preview.Location = new System.Drawing.Point(4, 5);
            this.preview.Name = "preview";
            this.preview.Size = new System.Drawing.Size(205, 81);
            this.preview.TabIndex = 3;
            this.preview.TabStop = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(4, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(205, 117);
            this.label1.TabIndex = 6;
            this.label1.Text = "Press SHIFT and move mouse to quickly modify terrain.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.selSize);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(4, 214);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(205, 38);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Target Size";
            // 
            // selSize
            // 
            this.selSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.selSize.count = 10;
            this.selSize.current = 0;
            this.selSize.dataSource = null;
            this.selSize.Location = new System.Drawing.Point(6, 14);
            this.selSize.Name = "selSize";
            this.selSize.Size = new System.Drawing.Size(189, 17);
            this.selSize.TabIndex = 6;
            // 
            // MountainController
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(213, 288);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonUp);
            this.Controls.Add(this.buttonDown);
            this.Controls.Add(this.preview);
            this.Name = "MountainController";
            this.Text = "Modify Terrain";
            ((System.ComponentModel.ISupportInitialize)(this.preview)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
    }
}
