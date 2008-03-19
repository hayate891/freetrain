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
using FreeTrain.World;
using FreeTrain.Framework.Plugin;

namespace FreeTrain.Contributions.Common
{
    /// <summary>
    /// Base class for SpecialRailContribution and RoadContritbuion
    /// </summary>
    [Serializable]
    public abstract class LineContribution : Contribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        protected LineContribution(string type, string id) : base(type, id) { }

        /// <summary>
        /// Returns true if rails can be built between the two given locations
        /// </summary>
        public abstract bool canBeBuilt(Location loc1, Location loc2);

        /// <summary>
        /// Builds rail roads between the two given locations.
        /// This method will be called only when canBeBuilt(loc1,loc2) returns true.
        /// </summary>
        public abstract void build(Location loc1, Location loc2);

        /// <summary>
        /// Removes this special rail road between the given two locations.
        /// It is not an error for some other kinds of rail to appear in between
        /// these two.
        /// </summary>
        public abstract void remove(Location loc1, Location loc2);

        /// <summary>
        /// Gets the name of this special rail.
        /// </summary>
        public abstract string name { get; }

        /// <summary>
        /// Gets a one line description of this rail.
        /// </summary>
        public abstract string oneLineDescription { get; }

        /// <summary>
        /// Gets the bitmap that will be used in the construction dialog.
        /// Should reload a fresh copy every time this method is called.
        /// The caller should dispose the object if it becomes unnecessary.
        /// </summary>
        public abstract Bitmap previewBitmap { get; }
        /// <summary>
        /// 
        /// </summary>
        public enum DirectionModes
        {
            /// <summary>
            /// 
            /// </summary>
            FourWay,
            /// <summary>
            /// 
            /// </summary>
            EightWay
        };

        /// <summary>
        /// Available directions
        /// </summary>
        public virtual DirectionModes DirectionMode { get { return DirectionModes.FourWay; } }
    }
}
