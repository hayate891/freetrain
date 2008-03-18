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

namespace FreeTrain.world
{
	/// <summary>
	/// Receive notifications of changes in voxel outlook.
	/// </summary>
	public interface VoxelOutlookListener
	{
		/// <summary>
		/// Called when all the voxels need to be fully updated.
		/// </summary>
		void onUpdateAllVoxels();

		/// <summary>
		/// Called when a particular voxel is updated.
		/// </summary>
		void onUpdateVoxel( Location loc ); 

		/// <summary>
		/// Called when a cube of voxels are updated.
		/// </summary>
		void onUpdateVoxel( Cube cube );
	}
}
