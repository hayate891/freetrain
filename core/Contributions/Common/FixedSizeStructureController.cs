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
        protected readonly FixedSizeStructureContribution contrib;

        private readonly AlphaBlendSpriteSet alphaSprites;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="_contrib"></param>
        /// <param name="_site"></param>
        public FixedSizeStructurePlacementController(FixedSizeStructureContribution _contrib, IControllerSite _site)
            : base(_contrib.size, _site)
        {
            this.contrib = _contrib;
            this.alphaSprites = new AlphaBlendSpriteSet(contrib.sprites);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cube"></param>
        protected override void onSelected(Cube cube)
        {
            if (contrib.canBeBuilt(cube.corner, ControlMode.Player))
            {
                MessageBox.Show("Can not build");
                //! MessageBox.Show("設置できません");
            }
            else
            {
                CompletionHandler handler = new CompletionHandler(contrib, cube.corner, true);
                new ConstructionSite(cube.corner, new EventHandler(handler.handle), contrib.size);
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
            if (currentCube.contains(loc))
                alphaSprites.getSprite(loc - this.location).drawAlpha(canvas.Surface, pt);
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
                contribution.create(loc, owned);
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class FixedSizeStructureRemovalController : CubeSelectorController
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly FixedSizeStructureContribution contrib;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_contrib"></param>
        /// <param name="_site"></param>
        public FixedSizeStructureRemovalController(FixedSizeStructureContribution _contrib, IControllerSite _site)
            : base(_contrib.size, _site)
        {
            this.contrib = _contrib;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cube"></param>
        protected override void onSelected(Cube cube)
        {
            PThreeDimStructure s = WorldDefinition.World.GetEntityAt(cube.corner) as PThreeDimStructure;
            if (s == null || s.type != contrib)
            {
                MessageBox.Show("Wrong type");
                //! MessageBox.Show("種類が違います");
                return;
            }
            s.remove();
        }
    }
}
