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
using System.Xml;
using FreeTrain.Contributions.Common;
using FreeTrain.Contributions.Population;
using FreeTrain.Framework;
using FreeTrain.Framework.Plugin;
using FreeTrain.Framework.Plugin.Graphics;
using FreeTrain.Framework.Graphics;
using FreeTrain.World;
using FreeTrain.World.Structs;
using FreeTrain.Controllers;
using FreeTrain.Contributions.Structs;

namespace FreeTrain.World.Structs.HalfVoxelStructure
{
    /// <summary>
    /// 
    /// </summary>
    public enum PlaceSide : int 
    { 
        /// <summary>
        /// 
        /// </summary>
        Fore, 
        /// <summary>
        /// 
        /// </summary>
        Back 
    };
    /// <summary>
    /// 
    /// </summary>
    public enum SideStored : int 
    { 
        /// <summary>
        /// 
        /// </summary>
        None, 
        /// <summary>
        /// 
        /// </summary>
        Fore, 
        /// <summary>
        /// 
        /// </summary>
        Back, 
        /// <summary>
        /// 
        /// </summary>
        Both 
    };
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [CLSCompliant(false)]
    public class HalfVoxelContribution : StructureContribution
    {
        /// <summary>
        /// 
        /// </summary>
        static protected readonly int hl_patterns = 6;
        /// <summary>
        /// 
        /// </summary>
        static protected StructureGroup _group = new StructureGroup("HalfVoxel");
        /// <summary>
        /// 
        /// </summary>
        static protected readonly Point[] offsets = new Point[]
		{
			new Point(0,-8), new Point(-8,-8),new Point(0,-8),new Point(-8,-8),
			new Point(-8,-4), new Point(0,-4),new Point(-8,-4),new Point(0,-4)
		};

        /// <summary>
        /// Parses a commercial structure contribution from a DOM node.
        /// </summary>
        /// <exception cref="XmlException">If the parsing fails</exception>
        public HalfVoxelContribution(XmlElement e)
            : base(e)
        {
            _price = int.Parse(XmlUtil.selectSingleNode(e, "price").InnerText);
            height = int.Parse(XmlUtil.selectSingleNode(e, "height").InnerText);
            subgroup = XmlUtil.selectSingleNode(e, "subgroup").InnerText;
            XmlElement spr = (XmlElement)XmlUtil.selectSingleNode(e, "sprite");
            XmlElement pic = (XmlElement)XmlUtil.selectSingleNode(spr, "picture");
            variation = spr.SelectSingleNode("map");
            if (variation != null)
            {
                String idc = variation.Attributes["to"].Value;
                colors = PluginManager.theInstance.getContribution(idc) as ColorLibrary;
                sprites = new SpriteSet[colors.size];
                for (int i = 0; i < colors.size; i++)
                    sprites[i] = new SpriteSet(8);
            }
            else
            {
                colors = ColorLibrary.NullLibrary;
                sprites = new SpriteSet[1];
                sprites[0] = new SpriteSet(8);
            }
            loadSprites(spr, pic);
            XmlElement hle = (XmlElement)spr.SelectSingleNode("highlight");
            if (hle != null)
            {
                hilights = new SpriteSet[hl_patterns];
                loadHighSprites(spr, hle);
            }
            else
                hilights = null;
        }

