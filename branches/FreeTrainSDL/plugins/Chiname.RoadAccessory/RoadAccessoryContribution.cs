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
using System.Windows.Forms;
using System.Xml;
using FreeTrain.Controllers;
using FreeTrain.Contributions.Rail;
using FreeTrain.Framework.Plugin;
using FreeTrain.Framework.Graphics;
using FreeTrain.World;
using FreeTrain.World.Rail;
using FreeTrain.Contributions.Common;

namespace FreeTrain.World.Road.Accessory
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class RoadAccessoryContribution : RailAccessoryContribution
    {///
        public RoadAccessoryContribution(XmlElement e)
            : base(e)
        {
            XmlElement sprite = (XmlElement)XmlUtil.SelectSingleNode(e, "sprite");
            Picture picture = GetPicture(sprite);
            SpriteFactory factory = SpriteFactory.GetSpriteFactory(sprite);

            for (int y = 0; y < 2; y++)
                for (int x = 0; x < 2; x++)
                    sprites[x, y] = factory.CreateSprite(picture,
                        new Point(0, 16), new Point((y * 2 + x) * 32, 0), new Size(32, 32));
        }

        /// <summary>
        /// Sprites. Dimensinos are [x,y] where
        /// 
        /// x=0 if a sprite is for E/W direction.
        /// x=1 if a sprite is for N/S direction
        /// 
        /// y=0 if a sprite is behind a train and
        /// y=1 if a sprite is in front of a train 
        /// </summary>
        internal readonly ISprite[,] sprites = new ISprite[2, 2];
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pixelSize"></param>
        /// <returns></returns>
        public override PreviewDrawer CreatePreview(Size pixelSize)
        {
            PreviewDrawer drawer = new PreviewDrawer(pixelSize, new Size(10, 1), 0);
            for (int x = 9; x >= 0; x--)
            {
                if (x == 5) drawer.Draw(sprites[0, 0], x, 0);
                //drawer.draw( RoadPattern.get((byte)Direction.EAST), x,0 );
                if (x == 5) drawer.Draw(sprites[0, 1], x, 0);
            }
            return drawer;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public override IModalController CreateBuilder(IControllerSite site)
        {
            return new ControllerImpl(this, site, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public override IModalController CreateRemover(IControllerSite site)
        {
            return new ControllerImpl(this, site, true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public bool CanBeBuilt(Location loc)
        {
            TrafficVoxel voxel = TrafficVoxel.get(loc);
            if (voxel == null) return false;

            BaseRoad r = voxel.road;
            if (r == null) return false;

            return true;
        }

        /// <summary>
        /// Create a new road accessory at the specified location.
        /// </summary>
        /// <param name="loc"></param>
        public void Create(Location loc)
        {
            Debug.Assert(CanBeBuilt(loc));

            int x;
            RoadPattern rp = TrafficVoxel.get(loc).road.pattern;
            if (rp.hasRoad(Direction.NORTH)) x = 1;
            else x = 0;

            new RoadAccessory(TrafficVoxel.get(loc), this, x);
        }
    }
}
