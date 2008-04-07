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
using FreeTrain.Contributions.Common;
using FreeTrain.Views;
using FreeTrain.Views.Map;
using FreeTrain.World;
using FreeTrain.World.Structs;
using FreeTrain.Contributions.Rail;
using FreeTrain.Framework;
using FreeTrain.Framework.Graphics;
using FreeTrain.Framework.Plugin;
using FreeTrain.Util;

namespace FreeTrain.Controllers.Structs
{
    /// <summary>
    /// Controller that allows the user to
    /// place/remove structures.
    /// </summary>
    public abstract class StructPlacementController : AbstractControllerImpl, IMapOverlay, ILocationDisambiguator
    {
        private System.Windows.Forms.ComboBox structType;
        private System.Windows.Forms.PictureBox preview;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.RadioButton buttonRemove;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.RadioButton buttonPlace;
        private System.ComponentModel.IContainer components = null;
        private FreeTrain.Controls.IndexSelector indexSelector;

        private Bitmap previewBitmap;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupGroup"></param>
        protected StructPlacementController(StructureGroupGroup groupGroup)
        {
            InitializeComponent();
            WorldDefinition.World.ViewOptions.OnViewOptionChanged += new OptionChangedHandler(UpdatePreview);
            previewBitmap = null;
            // load station type list
            structType.DataSource = groupGroup;
            structType.DisplayMember = "name";
            updateAfterResize(null, null);
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

            if (alphaSprites != null)
                alphaSprites.Dispose();
        }
        /// <summary>
        /// 
        /// </summary>
        public override ILocationDisambiguator Disambiguator { get { return this; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public abstract bool IsSelectable(Location loc);

        #region Designer generated code
        /// <summary>
        /// Designer サポートに必要なメソッドです。コード エディタで
        /// このメソッドのコンテンツを変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.structType = new System.Windows.Forms.ComboBox();
            this.preview = new System.Windows.Forms.PictureBox();
            this.buttonRemove = new System.Windows.Forms.RadioButton();
            this.buttonPlace = new System.Windows.Forms.RadioButton();
            this.indexSelector = new FreeTrain.Controls.IndexSelector();
            ((System.ComponentModel.ISupportInitialize)(this.preview)).BeginInit();
            this.SuspendLayout();
            // 
            // structType
            // 
            this.structType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                    | System.Windows.Forms.AnchorStyles.Right)));
            this.structType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.structType.Location = new System.Drawing.Point(8, 9);
            this.structType.Name = "structType";
            this.structType.Size = new System.Drawing.Size(130, 21);
            this.structType.Sorted = true;
            this.structType.TabIndex = 2;
            this.structType.SelectedIndexChanged += new System.EventHandler(this.onGroupChanged);
            // 
            // preview
            // 
            this.preview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                    | System.Windows.Forms.AnchorStyles.Left)
                                    | System.Windows.Forms.AnchorStyles.Right)));
            this.preview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.preview.Location = new System.Drawing.Point(8, 69);
            this.preview.Name = "preview";
            this.preview.Size = new System.Drawing.Size(130, 87);
            this.preview.TabIndex = 1;
            this.preview.TabStop = false;
            // 
            // buttonRemove
            // 
            this.buttonRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemove.Appearance = System.Windows.Forms.Appearance.Button;
            this.buttonRemove.Location = new System.Drawing.Point(82, 165);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(56, 26);
            this.buttonRemove.TabIndex = 1;
            this.buttonRemove.Text = "Remove";
            //! this.buttonRemove.Text = "撤去";
            this.buttonRemove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonPlace
            // 
            this.buttonPlace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPlace.Appearance = System.Windows.Forms.Appearance.Button;
            this.buttonPlace.Checked = true;
            this.buttonPlace.Location = new System.Drawing.Point(8, 165);
            this.buttonPlace.Name = "buttonPlace";
            this.buttonPlace.Size = new System.Drawing.Size(56, 26);
            this.buttonPlace.TabIndex = 0;
            this.buttonPlace.TabStop = true;
            this.buttonPlace.Text = "Build";
            //! this.buttonPlace.Text = "設置";
            this.buttonPlace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // indexSelector
            // 
            this.indexSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                    | System.Windows.Forms.AnchorStyles.Right)));
            this.indexSelector.count = 10;
            this.indexSelector.current = 0;
            this.indexSelector.dataSource = null;
            this.indexSelector.Location = new System.Drawing.Point(8, 39);
            this.indexSelector.Name = "indexSelector";
            this.indexSelector.Size = new System.Drawing.Size(130, 22);
            this.indexSelector.TabIndex = 3;
            this.indexSelector.indexChanged += new System.EventHandler(this.onTypeChanged);
            // 
            // StructPlacementController
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(146, 195);
            this.Controls.Add(this.indexSelector);
            this.Controls.Add(this.buttonPlace);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.structType);
            this.Controls.Add(this.preview);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "StructPlacementController";
            this.Text = "Building construction";
            //! this.Text = "建物の工事(仮)";
            this.Resize += new System.EventHandler(this.updateAfterResize);
            ((System.ComponentModel.ISupportInitialize)(this.preview)).EndInit();
            this.ResumeLayout(false);
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void updateAfterResize(object sender, System.EventArgs e)
        {
            this.buttonPlace.Width = ((this.preview.Left + this.preview.Width) - this.buttonPlace.Left - 5) / 2;
            this.buttonRemove.Left = (this.buttonPlace.Left + this.buttonPlace.Width) + 10;
            this.buttonRemove.Width = this.preview.Width - this.buttonPlace.Width - 10;
            UpdatePreview();
        }


        /// <summary>
        /// 
        /// </summary>
        protected bool isPlacing { get { return buttonPlace.Checked; } }



        private Location baseLoc = World.Location.Unplaced;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        public override void OnMouseMove(MapViewWindow view, Location loc, Point ab)
        {
            WorldDefinition w = WorldDefinition.World;

            if (baseLoc != loc)
            {
                // update the screen
                baseLoc = loc;
                // TODO: we need to correctly update the screen
                w.OnAllVoxelUpdated();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="surface"></param>
        public void DrawBefore(QuarterViewDrawer view, DrawContext surface) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="canvas"></param>
        /// <param name="loc"></param>
        /// <param name="pt"></param>
        public void DrawVoxel(QuarterViewDrawer view, DrawContext canvas, Location loc, Point pt)
        {
            if (!isPlacing) return;
            if (alphaSprites != null)
            {
                if (Cube.createExclusive(baseLoc, alphaSprites.size).contains(loc))
                    alphaSprites.getSprite(loc - baseLoc).drawAlpha(canvas.Surface, pt);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="surface"></param>
        public void DrawAfter(QuarterViewDrawer view, DrawContext surface) { }


        /// <summary>
        /// Currently selected structure contribution.
        /// </summary>
        protected StructureContribution selectedType
        {
            get
            {
                return (StructureContribution)indexSelector.currentItem;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            updateAlphaSprites();
        }

        private AlphaBlendSpriteSet alphaSprites;

        /// <summary>
        /// Re-builds an alpha-blending preview.
        /// </summary>
        protected void updateAlphaSprites()
        {
            if (alphaSprites != null)
                alphaSprites.Dispose();

            // builds a new alpha blended preview
            alphaSprites = createAlphaSprites();
        }

        /// <summary>
        /// Implemented by the derived class to provide a sprite set used
        /// to draw a preview of this structure on MapView.
        /// </summary>
        protected abstract AlphaBlendSpriteSet createAlphaSprites();

        private void onGroupChanged(object sender, System.EventArgs e)
        {
            indexSelector.dataSource = (StructureGroup)structType.SelectedItem;
            onTypeChanged(null, null);
        }


        /// <summary>
        /// Called when a selection of the structure has changed.
        /// </summary>
        protected virtual void onTypeChanged(object sender, System.EventArgs e)
        {
            UpdatePreview();
        }
        /// <summary>
        /// 
        /// </summary>
        public override void UpdatePreview()
        {
            /*using( PreviewDrawer drawer = selectedType.createPreview(preview.Size) )
            {
                if( previewBitmap!=null )	previewBitmap.Dispose();
                preview.Image = previewBitmap = drawer.createBitmap();
            }*/

            if (selectedType != null) updateAlphaSprites();
        }
    }
}

