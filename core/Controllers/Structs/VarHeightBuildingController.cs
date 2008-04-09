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
        /// <summary>
        /// 
        /// </summary>
        public VarHeightBuildingController()
            : base(PluginManager.varHeightBuildingsGroup)
        {
            InitializeComponent();
            buttonPlace.Top += 24;
            buttonRemove.Top += 24;
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        public override void OnClick(MapViewWindow view, Location loc, Point ab)
        {
            if (isPlacing)
            {
                if (!selectedType.CanBeBuilt(loc, height))
                {
                    MessageBox.Show("Can not build");
                    //! MessageBox.Show("設置できません");
                }
                else
                {
                    CompletionHandler handler = new CompletionHandler(selectedType, loc, height, true);
                    new ConstructionSite(loc, new EventHandler(handler.handle),
                        new Distance(selectedType.Size, height));
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
        public override bool IsSelectable(Location loc)
        {
            if (isPlacing)
            {
                // structures can be placed only on the ground
                return GroundDisambiguator.theInstance.IsSelectable(loc);
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
                contribution.Create(loc, height, owned);
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
            Size sz = selectedType.Size;
            ISprite[, ,] sprites = new ISprite[sz.Width, sz.Height, height];
            for (int z = 0; z < height; z++)
                for (int y = 0; y < sz.Height; y++)
                    for (int x = 0; x < sz.Width; x++)
                    {
                        sprites[x, y, z] = selectedType.GetSprites(x, y, z, height)[0];
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
            UpdatePreview();
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
            heightBox.Minimum = selectedType.MinHeight * 4;
            heightBox.Maximum = selectedType.MaxHeight * 4;
        }
    }
}

