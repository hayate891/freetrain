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
using FreeTrain.Contributions.Rail;
using FreeTrain.World.Structs;
using FreeTrain.Framework.Plugin;

namespace FreeTrain.World.Rail
{
    /// <summary>
    /// RailStationaryStructure の概要の説明です。
    /// </summary>
    [Serializable]
    public class RailStationaryStructure : PThreeDimStructure
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="wloc"></param>
        public RailStationaryStructure(RailStationaryContribution type, WorldLocator wloc) : base(type, wloc) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wloc"></param>
        /// <returns></returns>
        protected override StructureVoxel CreateVoxel(WorldLocator wloc)
        {
            if (type.Size.x == 1 && type.Size.y == 1)
                return new StationaryVoxel(this, wloc);
            else
                return base.CreateVoxel(wloc);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool onClick() { return false; }

        #region Entity implementation
        /// <summary>
        /// 
        /// </summary>
        public override bool isSilentlyReclaimable { get { return false; } }
        /// <summary>
        /// 
        /// </summary>
        public override bool isOwned { get { return true; } }

        // TODO: value?
        /// <summary>
        /// 
        /// </summary>
        public override int EntityValue { get { return 0; } }

        #endregion

        /// <summary>
        /// Gets the station object if one is in the specified location.
        /// </summary>
        public static RailStationaryStructure get(Location loc)
        {
            return WorldDefinition.World.GetEntityAt(loc) as RailStationaryStructure;
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected override Color heightCutColor { get { return Color.Gray; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static RailStationaryStructure get(int x, int y, int z) { return get(new Location(x, y, z)); }
        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        protected class StationaryVoxel : VoxelImpl
        {
            internal StationaryVoxel(PThreeDimStructure _owner, WorldLocator wloc)
                : base(_owner, wloc) { }
            /// <summary>
            /// 
            /// </summary>
            public override bool Transparent { get { return true; } }
        }
    }
}
