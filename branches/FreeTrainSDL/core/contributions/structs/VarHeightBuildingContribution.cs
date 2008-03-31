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
using System.Diagnostics;
using System.Drawing;
using System.Xml;
using FreeTrain.Util;
using FreeTrain.Controllers;
using FreeTrain.Contributions.Common;
using FreeTrain.Framework.Graphics;
using FreeTrain.Framework.Plugin;
using FreeTrain.World;
using FreeTrain.World.Structs;

namespace FreeTrain.Contributions.Structs
{
    /// <summary>
    /// Building of a variable height.
    /// </summary>
    [Serializable]
    public class VarHeightBuildingContribution : StructureContribution, IPreviewWorldBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public VarHeightBuildingContribution(XmlElement e)
            : base(e)
        {
            _price = int.Parse(XmlUtil.selectSingleNode(e, "price").InnerText);

            size = XmlUtil.parseSize(XmlUtil.selectSingleNode(e, "size").InnerText);
            _ppa = _price / Math.Max(1, size.x * size.y);
            minHeight = int.Parse(XmlUtil.selectSingleNode(e, "minHeight").InnerText);
            maxHeight = int.Parse(XmlUtil.selectSingleNode(e, "maxHeight").InnerText);

            XmlElement pics = (XmlElement)XmlUtil.selectSingleNode(e, "pictures");

            tops = loadSpriteSets(pics.SelectNodes("top"));
            bottoms = loadSpriteSets(pics.SelectNodes("bottom"));

            XmlElement m = (XmlElement)XmlUtil.selectSingleNode(pics, "middle");
            middle = PluginUtil.getSpriteLoader(m).load2D(m, size, 16);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="master"></param>
        /// <param name="pic"></param>
        /// <param name="main"></param>
        /// <param name="opposite"></param>
        public VarHeightBuildingContribution(AbstractExStructure master, XmlElement pic, XmlElement main, bool opposite)
            : base(main)
        {
            _price = master.unitPrice;
            if (opposite)
                size = new SIZE(master.size.y, master.size.x);
            else
                size = master.size;
            _ppa = _price / Math.Max(1, size.x * size.y);
            minHeight = master.minHeight;
            maxHeight = master.maxHeight;


            tops = loadSpriteSets(pic.SelectNodes("top"));
            bottoms = loadSpriteSets(pic.SelectNodes("bottom"));

            XmlElement m = (XmlElement)XmlUtil.selectSingleNode(pic, "middle");
            XmlAttribute a = m.Attributes["overlay"];
            if (a != null && a.InnerText.Equals("true"))
                overlay = true;
            middle = PluginUtil.getSpriteLoader(m).load2D(m, size, size.x * 8);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override StructureGroup getGroup(string name)
        {
            return PluginManager.theInstance.varHeightBuildingsGroup[name];
        }

        private Sprite[][,] loadSpriteSets(XmlNodeList list)
        {
            Sprite[][,] sprites = new Sprite[list.Count][,];

            int idx = 0;
            foreach (XmlElement e in list)
            {
                sprites[idx++] = PluginUtil.getSpriteLoader(e).load2D(e, size, size.x * 8);
            }
            return sprites;
        }

        /// <summary>Price of this structure per height.</summary>
        [CLSCompliant(false)]
        protected readonly int _price;
        /// <summary>
        /// 
        /// </summary>
        public override int price { get { return _price; } }
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        protected readonly double _ppa;
        /// <summary>
        /// 
        /// </summary>
        public override double pricePerArea { get { return _price; } }

        /// <summary>Sprite sets.</summary>
        private readonly Sprite[][,] tops, bottoms;
        private readonly Sprite[,] middle;
        private bool overlay = false;

        /// <summary> Sprite to draw the structure </summary>
        public Sprite[] getSprites(int x, int y, int z, int height)
        {
            if (z >= height - tops.Length)
            {
                if (overlay && z == bottoms.Length - 1)
                    return new Sprite[] { bottoms[z][x, y], tops[height - z - 1][x, y] };
                else
                    return new Sprite[] { tops[height - z - 1][x, y] };
            }
            if (z < bottoms.Length)
            {
                if (overlay && z == bottoms.Length - 1)
                    return new Sprite[] { bottoms[z][x, y], middle[x, y] };
                else
                    return new Sprite[] { bottoms[z][x, y] };
            }
            return new Sprite[] { middle[x, y] };
        }

        /// <summary> Size of the basement of this structure in voxel by voxel. </summary>
        public readonly SIZE size;

        /// <summary> Range of the possible height of the structure in voxel unit. </summary>
        public readonly int minHeight, maxHeight;



        /// <summary>
        /// Creates a new instance of this structure type to the specified location.
        /// </summary>
        public Structure create(WorldLocator wLoc, int height, bool initiallyOwned)
        {
            return new VarHeightBuilding(this, wLoc, height, initiallyOwned);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseLoc"></param>
        /// <param name="height"></param>
        /// <param name="initiallyOwned"></param>
        /// <returns></returns>
        public Structure create(Location baseLoc, int height, bool initiallyOwned)
        {
            Debug.Assert(canBeBuilt(baseLoc, height));
            return create(new WorldLocator(WorldDefinition.World, baseLoc), height, initiallyOwned);
        }

        /// <summary>
        /// Returns true iff this structure can be built at the specified location.
        /// </summary>
        public bool canBeBuilt(Location baseLoc, int height)
        {
            for (int z = 0; z < height; z++)
                for (int y = 0; y < size.y; y++)
                    for (int x = 0; x < size.x; x++)
                        if (WorldDefinition.World[baseLoc.x + x, baseLoc.y + y, baseLoc.z + z] != null)
                            return false;

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pixelSize"></param>
        /// <returns></returns>
        public override PreviewDrawer createPreview(Size pixelSize)
        {
            PreviewDrawer drawer = new PreviewDrawer(pixelSize, size, tops.Length + bottoms.Length + 1/*middle*/ );

            int z = 0;
            for (int i = bottoms.Length - 1; i >= 0; i--)
                drawer.draw(bottoms[i], 0, 0, z++);
            if (overlay)
                drawer.draw(middle, 0, 0, z - 1);
            drawer.draw(middle, 0, 0, z++);
            for (int i = tops.Length - 1; i >= 0; i--)
                drawer.draw(tops[i], 0, 0, z++);

            return drawer;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pixelSize"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public PreviewDrawer createPreview(Size pixelSize, int height)
        {
            PreviewDrawer drawer = new PreviewDrawer(pixelSize, size, maxHeight/*middle*/ );
            int mh = height - 2;
            int z = 0;
            for (int i = bottoms.Length - 1; i >= 0; i--)
                drawer.draw(bottoms[i], 0, 0, z++);
            if (overlay)
            {
                z--;
                mh++;
            }
            for (int i = 0; i < mh; i++)
                drawer.draw(middle, 0, 0, z++);
            for (int i = tops.Length - 1; i >= 0; i--)
                drawer.draw(tops[i], 0, 0, z++);

            return drawer;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public override IModalController createBuilder(IControllerSite site)
        {
            // TODO
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public override IModalController createRemover(IControllerSite site)
        {
            // TODO
            throw new NotImplementedException();
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
            Distance d = new Distance(size.x * 2 + 1, size.y * 2 + 1, maxHeight);
            WorldDefinition w = WorldDefinition.CreatePreviewWorld(minsizePixel, d);
            int v = w.Size.y - size.y - 2;
            Location l = w.toXYZ((w.Size.x - size.x - size.y - 1) / 2, v, 0);
            create(new WorldLocator(w, l), maxHeight, false);
            l = w.toXYZ((w.Size.x) / 2, v, 0);
            create(new WorldLocator(w, l), minHeight, false);
            return w;
        }

        #endregion
    }
}
