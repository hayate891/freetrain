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

namespace FreeTrain.world.Rail
{
    /// <summary>
    /// Structures that "use" a station.
    /// 
    /// This interface is implemented by structures that have
    /// population that uses a station. Because of the way
    /// stations find listeners, listeners need to occupy
    /// at least one voxel.
    /// 
    /// StationListener interface should be accessible through the queryAspect method.
    /// </summary>
    public interface StationListener
    {
        /// <summary> Obtains the population that uses a station right now. </summary>
        /// <remarks>Usually this value varies depending on the current time.</remarks>
        int getPopulation(Station s);

        /// <summary>
        /// Notifies the removal of the station.
        /// </summary>
        /// <remarks>
        /// Affected listener should look for another station to attach.
        /// listeners will be removed automatically from the old station,
        /// so don't call the <code>listeners.remove</code> method.
        /// </remarks>
        void onStationRemoved(Station s);

        /// <summary>
        /// Notifies a newly created station.
        /// </summary>
        /// <remarks>
        /// This method is called by a newly created station object
        /// to "recruit" existing listeners to the new station.
        /// This method is called only when the receiving listener
        /// is eligible to attach to the new station.
        /// </remarks>
        /// <param name="s"></param>
        /// <returns>true if succesfuly advertised</returns>
        bool advertiseStation(Station s);
    }
}
