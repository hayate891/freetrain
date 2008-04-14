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
using System.Runtime.Serialization;
using FreeTrain.Util;

namespace FreeTrain.World.Subsidiaries
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="company"></param>
    public delegate void SubsidiaryMarketListener(SubsidiaryMarket sender, SubsidiaryCompany company);


    /// <summary>
    /// A list of <c>SubsidiaryCompany</c>s that are being sold.
    /// </summary>
    [Serializable]
    public class SubsidiaryMarket : IDeserializationCallback
    {
        private SubsidiaryMarket()
        {
            volatileEvents = new Events();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        public void OnDeserialization(object sender)
        {
            volatileEvents = new Events();
        }


        /// <summary>
        /// The market for third party companies. The player can buy them.
        /// </summary>
        public static SubsidiaryMarket BUY
        {
            get
            {
                return theInstance("buy");
            }
        }
        /// <summary>
        /// The market for companies owned by the player. The player can sell them.
        /// </summary>
        public static SubsidiaryMarket SELL
        {
            get
            {
                return theInstance("sell");
            }
        }

        private static SubsidiaryMarket theInstance(string suffix)
        {
            string name = typeof(SubsidiaryMarket).Name + suffix;
            SubsidiaryMarket r = (SubsidiaryMarket)WorldDefinition.World.OtherObjects[name];
            if (r == null)
                WorldDefinition.World.OtherObjects[name] = r = new SubsidiaryMarket();
            return r;
        }



        /// <summary>
        /// Set of on-sale companies.
        /// </summary>
        private readonly Set onSale = new Set();
        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        public class Events
        {
            /// <summary>
            /// Fires when a new company enters the market.
            /// </summary>
            public SubsidiaryMarketListener onAdded;

            /// <summary>
            /// Fires when a company leaves the market.
            /// </summary>
            public SubsidiaryMarketListener onRemoved;
        }
        /// <summary>
        /// 
        /// </summary>
        public readonly Events persistentEvents = new Events();
        /// <summary>
        /// 
        /// </summary>
        [NonSerialized]
        public Events volatileEvents;


        /// <summary>
        /// Returns the list of companies on sale.
        /// </summary>
        public SubsidiaryCompany[] companiesOnSale
        {
            get
            {
                return (SubsidiaryCompany[])onSale.ToArray(typeof(SubsidiaryCompany));
            }
        }

        /// <summary>
        /// Shouldn't be called from outside the package.
        /// </summary>
        internal void add(SubsidiaryCompany company)
        {
            onSale.Add(company);
            if (persistentEvents.onAdded != null)
                persistentEvents.onAdded(this, company);
            if (volatileEvents.onAdded != null)
                volatileEvents.onAdded(this, company);
        }

        /// <summary>
        /// Shouldn't be called from outside the package.
        /// </summary>
        internal void remove(SubsidiaryCompany company)
        {
            onSale.Remove(company);
            if (persistentEvents.onRemoved != null)
                persistentEvents.onRemoved(this, company);
            if (volatileEvents.onRemoved != null)
                volatileEvents.onRemoved(this, company);
        }

        /// <summary>
        /// Shouldn't be called from outside the package.
        /// </summary>
        internal bool contains(SubsidiaryCompany company)
        {
            return onSale.Contains(company);
        }
    }
}
