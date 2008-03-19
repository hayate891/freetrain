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
using System.IO;
using System.Xml;
using SDL.net;
using FreeTrain.Contributions.Rail;
using FreeTrain.Framework;
using FreeTrain.Framework.Graphics;
using FreeTrain.Framework.plugin;
using FreeTrain.Util;

namespace FreeTrain.world.Rail
{
    /// <summary>
    /// SpecialRailContribution implementation for the steal-supported rail
    /// </summary>
    [Serializable]
    public class StealSupportedRailContributionImpl : SpecialRailContribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public StealSupportedRailContributionImpl(XmlElement e) : base(e) { }


        /// <summary>
        /// 
        /// </summary>
        protected internal override void onInitComplete()
        {
            Picture picture = loadPicture("StealSupportedRail.bmp");
            sprites[0] = new SimpleSprite(picture, new Point(0, 16), new Point(0, 0), new Size(32, 32));
            sprites[1] = new SimpleSprite(picture, new Point(0, 16), new Point(32, 0), new Size(32, 32));
            sprites[2] = new SimpleSprite(picture, new Point(0, 16), new Point(64, 0), new Size(32, 32));
            sprites[3] = new SimpleSprite(picture, new Point(0, 16), new Point(96, 0), new Size(32, 32));
        }

        // sprites
        private static readonly Sprite[] sprites = new Sprite[4];





        [Serializable]
        internal class RailImpl : SpecialPurposeRailRoad
        {
            internal RailImpl(TrafficVoxel tv, Direction d) : base(tv, d) { }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="display"></param>
            /// <param name="pt"></param>
            public override void drawBefore(DrawContext display, Point pt)
            {
                Sprite s = null;
                switch (dir1.index)
                {
                    case 0:
                    case 4: s = sprites[3]; break;
                    case 1:
                    case 5: s = sprites[1]; break;
                    case 2:
                    case 6: s = sprites[0]; break;
                    case 3:
                    case 7: s = sprites[2]; break;
                }

                s.draw(display.surface, pt);
                // don't call the base class so that we won't draw the rail road unnecessarily
            }
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public override bool canBeBuilt(Location from, Location to)
        {
            if (from == to) return false;

            Direction d = from.getDirectionTo(to);

            Location here = from;

            while (true)
            {
                if (World.world[here] != null)
                {
                    TrafficVoxel v = TrafficVoxel.get(here);
                    if (v == null) return false;	// occupied
                    if (v.railRoad == null) return false;	// occupied by something other than RR

                    if (!v.railRoad.hasRail(d) || !v.railRoad.hasRail(d.opposite))
                        return false;	// rail is running 
                }

                if (World.world.getGroundLevel(here) >= here.z)
                    return false;	// must be all raised 

                if (here == to) return true;
                here = here.toward(to);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="here"></param>
        /// <param name="to"></param>
        public override void build(Location here, Location to)
        {
            Debug.Assert(canBeBuilt(here, to));

            Direction d = here.getDirectionTo(to);

            while (true)
            {
                if (RailRoad.get(here) == null)
                {
                    TrafficVoxel tv = TrafficVoxel.getOrCreate(here);
                    new RailImpl(tv, d);
                    BridgePierVoxel.electBridgeSupport(here, tv);
                }

                if (here == to) return;
                here = here.toward(to);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="here"></param>
        /// <param name="to"></param>
        public override void remove(Location here, Location to)
        {
            if (here == to) return;

            Direction d = here.getDirectionTo(to);

            while (true)
            {
                RailImpl rr = RailRoad.get(here) as RailImpl;
                if (rr != null && rr.hasRail(d))
                {
                    // destroy it
                    rr.voxel.railRoad = null;
                    // TODO: delete piers

                    BridgePierVoxel.teardownBridgeSupport(here, TrafficVoxel.get(here));
                }

                if (here == to) return;
                here = here.toward(to);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public override string name { get { return "Girder Viaduct"; } }
        //! public override string name { get { return "ガード高架"; } }
        /// <summary>
        /// 
        /// </summary>
        public override string oneLineDescription { get { return "Steel reinforced viaduct"; } }
        //! public override string oneLineDescription { get { return "スチールで強化された高架"; } }
        /// <summary>
        /// 
        /// </summary>
        public override DirectionModes DirectionMode { get { return DirectionModes.EightWay; } }
        /// <summary>
        /// 
        /// </summary>
        public override Bitmap previewBitmap
        {
            get
            {
                using (PreviewDrawer d = new PreviewDrawer(new Size(100, 100), new Size(5, 1), 0))
                {
                    for (int i = 6; i >= -2; i--)
                    {
                        d.draw(BridgePierVoxel.defaultSprite, i, 0);
                        d.draw(sprites[0], i + 1, -1);
                    }

                    return d.createBitmap();
                }
            }
        }
    }
}
