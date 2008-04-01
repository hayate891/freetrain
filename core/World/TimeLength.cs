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

namespace FreeTrain.World
{
    /// <summary>
    /// Span of time
    /// </summary>
    [Serializable]
    public struct TimeLength
    {
        private TimeLength(long min) { totalMinutes = min; }

        // create new time span objects that correspond to the specified period of the time.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="min"></param>
        /// <returns></returns>
        public static TimeLength fromMinutes(long min) { return new TimeLength(min); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static TimeLength fromHours(long hours) { return new TimeLength(hours * 60); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public static TimeLength fromDays(long days) { return TimeLength.fromHours(days * 24); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static TimeLength random(TimeLength min, TimeLength max)
        {
            return new TimeLength((long)
                (rnd.NextDouble() * (max.totalMinutes - min.totalMinutes) + min.totalMinutes));
        }

        // create a time span object that represents the period until tomorrow's 0:00.
        // if the current time is just 0:00, it returns "24hours" rather than 0.
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static TimeLength untilTomorrow()
        {
            Clock c = WorldDefinition.World.clock;
            return fromMinutes(ONEDAY.totalMinutes - (c.hour * 60 + c.minutes) % ONEDAY.totalMinutes);
        }

        //
        // time constants
        //
        /// <summary>
        /// 
        /// </summary>
        public static readonly TimeLength ZERO = new TimeLength(0);
        /// <summary>
        /// 
        /// </summary>
        public static readonly TimeLength ONEDAY = new TimeLength(60 * 24);

        private static Random rnd = new Random();

        /// <summary>
        /// Time span in minutes.
        /// </summary>
        public long totalMinutes;
        /// <summary>
        /// 
        /// </summary>
        public bool isPositive { get { return totalMinutes > 0; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static TimeLength operator +(TimeLength a, TimeLength b)
        {
            return new TimeLength(a.totalMinutes + b.totalMinutes);
        }
    }
}
