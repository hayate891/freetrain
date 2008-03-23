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
using FreeTrain.Framework;
using FreeTrain.Framework.Graphics;
using FreeTrain.Util;
using FreeTrain.World;
using FreeTrain.World.Rail;
using FreeTrain.Views;
using FreeTrain.Views.Map;

namespace FreeTrain.Controllers.Rail
{
    /// <summary>
    /// Railroad construction dialog
    /// </summary>
    /// This controller has two states.
    /// In one state, we expect the user to select one voxel.
    /// In the other state, we expect the user to select next voxel,
    /// so that we can build railroads.
    public partial class RailRoadController : AbstractControllerImpl, MapOverlay
    {
        #region Windows Form Designer generated code
        private System.Windows.Forms.RadioButton buttonPlace;
        private System.Windows.Forms.RadioButton buttonRemove;
        private FreeTrain.Controls.CostBox costBox;
        private System.Windows.Forms.Label message;
        private System.ComponentModel.Container components = null;

        private void InitializeComponent()
        {
            this.message = new System.Windows.Forms.Label();
            this.buttonPlace = new System.Windows.Forms.RadioButton();
            this.buttonRemove = new System.Windows.Forms.RadioButton();
            this.costBox = new FreeTrain.Controls.CostBox();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Size = new System.Drawing.Size(121, 15);
            this.lblTitle.Text = "RAILROAD";
            // 
            // lblExit
            // 
            this.lblExit.Location = new System.Drawing.Point(87, 5);
            // 
            // message
            // 
            this.message.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.message.Location = new System.Drawing.Point(12, 46);
            this.message.Name = "message";
            this.message.Size = new System.Drawing.Size(107, 26);
            this.message.TabIndex = 1;
            this.message.Text = "Click two points on the map to place tracks";
            this.message.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonPlace
            // 
            this.buttonPlace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPlace.Appearance = System.Windows.Forms.Appearance.Button;
            this.buttonPlace.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(236)))));
            this.buttonPlace.Checked = true;
            this.buttonPlace.Location = new System.Drawing.Point(12, 115);
            this.buttonPlace.Name = "buttonPlace";
            this.buttonPlace.Size = new System.Drawing.Size(46, 26);
            this.buttonPlace.TabIndex = 2;
            this.buttonPlace.TabStop = true;
            this.buttonPlace.Text = "Place";
            this.buttonPlace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonPlace.UseVisualStyleBackColor = false;
            this.buttonPlace.CheckedChanged += new System.EventHandler(this.modeChanged);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemove.Appearance = System.Windows.Forms.Appearance.Button;
            this.buttonRemove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(236)))));
            this.buttonRemove.Location = new System.Drawing.Point(58, 115);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(61, 26);
            this.buttonRemove.TabIndex = 3;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonRemove.UseVisualStyleBackColor = false;
            this.buttonRemove.CheckedChanged += new System.EventHandler(this.modeChanged);
            // 
            // costBox
            // 
            this.costBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.costBox.cost = 0;
            this.costBox.label = "Cost:";
            this.costBox.Location = new System.Drawing.Point(12, 84);
            this.costBox.Name = "costBox";
            this.costBox.Size = new System.Drawing.Size(107, 25);
            this.costBox.TabIndex = 4;
            // 
            // RailRoadController
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(131, 153);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonPlace);
            this.Controls.Add(this.costBox);
            this.Controls.Add(this.message);
            this.Name = "RailRoadController";
            this.Resize += new System.EventHandler(this.updateAfterResize);
            this.Load += new System.EventHandler(this.RailRoadController_Load);
            this.Controls.SetChildIndex(this.lblTitle, 0);
            this.Controls.SetChildIndex(this.lblExit, 0);
            this.Controls.SetChildIndex(this.message, 0);
            this.Controls.SetChildIndex(this.costBox, 0);
            this.Controls.SetChildIndex(this.buttonPlace, 0);
            this.Controls.SetChildIndex(this.buttonRemove, 0);
            this.ResumeLayout(false);

        }
        #endregion
    }

}
