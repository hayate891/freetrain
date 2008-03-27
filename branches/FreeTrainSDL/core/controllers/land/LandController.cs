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
using FreeTrain.Contributions.Land;
using FreeTrain.Contributions.Common;
using FreeTrain.Contributions.Sound;
using FreeTrain.Views;
using FreeTrain.Views.Map;
using FreeTrain.World;
using FreeTrain.World.Terrain;
using FreeTrain.Framework;
using FreeTrain.Framework.Graphics;
using FreeTrain.Framework.Plugin;
using FreeTrain.Util;

namespace FreeTrain.Controllers.Land
{
    /// <summary>
    /// Controller that allows the user to
    /// place/remove lands.
    /// </summary>
    public class LandController : ControllerHostForm
    {
        #region Singleton instance management
        /// <summary>
        /// Creates a new controller window, or active the existing one.
        /// </summary>
        public static void create()
        {
            if (theInstance == null)
                theInstance = new LandController();
            theInstance.Show();
            theInstance.Activate();
        }

        public static LandController theInstance;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            theInstance = null;
        }
        #endregion

        private Bitmap previewBitmap;
        /// <summary>
        /// 
        /// </summary>
        protected LandController()
        {
            InitializeComponent();

            // load list of lands
            groupBox.DataSource = Core.plugins.landBuilderGroup;
            groupBox.DisplayMember = "name";
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
        private FreeTrain.Controls.IndexSelector indexSelector;
        private System.Windows.Forms.ComboBox groupBox;

        private void InitializeComponent()
        {
            this.groupBox = new System.Windows.Forms.ComboBox();
            this.preview = new System.Windows.Forms.PictureBox();
            this.indexSelector = new FreeTrain.Controls.IndexSelector();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            this.groupBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.groupBox.Location = new System.Drawing.Point(8, 8);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(112, 20);
            this.groupBox.Sorted = true;
            this.groupBox.TabIndex = 2;
            this.groupBox.SelectedIndexChanged += new System.EventHandler(this.onGroupChanged);
            this.groupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)) | System.Windows.Forms.AnchorStyles.Top));
            // 
            // preview
            // 
            this.preview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.preview.Location = new System.Drawing.Point(8, 64);
            this.preview.Name = "preview";
            this.preview.Size = new System.Drawing.Size(112, 80);
            this.preview.TabIndex = 1;
            this.preview.TabStop = false;
            this.preview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)) | System.Windows.Forms.AnchorStyles.Bottom));
            // 
            // indexSelector
            // 
            this.indexSelector.count = 10;
            this.indexSelector.current = 0;
            this.indexSelector.dataSource = null;
            this.indexSelector.Location = new System.Drawing.Point(8, 36);
            this.indexSelector.Name = "indexSelector";
            this.indexSelector.Size = new System.Drawing.Size(112, 20);
            this.indexSelector.TabIndex = 3;
            this.indexSelector.indexChanged += new System.EventHandler(this.onTypeChanged);
            this.indexSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Left | (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // LandController
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
            this.ClientSize = new System.Drawing.Size(128, 155);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.indexSelector,
																		  this.groupBox,
																		  this.preview});
            this.Name = "LandController";
            this.Text = "Terrain view";
            //! this.Text = "地表";
            this.Resize += new EventHandler(this.updateSize);
            this.ResumeLayout(false);

        }
        #endregion






        private void onGroupChanged(object sender, System.EventArgs e)
        {
            indexSelector.dataSource = (LandBuilderGroup)groupBox.SelectedItem;
            onTypeChanged(null, null);
        }

        /// <summary>
        /// Called when a selection of the structure has changed.
        /// </summary>
        protected virtual void onTypeChanged(object sender, System.EventArgs e)
        {
            updatePreview();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void updateSize(object sender, System.EventArgs e)
        {
            updatePreview();
        }
        /// <summary>
        /// 
        /// </summary>
        public override void updatePreview()
        {
            LandBuilderContribution builder = (LandBuilderContribution)indexSelector.currentItem;
            using (PreviewDrawer drawer = builder.createPreview(preview.Size))
            {
                if (previewBitmap != null) previewBitmap.Dispose();
                preview.Image = previewBitmap = drawer.createBitmap();
            }

            currentController = builder.createBuilder(new ControllerSiteImpl(this));
        }
    }
}

