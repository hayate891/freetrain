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

namespace freetrain.world.rail
{
    /// <summary>
    /// Rail road implementation for those special purpose rail roads
    /// that doesn't allow any attachment/detachment
    /// </summary>
    [Serializable]
    public class SpecialPurposeRailRoad : RailRoad
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="voxel"></param>
        /// <param name="d"></param>
        public SpecialPurposeRailRoad(TrafficVoxel voxel, Direction d)
            : base(voxel, RailPattern.get(d, d.opposite))
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newDir"></param>
        /// <returns></returns>
        public override bool canAttach(Direction newDir)
        {
            return hasRail(newDir);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newDir"></param>
        /// <returns></returns>
        public override bool attach(Direction newDir)
        {
            return hasRail(newDir);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        public override void detach(Direction d1, Direction d2)
        {
            // can't be detached
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Direction guide()
        {
            Direction d = voxel.car.state.asInside().direction;
            // we have straight rails only, so the direction must stay the same
            Debug.Assert(hasRail(d));
            return d;
        }
    }
}
