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

namespace freetrain.world
{
    /// <summary>
    /// Current state of a car. Immutable.
    /// </summary>
    [Serializable]
    public abstract class CarState
    {
        public Inside asInside() { return this as Inside; }
        public Unplaced asUnplaced() { return this as Unplaced; }
        public Outside asOutside() { return this as Outside; }
        public Placed asPlaced() { return this as Placed; }

        public bool isInside { get { return this is Inside; } }
        public bool isUnplaced { get { return this is Unplaced; } }
        public bool isOutside { get { return this is Outside; } }

        public abstract object accept(Visitor visitor);


        public interface Visitor
        {
            object onInside(Inside state);
            object onUnplaced(Unplaced state);
            object onOutsie(Outside state);
        }

        [Serializable]
        public abstract class Placed : CarState
        {
            /// <summary>
            /// Direction of the car.
            /// </summary>
            public readonly Direction direction;

            /// <summary>
            /// Current location of the car.
            /// </summary>
            public readonly Location location;

            /// <summary>
            /// Voxel that represents the location.
            /// </summary>
            public TrafficVoxel voxel { get { return (TrafficVoxel)World.world[location]; } }

            public Placed(Location loc, Direction dir)
            {
                this.location = loc;
                this.direction = dir;
            }
        }

        /// <summary>
        /// Inside the world.
        /// </summary>
        [Serializable]
        public class Inside : Placed
        {
            public Inside(Location loc, Direction dir) : base(loc, dir) { }
            public override object accept(Visitor visitor)
            {
                return visitor.onInside(this);
            }
        }

        /// <summary>
        /// In the inventory but not used.
        /// </summary>
        [Serializable]
        public class Unplaced : CarState, IObjectReference
        {
            private Unplaced() { }

            public object GetRealObject(StreamingContext ctxt)
            {
                return theInstance;
            }
            public override object accept(Visitor visitor)
            {
                return visitor.onUnplaced(this);
            }

            // singleton pattern.
            public readonly static CarState theInstance = new Unplaced();
        }

        /// <summary>
        /// Outside the world. The member variables keep the location
        /// and the direction of a car when it left the world.
        /// IOW the location is always outside the world.
        /// </summary>
        [Serializable]
        public class Outside : Placed
        {
            /// <summary>
            /// Decreasing counter. When it hits zero, the car will be back to the world.
            /// </summary>
            public readonly int timeLeft;

            public Outside(Location loc, Direction dir, int _timeLeft)
                : base(loc, dir)
            {
                this.timeLeft = _timeLeft;
            }
            public override object accept(Visitor visitor)
            {
                return visitor.onOutsie(this);
            }
        }
    }
}
