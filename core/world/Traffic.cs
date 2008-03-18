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
using SDL.net;
using FreeTrain.world.Rail;
using FreeTrain.world.road;

namespace FreeTrain.world
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="v"></param>
    public delegate void TrafficVoxelHandler(TrafficVoxel v);

    /// <summary>
    /// 線路や道路などによって占有されているブロック
    /// </summary>
    /// TrafficOccupantには線路・道路・および車（電車もしくは自動車）が入れる
    [Serializable]
    public sealed class TrafficVoxel : AbstractVoxelImpl, Entity
    {
        private Car _car;

        private RailRoad _railRoad;

        private Road _road;

        private IAccessory _accessory;
        /// <summary>
        /// 
        /// </summary>
        public override bool transparent { get { return true; } }

        /// <summary>
        /// Fired when a car enters/leaves this traffic voxel
        /// </summary>
        public event TrafficVoxelHandler onCarChanged;

        /// <summary>
        /// Fired when a railroad is placed or removed.
        /// </summary>
        public static event TrafficVoxelHandler onRailRoadChanged;



        private TrafficVoxel(Location loc) : base(loc) { }

        /// <summary>
        /// 
        /// </summary>
        public interface IAccessory
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="display"></param>
            /// <param name="pt"></param>
            void drawBefore(DrawContext display, Point pt);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="display"></param>
            /// <param name="pt"></param>
            void drawAfter(DrawContext display, Point pt);
            /// <summary>
            /// Called right after the accessory is removed.
            /// </summary>
            void onRemoved();
        }



        /// <summary>
        /// Car that occupies this voxel, if any, or null.
        /// </summary>
        public Car car
        {
            get
            {
                return _car;
            }
            set
            {
                _car = value;
                onModified();
                if (onCarChanged != null) onCarChanged(this);
            }
        }


        /// <summary>
        /// Railroad that occupies this voxel, or null otherwise.
        /// </summary>
        public RailRoad railRoad
        {
            get
            {
                return _railRoad;
            }
            set
            {
                Debug.Assert(car == null);
                _railRoad = value;
                onModified();
                if (onRailRoadChanged != null) onRailRoadChanged(this);
            }
        }

        /// <summary>
        /// Road that occupies this voxel, or null otherwise.
        /// </summary>
        public Road road
        {
            get
            {
                return _road;
            }
            set
            {
                Debug.Assert(car == null);
                _road = value;
                onModified();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public IAccessory accessory
        {
            get
            {
                return _accessory;
            }
            set
            {
                if (value == null && _accessory != null) _accessory.onRemoved();
                _accessory = value;
                onModified();
            }
        }

        /// <summary>
        /// Returns true if a car is already in this voxel.
        /// </summary>
        public bool isOccupied { get { return car != null; } }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="aspect"></param>
        /// <returns></returns>
        public override object queryInterface(Type aspect)
        {
            object o;

            if (railRoad != null
            && (o = railRoad.queryInterface(aspect)) != null) return o;

            // TODO: add queryInterface for trains and roads.

            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool onClick()
        {
            if (car != null && car.onClick()) return true;
            if (_railRoad != null && _railRoad.onClick()) return true;
            return false;
        }


        private void onModified()
        {
            if (_accessory == null && _railRoad == null && _road == null)
                remove();

            // update surrounding voxels if necessary
            if (_railRoad != null)
                _railRoad.invalidateVoxel();
            if (_road != null)
                _road.invalidateVoxel();

            // create/remove crossing.
            bool b = shouldHaveCrossing();
            if (b && _accessory == null)
            {
                accessory = new RRCrossing(this);
                return;
            }
            if (!b && _accessory is RRCrossing)
            {
                accessory = null;
                return;
            }
        }

        /// <summary>
        /// Check if this voxel should have a rail road crossing.
        /// </summary>
        /// <returns></returns>
        private bool shouldHaveCrossing()
        {
            if (_railRoad == null || _road == null) return false;

            RailPattern rp = _railRoad.getPattern();
            Direction rdir1 = _railRoad.dir1;
            if (rp.numberOfRails != 2 || !rp.hasRail(rdir1.opposite))
                return false;	// the rail must be going straight.
            if (!rdir1.isSharp)
                return false;	// rail must be paralell to X-axis or Y-axis.

            if (!_road.hasRoad(rdir1.left90) || !_road.hasRoad(rdir1.right90))
                return false;	// road must be orthogonal to rail road

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public override Entity entity { get { return this; } }

        #region Entity implementation
        /// <summary>
        /// 
        /// </summary>
        public bool isSilentlyReclaimable { get { return false; } }
        /// <summary>
        /// 
        /// </summary>
        public bool isOwned { get { return true; } }

        // TODO: what's the value?
        /// <summary>
        /// 
        /// </summary>
        public int entityValue { get { return 0; } }
        /// <summary>
        /// 
        /// </summary>
        public void remove()
        {
            if (isOccupied)
            {
                if (car is Train.TrainCar)
                    ((Train.TrainCar)car).parent.remove();
                else
                    car.remove();
            }
            BridgePierVoxel.teardownBridgeSupport(location, this);
            if (onEntityRemoved != null)
                onEntityRemoved(this, null);
            World.world.remove(this);
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler onEntityRemoved;
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="display"></param>
        /// <param name="pt"></param>
        /// <param name="heightCutDiff"></param>
        public override void draw(DrawContext display, Point pt, int heightCutDiff)
        {
            if (_road != null) _road.drawBefore(display, pt);
            if (_accessory != null) _accessory.drawBefore(display, pt);
            if (_railRoad != null) _railRoad.drawBefore(display, pt);
            if (_car != null) _car.draw(display, pt);
            if (_railRoad != null) _railRoad.drawAfter(display, pt);
            if (_accessory != null) _accessory.drawAfter(display, pt);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Color getColorOfTile()
        {
            if (_car != null) return Color.White;
            if (_railRoad != null) return Color.Black;
            if (_road != null) return Color.Gray;
            if (_accessory != null) return Color.Yellow;

            return Color.Blue;
        }

        /// <summary>
        /// Gets the existing TrafficVoxel in the specified location,
        /// or create new TrafficVoxel if the location is unoccupied.
        /// </summary>
        /// <returns>null if the specified location is already occupied</returns>
        public static TrafficVoxel getOrCreate(Location loc)
        {
            Voxel v = World.world[loc];
            if (v != null) return v as TrafficVoxel;

            return new TrafficVoxel(loc);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static TrafficVoxel getOrCreate(int x, int y, int z)
        {
            return getOrCreate(new Location(x, y, z));
        }

        /// <summary>
        /// Gets a TrafficVoxel in the specified location, if any.
        /// Otherwise null.
        /// </summary>
        public static TrafficVoxel get(Location loc)
        {
            return World.world[loc] as TrafficVoxel;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static TrafficVoxel get(int x, int y, int z)
        {
            return get(new Location(x, y, z));
        }
    }
}
