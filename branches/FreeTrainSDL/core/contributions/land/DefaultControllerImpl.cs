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
using FreeTrain.Framework.Graphics;
using FreeTrain.Views;
using FreeTrain.Controllers;
using FreeTrain.world;

namespace FreeTrain.Contributions.Land
{
    /// <summary>
    /// ModalController implementation typical for most of the land builder contribution.
    /// This class is here just for the code reuse.
    /// </summary>
    public class DefaultControllerImpl : RectSelectorController, MapOverlay
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public delegate Sprite SpriteBuilder();

        private readonly LandBuilderContribution contrib;
        private readonly SpriteBuilder spriteBuilder;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_contrib"></param>
        /// <param name="_site"></param>
        /// <param name="_spriteBuilder"></param>
        public DefaultControllerImpl(LandBuilderContribution _contrib, IControllerSite _site,
            SpriteBuilder _spriteBuilder)
            : base(_site)
        {
            this.contrib = _contrib;
            this.spriteBuilder = _spriteBuilder;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc1"></param>
        /// <param name="loc2"></param>
        protected override void onRectSelected(Location loc1, Location loc2)
        {
            contrib.create(loc1, loc2, true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="surface"></param>
        public void drawBefore(QuarterViewDrawer view, DrawContextEx surface) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="canvas"></param>
        /// <param name="loc"></param>
        /// <param name="pt"></param>
        public void drawVoxel(QuarterViewDrawer view, DrawContextEx canvas, Location loc, Point pt)
        {
            if (loc.z != currentLoc.z) return;

            if (anchor != UNPLACED && loc.inBetween(anchor, currentLoc))
                spriteBuilder().drawAlpha(canvas.surface, pt);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="surface"></param>
        public void drawAfter(QuarterViewDrawer view, DrawContextEx surface) { }
    }
}
