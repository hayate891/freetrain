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
using System.Collections;
using System.Windows.Forms;
using System.Xml;
using FreeTrain.Controllers;
using FreeTrain.Contributions.Rail;
using FreeTrain.Framework.Plugin;
using FreeTrain.Framework.Graphics;
using FreeTrain.World;
using FreeTrain.World.Rail;
using FreeTrain.Contributions.Common;

namespace FreeTrain.World.Road.DummyCars
{
    /// <summary>
    /// 
    /// 
    /// </summary>
    [Serializable]
    [CLSCompliant(false)]
    public class DummyCarContribution : RailAccessoryContribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public DummyCarContribution(XmlElement e)
            : base(e)
        {
            // pictures
            XmlElement sprite = (XmlElement)XmlUtil.selectSingleNode(e, "sprite");
            Picture picture = getPicture(sprite);
            Point origin = XmlUtil.parsePoint(sprite.Attributes["origin"].Value);
            int offset = int.Parse(sprite.Attributes["offset"].Value);
            Size sz = new Size(32, 16 + offset);
            Point sprOrigin0 = new Point(origin.X, origin.Y);
            Point sprOrigin1 = new Point(32 + origin.X, origin.Y);

            XmlElement splist = (XmlElement)XmlUtil.selectSingleNode(sprite, "variations");
            colorVariations = 0;
            IEnumerator ienum = splist.ChildNodes.GetEnumerator();
            while (ienum.MoveNext()) colorVariations++;
            sprites = new ISprite[colorVariations, 2];
            currentColor = 0;
            ienum.Reset();
            colorVariations = 0;
            while (ienum.MoveNext())
            {
                XmlNode child = (XmlNode)ienum.Current;
                if (child.Name == "colorVariation")
                {
                    SpriteFactory factory = SpriteFactory.getSpriteFactory(child);
                    sprites[colorVariations, 0] = factory.createSprite(picture, new Point(0, offset), sprOrigin0, sz);
                    sprites[colorVariations, 1] = factory.createSprite(picture, new Point(0, offset), sprOrigin1, sz);
                    colorVariations++;
                }
            }
        }

        /// <summary>
        /// Sprites. Dimensinos are [x,y] where
        /// 
        /// x is an index of variations.
        /// 
        /// y=0 if a pole is for E/W direction.
        /// y=1 if a pole is for N/S direction
        /// 
        /// </summary>
        internal readonly ISprite[,] sprites = new ISprite[10, 2];

        /// <summary>
        /// 
        /// </summary>
        public readonly int colorVariations;
        /// <summary>
        /// 
        /// </summary>
        public int currentColor;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [CLSCompliant(false)]
        public ISprite getSprites()
        {
            if (currentColor >= colorVariations) currentColor = 0;
            return sprites[currentColor, 0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pixelSize"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public override PreviewDrawer createPreview(Size pixelSize)
        {
            PreviewDrawer drawer = new PreviewDrawer(pixelSize, new Size(10, 1), 0);
            for (int x = 9; x >= 0; x--)
            {
                if (x == 5) drawer.draw(sprites[currentColor, 0], x, 0);
                //drawer.draw( RoadPattern.get((byte)Direction.EAST), x,0 );
            }
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
            return new ControllerImpl(this, site, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public override IModalController createRemover(IControllerSite site)
        {
            return new ControllerImpl(this, site, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public bool canBeBuilt(Location loc)
        {
            TrafficVoxel voxel = TrafficVoxel.get(loc);
            if (voxel == null) return false;

            BaseRoad r = voxel.road;
            if (r == null) return false;

            return true;
        }

        /// <summary>
        /// Create a new car at the specified location.
        /// dir = 0 or 1
        /// </summary>
        /// <param name="loc"></param>
        [CLSCompliant(false)]
        public void create(Location loc)
        {
            Debug.Assert(canBeBuilt(loc));

            int x;
            RoadPattern rp = TrafficVoxel.get(loc).road.pattern;
            if (rp.hasRoad(Direction.NORTH)) x = 0;
            else x = 1;

            new DummyCar(TrafficVoxel.get(loc), this, currentColor, x);
        }

    }

}
