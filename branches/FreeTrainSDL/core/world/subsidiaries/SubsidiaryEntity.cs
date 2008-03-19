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

namespace FreeTrain.World.Subsidiaries
{
    /// <summary>
    /// 
    /// </summary>
    public interface SubsidiaryEntity : Entity
    {
        /// <summary>
        /// 
        /// </summary>
        string name { get; }

        /// <summary>
        /// price of the structure
        /// </summary>
        long structurePrice { get; }

        /// <summary>
        /// Sum of the land prices.
        /// </summary>
        long totalLandPrice { get; }

        /// <summary>
        /// Returns the location such that the returned value <code>v</code>
        /// will satisfy <code>World.world[v].entity==this</code>.
        /// 
        /// It is desirable for this method to return the location close to the
        /// center of the entity.
        /// </summary>
        Location locationClue { get; }
    }
}
