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
            Picture picture = GetPicture(e);
            SpriteFactory spriteFactory = SpriteFactory.GetSpriteFactory(e);


            XmlElement pic = (XmlElement)XmlUtil.SelectSingleNode(e, "picture");
            int offset = int.Parse(pic.Attributes["offset"].Value);

            Point pt = new Point(0, 8);
            Size sz = new Size(32, 24);

            sprites = new ISprite[3];
            sprites[0] = spriteFactory.CreateSprite(picture, pt, new Point(offset, 0), sz);
            sprites[1] = spriteFactory.CreateSprite(picture, pt, new Point(offset, 24), sz);
            sprites[2] = spriteFactory.CreateSprite(picture, pt, new Point(offset, 48), sz);

        }

        /// <summary> Sprite of this land contribution. </summary>
        private readonly ISprite[] sprites;

        /// <summary>
        /// 
        /// </summary>
        public ISprite[] Sprites
        {
            get { return sprites; }
        } 

        /// <summary>
        /// Gets the land that should be used to fill (x,y) within [x1,y1]-[x2,y2] (inclusive).
        /// </summary>
        public override void Create(int x1, int y1, int x2, int y2, int z, bool owned)
        {
            for (int x = x1; x <= x2; x++)
            {
                for (int y = y1; y <= y2; y++)
                {
                    Location loc = new Location(x, y, z);
                    if (VinylHouseVoxel.canBeBuilt(loc))
                        new VinylHouseVoxel(loc, this,
                            GetSpriteIndex(x, y, x1, y1, x2, y2)).isOwned = owned;
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
        protected abstract int GetSpriteIndex(int x, int y, int x1, int y1, int x2, int y2);

        /// <summary>
        /// Creates the preview image of the land builder.
        /// </summary>
        public override PreviewDrawer CreatePreview(Size pixelSize)
        {
            PreviewDrawer drawer = new PreviewDrawer(pixelSize, new Size(3, 3), 0);

            for (int y = 0; y < 3; y++)
                for (int x = 2; x >= 0; x--)
                    drawer.Draw(sprites[GetSpriteIndex(x, y, 0, 2, 0, 2)], x, y);

            return drawer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public override IModalController CreateBuilder(IControllerSite site)
        {
            return new Logic(this, site);
        }

        private class Logic : RectSelectorController, IMapOverlay
        {
            private readonly VinylHouseBuilder contrib;

            public Logic(VinylHouseBuilder contrib, IControllerSite site)
                : base(site)
            {
                this.contrib = contrib;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="loc1"></param>
            /// <param name="loc2"></param>
            protected override void OnRectSelected(Location loc1, Location loc2)
            {
                contrib.Create(loc1, loc2, true);
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
                if (loc.z != CurrentLocation.z) return;

                if (Anchor != Unplaced && loc.inBetween(Anchor, CurrentLocation))
                {
                    Location loc1 = base.LocationNW;
                    Location loc2 = base.LocationSE;
                    contrib.Sprites[contrib.GetSpriteIndex(loc.x, loc.y, loc1.x, loc1.y, loc2.x, loc2.y)]
                        .DrawAlpha(canvas.Surface, pt);
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="view"></param>
            /// <param name="surface"></param>
            public void DrawAfter(QuarterViewDrawer view, DrawContext surface) { }
        }
    }
}
