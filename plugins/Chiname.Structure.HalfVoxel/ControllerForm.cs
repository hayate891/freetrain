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

namespace FreeTrain.World.Structs.HalfVoxelStructure
{
    /// <summary>
    /// Controller that allows the user to
    /// place/remove cars.
    /// </summary>
    public class ControllerForm : AbstractControllerImpl
    {
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton btnRemove;
        private System.Windows.Forms.RadioButton btnPlace;
        private FreeTrain.Controls.CostBox price;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox group;
        private System.Windows.Forms.Label namelabel;
        private FreeTrain.Controls.IndexSelector idxDesign;
        private FreeTrain.Controls.IndexSelector idxColor;
        private System.Windows.Forms.ComboBox typeBox;
        private System.Windows.Forms.CheckBox cbRndColor;
        private System.Windows.Forms.CheckBox cbRndDesign;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbRndColor2;
        private FreeTrain.Controls.IndexSelector idxColor2;

        private Bitmap previewBitmap;
        private Random rnd;
        /// <summary>
        /// 
        /// </summary>
        public ControllerForm()
        {
            InitializeComponent();
            WorldDefinition.World.ViewOptions.OnViewOptionChanged += new OptionChangedHandler(UpdatePreview);
            rnd = new Random();

            callback = new createCallback(randomize);
            typeBox.DataSource = loadContributions();
            onTypeChanged(this, null);
            onButtonClicked(this, null);
        }

