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
using freetrain.contributions.common;
using freetrain.world.rail;
using freetrain.framework.plugin;

namespace freetrain.world.structs
{
	/// <summary>
	/// Structure that has population.
	/// </summary>
	// TODO: this doesn't work quite well. for example, VarHeightBuilding is a populated structure
	// but doesn't derive from this class. Needs to be fixed.
	[Serializable]
	public abstract class PopulatedStructure : PThreeDimStructure
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="wloc"></param>
		public PopulatedStructure( FixedSizeStructureContribution type, WorldLocator wloc )
			: base(type,wloc) {

			if( type.population!=null && wloc.world==World.world)
				stationListener = new StationListenerImpl( type.population, wloc.location );
		}

		/// <summary>
		/// Station to which this structure sends population to.
		/// </summary>
		private readonly StationListenerImpl stationListener;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aspect"></param>
        /// <returns></returns>
		public override object queryInterface( Type aspect ) {
			// if type.population is null, we don't have any population
			if( aspect==typeof(rail.StationListener) )
				return stationListener;
			else
				return base.queryInterface(aspect);
		}

        /// <summary>
        /// 
        /// </summary>
		public override void remove() {
			base.remove();

			if( stationListener!=null )
				stationListener.onRemoved();
		}
	}
}
