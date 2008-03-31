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
using System.Runtime.Serialization;
using System.Xml;
using FreeTrain.Framework.Graphics;
using FreeTrain.Contributions.Land;
using FreeTrain.Contributions.Population;
using FreeTrain.Controllers;
using FreeTrain.Framework.Plugin;
using FreeTrain.Views;

namespace FreeTrain.World.Land.VinylHouse
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [CLSCompliant(false)]
    public abstract class VinylHouseBuilder : LandBuilderContribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public VinylHouseBuilder(XmlElement e)
            : base(e)
        {
            // pictures
            Picture picture = getPicture(e);
            SpriteFactory spriteFactory = SpriteFactory.getSpriteFactory(e);


            XmlElement pic = (XmlElement)XmlUtil.selectSingleNode(e, "picture");
            int offset = int.Parse(pic.Attributes["offset"].Value);

            Point pt = new Point(0, 8);
            Size sz = new Size(32, 24);

            sprites = new Sprite[3];
            sprites[0] = spriteFactory.createSprite(picture, pt, new Point(offset, 0), sz);
            sprites[1] = spriteFactory.createSprite(picture, pt, new Point(offset, 24), sz);
            sprites[2] = spriteFactory.createSprite(picture, pt, new Point(offset, 48), sz);

        }


        /// <summary> Sprite of this land contribution. </summary>
        [CLSCompliant(false)]
        public readonly Sprite[] sprites;



        /// <summary>
        /// Gets the land that should be used to fill (x,y) within [x1,y1]-[x2,y2] (inclusive).
        /// </summary>
        public override void create(int x1, int y1, int x2, int y2, int z, bool owned)
        {
            for (int x = x1; x <= x2; x++)
            {
                for (int y = y1; y <= y2; y++)
                {
                    Location loc = new Location(x, y, z);
                    if (VinylHouseVoxel.canBeBuilt(loc))
                        new VinylHouseVoxel(loc, this,
                            getSpriteIndex(x, y, x1, y1, x2, y2)).isOwned = owned;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        protected abstract int getSpriteIndex(int x, int y, int x1, int y1, int x2, int y2);



        /// <summary>
        /// Creates the preview image of the land builder.
        /// </summary>
        [CLSCompliant(false)]
        public override PreviewDrawer createPreview(Size pixelSize)
        {
            PreviewDrawer drawer = new PreviewDrawer(pixelSize, new Size(3, 3), 0);

            for (int y = 0; y < 3; y++)
                for (int x = 2; x >= 0; x--)
                    drawer.draw(sprites[getSpriteIndex(x, y, 0, 2, 0, 2)], x, y);

            return drawer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public override IModalController createBuilder(IControllerSite site)
        {
            return new Logic(this, site);
        }

        private class Logic : RectSelectorController, IMapOverlay
        {
            private readonly VinylHouseBuilder contrib;

            public Logic(VinylHouseBuilder _contrib, IControllerSite site)
                : base(site)
            {
                this.contrib = _contrib;
            }

            protected override void onRectSelected(Location loc1, Location loc2)
            {
                contrib.create(loc1, loc2, true);
            }

            public void DrawBefore(QuarterViewDrawer view, DrawContext surface) { }

            public void DrawVoxel(QuarterViewDrawer view, DrawContext canvas, Location loc, Point pt)
            {
                if (loc.z != currentLoc.z) return;

                if (anchor != UNPLACED && loc.inBetween(anchor, currentLoc))
                {
                    Location loc1 = base.location1;
                    Location loc2 = base.location2;
                    contrib.sprites[contrib.getSpriteIndex(loc.x, loc.y, loc1.x, loc1.y, loc2.x, loc2.y)]
                        .drawAlpha(canvas.Surface, pt);
                }
            }

            public void DrawAfter(QuarterViewDrawer view, DrawContext surface) { }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [CLSCompliant(false)]
    public class XVinylHouseBuilder : VinylHouseBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public XVinylHouseBuilder(XmlElement e) : base(e) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        protected override int getSpriteIndex(int x, int y, int x1, int y1, int x2, int y2)
        {
            if (x == x1) return 2;
            if (x == x2) return 0;
            return 1;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [CLSCompliant(false)]
    public class YVinylHouseBuilder : VinylHouseBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public YVinylHouseBuilder(XmlElement e) : base(e) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        protected override int getSpriteIndex(int x, int y, int x1, int y1, int x2, int y2)
        {
            if (y == y2) return 2;
            if (y == y1) return 0;
            return 1;
        }
    }
}
