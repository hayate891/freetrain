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
using System.Xml;
using FreeTrain.Framework.Graphics;
using FreeTrain.World;
using FreeTrain.World.Terrain;
using FreeTrain.Contributions.Common;
using FreeTrain.Controllers;
using FreeTrain.Framework;
using FreeTrain.Framework.Plugin;

namespace FreeTrain.Contributions.Land
{
    /// <summary>
    /// Plug-in that places land voxels.
    /// 
    /// This contribution allows the tiling algorithm to be customized.
    /// </summary>
    [Serializable]
    public abstract class LandBuilderContribution : StructureContribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected LandBuilderContribution(XmlElement e)
            : base(e)
        {
            XmlNode gridNode = e.SelectSingleNode("grid");
            if (gridNode == null)
                _grid = new Size(1, 1);
            else
                _grid = XmlUtil.ParseSize(gridNode.InnerText);

            _price = int.Parse(XmlUtil.SelectSingleNode(e, "price").InnerText);
        }

        private readonly Size _grid;
        /// <summary>
        /// 
        /// </summary>
        public Size Grid { get { return _grid; } }

        /// <summary> Price of the land per voxel. </summary>
        [CLSCompliant(false)]
        protected readonly int _price;
        /// <summary>
        /// 
        /// </summary>
        public override int Price { get { return _price; } }
        /// <summary>
        /// 
        /// </summary>
        public override double PricePerArea { get { return _price; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override StructureGroup getGroup(string name)
        {
            return (StructureGroup)PluginManager.theInstance.landBuilderGroup[name];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="z"></param>
        /// <param name="owned"></param>
        public abstract void create(int x1, int y1, int x2, int y2, int z, bool owned);

        /// <summary>
        /// Fills the specified region with lands.
        /// </summary>
        public void create(Location loc1, Location loc2, bool owned)
        {
            Debug.Assert(loc1.z == loc2.z);
            int z = loc1.z;
            int minx, maxx;
            int miny, maxy;
            int wx, wy;
            if (loc1.x > loc2.x)
            {
                wx = Math.Max(loc1.x - loc2.x, _grid.Width - 1);
                maxx = loc1.x;
                minx = maxx - wx + wx % _grid.Width;
            }
            else
            {
                wx = Math.Max(loc2.x - loc1.x, _grid.Width - 1);
                minx = loc1.x;
                maxx = minx + wx - wx % _grid.Width;
            }
            if (loc1.y > loc2.y)
            {
                wy = Math.Max(loc1.y - loc2.y, _grid.Height - 1);
                maxy = loc1.y;
                miny = maxy - wy + wy % _grid.Height;
            }
            else
            {
                wy = Math.Max(loc2.y - loc1.y, _grid.Height - 1);
                miny = loc1.y;
                maxy = miny + wy - wy % _grid.Height;
            }

            create(minx, miny, maxx, maxy, z, owned);
        }

        /// <summary> Creates a single patch. </summary>
        public void create(Location loc, bool owned)
        {
            create(loc, loc, owned);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public override IModalController CreateRemover(IControllerSite site)
        {
            return new DefaultControllerImpl(this, site,
                new DefaultControllerImpl.SpriteBuilder(getSprite));
        }

        private static ISprite getSprite()
        {
            return ResourceUtil.removerChip;
            //return ResourceUtil.emptyChip;
        }
    }
}
