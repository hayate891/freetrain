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
        Unknown,
        /// <summary>
        /// 
        /// </summary>
        Basic,
        /// <summary>
        /// 
        /// </summary>
        VarHeight
    };

    /// <summary>
    /// Contribution of rail signal.
    /// </summary>
    [Serializable]
    public class GenericStructureContribution : StructureContribution, IAbstractStructure
    {
        /// <summary>
        /// 
        /// </summary>
        private static readonly Char[] splitter = new Char[] { '|' };

        /// <summary>
        /// 
        /// </summary>
        protected static Char[] Splitter
        {
            get { return GenericStructureContribution.splitter; }
        } 

        /// <summary> sub type of this structure. </summary>
        private StructCategories categories;
        /// <summary>
        /// 
        /// </summary>
        public StructCategories Categories 
        { 
            get 
            { 
                return categories; 
            }
            set
            {
                categories = value;
            }
        }
        /// <summary> sub type of this structure. </summary>
        private string design;
        /// <summary>
        /// 
        /// </summary>
        public string Design 
        { 
            get 
        { 
            return design; 
            }
            set
            {
                design = value;
            }
        }
        /// <summary> unit price of this structure. equals to whole price for fixed height. </summary>
        int unitPrice;
        /// <summary>
        /// 
        /// </summary>
        public int UnitPrice 
        { 
            get 
            { 
                return unitPrice; 
            }
            set
            {
                unitPrice = value;
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //int pricePerArea;
        ///// <summary>
        ///// 
        ///// </summary>
        //public override double PricePerArea 
        //{ 
        //    get 
        //    { 
        //        return pricePerArea; 
        //    }
        //    set
        //    {
        //        pricePerArea = value;
        //    }
        //}

        private Size size;

        /// <summary>
        /// 
        /// </summary>
        public Size Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
            }
        }

        /// <summary> valid for variable height structure only. </summary>
        int minHeight;
        /// <summary>
        /// 
        /// </summary>
        public int MinHeight 
        { 
            get 
            { 
                return minHeight; 
            }
            set
            {
                minHeight = value;
            }
        }
        /// <summary> used as well as fixed height structure. </summary>
        private int maxHeight;
        /// <summary>
        /// 
        /// </summary>
        public int MaxHeight 
        { 
            get 
            { 
                return maxHeight; 
            }
            set
            {
                maxHeight = value;
            }
        }

        /// <summary> sprite table type of this structure. </summary>
        SpriteTableType stType = SpriteTableType.Unknown;
        /// <summary>
        /// 
        /// </summary>
        public SpriteTableType PatternType { get { return stType; } }

        internal Contribution[,] contribs;
        /// <summary>
        /// 
        /// </summary>
        private int[] dirTable = new int[8];

        /// <summary>
        /// 
        /// </summary>
        protected int[] DirTable
        {
            get { return dirTable; }
            set { dirTable = value; }
        }

        /// <summary> counts of color variations. </summary>
        private int _colorMax;

        /// <summary>
        /// 
        /// </summary>
        protected int ColorMax
        {
            get { return _colorMax; }
            set { _colorMax = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ColorVariations { get { return _colorMax; } }
        /// <summary> 
        /// counts of direction variations. 
        /// </summary>
        private int _dirMax;

        /// <summary>
        /// 
        /// </summary>
        protected int DirMax
        {
            get { return _dirMax; }
            set { _dirMax = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int DirectionVariations { get { return _dirMax; } }

        /// <summary>
        /// 
        /// </summary>
        private int _colorIndex = 0;

        /// <summary>
        /// 
        /// </summary>
        public int ColorIndex
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
        int _dirIndex = 0;
        /// <summary>
        /// 
        /// </summary>
        public int DirIndex
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
        public IEntityBuilder Current
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
            return Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override StructureGroup GetGroup(string name)
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
                categories = new StructCategories(xn, this.Id);
            else
                categories = new StructCategories();

            if (categories.Count == 0)
            {
                StructCategory.Root.Entries.Add(this.Id);
                categories.Add(StructCategory.Root);
            }

            try
            {
                design = e.SelectSingleNode("design").InnerText;
            }
            catch
            {
                //! _design = "標準";
                design = "default";
            }

            unitPrice = int.Parse(XmlUtil.SelectSingleNode(e, "price").InnerText);
            size = XmlUtil.ParseSize(XmlUtil.SelectSingleNode(e, "size").InnerText);
            PricePerArea = unitPrice / Math.Max(1, size.Width * size.Height);

            minHeight = 2;
            try
            {
                maxHeight = int.Parse(e.SelectSingleNode("maxHeight").InnerText);
                try
                {
                    // if minHeight is not defined, use default.
                    minHeight = int.Parse(e.SelectSingleNode("minHeight").InnerText);
                }
                catch { }
            }
            catch
            {
                // if maxHeight tag is nod find, height tag must be exist.
                maxHeight = int.Parse(XmlUtil.SelectSingleNode(e, "height").InnerText);
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
                        case SpriteTableType.Unknown:
                            stType = SpriteTableType.Basic;
                            break;
                        case SpriteTableType.Basic:
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
                        case SpriteTableType.Unknown:
                            stType = SpriteTableType.VarHeight;
                            break;
                        case SpriteTableType.VarHeight:
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
                    e.Attributes["id"].Value = this.Id + "-" + i + ":" + j;
                    XmlNode temp = ((XmlNode)spriteNodes[j]).Clone();
                    if (parseDirection(temp, j) == 0 && defaultDir > j)
                        defaultDir = j;
                    Contribution newContrib
                        = CreatePrimitiveContrib((XmlElement)temp, (XmlNode)colors[i], e);
                    PluginManager.AddContribution(newContrib);
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
        protected virtual Contribution CreatePrimitiveContrib(XmlElement sprite, XmlNode color, XmlElement contrib)
        {
            bool opposite = (sprite.Attributes["opposite"] != null && sprite.Attributes["opposite"].Value.Equals("true"));
            Contribution newContrib;
            if (stType == SpriteTableType.VarHeight)
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
                    string[] dirs = front.ToUpper().Split(splitter);
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
        //				PluginManager.getContribution( "spriteFactory:"+name );
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
        public Contribution GetPrimitiveContrib(Direction dir, int color)
        {
            return contribs[dirTable[dir.index], color];
        }

        /// <summary>
        /// Creates a preview
        /// </summary>
        /// <param name="pixelSize"></param>
        /// <returns></returns>
        public override PreviewDrawer CreatePreview(Size pixelSize)
        {
            return Current.CreatePreview(pixelSize);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public override IModalController CreateBuilder(IControllerSite site)
        {
            return Current.CreateBuilder(site);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public override IModalController CreateRemover(IControllerSite site)
        {
            return Current.CreateRemover(site);
        }

    }
}
