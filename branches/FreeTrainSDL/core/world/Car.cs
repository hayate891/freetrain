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

namespace FreeTrain.World
{
    /// <summary>
    /// A car can be in three states.
    /// 
    /// (1) inside a map
    ///		direction!=null, and location has a valid value
    ///	(2) not placed
    ///		direction==null, location==UNPLACED
    ///	(3) outside map
    /// </summary>
    [Serializable]
    public abstract class Car
    {
        private CarState _state = CarState.Unplaced.theInstance;

        /// <summary>
        /// Set the new state.
        /// </summary>
        /// <returns>the previous state</returns>
        protected CarState setState(CarState s)
        {
            CarState oldState = _state;

            CarState.Inside ss = _state.asInside();
            if (ss != null)
            {
                Debug.Assert(ss.voxel.car == this);
                ss.voxel.car = null;
                WorldDefinition.World.onVoxelUpdated(ss.voxel);
            }

            _state = s;

            ss = s.asInside();
            if (ss != null)
            {
                Debug.Assert(ss.voxel.car == null);
                ss.voxel.car = this;
                WorldDefinition.World.onVoxelUpdated(ss.voxel);
            }

            return oldState;
        }

        /// <summary>
        /// Current location/direction of the car.
        /// </summary>
        public CarState state { get { return _state; } }

        /// <summary>
        /// 車両を配置する
        /// </summary>
        public void place(Location loc, Direction dir)
        {
            Debug.Assert(state.isUnplaced);
            setState(new CarState.Inside(loc, dir));
        }

        /// <summary>
        /// 車両を現在位置から撤去する
        /// </summary>
        public void remove()
        {
            Debug.Assert(!state.isUnplaced);
            setState(CarState.Unplaced.theInstance);
        }

        /// <summary>
        /// Called when a car is clicked.
        /// </summary>
        /// <returns>true if the click is processed and consumed</returns>
        public virtual bool onClick() { return false; }

        /// <summary>
        /// Draws the car into the specified location.
        /// </summary>
        public abstract void draw(DrawContext display, Point pt);

        /// <summary>
        /// Gets a car that occupies the specified place, if any. Or null otherwise.
        /// </summary>
        public static Car get(Location loc)
        {
            TrafficVoxel v = TrafficVoxel.get(loc);
            if (v == null) return null;
            else return v.car;
        }

    }
}
