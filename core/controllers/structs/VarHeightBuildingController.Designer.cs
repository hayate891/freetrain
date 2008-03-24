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
using FreeTrain.Contributions.Structs;
using FreeTrain.Views.Map;
using FreeTrain.World;
using FreeTrain.World.Structs;
using FreeTrain.Framework;
using FreeTrain.Framework.Graphics;
using FreeTrain.Framework.Plugin;
using FreeTrain.Util;

namespace FreeTrain.Controllers.Structs
{
    /// <summary>
    /// Controller that allows the user to
    /// place/remove VarHeightBuildingContribution.
    /// </summary>
    public partial class VarHeightBuildingController : StructPlacementController
    {
        #region Designer generated code
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown heightBox;
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Designer サポートに必要なメソッドです。コード エディタで
        /// このメソッドのコンテンツを変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.heightBox = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.heightBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 166);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "&Height:";
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            //! this.label1.Text = "高さ(&H)：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // heightBox
            // 
            this.heightBox.Increment = new System.Decimal(new int[] {4,
																		0,
																		0,
																		0});
            this.heightBox.Location = new System.Drawing.Point(56, 162);
            this.heightBox.Maximum = new System.Decimal(new int[] {32,
																	  0,
																	  0,
																	  0});
            this.heightBox.Minimum = new System.Decimal(new int[] {4,
																	  0,
																	  0,
																	  0});
            this.heightBox.Name = "heightBox";
            this.heightBox.Size = new System.Drawing.Size(64, 19);
            this.heightBox.TabIndex = 4;
            this.heightBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.heightBox.Value = new System.Decimal(new int[] {4,
																	0,
																	0,
																	0});
            this.heightBox.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Left | (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.heightBox.Validating += new System.ComponentModel.CancelEventHandler(this.heightBox_Validating);
            this.heightBox.ValueChanged += new System.EventHandler(this.heightBox_ValueChanged);
            // 
            // VarHeightBuildingController
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
            this.ClientSize = new System.Drawing.Size(144, 222);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.heightBox,
																		  this.label1});
            this.Name = "VarHeightBuildingController";
            this.Text = "Building construction";
            //! this.Text = "建物の工事(仮)";
            ((System.ComponentModel.ISupportInitialize)(this.heightBox)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
    }
}