        #region helper methods used on reading XML
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ep"></param>
        protected virtual void loadSprites(XmlElement e, XmlElement ep)
        {
            Picture pic = getPicture(ep, null);
            XmlNode cn = e.FirstChild;
            while (cn != null)
            {
                if (cn.Name.Equals("pattern"))
                {
                    SideStored ss = parseSide(cn);
                    Direction d = parseDirection(cn);
                    Point orgn = XmlUtil.parsePoint(cn.Attributes["origin"].Value);
                    Point offF = getOffset(d, PlaceSide.Fore);
                    Point offB = getOffset(d, PlaceSide.Back);
                    Size sz = new Size(24, 8 + height * 16);
                    if (variation != null)
                    {
                        for (int i = 0; i < colors.size; i++)
                        {
                            Color c = colors[i];
                            string v = c.R.ToString() + "," + c.G.ToString() + "," + c.B.ToString();
                            variation.Attributes["to"].Value = v;
                            SpriteFactory factory = new HueTransformSpriteFactory(e);
                            if ((ss & SideStored.Fore) != 0)
                                sprites[i][d, PlaceSide.Fore] = factory.createSprite(pic, offF, orgn, sz);
                            if ((ss & SideStored.Back) != 0)
                                sprites[i][d, PlaceSide.Back] = factory.createSprite(pic, offB, orgn, sz);
                        }
                    }
                    else
                    {
                        SpriteFactory factory = new SimpleSpriteFactory();
                        if ((ss & SideStored.Fore) != 0)
                            sprites[0][d, PlaceSide.Fore] = factory.createSprite(pic, offF, orgn, sz);
                        if ((ss & SideStored.Back) != 0)
                            sprites[0][d, PlaceSide.Back] = factory.createSprite(pic, offB, orgn, sz);
                    }
                }
                cn = cn.NextSibling;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="hle"></param>
        protected virtual void loadHighSprites(XmlElement e, XmlElement hle)
        {
            Picture pic = getPicture(hle, "HL");
            if (pic == null || hle.Attributes["src"] == null)
                throw new FormatException("highlight picture not found.");
            string baseFileName = XmlUtil.resolve(hle, hle.Attributes["src"].Value).LocalPath;
            using (Bitmap bit = new Bitmap(baseFileName))
            {
                for (int i = 0; i < hl_patterns; i++)
                    hilights[i] = new SpriteSet(8);

                XmlNode cn = e.FirstChild;
                while (cn != null)
                {
                    if (cn.Name.Equals("pattern"))
                    {
                        SideStored ss = parseSide(cn);
                        Direction d = parseDirection(cn);
                        Point orgn = XmlUtil.parsePoint(cn.Attributes["origin"].Value);
                        Point offF = getOffset(d, PlaceSide.Fore);
                        Point offB = getOffset(d, PlaceSide.Back);
                        Size sz = new Size(24, 8 + height * 16);

                        // create highlight patterns
                        XmlNode hlp = cn.SelectSingleNode("highlight");
                        if (hlp != null)
                        {
                            HueShiftSpriteFactory factory = new HueShiftSpriteFactory(hl_patterns);
                            if ((ss & SideStored.Fore) != 0)
                            {
                                Sprite[] arr = factory.createSprites(bit, pic, offF, orgn, sz);
                                for (int i = 0; i < hl_patterns; i++)
                                    hilights[i][d, PlaceSide.Fore] = arr[i];
                            }
                            if ((ss & SideStored.Back) != 0)
                            {
                                Sprite[] arr = factory.createSprites(bit, pic, offB, orgn, sz);
                                for (int i = 0; i < hl_patterns; i++)
                                    hilights[i][d, PlaceSide.Back] = arr[i];
                            }
                        }
                    }//if(cn.Name.Equals("pattern"))
                    cn = cn.NextSibling;
                }//while
            }//using
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        protected Point getOffset(Direction d, PlaceSide s)
        {
            Point o = offsets[d.index / 2 + (int)s * 4];
            return new Point(o.X, o.Y + height * 16);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        protected SideStored parseSide(XmlNode n)
        {
            String s = n.Attributes["side"].Value;
            if (s == null || s.Equals("either"))
                return SideStored.Both;
            if (s.Equals("fore"))
                return SideStored.Fore;
            if (s.Equals("back"))
                return SideStored.Back;
            return SideStored.None;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        protected Direction parseDirection(XmlNode n)
        {
            String s = n.Attributes["direction"].Value;
            if (s == null)
                throw new FormatException("missing direction attribute.");
            if (s.Equals("north"))
                return Direction.NORTH;
            if (s.Equals("south"))
                return Direction.SOUTH;
            if (s.Equals("west"))
                return Direction.WEST;
            if (s.Equals("east"))
                return Direction.EAST;
            throw new FormatException("invalid direction attribute.");
            //return null;
        }

        internal static Picture getPicture(XmlElement pic, string suffix)
        {
            //XmlElement pic = (XmlElement)XmlUtil.selectSingleNode(sprite,suffix);			
            XmlAttribute r = pic.Attributes["ref"];
            if (r != null)
                // reference to externally defined pictures.
                return PictureManager.get(r.Value);

            // otherwise look for local picture definition
            XmlAttribute s = pic.Attributes["src"];
            if (s == null)
                return null;
            if (suffix != null)
                return new Picture(pic,
                    pic.SelectSingleNode("ancestor-or-self::contribution/@id").InnerText + "#" + suffix);
            else
                return new Picture(pic,
                    pic.SelectSingleNode("ancestor-or-self::contribution/@id").InnerText);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int getHighlihtPatternCount()
        {
            if (hilights == null) return 1;
            else return hl_patterns;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="s"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public Sprite getSprite(Direction d, PlaceSide s, int col)
        {
            return sprites[col][d, s];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="s"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public Sprite getHighLightSprite(Direction d, PlaceSide s, int col)
        {
            if (hilights != null)
                return hilights[col][d, s];
            else
                return null;
        }


        internal SpriteSet[] sprites;
        internal SpriteSet[] hilights;
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        protected readonly int _price;
        /// <summary>
        /// 
        /// </summary>
        public override int price { get { return _price; } }
        /// <summary>
        /// 
        /// </summary>
        public override double pricePerArea { get { return _price << 1; } }

        /// <summary>
        /// 
        /// </summary>
        public readonly int height;

        /// <summary>
        /// 
        /// </summary>
        [NonSerialized]
        public readonly string subgroup;
        /// <summary>
        /// 
        /// 
        /// </summary>
        [NonSerialized]
        [CLSCompliant(false)]
        public readonly ColorLibrary colors;
        /// <summary>
        /// 
        /// </summary>
        [NonSerialized]
        protected readonly XmlNode variation;
        /// <summary>
        /// 
        /// </summary>
        [NonSerialized]
        [CLSCompliant(false)]
        protected int _currentCol;
        /// <summary>
        /// 
        /// </summary>
        public int currentColor
        {
            get { return _currentCol; }
            set
            {
                if (value >= 0 && value < colors.size)
                    _currentCol = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [NonSerialized]
        [CLSCompliant(false)]
        protected int _currentHLIdx;
        /// <summary>
        /// 
        /// </summary>
        public int currentHighlight
        {
            get { return _currentHLIdx; }
            set
            {
                if (value >= 0 && value < hl_patterns)
                    _currentHLIdx = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        protected override StructureGroup getGroup(string name)
        {
            return _group;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public override ModalController createBuilder(IControllerSite site)
        {
            return new HVControllerImpl(this, site, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public override ModalController createRemover(IControllerSite site)
        {
            return new HVControllerImpl(this, site, true);
        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="baseLoc"></param>
        /// <param name="front"></param>
        /// <param name="side"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public Structure create(Location baseLoc, Direction front, PlaceSide side)
        {
            ContributionReference reffer = new ContributionReference(this, currentColor, currentHighlight, front, side);
            HalfDividedVoxel v = WorldDefinition.world[baseLoc] as HalfDividedVoxel;
            if (v == null)
                return new HVStructure(reffer, baseLoc);
            else
            {
                if (!v.owner.add(reffer))
                {
                    MainWindow.showError("Not enough space or no fit");
                }
                
                return v.owner;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseLoc"></param>
        /// <param name="front"></param>
        /// <param name="side"></param>
        [CLSCompliant(false)]
        public void destroy(Location baseLoc, Direction front, PlaceSide side)
        {
            HalfDividedVoxel v = WorldDefinition.world[baseLoc] as HalfDividedVoxel;
            if (v != null)
                v.owner.remove(side);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseLoc"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static bool canBeBuilt(Location baseLoc)
        {
            Voxel v = WorldDefinition.world[baseLoc];
            if (v != null)
            {
                HalfDividedVoxel hv = v as HalfDividedVoxel;
                if (hv != null)
                {
                    return hv.hasSpace;
                }
                else
                    return false;
            }
            else
                return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pixelSize"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public override PreviewDrawer createPreview(Size pixelSize)
        {
            PreviewDrawer drawer = new PreviewDrawer(pixelSize, new Size(7, 1), 1);
            drawer.draw(sprites[currentColor][Direction.WEST, PlaceSide.Fore], 3, 1);
            drawer.draw(sprites[currentColor][Direction.EAST, PlaceSide.Back], 2, 0);
            if (hilights != null)
            {
                drawer.draw(hilights[currentHighlight][Direction.WEST, PlaceSide.Fore], 3, 1);
                drawer.draw(hilights[currentHighlight][Direction.EAST, PlaceSide.Back], 2, 0);
            }
            return drawer;
        }

    }
    #region ContributionReference
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ContributionReference
    {
        private readonly HalfVoxelContribution contrib;
        private readonly int patternIdx;
        private readonly int colorIdx;
        private readonly int hilightIdx;
        /// <summary>
        /// 
        /// </summary>
        public readonly PlaceSide placeSide;
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public readonly Direction frontface;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hvc"></param>
        /// <param name="color"></param>
        /// <param name="hilight"></param>
        /// <param name="front"></param>
        /// <param name="side"></param>
        [CLSCompliant(false)]
        public ContributionReference(HalfVoxelContribution hvc, int color, int hilight, Direction front, PlaceSide side)
        {
            this.contrib = hvc;
            this.colorIdx = color;
            this.hilightIdx = hilight;
            this.placeSide = side;
            this.frontface = front;
            this.patternIdx = SpriteSet.getIndexOf(front, side);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [CLSCompliant(false)]
        public virtual Sprite getSprite()
        {
            return contrib.sprites[colorIdx][patternIdx];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [CLSCompliant(false)]
        public virtual Sprite getHighlightSprite()
        {
            if (contrib.hilights != null)
                return contrib.hilights[hilightIdx][patternIdx];
            else
                return null;
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual int height
        {
            get { return contrib.height; }
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual int price
        {
            get { return contrib.price; }
        }
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public virtual BasePopulation population
        {
            get { return contrib.population; }
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual string name
        {
            get { return contrib.subgroup; }
        }
    }

    internal class EmptyReference : ContributionReference
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hvc"></param>
        /// <param name="color"></param>
        /// <param name="hilight"></param>
        /// <param name="front"></param>
        /// <param name="side"></param>
        public EmptyReference(HalfVoxelContribution hvc, int color, int hilight, Direction front, PlaceSide side)
            : base(null, -1, -1, front, side) { }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Sprite getSprite() { return null; }

        /// <summary>
        /// 
        /// </summary>
        public override int height { get { return 0; } }
        /// <summary>
        /// 
        /// </summary>
        public override int price { get { return 0; } }
        /// <summary>
        /// 
        /// </summary>
        public override BasePopulation population { get { return null; } }
    }
    #endregion
    #region SpriteSet
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class SpriteSet
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        public SpriteSet(int size)
        {
            sprites = new Sprite[size];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="org"></param>
        /// <param name="variation"></param>
        public SpriteSet(SpriteSet org, Color variation)
        {
        }

        static internal int getIndexOf(Direction d, PlaceSide s)
        {
            return d.index / 2 + (int)s * 4;
        }

        internal Sprite this[int idx]
        {
            get { return sprites[idx]; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public Sprite this[Direction d, PlaceSide s]
        {
            get
            {
                return sprites[getIndexOf(d, s)];
            }
            set
            {
                sprites[getIndexOf(d, s)] = value;
            }
        }

        private Sprite[] sprites;
    }
    #endregion
}
