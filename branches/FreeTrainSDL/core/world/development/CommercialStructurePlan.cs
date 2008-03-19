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
using FreeTrain.Contributions.Structs;
using FreeTrain.World.Structs;

namespace FreeTrain.World.Development
{
	/// <summary>
	/// CommercialStructurePlan の概要の説明です。
	/// </summary>
	[Serializable]
	class CommercialStructurePlan : Plan
	{
		private readonly CommercialStructureContribution contrib;
		private readonly Location loc;

		internal CommercialStructurePlan(
			CommercialStructureContribution _contrib,
			ULVFactory factory, Location _loc )
			: base( factory.create(Cube.createExclusive(_loc, new Distance(_contrib.size.x, _contrib.size.y, 0) )))
		{
			this.contrib = _contrib;
			this.loc = _loc;
		}

		public override int value { get { return contrib.price; } }

		public override Cube cube { get { return Cube.createExclusive(loc,contrib.size); } }


		public override void build() {
			new ConstructionSite( loc, new EventHandler(handle), contrib.size );
		}

		public void handle( object sender, EventArgs args ) {
			contrib.create(loc,false);
		}
	}
}
