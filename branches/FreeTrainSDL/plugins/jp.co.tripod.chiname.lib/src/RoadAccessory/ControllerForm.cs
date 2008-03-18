﻿#region LICENSE
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
using FreeTrain.Controllers;
using FreeTrain.Contributions.Common;
using FreeTrain.Views;
using FreeTrain.Views.Map;
using FreeTrain.world;
using FreeTrain.world.Terrain;
using FreeTrain.Framework;
using FreeTrain.Framework.graphics;
using FreeTrain.Framework.plugin;
using FreeTrain.Util;
using SDL.net;

namespace FreeTrain.world.road.Accessory
{
    /// <summary>
    /// Controller that allows the user to
    /// place/remove road accessories.
    /// </summary>
    [CLSCompliant(false)]
    public class ControllerForm : ControllerHostForm
    {
        private FreeTrain.controls.CostBox costBox;
        private System.Windows.Forms.RadioButton buttonRemove;
        private System.Windows.Forms.RadioButton buttonPlace;
        private System.Windows.Forms.ComboBox typeBox;

        private Bitmap previewBitmap;

        /// <summary>
        /// 
        /// </summary>
        public ControllerForm()
        {
            InitializeComponent();

            // load list of lands
            typeBox.DataSource = Core.plugins.listContributions(typeof(RoadAccessoryContribution));
            typeBox.DisplayMember = "name";
            if (typeBox.Items.Count == 0)
            {
                buttonPlace.Enabled = false;
                buttonRemove.Enabled = false;
            }
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

            if (previewBitmap != null)
                previewBitmap.Dispose();
        }

        #region Designer generated code
        private System.Windows.Forms.PictureBox preview;
        private System.ComponentModel.IContainer components = null;
        private FreeTrain.controls.IndexSelector indexSelector;

        private void InitializeComponent()
        {
            this.typeBox = new System.Windows.Forms.ComboBox();
            this.preview = new System.Windows.Forms.PictureBox();
            this.costBox = new FreeTrain.controls.CostBox();
            this.buttonRemove = new System.Windows.Forms.RadioButton();
            this.buttonPlace = new System.Windows.Forms.RadioButton();
            this.indexSelector = new FreeTrain.controls.IndexSelector();
            this.SuspendLayout();
            // 
            // typeBox
            // 
            this.typeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | (System.Windows.Forms.AnchorStyles.Left | (System.Windows.Forms.AnchorStyles.Right))));
            this.typeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.typeBox.Location = new System.Drawing.Point(8, 8);
            this.typeBox.Name = "typeBox";
            this.typeBox.Size = new System.Drawing.Size(112, 20);
            this.typeBox.Sorted = true;
            this.typeBox.TabIndex = 2;
            this.typeBox.SelectedIndexChanged += new System.EventHandler(this.onTypeChanged);
            // 
            // preview
            // 
            this.preview.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | (System.Windows.Forms.AnchorStyles.Left | (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right))));
            this.preview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.preview.Location = new System.Drawing.Point(8, 40);
            this.preview.Name = "preview";
            this.preview.Size = new System.Drawing.Size(112, 80);
            this.preview.TabIndex = 1;
            this.preview.TabStop = false;
            // 
            // costBox
            // 
            this.costBox.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Left | (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.costBox.cost = 0;
            this.costBox.label = "Cost:";
            //! this.costBox.label = "費用：";
            this.costBox.Location = new System.Drawing.Point(8, 128);
            this.costBox.Name = "costBox";
            this.costBox.Size = new System.Drawing.Size(112, 32);
            this.costBox.TabIndex = 10;
            // 
            // buttonRemove
            // 
            this.buttonRemove.Appearance = System.Windows.Forms.Appearance.Button;
            this.buttonRemove.Location = new System.Drawing.Point(64, 160);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(56, 24);
            this.buttonRemove.TabIndex = 9;
            this.buttonRemove.Text = "Remove";
            //! this.buttonRemove.Text = "撤去";
            this.buttonRemove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonRemove.CheckedChanged += new System.EventHandler(this.onTypeChanged);
            // 
            // buttonPlace
            // 
            this.buttonPlace.Appearance = System.Windows.Forms.Appearance.Button;
            this.buttonPlace.Checked = true;
            this.buttonPlace.Location = new System.Drawing.Point(8, 160);
            this.buttonPlace.Name = "buttonPlace";
            this.buttonPlace.Size = new System.Drawing.Size(56, 24);
            this.buttonPlace.TabIndex = 8;
            this.buttonPlace.TabStop = true;
            this.buttonPlace.Text = "Build";
            //! this.buttonPlace.Text = "設置";
            this.buttonPlace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonPlace.CheckedChanged += new System.EventHandler(this.onTypeChanged);
            // 
            // ControllerForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
            this.ClientSize = new System.Drawing.Size(128, 190);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.costBox,
																		  this.buttonRemove,
																		  this.buttonPlace,
																		  this.typeBox,
																		  this.preview});
            this.Name = "ControllerForm";
            this.Text = "Road Accessories";
            this.Resize += new EventHandler(this.updateAfterResize);
            //! this.Text = "道路アクセサリ";
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        protected bool isPlacing
        {
            get
            {
                return buttonPlace.Checked;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void updateAfterResize(object sender, System.EventArgs e)
        {
            this.buttonPlace.Left = this.preview.Left;
            this.buttonPlace.Width = ((this.preview.Width)) / 2;
            this.buttonRemove.Left = (this.buttonPlace.Left + this.buttonPlace.Width);
            this.buttonRemove.Width = this.buttonPlace.Width;
            this.buttonPlace.Top = this.ClientSize.Height - 30;
            this.buttonRemove.Top = this.buttonPlace.Top;
            updatePreview();
        }

        /// <summary>
        /// Called when a selection of the structure has changed.
        /// </summary>
        protected virtual void onTypeChanged(object sender, System.EventArgs e)
        {
            RoadAccessoryContribution builder = (RoadAccessoryContribution)typeBox.SelectedItem;
            if (builder != null)
            {
                using (PreviewDrawer drawer = builder.createPreview(preview.Size))
                {
                    if (previewBitmap != null) previewBitmap.Dispose();
                    preview.Image = previewBitmap = drawer.createBitmap();
                }
                if (isPlacing)
                    currentController = builder.createBuilder(this.siteImpl);
                else
                    currentController = builder.createRemover(this.siteImpl);
            }
        }
    }
}

