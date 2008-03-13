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

namespace freetrain.world.development
{	
	// TODO: allow a contribution to implement PlanFactory

	/// <summary>
	/// A planned structure that the development algorithm considers.
	/// 
	/// A planned structure has a structure type, the location, and the size in it.
	/// IOW, it has enough information to build itself by the build method
	/// without any external context information.
	/// </summary>
	[Serializable]
	abstract class Plan
	{
		/// <summary>
		/// Gets the ULV of the planned construction site
		/// </summary>
		public readonly ULV ulv;

		/// <summary>
		/// Value of the planned new structure.
		/// </summary>
		public abstract int value { get; }
		
		/// <summary>
		/// Bias can be used to change the likelihood of this plan to
		/// be realized. 1 is the normal value. Bigger value means better
		/// chance, smaller value means lower chance.
		/// The likelihood will be doubled if the bias is doubled
		/// </summary>
		public virtual double bias { get { return 1.0; } }
		// TODO

		/// <summary>
		/// Bounding cube that this plan will occuply (once realized)
		/// </summary>
		public abstract Cube cube { get; }

		/// <summary>
		/// Builds the structure in the world.
		/// </summary>
		public abstract void build();

		protected Plan( ULV ulv ) {
			this.ulv = ulv;
		}
	}
}
