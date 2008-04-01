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
using FreeTrain.World;
using FreeTrain.World.Structs;
using FreeTrain.Framework.Plugin;

namespace FreeTrain.Contributions.Population
{
    /// <summary>
    /// Population depends on hour of the day
    /// </summary>
    [Serializable]
    public class HourlyPopulation : BasePopulation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="basep"></param>
        /// <param name="weekdayHourTable"></param>
        /// <param name="weekendHourTable"></param>
        public HourlyPopulation(int basep, int[] weekdayHourTable, int[] weekendHourTable)
        {
            Debug.Assert(weekdayHourTable.Length == 24 || weekendHourTable.Length == 24);

            this.population = basep;
            this.weekdayHourTable = weekdayHourTable;
            this.weekendHourTable = weekendHourTable;
        }

        /// <summary>
        /// Ration of each hour in percentage
        /// </summary>
        private readonly int[] weekdayHourTable;
        private readonly int[] weekendHourTable;
        private readonly int population;
        /// <summary>
        /// 
        /// </summary>
        public override int residents { get { return population; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentTime"></param>
        /// <returns></returns>
        public override int calcPopulation(Time currentTime)
        {
            if (currentTime.isWeekend)
                return population * weekendHourTable[currentTime.hour] / 100;
            else
                return population * weekdayHourTable[currentTime.hour] / 100;
        }

    }
}
