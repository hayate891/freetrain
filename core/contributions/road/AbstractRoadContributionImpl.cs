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
using freetrain.framework;
using freetrain.framework.graphics;
using freetrain.framework.plugin;
using freetrain.world;
using freetrain.world.road;

namespace freetrain.contributions.road
{
    /// <summary>
    /// Usual implementation of RoadContribution.
    /// 
    /// Provided just for code sharing.
    /// </summary>
    [Serializable]
    public abstract class AbstractRoadContributionImpl : RoadContribution
    {
        private readonly string _name;
        private readonly string description;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected AbstractRoadContributionImpl(XmlElement e)
            : base(e)
        {
            _name = XmlUtil.selectSingleNode(e, "name").InnerText;
            description = XmlUtil.selectSingleNode(e, "description").InnerText;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirs"></param>
        /// <returns></returns>
        protected internal abstract Sprite getSprite(byte dirs);
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
                    if (v.road != null)
                    {
                        if (!v.road.canAttach(d) && here != to) return false;
                        if (!v.road.canAttach(d.opposite) && here != from) return false;
                    }
                    if (v.car != null) return false;	// car is in place
                    // TODO: check v.railRoad
                }

                if (here == to) return true;
                here = here.toward(to);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public override void build(Location from, Location to)
        {
            Debug.Assert(canBeBuilt(from, to));

            Direction d = from.getDirectionTo(to);

            Location here = from;
            while (true)
            {
                Road r = Road.get(here);
                if (r == null)
                {
                    RoadPattern p = RoadPattern.getStraight(d);
                    if (here == from) p = RoadPattern.get((byte)(1 << (d.index / 2)));
                    if (here == to) p = RoadPattern.get((byte)(1 << (d.opposite.index / 2)));

                    create(TrafficVoxel.getOrCreate(here), p);
                }
                else
                {
                    if (here != from) r.attach(d.opposite);
                    if (here != to) r.attach(d);
                }

                if (here == to) return;
                here = here.toward(to);
            }
        }

        /// <summary>
        /// Creates a new road with a given pattern.
        /// </summary>
        protected virtual Road create(TrafficVoxel voxel, RoadPattern pattern)
        {
            return new RoadImpl(this, voxel, pattern);
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
                Road r = Road.get(here);
                if (r != null)
                    r.detach(d, d.opposite);

                if (here == to) return;
                here = here.toward(to);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override string name
        {
            get
            {
                return _name;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override string oneLineDescription
        {
            get
            {
                return description;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override Bitmap previewBitmap
        {
            get
            {
                using (PreviewDrawer drawer = new PreviewDrawer(new Size(100, 100), new Size(10, 1), 0))
                {
                    int x, y;
                    for (int i = 0; i < 28; i++)
                    {
                        if (i <= 9)
                        {
                            x = 0;
                            y = i;
                        }
                        else
                        {
                            x = i - 9;
                            y = 9;
                        }
                        while (y >= 0 && x < 10)
                        {
                            if (previewPattern[PreviewPatternIdx, x, y] > 0)
                                drawer.draw(getSprite(previewPattern[PreviewPatternIdx, x, y]), 9 - x, y - 5);
                            x++;
                            y--;
                        }
                    }
                    return drawer.createBitmap();
                }
            }
        }

        /// <summary>
        /// Road implementation
        /// </summary>
        [Serializable]
        internal class RoadImpl : Road
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="contrib"></param>
            /// <param name="tv"></param>
            /// <param name="pattern"></param>
            internal protected RoadImpl(AbstractRoadContributionImpl contrib, TrafficVoxel tv, RoadPattern pattern)
                :
                base(tv, pattern, contrib.style)
            {

                this.contribution = contrib;
            }

            private readonly AbstractRoadContributionImpl contribution;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="display"></param>
            /// <param name="pt"></param>
            public override void drawBefore(DrawContext display, Point pt)
            {
                contribution.getSprite(pattern.dirs).draw(display.surface, pt);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="d"></param>
            /// <returns></returns>
            public override bool attach(Direction d)
            {
                byte dirs = pattern.dirs;
                dirs |= (byte)(1 << (d.index / 2));
                voxel.road = new RoadImpl(contribution, voxel, RoadPattern.get(dirs));
                return true;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="d1"></param>
            /// <param name="d2"></param>
            public override void detach(Direction d1, Direction d2)
            {
                byte dirs = pattern.dirs;
                dirs &= (byte)~(1 << (d1.index / 2));
                dirs &= (byte)~(1 << (d2.index / 2));

                if (dirs == 0)
                    // destroy this road
                    voxel.road = null;
                else
                {
                    voxel.road = new RoadImpl(contribution, voxel, RoadPattern.get(dirs));
                }

                World.world.onVoxelUpdated(location);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="d"></param>
            /// <returns></returns>
            public override bool canAttach(Direction d)
            {
                return true;
            }
        }
    }
}
