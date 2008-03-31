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
using FreeTrain.Framework.Graphics;
using FreeTrain.Contributions.Rail;
using FreeTrain.Framework;
using FreeTrain.Framework.Plugin;
using FreeTrain.Util;
using FreeTrain.World.Terrain;

namespace FreeTrain.World.Rail
{
    /// <summary>
    /// SpecialRailContribution implementation for the BridgeRail
    /// </summary>
    [Serializable]
    public class TunnelRailContributionImpl : SpecialRailContribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public TunnelRailContributionImpl(XmlElement e) : base(e) { }





        // static initializer
        /// <summary>
        /// 
        /// </summary>
        protected internal override void onInitComplete()
        {
            Picture picture = loadPicture("TunnelRail.bmp");
            for (int i = 0; i < 2; i++)
            {
                backgrounds[i] = new SimpleSprite(picture, new Point(0, 16), new Point(32 * i, 0), new Size(32, 32));
                foregrounds[i] = new SimpleSprite(picture, new Point(0, 16), new Point(32 * i + 64, 0), new Size(32, 32));
            }
        }

        // sprites
        private static readonly Sprite[] foregrounds = new Sprite[2];
        private static readonly Sprite[] backgrounds = new Sprite[2];






        /// <summary>
        /// Tunnel rail roads.
        /// </summary>
        [Serializable]
        internal class TunnelRail : SpecialPurposeRailRoad
        {
            internal TunnelRail(TrafficVoxel tv, Direction d, byte pictIdx, byte[] _heights)
                : base(tv, d)
            {
                this.pictureIndex = pictIdx;
                this.heights = _heights;

                if (d.index < 4) sOrW = d.opposite;
                else sOrW = d;
            }

            /// <summary>
            /// stores corner heights of the mountain voxel so that we can restore it
            /// when this tunnel is removed.
            /// </summary>
            private readonly byte[] heights;

            /// <summary>
            /// this.dir1==sOrW || this.dir2==sOrW;
            /// and
            /// sOrW==Direction.SOUTH || sOrW==Direction.WEST;
            /// </summary>
            private readonly Direction sOrW;

            /// <summary>
            /// Removes this tunnel rail road and restore the original mountain voxel
            /// </summary>
            internal void remove()
            {
                Location loc = this.Location;
                WorldDefinition.World.remove(loc);
                new MountainVoxel(loc, heights[0], heights[1], heights[2], heights[3]);
            }


            //
            // drawing
            //
            private readonly byte pictureIndex;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="display"></param>
            /// <param name="pt"></param>
            public override void drawBefore(DrawContext display, Point pt)
            {
                backgrounds[pictureIndex].draw(display.Surface, pt);
                // don't call the base class so that we won't draw the rail road unnecessarily
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="display"></param>
            /// <param name="pt"></param>
            public override void drawAfter(DrawContext display, Point pt)
            {
                foregrounds[pictureIndex].draw(display.Surface, pt);
            }
            /// <summary>
            /// 
            /// </summary>
            public override void invalidateVoxel()
            {
                if (sOrW == null || !(RailRoad.get(this.Location + sOrW) is TunnelRail))
                    WorldDefinition.World.OnVoxelUpdated(this.Location);

                // otherwise no need to update the voxel since a train will be hidden by this tunnel
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
            if (from.z < WorldDefinition.World.waterLevel) return false;	// below the water level

            Debug.Assert(from.z == to.z);

            Direction d = from.getDirectionTo(to);

            Location here = from;
            bool atLeastOneMountain = false;

            // there must be at least one water between two locations
            while (true)
            {
                if (WorldDefinition.World[here] != null)
                {
                    if ((WorldDefinition.World[here] as MountainVoxel) != null)
                    {
                        atLeastOneMountain = true;
                    }
                    else
                    {
                        TrafficVoxel v = TrafficVoxel.get(here);
                        if (v == null) return false;	// occupied
                        if (v.railRoad == null) return false;	// occupied by something other than RR

                        if (!v.railRoad.hasRail(d) || !v.railRoad.hasRail(d.opposite))
                            return false;	// rail is running 
                    }
                }

                if (here == to) return atLeastOneMountain;
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
                    MountainVoxel mv = WorldDefinition.World[here] as MountainVoxel;
                    if (mv != null)
                    {
                        // build a tunnel
                        byte[] heights = new byte[4];
                        for (int i = 0; i < 4; i++)
                            heights[i] = (byte)mv.getHeight(Direction.get(i * 2 + 1));

                        WorldDefinition.World.remove(here);	// remove this mountain

                        create(TrafficVoxel.getOrCreate(here), d, heights);
                    }
                    else
                    {
                        // build a normal tunnel
                        new SingleRailRoad(TrafficVoxel.getOrCreate(here), RailPattern.get(d, d.opposite));
                    }
                }
                if (here == to) return;
                here = here.toward(to);
            }
        }

        private void create(TrafficVoxel v, Direction d, byte[] heights)
        {
            Debug.Assert(d.isSharp);

            if (d.isParallelToY)
                new TunnelRail(v, d, 1, heights);
            else
                new TunnelRail(v, d, 0, heights);
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

            for (; here != to; here = here.toward(to))
            {
                TunnelRail trr = RailRoad.get(here) as TunnelRail;
                if (trr != null && trr.hasRail(d))
                    trr.remove();	// destroy it
            }
        }




        /// <summary>
        /// 
        /// </summary>
        public override string name { get { return "Tunnel"; } }
        //! public override string name { get { return "トンネル"; } }
        /// <summary>
        /// 
        /// </summary>
        public override string oneLineDescription { get { return "Tunnel leading out of a mountainside"; } }
        //! public override string oneLineDescription { get { return "山肌を突き抜けるためのトンネル"; } }
        /// <summary>
        /// 
        /// </summary>
        public override Bitmap previewBitmap
        {
            get
            {
                using (PreviewDrawer d = new PreviewDrawer(new Size(100, 100), new Size(5, 1), 0))
                {
                    for (int i = 5; i >= 2; i--)
                    {
                        d.draw(backgrounds[0], i, 0);
                        d.draw(foregrounds[0], i, 0);
                    }
                    for (int i = 1; i >= -5; i--)
                    {
                        d.draw(RailPattern.get(Direction.EAST, Direction.WEST), i, 0);
                    }
                    return d.createBitmap();
                }
            }
        }
    }
}
