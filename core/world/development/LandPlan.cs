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
using FreeTrain.Contributions.land;

namespace FreeTrain.world.development
{
	/// <summary>
	/// Plan of land surfaces such as crop fields.
	/// </summary>
	[Serializable]
	class LandPlan : Plan
	{
		private readonly LandBuilderContribution contrib;
		private readonly Location loc;
		private readonly SIZE size;

		internal LandPlan( LandBuilderContribution _contrib, ULVFactory factory, Location _loc, SIZE _size )
			: base(factory.create(new Cube(_loc,_size.x,_size.y,0))) {
			this.contrib = _contrib;
			this.loc = _loc;
			this.size = _size;
		}

		public override int value { get { return contrib.price*4; } }

		public override Cube cube { get { return new Cube(loc,size.x,size.y,1); } }

		public override void build() {
			contrib.create(loc,loc+new Distance(size.x-1,size.y-1,0),false);	// inclusive
		}
	}

}
