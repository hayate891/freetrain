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
using FreeTrain.Controllers;
using FreeTrain.Contributions.Population;
using FreeTrain.Framework.Plugin;
using FreeTrain.Views;
using FreeTrain.World;
using FreeTrain.World.Land;

namespace FreeTrain.Contributions.Land
{
    /// <summary>
    /// Places static chip as the land.
    /// </summary>
    [Serializable]
    public class StaticLandBuilder : LandBuilderContribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public StaticLandBuilder(XmlElement e)
            : base(e)
        {
            // picture
            XmlElement spr = (XmlElement)XmlUtil.SelectSingleNode(e, "sprite");
            sprite = PluginUtil.getSpriteLoader(spr).load2D(spr, 1, 1, 0)[0, 0];
        }

        /// <summary> Sprite of this land contribution. </summary>
        public readonly ISprite sprite;


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
                    if (LandVoxel.canBeBuilt(loc))
                        new StaticLandVoxel(loc, this).isOwned = owned;
                }
            }
        }



        /// <summary>
        /// Creates the preview image of the land builder.
        /// </summary>
        public override PreviewDrawer CreatePreview(Size pixelSize)
        {
            PreviewDrawer drawer = new PreviewDrawer(pixelSize, new Size(10, 10), 0);

            for (int y = 0; y < 10; y++)
                for (int x = 0; x < 10; x++)
                    drawer.draw(sprite, x, y);

            return drawer;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public override IModalController CreateBuilder(IControllerSite site)
        {
            return new DefaultControllerImpl(this, site, new DefaultControllerImpl.SpriteBuilder(getSprite));
        }

        private ISprite getSprite()
        {
            return sprite;
        }
    }
}
