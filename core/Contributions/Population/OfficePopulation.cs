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
    /// residential structures (such as houses, apartments, etc.)
    /// </summary>
    [Serializable]
    public class OfficePopulation : HourlyPopulation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseP"></param>
        public OfficePopulation(int baseP) : base(baseP, weekdayDistribution, weekdayDistribution) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public OfficePopulation(XmlElement e)
            : this(int.Parse(XmlUtil.SelectSingleNode(e, "base").InnerText)) { }

        private static readonly int[] weekdayDistribution = new int[]{
			 10,  5,  5,  5,  5,  5,	//  0:00- 5:00
			 10, 10, 20, 20, 20, 20,	//  6:00-11:00
			 20, 25, 25, 25, 30, 80,	// 12:00-17:00
			100, 80, 50, 30, 20, 10,	// 18:00-23:00
		};

        // TODO: weekend distribution
    }
}
