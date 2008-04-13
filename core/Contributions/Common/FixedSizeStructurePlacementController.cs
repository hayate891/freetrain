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
using System.Drawing;
using System.Windows.Forms;
using FreeTrain.Controllers;
using FreeTrain.Framework;
using FreeTrain.Framework.Graphics;
using FreeTrain.Views;
using FreeTrain.World;
using FreeTrain.World.Structs;

namespace FreeTrain.Contributions.Common
{
    /// <summary>
    /// FixedSizeStructureController
    /// </summary>
    public class FixedSizeStructurePlacementController : CubeSelectorController, IMapOverlay
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly FixedSizeStructureContribution contrib;

        /// <summary>
        /// 
        /// </summary>
        protected FixedSizeStructureContribution Contrib
        {
            get { return contrib; }
        } 

        private readonly AlphaBlendSpriteSet alphaSprites;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contrib"></param>
        /// <param name="site"></param>
        public FixedSizeStructurePlacementController(FixedSizeStructureContribution contrib, IControllerSite site)
            : base(contrib.Size, site)
        {
            this.contrib = contrib;
            this.alphaSprites = new AlphaBlendSpriteSet(this.contrib.Sprites);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cube"></param>
        protected override void OnSelected(Cube cube)
        {
            if (contrib.CanBeBuilt(cube.Corner, ControlMode.Player))
            {
                MessageBox.Show("Can not build");
                //! MessageBox.Show("設置できません");
            }
            else
            {
                CompletionHandler handler = new CompletionHandler(contrib, cube.Corner, true);
                new ConstructionSite(cube.Corner, new EventHandler(handler.handle), contrib.Size);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnDetached()
        {
            alphaSprites.Dispose();
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
            if (CurrentCube.Contains(loc))
                alphaSprites.getSprite(loc - this.location).DrawAlpha(canvas.Surface, pt);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="surface"></param>
        public void DrawAfter(QuarterViewDrawer view, DrawContext surface) { }

        [Serializable]
        private class CompletionHandler
        {
            internal CompletionHandler(FixedSizeStructureContribution contribution, Location loc, bool owned)
            {
                this.contribution = contribution;
                this.loc = loc;
                this.owned = owned;
            }
            private readonly FixedSizeStructureContribution contribution;
            private readonly Location loc;
            private readonly bool owned;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="args"></param>
            public void handle(object sender, EventArgs args)
            {
                contribution.Create(loc, owned);
            }
        }
    }
}
