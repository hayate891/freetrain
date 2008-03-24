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
using FreeTrain.Framework;
using FreeTrain.Util;
using FreeTrain.World.Terrain;
using FreeTrain.Framework.Graphics;

namespace FreeTrain.World.Structs
{
	/// <summary>
	/// Base implementation of the generic "structure"
	/// that occupies a square-shaped block on the ground.
	/// </summary>
	[Serializable]
	public abstract class Structure : Entity
	{
        /// <summary>
        /// 
        /// </summary>
		public Structure() {
		}


		// actually none of the methods are implemented.
		// we just require Structure to implement Entity
		#region Entity implementation
        /// <summary>
        /// 
        /// </summary>
		public abstract bool isSilentlyReclaimable { get; }
        /// <summary>
        /// 
        /// </summary>
		public abstract bool isOwned { get; }
        /// <summary>
        /// 
        /// </summary>
		public abstract void remove();
        /// <summary>
        /// 
        /// </summary>
		public abstract int entityValue { get; }
        /// <summary>
        /// 
        /// </summary>
		public abstract event EventHandler onEntityRemoved;
		#endregion


		/// <summary>
		/// This method is called when one of the voxel is clicked.
		/// </summary>
		public abstract bool onClick();

		/// <summary>
		/// Name of the structure.
		/// </summary>
		public abstract string name { get; }

        ///// <summary>
        ///// Returns true if there is enough space in the spcified location
        ///// to built a structure of a given size.
        ///// 
        ///// Usually, derived classes override this method and add necessary
        ///// checks specific to that structure.
        ///// </summary>
//		public static bool canBeBuilt( Location loc, Distance sz ) 
//		{
//			return canBeBuilt(loc,sz,ControlMode.player);
//		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="sz"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
		public static bool canBeBuilt( Location loc, Distance sz, ControlMode mode ) 
		{
			if(mode == ControlMode.com)
			{
				foreach( Voxel v in Cube.createExclusive(loc,sz).voxels )
					if( !v.entity.isOwned )
						return false;
				return true;
			}
			else
			{
				foreach( Voxel v in Cube.createExclusive(loc,sz).voxels )
					if( !v.entity.isSilentlyReclaimable )
						return false;
				return true;
			}
		}

		/// <summary>
		/// Make sure all the relevant voxels are on the ground
		/// </summary>
		/// <param name="loc"></param>
		/// <param name="sz"></param>
		/// <returns></returns>
		public static bool isOnTheGround( Location loc, Distance sz ) {
			for( int y=0; y<sz.y; y++ )
				for( int x=0; x<sz.x; x++ ) {
					if( WorldDefinition.world.getGroundLevel(loc.x+x,loc.y+y)!=loc.z )
						return false;
					if(WorldDefinition.world[loc.x+x,loc.y+y,loc.z] is MountainVoxel)
						return false;
				}
			return true;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aspect"></param>
        /// <returns></returns>
		public virtual object queryInterface( Type aspect ) { return null; }


		/// <summary>
		/// Individual voxel that a structure occupies.
		/// </summary>
		[Serializable]
		public abstract class StructureVoxel : AbstractVoxelImpl {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="_owner"></param>
            /// <param name="_loc"></param>
			protected StructureVoxel( Structure _owner, Location _loc ) : base(_loc) {
				this.owner = _owner;
			}
            /// <summary>
            /// 
            /// </summary>
            /// <param name="_owner"></param>
            /// <param name="wloc"></param>
			protected StructureVoxel( Structure _owner, WorldLocator wloc ) : base(wloc) {
				this.owner = _owner;
			}

			/// <summary>
			/// The structure object to which this voxel belongs.
			/// </summary>
			public readonly Structure owner;
            /// <summary>
            /// 
            /// </summary>
			public override Entity entity { get { return owner; } }

			/// <summary>
			/// onClick event is delegated to the parent.
			/// </summary>
			public override bool onClick() {
				return owner.onClick();
			}
		}
	}
}
