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

namespace FreeTrain.world.Rail
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="to"></param>
    /// <param name="amount"></param>
    public delegate void TransportEvent(Station to, int amount);
    /// <summary>
    /// GlobalStationListener の概要の説明です。
    /// </summary>
    public class GlobalTrafficMonitor
    {
        static private GlobalTrafficMonitor theInstance = new GlobalTrafficMonitor();
        private GlobalTrafficMonitor() { }
        static internal GlobalTrafficMonitor TheInstance { get { return theInstance; } }
        /// <summary>
        /// 
        /// </summary>
        public TransportEvent OnPassengerTransported;

        internal void NotifyPassengerTransport(Station to, int amount)
        {
            Debug.WriteLine(string.Format("Transport {0} passengers to [{1}].", amount, to));
            if (OnPassengerTransported != null && amount != 0)
                OnPassengerTransported(to, amount);
        }
    }
}
