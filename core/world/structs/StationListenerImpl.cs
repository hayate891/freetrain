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
using System.Collections;
using System.Diagnostics;
using FreeTrain.Contributions.Population;
using FreeTrain.Framework.Plugin;
using FreeTrain.World.Rail;

namespace FreeTrain.World.Structs
{
    /// <summary>
    /// StationListener implementation that uses
    /// Population object to calculate population.
    /// </summary>
    [Serializable]
    public class StationListenerImpl : Rail.StationListener
    {
        /// <summary>
        /// 
        /// </summary>
        public const int MaxStationCount = 4;
        /// <param name="pop">Population pattern</param>
        /// <param name="loc">The location used to decide if this object
        /// can subscribe to a given station.</param>
        public StationListenerImpl(BasePopulation pop, Location loc)
        {
            this.population = pop;
            this.location = loc;
            stations = new ArrayList(MaxStationCount);
            if (population != null)
                attachToStation();	// attach to the existing station if any
        }

        /// <summary>
        /// Station to which this structure sends population to.
        /// </summary>
        //private Station station;
        private ArrayList stations;

        private readonly Location location;

        private readonly BasePopulation population;


        /// <summary>
        /// Should be called when the owner is removed.
        /// </summary>
        public void onRemoved()
        {
            // remove from the currently attached station
            foreach (Station station in stations)
            {
                station.listeners.remove(this);
            }
            stations.Clear();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public int getPopulation(Station s)
        {
            int v = WorldDefinition.world.landValue[location];
            int p = population.calcPopulation(WorldDefinition.world.clock);
            p /= stations.Count;
            return Math.Min(p, v + 10);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool advertiseStation(Station s)
        {
            // keep stations within 4
            if (stations.Count < MaxStationCount)
                s.listeners.add(this);
            else
            {
                int dmax = location.distanceTo(s.baseLocation);
                Station remove = null;
                foreach (Station station in stations)
                {
                    int d = location.distanceTo(station.baseLocation);
                    if (d > dmax)
                    {
                        remove = station;
                        dmax = d;
                    }
                }
                if (remove != null)
                {
                    remove.listeners.remove(this);
                    stations[stations.IndexOf(remove)] = s;
                }
                return false;
            }
            stations.Add(s);
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public void onStationRemoved(Station s)
        {
            stations.Remove(s);
        }

        /// <summary>
        /// Finds the nearest station and attaches to it.
        /// </summary>
        private void attachToStation()
        {
            foreach (Station s in WorldDefinition.world.stations)
            {
                if (!s.withinReach(location))
                    continue;
                advertiseStation(s);
            }
        }
    }
}
