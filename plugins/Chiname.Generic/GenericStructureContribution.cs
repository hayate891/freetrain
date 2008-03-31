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
using System.Diagnostics;
using System.Xml;
using System.Collections;
using FreeTrain.Framework;
using FreeTrain.Framework.Graphics;
using FreeTrain.Framework.Plugin;
using FreeTrain.Controllers;
using FreeTrain.Controllers.Structs;
using FreeTrain.Contributions;
using FreeTrain.Contributions.Common;
using FreeTrain.Contributions.Population;
using FreeTrain.Contributions.Structs;
using FreeTrain.Views;
using FreeTrain.Views.Map;
using FreeTrain.World;
using FreeTrain.World.Structs;

namespace FreeTrain.Framework.Plugin.Generic
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public enum SpriteTableType : int 
    { 
        /// <summary>
        /// 
        /// </summary>
        UNKNOWN, 
        /// <summary>
        /// 
        /// </summary>
        BASIC, 
        /// <summary>
        /// 
        /// </summary>
        VARHEIGHT 
    };

    /// <summary>
    /// Contribution of rail signal.
    /// </summary>
    [Serializable]
    [CLSCompliant(false)]
    public class GenericStructureContribution : StructureContribution, AbstractExStructure
    {
        /// <summary>
        /// 
        /// </summary>
        protected static readonly Char[] spliter = new Char[] { '|' };
        /// <summary> sub type of this structure. </summary>
        [CLSCompliant(false)]
        protected StructCategories _categories;
        /// <summary>
        /// 
        /// </summary>
        public StructCategories categories { get { return _categories; } }
        /// <summary> sub type of this structure. </summary>
        [CLSCompliant(false)]
        protected string _design;
        /// <summary>
        /// 
        /// </summary>
        public string design { get { return _design; } }
        /// <summary> unit price of this structure. equals to whole price for fixed height. </summary>
        protected int _unitPrice;
        /// <summary>
        /// 
        /// </summary>
        public int unitPrice { get { return _unitPrice; } }
        /// <summary>
        /// 
        /// </summary>
        public override int price { get { return _unitPrice; } }
        /// <summary>
        /// 
        /// </summary>
        protected int _areaPrice;
        /// <summary>
        /// 
        /// </summary>
        public override double pricePerArea { get { return _areaPrice; } }

        /// <summary> size of this structur. </summary>
        [CLSCompliant(false)]
        protected SIZE _size;
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public SIZE size { get { return _size; } }
        /// <summary> valid for variable height structure only. </summary>
        [CLSCompliant(false)]
        protected int _minHeight;
        /// <summary>
        /// 
        /// </summary>
        public int minHeight { get { return _minHeight; } }
        /// <summary> used as well as fixed height structure. </summary>
        protected int _maxHeight;
        /// <summary>
        /// 
        /// </summary>
        public int maxHeight { get { return _maxHeight; } }
        /// <summary> sprite table type of this structure. </summary>
        protected SpriteTableType stType = SpriteTableType.UNKNOWN;
        /// <summary>
        /// 
        /// </summary>
        public SpriteTableType patternType { get { return stType; } }

        internal Contribution[,] contribs;
        /// <summary>
        /// 
        /// </summary>
        protected int[] dirTable = new int[8];

        /// <summary> counts of color variations. </summary>
        protected int _colorMax;
        /// <summary>
        /// 
        /// </summary>
        public int colorVariations { get { return _colorMax; } }
        /// <summary> 
        /// counts of direction variations. 
        /// </summary>
        protected int _dirMax;
        /// <summary>
        /// 
        /// </summary>
        public int directionVariations { get { return _dirMax; } }

        /// <summary>
        /// 
        /// </summary>
        protected int _colorIndex = 0;
        /// <summary>
        /// 
        /// </summary>
        public int colorIndex
        {
            get { return _colorIndex; }
            set
            {
                _colorIndex = value;
                if (_colorIndex >= _colorMax)
                    _colorIndex = _colorMax - 1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected int _dirIndex = 0;
        /// <summary>
        /// 
        /// </summary>
        public int dirIndex
        {
            get { return _dirIndex; }
            set
            {
                _dirIndex = value;
                if (_dirIndex >= _dirMax)
                    _dirIndex = _dirMax - 1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public IEntityBuilder current
        {
            get { return (IEntityBuilder)contribs[_dirIndex, _colorIndex]; }
        }

        private static StructureGroup _group = null;

        static GenericStructureContribution()
        {
            // prepare category tree
            if (StructCategoryTree.theInstance == null)
                StructCategoryTree.loadDefaultTree();
        }

        // the constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public GenericStructureContribution(XmlElement e)
            : base(e)
        {
            loadPrimitiveParams(e);
            for (int i = 0; i < 8; i++)
                dirTable[i] = -1;
            loadSprites(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        protected override StructureGroup getGroup(string name)
        {
            if (_group == null)
                _group = new StructureGroup("GenericStructure");
            return _group;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void loadPrimitiveParams(XmlElement e)
        {
            XmlNode xn = e.SelectSingleNode("structure");
            if (xn != null)
                _categories = new StructCategories(xn, this.id);
            else
                _categories = new StructCategories();

            if (_categories.Count == 0)
            {
                StructCategory.Root.Entries.Add(this.id);
                _categories.Add(StructCategory.Root);
            }

            try
            {
                _design = e.SelectSingleNode("design").InnerText;
            }
            catch
            {
                //! _design = "標準";
                _design = "default";
            }

            _unitPrice = int.Parse(XmlUtil.selectSingleNode(e, "price").InnerText);
            _size = XmlUtil.parseSize(XmlUtil.selectSingleNode(e, "size").InnerText);
            _areaPrice = _unitPrice / Math.Max(1, size.x * size.y);

            _minHeight = 2;
            try
            {
                _maxHeight = int.Parse(e.SelectSingleNode("maxHeight").InnerText);
                try
                {
                    // if minHeight is not defined, use default.
                    _minHeight = int.Parse(e.SelectSingleNode("minHeight").InnerText);
                }
                catch { }
            }
            catch
            {
                // if maxHeight tag is nod find, height tag must be exist.
                _maxHeight = int.Parse(XmlUtil.selectSingleNode(e, "height").InnerText);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void loadSprites(XmlElement e)
        {
            IEnumerator ie = e.ChildNodes.GetEnumerator();
            ArrayList colors = new ArrayList();
            ArrayList spriteNodes = new ArrayList();
            while (ie.MoveNext())
            {
                XmlNode child = (XmlNode)ie.Current;
                if (child.Name.Equals("spriteType"))
                {
                    colors.Add(child);
                }
                else if (child.Name.Equals("colorVariation"))
                {
                    colors.Add(child);
                    //throw new NotSupportedException("prease use <spriteType> tag.");
                }
                else if (child.Name.Equals("picture") || child.Name.Equals("sprite"))
                {
                    switch (stType)
                    {
                        case SpriteTableType.UNKNOWN:
                            stType = SpriteTableType.BASIC;
                            break;
                        case SpriteTableType.BASIC:
                            break;
                        default:
                            throw new FormatException("<sprite> tag is not available together with <pictures> or <sprites> tags.");
                    }
                    spriteNodes.Add(child);
                }
                else if (child.Name.Equals("pictures") || child.Name.Equals("sprites"))
                {
                    switch (stType)
                    {
                        case SpriteTableType.UNKNOWN:
                            stType = SpriteTableType.VARHEIGHT;
                            break;
                        case SpriteTableType.VARHEIGHT:
                            break;
                        default:
                            throw new FormatException("<" + child.Name + "> tag is not available together with <sprite> tag.");
                    }
                    spriteNodes.Add(child);
                }
            }
            if (colors.Count == 0)
            {
                colors.Add(e.FirstChild.Clone());
            }
            _colorMax = colors.Count;
            _dirMax = spriteNodes.Count;
            contribs = new Contribution[_dirMax, _colorMax];
            int defaultDir = _dirMax;
            for (int i = 0; i < _colorMax; i++)
            {
                for (int j = 0; j < _dirMax; j++)
                {
                    e.Attributes["id"].Value = this.id + "-" + i + ":" + j;
                    XmlNode temp = ((XmlNode)spriteNodes[j]).Clone();
                    if (parseDirection(temp, j) == 0 && defaultDir > j)
                        defaultDir = j;
                    Contribution newContrib
                        = createPrimitiveContrib((XmlElement)temp, (XmlNode)colors[i], e);
                    PluginManager.theInstance.addContribution(newContrib);
                    contribs[j, i] = newContrib;
                }
            }

            // set unassigned direction table
            for (int j = 0; j < dirTable.Length; j++)
            {
                if (dirTable[j] == -1)
                    dirTable[j] = defaultDir;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="color"></param>
        /// <param name="contrib"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        protected virtual Contribution createPrimitiveContrib(XmlElement sprite, XmlNode color, XmlElement contrib)
        {
            bool opposite = (sprite.Attributes["opposite"] != null && sprite.Attributes["opposite"].Value.Equals("true"));
            Contribution newContrib;
            if (stType == SpriteTableType.VARHEIGHT)
            {
                foreach (XmlNode child in sprite.ChildNodes)
                    child.AppendChild(color.Clone());
                newContrib = new VarHeightBuildingContribution(this, sprite, contrib, opposite);
            }
            else
            {
                sprite.AppendChild(color.Clone());
                newContrib = new CommercialStructureContribution(this, sprite, contrib, opposite);
            }
            return newContrib;
        }

        /// <summary>
        /// parse direction information
        /// </summary>
        /// <param name="node">parent node contains 'direction' tags</param>
        /// <param name="refindex">first index of contribs array</param>
        protected int parseDirection(XmlNode node, int refindex)
        {
            int c = 0;
            foreach (XmlNode cn in node.ChildNodes)
            {
                if (cn.Name.Equals("direction"))
                {
                    string front = cn.Attributes["front"].Value;
                    string[] dirs = front.ToUpper().Split(spliter);
                    for (int i = 0; i < dirs.Length; i++)
                    {
                        c++;
                        if (dirs[i].Equals("ALL"))
                        {
                            dirTable[Direction.NORTH.index] = refindex;
                            dirTable[Direction.SOUTH.index] = refindex;
                            dirTable[Direction.EAST.index] = refindex;
                            dirTable[Direction.WEST.index] = refindex;
                        }
                        else if (dirs[i].Equals("SOUTH"))
                            dirTable[Direction.SOUTH.index] = refindex;
                        else if (dirs[i].Equals("EAST"))
                            dirTable[Direction.EAST.index] = refindex;
                        else if (dirs[i].Equals("WEST"))
                            dirTable[Direction.WEST.index] = refindex;
                        else if (dirs[i].Equals("NORTH"))
                            dirTable[Direction.NORTH.index] = refindex;
                    }
                }
            }
            return c;
        }

        //		protected SpriteFactory createFactory( XmlNode node, string name )
        //		{
        //			SpriteFactoryContribution contrib = (SpriteFactoryContribution)
        //				PluginManager.theInstance.getContribution( "spriteFactory:"+name );
        //			if(contrib==null)
        //				throw new FormatException("unable to locate spriteFactory:"+ name);
        //			return contrib.createSpriteFactory((XmlElement)node); 
        //		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public Contribution GetPrimitiveContrib(Direction dir, int color)
        {
            return contribs[dirTable[dir.index], color];
        }

        /// <summary>
        /// Creates a preview
        /// </summary>
        /// <param name="pixelSize"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public override PreviewDrawer createPreview(Size pixelSize)
        {
            return current.createPreview(pixelSize);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public override IModalController createBuilder(IControllerSite site)
        {
            return current.createBuilder(site);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public override IModalController createRemover(IControllerSite site)
        {
            return current.createRemover(site);
        }

    }
}
