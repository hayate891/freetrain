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

namespace FreeTrain.World.Land
{
    /// <summary>
    /// Land filler that occupies only one voxel.
    /// </summary>
    [Serializable]
    public abstract class LandVoxel : AbstractVoxelImpl, Entity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        public LandVoxel(Location loc)
            : base(loc)
        {
            Debug.Assert(canBeBuilt(loc));
            Debug.Assert(loc.z == WorldDefinition.world.getGroundLevel(loc));
        }
        /// <summary>
        /// 
        /// </summary>
        public override bool transparent { get { return true; } }
        /// <summary>
        /// 
        /// </summary>
        public override Entity entity { get { return this; } }

        #region Entity implementation
        /// <summary>
        /// 
        /// </summary>
        public virtual bool isSilentlyReclaimable { get { return true; } }
        /// <summary>
        /// 
        /// </summary>
        public bool isOwned { get { return owned; } set { owned = value; } }
        /// <summary>
        /// 
        /// </summary>
        protected bool owned = false;
        /// <summary>
        /// 
        /// </summary>
        public abstract int entityValue { get; }
        /// <summary>
        /// 
        /// </summary>
        public void remove()
        {
            WorldDefinition.world.remove(this);
            if (onEntityRemoved != null) onEntityRemoved(this, null);
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler onEntityRemoved;
        #endregion


        /// <summary>
        /// Utility method for derived classes. Returns true
        /// if a land voxel can be placed at the specified location
        /// </summary>
        public static bool canBeBuilt(Location loc)
        {
            if (WorldDefinition.world.getGroundLevel(loc) != loc.z)
                return false;	// can only be placed on the ground
            return WorldDefinition.world.isReusable(loc);
        }
    }
}