        /// <summary>
        /// Called to prepare array of contribution used for typeBox.DataSource
        /// </summary>
        /// <returns></returns>
        private ArrayList loadContributions()
        {
            Array src = PluginManager.ListContributions(typeof(HalfVoxelContribution));
            Hashtable h = new Hashtable();
            foreach (HalfVoxelContribution c in src)
            {
                string key = c.subgroup;
                if (!h.ContainsKey(key))
                    h.Add(key, new SubGroup(key));
                ((SubGroup)h[key]).Add(c);
            }
            ArrayList dest = new ArrayList();
            foreach (object o in h.Values)
                dest.Add(o);
            return dest;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            WorldDefinition.World.ViewOptions.OnViewOptionChanged -= new OptionChangedHandler(UpdatePreview);
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
            this.components = new System.ComponentModel.Container();
            this.preview = new System.Windows.Forms.PictureBox();
            this.btnRemove = new System.Windows.Forms.RadioButton();
            this.btnPlace = new System.Windows.Forms.RadioButton();
            this.group = new System.Windows.Forms.GroupBox();
            this.namelabel = new System.Windows.Forms.Label();
            this.idxDesign = new FreeTrain.Controls.IndexSelector();
            this.idxColor = new FreeTrain.Controls.IndexSelector();
            this.label1 = new System.Windows.Forms.Label();
            this.typeBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbRndColor = new System.Windows.Forms.CheckBox();
            this.cbRndDesign = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbRndColor2 = new System.Windows.Forms.CheckBox();
            this.idxColor2 = new FreeTrain.Controls.IndexSelector();
            this.price = new FreeTrain.Controls.CostBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.indexSelector = new FreeTrain.Controls.IndexSelector();
            ((System.ComponentModel.ISupportInitialize)(this.preview)).BeginInit();
            this.group.SuspendLayout();
            this.SuspendLayout();
            // 
            // preview
            // 
            this.preview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.preview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.preview.Location = new System.Drawing.Point(193, 9);
            this.preview.Name = "preview";
            this.preview.Size = new System.Drawing.Size(80, 142);
            this.preview.TabIndex = 1;
            this.preview.TabStop = false;
            this.toolTip1.SetToolTip(this.preview, "Click to create another random pattern");
            this.preview.Click += new System.EventHandler(this.onPreviewClick);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnRemove.Location = new System.Drawing.Point(229, 199);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(56, 26);
            this.btnRemove.TabIndex = 8;
            this.btnRemove.Text = "Remove";
            this.btnRemove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnRemove.Click += new System.EventHandler(this.onButtonClicked);
            // 
            // btnPlace
            // 
            this.btnPlace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlace.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnPlace.Checked = true;
            this.btnPlace.Location = new System.Drawing.Point(181, 199);
            this.btnPlace.Name = "btnPlace";
            this.btnPlace.Size = new System.Drawing.Size(48, 26);
            this.btnPlace.TabIndex = 7;
            this.btnPlace.TabStop = true;
            this.btnPlace.Text = "Place";
            this.btnPlace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPlace.Click += new System.EventHandler(this.onButtonClicked);
            // 
            // group
            // 
            this.group.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.group.Controls.Add(this.namelabel);
            this.group.Controls.Add(this.idxDesign);
            this.group.Controls.Add(this.idxColor);
            this.group.Controls.Add(this.label1);
            this.group.Controls.Add(this.typeBox);
            this.group.Controls.Add(this.label3);
            this.group.Controls.Add(this.cbRndColor);
            this.group.Controls.Add(this.cbRndDesign);
            this.group.Controls.Add(this.label2);
            this.group.Controls.Add(this.cbRndColor2);
            this.group.Controls.Add(this.idxColor2);
            this.group.Location = new System.Drawing.Point(0, 0);
            this.group.Name = "group";
            this.group.Size = new System.Drawing.Size(177, 225);
            this.group.TabIndex = 1;
            this.group.TabStop = false;
            this.toolTip1.SetToolTip(this.group, "Select a building to the left");
            // 
            // namelabel
            // 
            this.namelabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.namelabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.namelabel.Location = new System.Drawing.Point(8, 82);
            this.namelabel.Name = "namelabel";
            this.namelabel.Size = new System.Drawing.Size(161, 31);
            this.namelabel.TabIndex = 13;
            this.namelabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // idxDesign
            // 
            this.idxDesign.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.idxDesign.count = 10;
            this.idxDesign.current = 0;
            this.idxDesign.dataSource = null;
            this.idxDesign.Location = new System.Drawing.Point(8, 61);
            this.idxDesign.Name = "idxDesign";
            this.idxDesign.Size = new System.Drawing.Size(161, 17);
            this.idxDesign.TabIndex = 5;
            this.idxDesign.indexChanged += new System.EventHandler(this.onDesignChanged);
            // 
            // idxColor
            // 
            this.idxColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.idxColor.count = 10;
            this.idxColor.current = 0;
            this.idxColor.dataSource = null;
            this.idxColor.Location = new System.Drawing.Point(8, 137);
            this.idxColor.Name = "idxColor";
            this.idxColor.Size = new System.Drawing.Size(161, 17);
            this.idxColor.TabIndex = 11;
            this.idxColor.indexChanged += new System.EventHandler(this.onColorChanged);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(8, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 18);
            this.label1.TabIndex = 12;
            this.label1.Text = "Design:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // typeBox
            // 
            this.typeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.typeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.typeBox.Location = new System.Drawing.Point(3, 9);
            this.typeBox.Name = "typeBox";
            this.typeBox.Size = new System.Drawing.Size(171, 21);
            this.typeBox.Sorted = true;
            this.typeBox.TabIndex = 1;
            this.toolTip1.SetToolTip(this.typeBox, "Select a building to the left");
            this.typeBox.SelectedIndexChanged += new System.EventHandler(this.onTypeChanged);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(8, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 17);
            this.label3.TabIndex = 12;
            this.label3.Text = "Color:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbRndColor
            // 
            this.cbRndColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbRndColor.BackColor = System.Drawing.Color.Transparent;
            this.cbRndColor.Location = new System.Drawing.Point(109, 117);
            this.cbRndColor.Name = "cbRndColor";
            this.cbRndColor.Size = new System.Drawing.Size(68, 17);
            this.cbRndColor.TabIndex = 7;
            this.cbRndColor.Text = "Random";
            this.cbRndColor.UseVisualStyleBackColor = false;
            this.cbRndColor.CheckedChanged += new System.EventHandler(this.onCheckBoxChanged);
            // 
            // cbRndDesign
            // 
            this.cbRndDesign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbRndDesign.BackColor = System.Drawing.Color.Transparent;
            this.cbRndDesign.Location = new System.Drawing.Point(109, 43);
            this.cbRndDesign.Name = "cbRndDesign";
            this.cbRndDesign.Size = new System.Drawing.Size(68, 18);
            this.cbRndDesign.TabIndex = 3;
            this.cbRndDesign.Text = "Random";
            this.cbRndDesign.UseVisualStyleBackColor = false;
            this.cbRndDesign.CheckedChanged += new System.EventHandler(this.onCheckBoxChanged);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(8, 160);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 18);
            this.label2.TabIndex = 12;
            this.label2.Text = "Color 2:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbRndColor2
            // 
            this.cbRndColor2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbRndColor2.BackColor = System.Drawing.Color.Transparent;
            this.cbRndColor2.Location = new System.Drawing.Point(109, 160);
            this.cbRndColor2.Name = "cbRndColor2";
            this.cbRndColor2.Size = new System.Drawing.Size(68, 18);
            this.cbRndColor2.TabIndex = 7;
            this.cbRndColor2.Text = "Random";
            this.cbRndColor2.UseVisualStyleBackColor = false;
            this.cbRndColor2.CheckedChanged += new System.EventHandler(this.onCheckBoxChanged);
            // 
            // idxColor2
            // 
            this.idxColor2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.idxColor2.count = 10;
            this.idxColor2.current = 0;
            this.idxColor2.dataSource = null;
            this.idxColor2.Location = new System.Drawing.Point(8, 180);
            this.idxColor2.Name = "idxColor2";
            this.idxColor2.Size = new System.Drawing.Size(161, 17);
            this.idxColor2.TabIndex = 11;
            this.idxColor2.indexChanged += new System.EventHandler(this.onColor2Changed);
            // 
            // price
            // 
            this.price.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.price.cost = 0;
            this.price.label = "Cost:";
            this.price.Location = new System.Drawing.Point(193, 162);
            this.price.Name = "price";
            this.price.Size = new System.Drawing.Size(80, 35);
            this.price.TabIndex = 14;
            this.toolTip1.SetToolTip(this.price, "Building cost (total)");
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
            this.ClientSize = new System.Drawing.Size(289, 231);
            this.Controls.Add(this.group);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnPlace);
            this.Controls.Add(this.preview);
            this.Controls.Add(this.price);
            this.Name = "ControllerForm";
            this.Text = "Half-tile Construction";
            this.Resize += new System.EventHandler(this.UpdateSize);
            ((System.ComponentModel.ISupportInitialize)(this.preview)).EndInit();
            this.group.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public bool IsPlacing
        {
            get
            {
                return btnPlace.Checked;
            }
        }

