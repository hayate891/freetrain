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

namespace FreeTrain.World.Accounting
{
    /// <summary>
    /// Accounting genre. Used to categorize expenses and sales.
    /// </summary>
    [Serializable]
    public class AccountGenre : Contribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public AccountGenre(XmlElement e)
            : base(e)
        {
            name = XmlUtil.SelectSingleNode(e, "name").InnerText;
            WorldDefinition.onNewWorld += new EventHandler(onNewWorld);
        }

        /// <summary> Name of this genre. </summary>
        public readonly string name;

        private TransactionHistory _history;


        /// <summary> Notified whenever the data gets updated. </summary>
        [NonSerialized]
        public AccountListener onUpdate;

        /// <summary>
        /// Get the transaction history of this genre.
        /// </summary>
        public TransactionHistory History
        {
            get
            {
                if (_history == null)
                {
                    _history = (TransactionHistory)WorldDefinition.World.OtherObjects[this];
                    if (_history == null)
                        WorldDefinition.World.OtherObjects.Add(this, _history = new TransactionHistory());
                }

                return _history;
            }
        }

        private void onNewWorld(object sender, EventArgs e)
        {
            _history = null;
        }

        /// <summary>
        /// Short-cut to the <code>AccountManager.spend</code> method.
        /// </summary>
        /// <param name="delta"></param>
        public void Spend(long delta)
        {
            AccountManager.theInstance.spend(delta, this);
        }

        /// <summary>
        /// Short-cut to the <code>AccountManager.earn</code> method.
        /// </summary>
        /// <param name="delta"></param>
        public void Earn(long delta)
        {
            AccountManager.theInstance.earn(delta, this);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() { return name; }


        //
        // Built-in genre.
        //
        /// <summary>
        /// 
        /// </summary>
        public static AccountGenre RailService
        {
            get
            {
                return (AccountGenre)PluginManager.theInstance
                    .getContribution("{AC30BB0B-044C-4784-A83D-FCB1F60B3CF2}");
            }
        }
        /// <summary>
        /// 
        /// 
        /// </summary>
        public static AccountGenre RoadService
        {
            get
            {
                return (AccountGenre)PluginManager.theInstance
                    .getContribution("{CC00A6D1-D078-4D3C-AFB2-EDC6CB9F4CB3}");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static AccountGenre Subsidiaries
        {
            get
            {
                return (AccountGenre)PluginManager.theInstance
                    .getContribution("{2A666F1A-9F40-4F67-98F4-DEAC1E55296D}");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static AccountGenre Others
        {
            get
            {
                return (AccountGenre)PluginManager.theInstance
                    .getContribution("{C0A9ABA5-801F-4AA6-93EA-6FF563C2B407}");
            }
        }
    }
}
