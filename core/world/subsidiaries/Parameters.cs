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

namespace FreeTrain.world.subsidiaries
{
    internal sealed class Parameters
    {
        private Parameters() { }	// no instanciation

        /// <summary>
        /// Compute the profit of the given structure.
        /// </summary>
        public static long profit(long structurePrice, long landPrice)
        {
            long T = structurePrice;
            if (T < landPrice)
                return (long)(maxProfitPrime(T) * 2 * (landPrice - T) + maxProfit(T));
            else
                return (long)(maxProfitPrime(T) / 2 * (landPrice - T) + maxProfit(T));
        }

        public static long operationCost(long structurePrice, long landPrice)
        {
            return profit(structurePrice, 0) + landPrice;
        }

        public static long sales(long structurePrice, long landPrice)
        {
            return operationCost(structurePrice, landPrice) + profit(structurePrice, landPrice);
        }


        /// <summary>
        /// Max profit curve. 
        /// </summary>
        private static double maxProfit(long landPrice)
        {
            landPrice++;	// avoid log(0)
            return C * landPrice * Math.Log(landPrice) + D;
        }

        /// <summary>
        /// Derivative of maxProfit
        /// </summary>
        private static double maxProfitPrime(long landPrice)
        {
            landPrice++;	// avoid log(0)
            return C + C * Math.Log(landPrice);
        }

        private const double C = 0.01;
        private const double D = 0;

    }
}