        #region private properties
        private int currentColor2
        {
            get
            {
                return idxColor2.current;
            }
        }

        private int currentColor
        {
            get
            {
                return idxColor.current;
            }
        }

        private int currentDesign
        {
            get
            {
                return idxDesign.current;
            }
        }

        private SubGroup currentGroup
        {
            get
            {
                return (SubGroup)typeBox.SelectedItem;
            }
        }

        private HalfVoxelContribution currentContrib
        {
            get
            {
                return (HalfVoxelContribution)currentGroup[currentDesign];
            }
        }
        #endregion

        #region Event Handlers
        private void onTypeChanged(object sender, System.EventArgs e)
        {
            idxDesign.count = currentGroup.Size;
            onDesignChanged(sender, e);
        }

        private void onDesignChanged(object sender, System.EventArgs e)
        {
            idxColor.count = currentContrib.colors.size;
            idxColor2.count = currentContrib.getHighlihtPatternCount();
            namelabel.Text = currentContrib.Name;
            price.cost = currentContrib.Price;
            onButtonClicked(sender, e);
            onColorChanged(sender, e);
            onColor2Changed(sender, e);
        }

        private void onColorChanged(object sender, System.EventArgs e)
        {
            currentContrib.currentColor = currentColor;
            UpdatePreview();
        }

        private void onColor2Changed(object sender, System.EventArgs e)
        {
            currentContrib.currentHighlight = currentColor2;
            UpdatePreview();
        }

        private void onCheckBoxChanged(object sender, System.EventArgs e)
        {
            idxDesign.Enabled = !cbRndDesign.Checked;
            idxColor.Enabled = !cbRndColor.Checked;
            idxColor2.Enabled = !cbRndColor2.Checked;
            onPreviewClick(sender, e);
        }

        private void onPreviewClick(object sender, System.EventArgs e)
        {
            if (cbRndDesign.Checked)
            {
                idxDesign.current = rnd.Next(idxDesign.count);
                idxColor.count = currentContrib.colors.size;
            }
            if (cbRndColor.Checked)
                idxColor.current = rnd.Next(idxColor.count);
            if (cbRndColor2.Checked)
                idxColor2.current = rnd.Next(idxColor2.count);

            if (cbRndDesign.Checked)
                onDesignChanged(sender, e);
            else if (cbRndColor.Checked)
                onColorChanged(sender, e);
        }

        private void onButtonClicked(object sender, System.EventArgs e)
        {
            //if (currentController != null)
            //    ((HVControllerImpl)currentController).onCreated -= callback;
            //if (isPlacing)
            //{
            //    currentController = currentContrib.createBuilder(this.siteImpl);
            //    ((HVControllerImpl)currentController).onCreated += callback;
            //}
            //else
            //    currentController = currentContrib.createRemover(this.siteImpl);
        }

        #endregion

        internal void randomize()
        {
            onPreviewClick(this, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void UpdateSize(object sender, System.EventArgs e)
        {
            UpdatePreview();
        }

        private createCallback callback;

        /// <summary>
        /// Called when a selection of the structure has changed.
        /// </summary>
        public override void UpdatePreview()
        {
            using (PreviewDrawer drawer = currentContrib.CreatePreview(preview.Size))
            {
                if (previewBitmap != null) previewBitmap.Dispose();
                preview.Image = previewBitmap = drawer.createBitmap();
            }
        }

        #region SubGroup
        private class SubGroup : object
        {
            private readonly string name;

            public string Name
            {
                get { return name; }
            } 

            private ArrayList arr;
            public SubGroup(string name)
            {
                this.name = name;
                arr = new ArrayList();
            }

            public void Add(HalfVoxelContribution contrib)
            {
                arr.Add(contrib);
            }

            public int Size
            {
                get { return arr.Count; }
            }

            public HalfVoxelContribution this[int index]
            {
                get
                {
                    return (HalfVoxelContribution)arr[index];
                }
            }

            public override string ToString()
            {
                return name;
            }
        }
        #endregion
    }
}

