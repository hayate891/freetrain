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
    public partial class RailRoadController : AbstractControllerImpl, IMapOverlay
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private System.Windows.Forms.RadioButton buttonPlace;
        private System.Windows.Forms.RadioButton buttonRemove;
        private FreeTrain.Controls.CostBox costBox;
        private System.Windows.Forms.Label message;
        private System.ComponentModel.Container components = null;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RailRoadController));
            this.message = new System.Windows.Forms.Label();
            this.buttonPlace = new System.Windows.Forms.RadioButton();
            this.buttonRemove = new System.Windows.Forms.RadioButton();
            this.costBox = new FreeTrain.Controls.CostBox();
            this.SuspendLayout();
            // 
            // message
            // 
            resources.ApplyResources(this.message, "message");
            this.message.Name = "message";
            // 
            // buttonPlace
            // 
            resources.ApplyResources(this.buttonPlace, "buttonPlace");
            this.buttonPlace.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(236)))));
            this.buttonPlace.Checked = true;
            this.buttonPlace.Name = "buttonPlace";
            this.buttonPlace.TabStop = true;
            this.buttonPlace.UseVisualStyleBackColor = false;
            this.buttonPlace.CheckedChanged += new System.EventHandler(this.ModeChanged);
            // 
            // buttonRemove
            // 
            resources.ApplyResources(this.buttonRemove, "buttonRemove");
            this.buttonRemove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(236)))));
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.UseVisualStyleBackColor = false;
            this.buttonRemove.CheckedChanged += new System.EventHandler(this.ModeChanged);
            // 
            // costBox
            // 
            resources.ApplyResources(this.costBox, "costBox");
            this.costBox.cost = 0;
            this.costBox.label = "Cost:";
            this.costBox.Name = "costBox";
            // 
            // RailRoadController
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonPlace);
            this.Controls.Add(this.costBox);
            this.Controls.Add(this.message);
            this.Name = "RailRoadController";
            this.ResumeLayout(false);

        }
        #endregion
    }

}
