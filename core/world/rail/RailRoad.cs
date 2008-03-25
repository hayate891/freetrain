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
using System.Runtime.Serialization;
using System.Windows.Forms;
using FreeTrain.Framework.Graphics;
using FreeTrain.Framework;

namespace FreeTrain.World.Rail
{
    /// <summary>
    /// 線路
    /// </summary>
    [Serializable]
    public abstract class RailRoad
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tv"></param>
        /// <param name="pattern"></param>
        protected RailRoad(TrafficVoxel tv, RailPattern pattern)
        {
            this.Voxel = tv;
            this.pattern = pattern;
            Voxel.railRoad = this;
        }

        /// <summary>
        /// この線路が占めるVoxel
        /// </summary>
        public readonly TrafficVoxel Voxel;

        /// <summary>
        /// この線路の位置
        /// </summary>
        public Location Location { get { return Voxel.location; } }


        /// <summary>
        /// Determines the direction of the train car that occupies this voxel
        /// should go, based solely on the pattern of this voxel, without looking
        /// at any other surrounding voxels.
        /// </summary>
        public abstract Direction Guide();

        /// <summary>
        /// Gets the difference in z-axis for a car that heads to
        /// the specified direction.
        /// </summary>
        public int ZDiff(Direction d)
        {
            if (pattern.zangle == d) return pattern.zdiff;
            else return 0;
        }


        /// <summary>
        /// Called when a rail road is clicked.
        /// </summary>
        /// <returns>true if the click is processed and consumed</returns>
        public virtual bool OnClick() { return false; }

        /// <summary>
        /// Returns true if this railroad is connected to
        /// at least two adjacent railroads.
        /// </summary>
        public bool IsWellConnected
        {
            get
            {
                int cnt = 0;

                foreach (Direction d in Direction.directions)
                {
                    if (hasRail(d))
                    {
                        Voxel v = WorldDefinition.World[Location + d];
                        if (v is TrafficVoxel)
                        {
                            TrafficVoxel tv = (TrafficVoxel)v;
                            if (tv.railRoad != null)
                            {
                                if (tv.railRoad.hasRail(d.opposite))
                                    cnt += 1;
                            }
                        }
                    }
                }

                return cnt >= 2;
            }
        }

        /// <summary>
        /// Attachs another direction of RR to this existing RR.
        /// 
        /// If necessary, this method will create a junction.
        /// </summary>
        /// <returns>
        /// false if the operation is impossible. This happens if this
        /// rail road already is a junction, for example.
        /// </returns>
        public abstract bool Attach(Direction newDir);

        /// <summary>
        /// Returns true if a new RR with the given direction can be attached.
        /// </summary>
        public abstract bool CanAttach(Direction newDir);

        /// <summary>
        /// Detaches two directions from this RR.
        /// This method should remove itself or even the parent TrafficVoxel
        /// if this change would remove RR completely.
        /// Thus the caller shouldn't assume that any reference to this RailRoad
        /// object or its parent TrafficVoxel would be valid after the method invocation.
        /// </summary>
        public abstract void Detach(Direction d1, Direction d2);

        /// <summary>
        /// hasRail(x)==trueとなるような何らかのxを返す
        /// </summary>
        public Direction Dir1
        {
            get
            {
                for (int i = 0; ; i++)
                    if (pattern.hasRail(i)) return Direction.get(i);
            }
        }

        /// <summary>
        /// hasRail(x)==trueとなるようなxのうち、dir1!=xとなるような
        /// 何らかのxを返す
        /// </summary>
        public Direction Dir2
        {
            get
            {
                for (int i = 7; ; i--)
                    if (pattern.hasRail(i)) return Direction.get(i);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected RailPattern pattern;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public RailPattern getPattern() { return pattern; }

        /// <summary>
        /// どの方向にレールが伸びているか。trueなら伸びている
        /// </summary>
        public bool hasRail(Direction d)
        {
            return pattern.hasRail(d);
        }

        /// <summary>
        /// Called by the <c>TrafficVoxel</c> to invalidate
        /// voxels.
        /// </summary>
        public virtual void invalidateVoxel()
        {
            // by default, the occupied voxel is updated
            WorldDefinition.World.onVoxelUpdated(Voxel);
        }

        /// <summary>
        /// Draws a railroad. This method is called before the car is drawn.
        /// </summary>
        public virtual void drawBefore(DrawContext display, Point pt)
        {
            pattern.draw(display.surface, pt);
        }
        /// <summary>
        /// Draws a railroad. This method is called after the car is drawn.
        /// </summary>
        public virtual void drawAfter(DrawContext display, Point pt)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="callCount"></param>
        /// <returns></returns>
        public virtual TimeLength getStopTimeSpan(Train tr, int callCount)
        {
            return TimeLength.ZERO;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aspect"></param>
        /// <returns></returns>
        public virtual object queryInterface(Type aspect)
        {
            return null;
        }


        /// <summary>
        /// Gets the RailRoad object of the specified location, if any.
        /// Otherwise null.
        /// </summary>
        public static RailRoad get(Location loc)
        {
            Voxel v = WorldDefinition.World[loc];
            if (!(v is TrafficVoxel)) return null;
            return ((TrafficVoxel)v).railRoad;
        }
    }
}
