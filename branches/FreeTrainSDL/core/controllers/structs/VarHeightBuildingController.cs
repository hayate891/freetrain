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
using FreeTrain.world;
using FreeTrain.world.Structs;
using FreeTrain.Framework;
using FreeTrain.Framework.graphics;
using FreeTrain.Framework.plugin;
using FreeTrain.Util;
using SDL.net;

namespace FreeTrain.Controllers.Structs
{
    /// <summary>
    /// Controller that allows the user to
    /// place/remove VarHeightBuildingContribution.
    /// </summary>
    public class VarHeightBuildingController : StructPlacementController
    {
        #region Singleton instance management
        /// <summary>
        /// Creates a new controller window, or active the existing one.
        /// </summary>
        public static void create()
        {
            if (theInstance == null)
                theInstance = new VarHeightBuildingController();
            theInstance.Show();
            theInstance.Activate();
        }

        private static VarHeightBuildingController theInstance;
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
        /// <summary>
        /// 
        /// </summary>
        protected VarHeightBuildingController()
            : base(Core.plugins.varHeightBuildingsGroup)
        {
            InitializeComponent();
            buttonPlace.Top += 24;
            buttonRemove.Top += 24;
        }

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        public override void onClick(MapViewWindow view, Location loc, Point ab)
        {
            if (isPlacing)
            {
                if (!selectedType.canBeBuilt(loc, height))
                {
                    MainWindow.showError("Can not build");
                    //! MainWindow.showError("設置できません");
                }
                else
                {
                    CompletionHandler handler = new CompletionHandler(selectedType, loc, height, true);
                    new ConstructionSite(loc, new EventHandler(handler.handle),
                        new Distance(selectedType.size, height));
                }
            }
            else
            {
                VarHeightBuilding building = VarHeightBuilding.get(loc);
                if (building != null)
                    building.remove();
            }
        }

        /// <summary> LocationDisambiguator implementation </summary>
        public override bool isSelectable(Location loc)
        {
            if (isPlacing)
            {
                // structures can be placed only on the ground
                return GroundDisambiguator.theInstance.isSelectable(loc);
            }
            else
            {
                return VarHeightBuilding.get(loc) != null;
            }
        }

        [Serializable]
        private class CompletionHandler
        {
            internal CompletionHandler(VarHeightBuildingContribution contribution,
                Location loc, int height, bool initiallyOwned)
            {

                this.contribution = contribution;
                this.loc = loc;
                this.height = height;
                this.owned = initiallyOwned;
            }
            private readonly VarHeightBuildingContribution contribution;
            private readonly Location loc;
            private readonly int height;
            private readonly bool owned;
            public void handle(object sender, EventArgs args)
            {
                contribution.create(loc, height, owned);
            }
        }

        private new VarHeightBuildingContribution selectedType
        {
            get
            {
                return (VarHeightBuildingContribution)base.selectedType;
            }
        }
        private int height
        {
            get
            {
                if (heightBox == null) { return 1; }
                return (int)heightBox.Value / 4;
            }
        }

        /// <summary>
        /// Re-builds an alpha-blending preview.
        /// </summary>
        protected override AlphaBlendSpriteSet createAlphaSprites()
        {

            // builds a new alpha blended preview
            // TODO: make a proper 3D preview.
            Size sz = selectedType.size;
            Sprite[, ,] sprites = new Sprite[sz.Width, sz.Height, height];
            for (int z = 0; z < height; z++)
                for (int y = 0; y < sz.Height; y++)
                    for (int x = 0; x < sz.Width; x++)
                    {
                        sprites[x, y, z] = selectedType.getSprites(x, y, z, height)[0];
                    }
            return new AlphaBlendSpriteSet(sprites);
        }


        private void heightBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = ((heightBox.Value % 4) != 0);
        }

        private void heightBox_ValueChanged(object sender, System.EventArgs e)
        {
            updateAlphaSprites();
            updatePreview();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void onTypeChanged(object sender, System.EventArgs e)
        {
            base.onTypeChanged(sender, e);

            if (heightBox == null) return;	// during initialization
            // update the min/max of the height
            heightBox.Minimum = selectedType.minHeight * 4;
            heightBox.Maximum = selectedType.maxHeight * 4;
        }
    }
}

