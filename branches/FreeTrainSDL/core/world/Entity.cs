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

namespace freetrain.world
{
	/// <summary>
	/// Set of voxels that behaves as one logical concept.
	/// 
	/// Often multiple voxels are used together to represent a single concept.
	/// For example, a building usually consists of multiple voxels, whereas
	/// each voxel of a crop field behaves independently.
	/// 
	/// The notion of entity is primarily introduced to support the development
	/// algorithm where we need to consider a structure as "one thing" rather than
	/// a set of seemingly unrelated voxels.
	/// 
	/// The entity interface is accessible through the Voxel.queryInterface method.
	/// If two entity objects e1 and e2 are the same (i.e., e1==e2) then they are
	/// considered to the same entity. IOW, if two voxels return the same Entity object,
	/// that means those two voxels belong to the same entity.
	/// 
	/// TODO: this concept needs to be refined.
	/// 
	/// TODO: it's not clear why multiple tunnel voxels don't form one entity, for example.
	/// </summary>
	public interface Entity
	{
		/// <summary>
		/// The monetary value of the entity.
		/// </summary>
		int entityValue { get; }
		// TODO: not sure what this means.
		// this is certainly not a cash value when you buy/sell the property



		/// <summary>
		/// True if this entity belongs to the user, false otherwise.
		/// See the isSilentlyReclaimable property for details.
		/// </summary>
		bool isOwned { get; }
		// TODO: whether the ownership can be changed is a different issue,
		// and should be provided in a separate interface

		/// <summary>
		/// True if this entity can be torn down (through the remove method)
		/// by the user without any consent.
		/// 
		/// For example, crop fields are isSilentlyRecilaimable  !isOwned.
		/// These entities can be automatically destructed as the user places
		/// rail road and structures.
		/// 
		/// Mountains are examples of !isSilentlyReclaimable !isOwned.
		/// Stations are examples of !isSilentlyReclaimable isOwned, and
		/// finally land properties are examples of isSilentlyReclaimable
		///  isOwned.
		/// </summary>
		bool isSilentlyReclaimable { get; }

		/// <summary>
		/// Removes this entity from the world. If this entity is owned
		/// by the user, the callee will charge the cost to the user's account.
		/// 
		/// Note that this method can be called not just because of an user's action.
		/// For example, the development algorithm can choose to tear down some
		/// non-user owned structure.
		/// 
		/// This method must succeed.
		/// </summary>
		void remove();

		// TODO: have the isRemovable method which determines
		// whether the development algorithm can remove it
		
        /// <summary>
        /// 
        /// </summary>
		event EventHandler onEntityRemoved;

		/// <summary>
		/// Query this entity to return some "aspect" of it.
		/// 
		/// Aspect is usually a tear-off interface that allows
		/// entities to be extended through compositions.
		/// 
		/// <returns>null if the given aspect is not supported.</returns>
        /// </summary>
		object queryInterface( Type aspect );
	}
}
