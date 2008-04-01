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
using FreeTrain.Controllers;
using FreeTrain.Contributions.Common;
using FreeTrain.Views;
using FreeTrain.Views.Map;
using FreeTrain.World;
using FreeTrain.World.Terrain;
using FreeTrain.Framework;
using FreeTrain.Framework.Graphics;
using FreeTrain.Framework.Plugin;
using FreeTrain.Util;

namespace FreeTrain.World.Road.DummyCars
{
    /// <summary>
    /// Controller that allows the user to
    /// place/remove cars.
    /// </summary>
    [CLSCompliant(false)]
    public class ControllerForm : AbstractControllerImpl
    {
        private FreeTrain.Controls.CostBox costBox;
        private System.Windows.Forms.RadioButton buttonRemove;
        private System.Windows.Forms.RadioButton buttonPlace;
        private System.Windows.Forms.ComboBox typeBox;
        private FreeTrain.Controls.IndexSelector colSelector;

        private Bitmap previewBitmap;
        /// <summary>
        /// 
        /// </summary>
        public ControllerForm()
        {
            InitializeComponent();

            // load list of lands
            typeBox.DataSource = Core.Plugins.listContributions(typeof(DummyCarContribution));
            typeBox.DisplayMember = "name";
            if (typeBox.Items.Count == 0)
            {
                buttonPlace.Enabled = false;
                buttonRemove.Enabled = false;
                colSelector.Enabled = false;
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
        private FreeTrain.Controls.IndexSelector indexSelector;

        private void InitializeComponent()
        {
            this.typeBox = new System.Windows.Forms.ComboBox();
            this.preview = new System.Windows.Forms.PictureBox();
            this.buttonRemove = new System.Windows.Forms.RadioButton();
            this.buttonPlace = new System.Windows.Forms.RadioButton();
            this.colSelector = new FreeTrain.Controls.IndexSelector();
            this.costBox = new FreeTrain.Controls.CostBox();
            this.indexSelector = new FreeTrain.Controls.IndexSelector();
            ((System.ComponentModel.ISupportInitialize)(this.preview)).BeginInit();
            this.SuspendLayout();
            // 
            // typeBox
            // 
            this.typeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.typeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.typeBox.Location = new System.Drawing.Point(8, 9);
            this.typeBox.Name = "typeBox";
            this.typeBox.Size = new System.Drawing.Size(162, 21);
            this.typeBox.Sorted = true;
            this.typeBox.TabIndex = 2;
            this.typeBox.SelectedIndexChanged += new System.EventHandler(this.onTypeChanged);
            // 
            // preview
            // 
            this.preview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.preview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.preview.Location = new System.Drawing.Point(8, 69);
            this.preview.Name = "preview";
            this.preview.Size = new System.Drawing.Size(162, 98);
            this.preview.TabIndex = 1;
            this.preview.TabStop = false;
            // 
            // buttonRemove
            // 
            this.buttonRemove.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonRemove.Appearance = System.Windows.Forms.Appearance.Button;
            this.buttonRemove.Location = new System.Drawing.Point(89, 184);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(56, 26);
            this.buttonRemove.TabIndex = 9;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonRemove.CheckedChanged += new System.EventHandler(this.onColorChanged);
            // 
            // buttonPlace
            // 
            this.buttonPlace.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonPlace.Appearance = System.Windows.Forms.Appearance.Button;
            this.buttonPlace.Checked = true;
            this.buttonPlace.Location = new System.Drawing.Point(33, 184);
            this.buttonPlace.Name = "buttonPlace";
            this.buttonPlace.Size = new System.Drawing.Size(56, 26);
            this.buttonPlace.TabIndex = 8;
            this.buttonPlace.TabStop = true;
            this.buttonPlace.Text = "Place";
            this.buttonPlace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonPlace.CheckedChanged += new System.EventHandler(this.onColorChanged);
            // 
            // colSelector
            // 
            this.colSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.colSelector.count = 10;
            this.colSelector.current = 0;
            this.colSelector.Cursor = System.Windows.Forms.Cursors.Default;
            this.colSelector.dataSource = null;
            this.colSelector.Location = new System.Drawing.Point(8, 35);
            this.colSelector.Name = "colSelector";
            this.colSelector.Size = new System.Drawing.Size(162, 21);
            this.colSelector.TabIndex = 6;
            this.colSelector.indexChanged += new System.EventHandler(this.onColorChanged);
            // 
            // costBox
            // 
            this.costBox.cost = 0;
            this.costBox.label = "Cost:";
            this.costBox.Location = new System.Drawing.Point(0, 0);
            this.costBox.Name = "costBox";
            this.costBox.Size = new System.Drawing.Size(96, 32);
            this.costBox.TabIndex = 0;
            // 
            // indexSelector
            // 
            this.indexSelector.count = 10;
            this.indexSelector.current = 0;
            this.indexSelector.dataSource = null;
            this.indexSelector.Location = new System.Drawing.Point(0, 0);
            this.indexSelector.Name = "indexSelector";
            this.indexSelector.Size = new System.Drawing.Size(112, 24);
            this.indexSelector.TabIndex = 0;
            // 
            // ControllerForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(178, 217);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonPlace);
            this.Controls.Add(this.typeBox);
            this.Controls.Add(this.colSelector);
            this.Controls.Add(this.preview);
            this.Name = "ControllerForm";
            this.Text = "Automobile";
            this.Resize += new System.EventHandler(this.updateAfterResize);
            ((System.ComponentModel.ISupportInitialize)(this.preview)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public bool isPlacing
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
            UpdatePreview();
        }

        private int currentColor
        {
            get
            {
                return colSelector.current;
            }
        }

        private void onTypeChanged(object sender, System.EventArgs e)
        {
            DummyCarContribution builder = (DummyCarContribution)typeBox.SelectedItem;
            if (builder != null)
            {
                int colors = builder.colorVariations;
                if (this.colSelector.current > colors) this.colSelector.current = colors;
                this.colSelector.count = colors;
                builder.currentColor = colors;
                updatePreview(builder);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void onColorChanged(object sender, System.EventArgs e)
        {
            DummyCarContribution builder = (DummyCarContribution)typeBox.SelectedItem;
            if (builder != null)
            {
                builder.currentColor = this.currentColor;
                updatePreview(builder);
            }
        }

        /// <summary>
        /// Called when a selection of the structure has changed.
        /// </summary>
        protected virtual void updatePreview(DummyCarContribution builder)
        {
            //DummyCarContribution builder = (DummyCarContribution)typeBox.SelectedItem;
            using (PreviewDrawer drawer = new PreviewDrawer(preview.Size, new Size(10, 1), 0))
            {

                drawer.draw(builder.getSprites(), 5, 0);

                if (previewBitmap != null) previewBitmap.Dispose();
                preview.Image = previewBitmap = drawer.createBitmap();
            }
            //if (isPlacing)
                //currentController = builder.createBuilder(this.siteImpl);
            //else
                //currentController = builder.createRemover(this.siteImpl);
        }
    }
}

