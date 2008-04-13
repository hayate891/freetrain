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
using FreeTrain.Framework.Graphics;

namespace FreeTrain.World.Rail
{
    /// <summary>
    /// Slope rail road.
    /// 
    /// Consists of 8 voxels. Four voxels are TrafficVoxels,
    /// Two more are SlopeFillerVoxels, which are invisible,
    /// and the other two are SlopeSupport voxels, which are visible.
    /// </summary>
    [Serializable]
    public class SlopeEntity : IEntity
    {
        private Cube cube;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="dir"></param>
        public SlopeEntity(Location start, Direction dir)
        {
            Location end = new Location(start.x + dir.offsetX * 3, start.y + dir.offsetY * 3, start.z + 1);
            cube = Cube.CreateInclusive(start, end);
        }
        /// <summary>
        /// 
        /// </summary>
        public int EntityValue { get { return 0; } }
        /// <summary>
        /// 
        /// </summary>
        public bool isOwned { get { return true; } }
        /// <summary>
        /// 
        /// </summary>
        public bool isSilentlyReclaimable { get { return false; } }
        /// <summary>
        /// 
        /// </summary>
        public void remove()
        {
            foreach (Voxel v in cube.Voxels)
            {
                if (v.Entity == this && !(v is BridgePierVoxel))
                    WorldDefinition.World.remove(v);
                else
                {
                    TrafficVoxel tv = v as TrafficVoxel;
                    if (tv != null)
                    {
                        SlopeRailRoad srr = tv.railRoad as SlopeRailRoad;
                        if (srr != null && srr.entity == this)
                            tv.remove();
                    }
                }
                //				if(v.location.z==cube.z1)
                //					BridgePierVoxel.teardownBridgeSupport(v.location,this);
            }
            if (onEntityRemoved != null)
                onEntityRemoved(this, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aspect"></param>
        /// <returns></returns>
        public object QueryInterface(Type aspect) { return null; }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler onEntityRemoved;
        //		public object GetRealObject(StreamingContext sc) { return theInstance; }
    }

    /// <summary>
    /// Railroad with slope.
    /// </summary>
    [Serializable]
    public class SlopeRailRoad : RailRoad
    {

        internal SlopeEntity entity;
        private SlopeRailRoad(SlopeEntity e, TrafficVoxel v, RailPattern rp)
            : base(v, rp)
        {
            entity = e;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void invalidateVoxel()
        {
            // this voxel and the voxel below/above needs to be updated
            WorldDefinition.World.OnVoxelUpdated(Voxel);

            Location loc = Location;
            if (pattern.level < 2)	// the voxel above
                loc.z++;
            else
                loc.z--;
            WorldDefinition.World.OnVoxelUpdated(loc);
        }
        /// <summary>
        /// 
        /// </summary>
        public Direction climbDir { get { return pattern.climbDir; } }
        /// <summary>
        /// 
        /// </summary>
        public int level { get { return pattern.level; } }

        // can't attach rail to slope. So the only possibility is that
        // we already have a railroad to that direction
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public override bool Attach(Direction dir) { return hasRail(dir); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public override bool CanAttach(Direction dir) { return hasRail(dir); }

        // similarly, can't detach
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        public override void Detach(Direction d1, Direction d2)
        {
            ;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Direction Guide()
        {
            // slop rails don't curve. so a car should be
            // able to go to the same direction
            Direction d = Voxel.car.state.asInside().direction;
            Debug.Assert(hasRail(d));
            return d;
        }




        private const int SLOPE_CONSTRUCTION_UNIT_COST = 400000;	// taken from A4.
        private const int SLOPE_DESTRUCTION_UNIT_COST = 80000;




        /// <summary>
        /// Gets the SlopeRailRoad object of the specified location, if any.
        /// Otherwise null.
        /// </summary>
        public static new SlopeRailRoad get(Location loc)
        {
            RailRoad rr = RailRoad.get(loc);
            if (rr is SlopeRailRoad) return (SlopeRailRoad)rr;
            else return null;
        }

        /// <summary>
        /// Creates a new slope. A slope consists of four consective
        /// blocks of railroads. The base parameter specifies the location
        /// of the lowest railroad and the direction parameter
        /// specifies the direction to climb.
        /// 
        /// The caller must use the canCreateSlope method to check
        /// if this method can be invoked.
        /// </summary>
        public static void createSlope(Location _base, Direction dir)
        {
            Debug.Assert(canCreateSlope(_base, dir));

            // charge the cost before we alter something
            Accounting.AccountGenre.RailService.Spend(calcCostOfNewSlope(_base, dir));

            SlopeEntity entity = new SlopeEntity(_base, dir);

            for (int i = 0; i < 4; i++)
            {
                if (_base.z < WorldDefinition.World.GetGroundLevel(_base))
                {
                    new SlopeRailRoad(entity, TrafficVoxel.getOrCreate(
                        _base.x, _base.y, _base.z + (i / 2)),
                        RailPattern.getUGSlope(dir, i));
                    if (i < 2)
                    {
                        // space filler
                        new SlopeFillerVoxel(entity, _base.x, _base.y, _base.z + 1, i);
                    }
                    else
                    {
                        new SlopeSupportVoxel(entity, _base.x, _base.y, _base.z, i,
                            RailPattern.slopeWalls[dir.index + i - 2]);
                    }
                }
                else
                {
                    new SlopeRailRoad(entity, TrafficVoxel.getOrCreate(
                        _base.x, _base.y, _base.z + (i / 2)),
                        RailPattern.getSlope(dir, i));
                    if (i < 2)
                    {
                        // space filler
                        new SlopeFillerVoxel(entity, _base.x, _base.y, _base.z + 1, i);
                    }
                    else
                    {
                        new SlopeSupportVoxel(entity, _base.x, _base.y, _base.z, i,
                            RailPattern.slopeSupports[dir.index + (i - 2)]);
                    }
                }

                Type bridgeStyle;
                if (dir == Direction.NORTH || dir == Direction.EAST)
                    bridgeStyle = typeof(BridgePierVoxel.DefaultImpl);
                else
                    bridgeStyle = typeof(BridgePierVoxel.SlopeNEImpl);
                BridgePierVoxel.electBridgeSupport(_base, bridgeStyle, entity);

                _base += dir;
            }
        }

        /// <summary>
        /// Used for upper two invisible voxels
        /// </summary>
        [Serializable]
        internal class SlopeFillerVoxel : EmptyVoxel, IHoleVoxel
        {
            internal SlopeFillerVoxel(SlopeEntity entity, int x, int y, int z, int idx)
                : base(entity, x, y, z)
            {

                int glevel = WorldDefinition.World.GetGroundLevel(Location);
                drawSurfaceBelow = !(idx < 2 && glevel >= z);
                drawSurfaceAbove = !(idx >= 2 && glevel >= z + 1);
            }


            private readonly bool drawSurfaceAbove;
            private readonly bool drawSurfaceBelow;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="above"></param>
            /// <returns></returns>
            public bool DrawGround(bool above)
            {
                return above ? drawSurfaceAbove : drawSurfaceBelow;
            }
        }

        /// <summary>
        /// Used for lower two voxels. Visible but not rail road.
        /// </summary>
        [Serializable]
        internal class SlopeSupportVoxel : EmptyVoxel, IHoleVoxel
        {
            internal SlopeSupportVoxel(SlopeEntity entity, int x, int y, int z, int idx, ISprite s)
                : base(entity, x, y, z)
            {

                int glevel = WorldDefinition.World.GetGroundLevel(Location);
                drawSurfaceBelow = !(idx < 2 && glevel >= z);
                drawSurfaceAbove = !(idx >= 2 && glevel >= z + 1);
                sprite = s;
            }
            /// <summary>
            /// 
            /// </summary>
            public override bool Transparent { get { return true; } }

            private readonly bool drawSurfaceAbove;
            private readonly bool drawSurfaceBelow;
            private readonly ISprite sprite;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="above"></param>
            /// <returns></returns>
            public bool DrawGround(bool above)
            {
                return above ? drawSurfaceAbove : drawSurfaceBelow;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="surface"></param>
            /// <param name="pt"></param>
            /// <param name="heightCutDiff"></param>
            public override void Draw(DrawContext surface, Point pt, int heightCutDiff)
            {
                sprite.Draw(surface.Surface, pt);
            }
        }


        /// <summary>
        /// Return true if a slope RR can be built at the specified location.
        /// </summary>
        /// <param name="_base"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static bool canCreateSlope(Location _base, Direction dir)
        {
            return calcCostOfNewSlope(_base, dir) != 0;
        }

        /// <summary>
        /// Compute a construction cost of a slope rail.
        /// </summary>
        /// <returns>If a construction is impossible, return 0</returns>
        public static int calcCostOfNewSlope(Location _base, Direction dir)
        {
            if (!dir.isSharp) return 0;

            if (_base.z == WorldDefinition.World.Size.z - 1)
                return 0;	// we can't go above the ceil

            // 8 voxels around (depth-4 height-2) must be completely available.
            // it's not even OK to have a TrafficVoxel.
            for (int i = 0; i < 4; i++)
            {
                if (WorldDefinition.World[_base] != null) return 0;
                if (WorldDefinition.World[_base.x, _base.y, _base.z + 1] != null) return 0;

                _base += dir;
            }

            return SLOPE_CONSTRUCTION_UNIT_COST * Math.Max(1, _base.z - WorldDefinition.World.WaterLevel);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static bool canRemoveSlope(Location loc, Direction dir)
        {
            return calcCostOfTearDownSlope(loc, dir) != 0;
        }

        /// <summary>
        /// Compute the cost of destructing a slope rail.
        /// </summary>
        /// <returns>If a destruction is impossible, return 0</returns>
        public static int calcCostOfTearDownSlope(Location loc, Direction dir)
        {
            // make sure the first voxel is not occupied by a car
            if (Car.get(loc) != null) return 0;

            // the 2nd block has a distinctive zangle and zdiff. check it.
            loc += dir;
            RailRoad rr = RailRoad.get(loc);
            if (!(rr is SlopeRailRoad)) return 0;
            SlopeRailRoad srr = (SlopeRailRoad)rr;

            if (!(srr.pattern.zangle == dir && srr.pattern.zdiff == 1))
                return 0;

            // make sure the 2nd rail is not occupied by a car
            if (Car.get(loc) != null) return 0;

            // check 3rd and 4th rails.
            loc += dir;
            loc.z++;
            if (Car.get(loc) != null) return 0;
            loc += dir;
            if (Car.get(loc) != null) return 0;

            return SLOPE_DESTRUCTION_UNIT_COST * Math.Max(1, loc.z - WorldDefinition.World.WaterLevel);
        }

        /// <summary>
        /// Removes a slope. The format of the parameters are the same
        /// as the createSlope method. Ut us 
        /// </summary>
        public static void removeSlope(Location loc, Direction dir)
        {
            Debug.Assert(canRemoveSlope(loc, dir));

            // charge the cost before we alter something
            Accounting.AccountGenre.RailService.Spend(calcCostOfTearDownSlope(loc, dir));

            for (int i = 0; i < 4; i++)
            {
                TrafficVoxel v = TrafficVoxel.get(loc.x, loc.y, loc.z + (i / 2));
                v.railRoad = null;

                Location l = loc;
                l.z += -(i / 2) + 1;
                Debug.Assert(WorldDefinition.World[l] is EmptyVoxel);
                WorldDefinition.World.remove(l);

                BridgePierVoxel.teardownBridgeSupport(loc, v);

                loc += dir;
            }
        }
    }
}
