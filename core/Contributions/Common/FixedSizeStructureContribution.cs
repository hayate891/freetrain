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
using System.Collections;
using System.Drawing;
using System.Xml;
using FreeTrain.Util;
using FreeTrain.Framework.Graphics;
using FreeTrain.Framework.Plugin;
using FreeTrain.Controllers;
using FreeTrain.World;
using FreeTrain.World.Structs;
using FreeTrain.Contributions.Common;

namespace FreeTrain.Contributions.Common
{
    /// <summary>
    /// StructureContribution for structures of a fixed size.
    /// </summary>
    [Serializable]
    public abstract class FixedSizeStructureContribution : StructureContribution, IPreviewWorldBuilder
    {
        /// <summary>
        /// Parses a structure contribution from a DOM node.
        /// </summary>
        /// <exception cref="XmlException">If the parsing fails</exception>
        protected FixedSizeStructureContribution(XmlElement e)
            : base(e)
        {
            this.Price = int.Parse(XmlUtil.SelectSingleNode(e, "price").InnerText);

            Size sz = XmlUtil.ParseSize(XmlUtil.SelectSingleNode(e, "size").InnerText);
            int height = int.Parse(XmlUtil.SelectSingleNode(e, "height").InnerText);

            this.size = new Distance(sz, height);
            this.PricePerArea = this.Price / Math.Max(1, size.x * size.y);
            XmlElement spr = (XmlElement)XmlUtil.SelectSingleNode(e, "sprite");
            sprites = PluginUtil.getSpriteLoader(spr).load3D(spr, size.x, size.y, height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="master"></param>
        /// <param name="pic"></param>
        /// <param name="main"></param>
        /// <param name="opposite"></param>
        public FixedSizeStructureContribution(IAbstractStructure master, XmlElement pic, XmlElement main, bool opposite)
            : base(main)
        {
            this.Price = master.UnitPrice;
            int height = master.MaxHeight;
            if (opposite)
                size = new Distance(master.Size.Height, master.Size.Width, height);
            else
                this.size = new Distance(master.Size, height);
            this.PricePerArea = this.Price / Math.Max(1, size.x * size.y);
            sprites = PluginUtil.getSpriteLoader(pic).load3D(pic, size.x, size.y, height);
        }

        /// <summary>
        /// Sprite set to draw this structure. Indexed as [x,y,z]
        /// and may contain null if there's no need to draw that voxel.
        /// </summary>
        private readonly ISprite[, ,] sprites;

        /// <summary>
        /// 
        /// </summary>
        public ISprite[, ,] Sprites
        {
            get { return sprites; }
        } 

        /// <summary> Size of this structure in voxel by voxel. </summary>
        private readonly Distance size;

        /// <summary>
        /// 
        /// </summary>
        public Distance Size
        {
            get { return size; }
        } 

        /// <summary>
        /// Creates a new instance of this structure type to the specified location.
        /// </summary>
        /// <param name="initiallyOwned">
        /// If the structure is a subsidiary, this flag controls whether the
        /// structure is initially owned or not. Otherwise this flag has no effect.
        /// </param>
        /// <param name="baseLoc"></param>
        public Structure Create(Location baseLoc, bool initiallyOwned)
        {
            return Create(new WorldLocator(WorldDefinition.World, baseLoc), initiallyOwned);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wloc"></param>
        /// <param name="initiallyOwned"></param>
        /// <returns></returns>
        public abstract Structure Create(WorldLocator wloc, bool initiallyOwned);

        //// this method differs from the create method in its return type.
        //// delegates are so inflexible that we have to do this kind of adjustment.
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="baseLoc"></param>
        //public void create2(Location baseLoc)
        //{
        //    Create(new WorldLocator(WorldDefinition.World, baseLoc), false);
        //}

        /// <summary>
        /// Returns true iff this structure can be built at the specified location.
        /// </summary>
        public abstract bool CanBeBuilt(Location baseLoc, ControlMode cm);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public ISprite GetSprite(Distance d)
        {
            return sprites[d.x, d.y, d.z];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pixelSize"></param>
        /// <returns></returns>
        public override PreviewDrawer CreatePreview(Size pixelSize)
        {
            PreviewDrawer drawer = new PreviewDrawer(pixelSize, size);
            drawer.drawCenter(sprites);
            return drawer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public override IModalController CreateBuilder(IControllerSite site)
        {
            return new FixedSizeStructurePlacementController(this, site);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public override IModalController CreateRemover(IControllerSite site)
        {
            return new FixedSizeStructureRemovalController(this, site);
        }

        #region IPreviewWorldBuilder o
        /// <summary>
        /// 
        /// </summary>
        /// <param name="minsizePixel"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public WorldDefinition CreatePreviewWorld(Size minsizePixel, IDictionary options)
        {
            WorldDefinition w = WorldDefinition.CreatePreviewWorld(minsizePixel, size);
            Location l = w.toXYZ((w.Size.x - size.x + size.y) / 2, w.Size.y - size.y - 2, 0);
            Create(new WorldLocator(w, l), false);
            return w;
        }

        #endregion
    }
}
