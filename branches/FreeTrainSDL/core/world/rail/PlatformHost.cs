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

namespace freetrain.world.rail
{
    /// <summary>
    /// A structure that can host platforms.
    /// 
    /// Nodes added to this host will be notified when
    /// the host is destroied.
    /// </summary>
    public interface PlatformHost : Entity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="platform"></param>
        void addNode(Platform platform);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="platform"></param>
        void removeNode(Platform platform);
        /// <summary>
        /// 
        /// </summary>
        Station hostStation { get; }
    }
}
