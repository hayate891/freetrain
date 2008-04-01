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
using System.Xml;
using FreeTrain.Framework.Plugin;

namespace FreeTrain.Contributions.Population
{
    /// <summary>
    /// HourlyPopulation with a typical distribution for
    /// restaurants.
    /// </summary>
    [Serializable]
    public class RestaurantPopulation : HourlyPopulation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseP"></param>
        public RestaurantPopulation(int baseP) : base(baseP, weekdayDistribution, weekdayDistribution) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public RestaurantPopulation(XmlElement e)
            : this(int.Parse(XmlUtil.selectSingleNode(e, "base").InnerText)) { }

        private static readonly int[] weekdayDistribution = new int[]{
			  0,  0,  0,  0,  0,  0,	//  0:00- 5:00
			  0,  0,  0,  5, 20, 75,	//  6:00-11:00
			100, 75, 30, 10,  0, 10,	// 12:00-17:00
			 50, 80, 80, 40, 10,  0,	// 18:00-23:00
		};

        // TODO: weekend distribution
    }
}
