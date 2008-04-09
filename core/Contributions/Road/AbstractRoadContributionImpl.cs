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
using FreeTrain.Framework;
using FreeTrain.Framework.Graphics;
using FreeTrain.Framework.Plugin;
using FreeTrain.World;
using FreeTrain.World.Road;

namespace FreeTrain.Contributions.Road
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
            _name = XmlUtil.SelectSingleNode(e, "name").InnerText;
            description = XmlUtil.SelectSingleNode(e, "description").InnerText;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirs"></param>
        /// <returns></returns>
        protected internal abstract ISprite getSprite(byte dirs);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public override bool CanBeBuilt(Location from, Location to)
        {
            if (from == to) return false;

            Direction d = from.getDirectionTo(to);

            Location here = from;

            while (true)
            {
                if (WorldDefinition.World[here] != null)
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
        public override void Build(Location from, Location to)
        {
            Debug.Assert(CanBeBuilt(from, to));

            Direction d = from.getDirectionTo(to);

            Location here = from;
            while (true)
            {
                BaseRoad r = BaseRoad.get(here);
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
        protected virtual BaseRoad create(TrafficVoxel voxel, RoadPattern pattern)
        {
            return new RoadImpl(this, voxel, pattern);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="here"></param>
        /// <param name="to"></param>
        public override void Remove(Location here, Location to)
        {
            if (here == to) return;

            Direction d = here.getDirectionTo(to);

            while (true)
            {
                BaseRoad r = BaseRoad.get(here);
                if (r != null)
                    r.detach(d, d.opposite);

                if (here == to) return;
                here = here.toward(to);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override string Name
        {
            get
            {
                return _name;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override string OneLineDescription
        {
            get
            {
                return description;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override Bitmap PreviewBitmap
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
                                drawer.Draw(getSprite(previewPattern[PreviewPatternIdx, x, y]), 9 - x, y - 5);
                            x++;
                            y--;
                        }
                    }
                    return drawer.CreateBitmap();
                }
            }
        }

        /// <summary>
        /// Road implementation
        /// </summary>
        [Serializable]
        internal class RoadImpl : BaseRoad
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
                contribution.getSprite(pattern.dirs).Draw(display.Surface, pt);
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
                Voxel.road = new RoadImpl(contribution, Voxel, RoadPattern.get(dirs));
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
                    Voxel.road = null;
                else
                {
                    Voxel.road = new RoadImpl(contribution, Voxel, RoadPattern.get(dirs));
                }

                WorldDefinition.World.OnVoxelUpdated(Location);
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
