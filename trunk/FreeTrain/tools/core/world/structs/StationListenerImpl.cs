using System;
using System.Diagnostics;
using freetrain.contributions.population;
using freetrain.framework.plugin;
using freetrain.world.rail;

namespace freetrain.world.structs
{
	/// <summary>
	/// StationListener implementation that uses
	/// Population object to calculate population.
	/// </summary>
	[Serializable]
	public class StationListenerImpl : rail.StationListener
	{
		/// <param name="pop">Population pattern</param>
		/// <param name="loc">The location used to decide if this object
		/// can subscribe to a given station.</param>
		public StationListenerImpl( Population pop, Location loc ) {
			this.population = pop;
			this.location = loc;

			if( population!=null )
				attachToStation();	// attach to the existing station if any
		}

		/// <summary>
		/// Station to which this structure sends population to.
		/// </summary>
		private Station station;

		private readonly Location location;

		private readonly Population population;


		/// <summary>
		/// Should be called when the owner is removed.
		/// </summary>
		public void onRemoved() {
			// remove from the currently attached station
			if(station!=null)
				station.listeners.remove(this);
		}

		public int getPopulation( Station s ) {
			return population.calcPopulation(World.world.clock);
		}

		public void advertiseStation( Station s ) {
			if(station==null) {
				// attach to it
				station = s;
				station.listeners.add(this);
			} else
			if( location.distanceTo(s.baseLocation) < location.distanceTo(station.baseLocation) ) {
				// change to this new station
				station.listeners.remove(this);
				station = s;
				station.listeners.add(this);
			}
		}

		public void onStationRemoved( Station s ) {
			Debug.Assert(this.station==s);
			station = null;

			attachToStation();	// find another station to attach
		}

		/// <summary>
		/// Finds the nearest station and attaches to it.
		/// </summary>
		private void attachToStation() {
			Debug.Assert(station==null);

			int dist = int.MaxValue;
			foreach( Station s in World.world.stations ) {
				if( !s.withinReach(location) )
					continue;

				int d = s.baseLocation.distanceTo(location);
				if( d < dist ) {
					dist = d;
					station = s;
				}
			}

			if(station!=null)
				// register this object as a listener to the station
				station.listeners.add(this);
		}
	}
}
