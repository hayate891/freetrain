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
using System.Windows.Forms;
using FreeTrain.Framework;
using FreeTrain.Util;

namespace FreeTrain.World.Accounting
{
    /// <summary>
    /// 
    /// </summary>
    public delegate void AccountListener();
    //	public delegate void AccountTransactionListener( long delta, AccountGenre genre );

    /// <summary>
    /// Maintains accounting and financing.
    /// </summary>
    [Serializable]
    public class AccountManager
    {
        /// <summary> Debts. </summary>
        private readonly Set debts = new Set();

        /// <summary>
        /// This event is fired everytime there's a change
        /// in the account. Parameters are not used.
        /// </summary>
        public static AccountListener onAccountChange;

        /// <summary>
        /// Obtain a reference to the sole instance.
        /// </summary>
        public static AccountManager theInstance
        {
            get
            {
                return WorldDefinition.world.account;
            }
        }

        /// <summary>
        /// The current liquid assets.
        /// One can think of this as cold cash
        /// (though in reality a company never really has cash.)
        /// 
        /// The game is over if the liquid assets goes below zero.
        /// </summary>
        private long _liquidAssets;

        /// <summary>
        /// Total amount of debts.
        /// </summary>
        private long _totalDebts;
        //
        //		/// <summary> Fired when there is a transaction. </summary>
        //		public event AccountTransactionListener onEarned;
        //		public event AccountTransactionListener onSpent;

        /// <summary>
        /// 
        /// </summary>
        public long liquidAssets { get { return _liquidAssets; } }
        /// <summary>
        /// 
        /// </summary>
        public long totalDebts { get { return _totalDebts; } }


        /// <summary>
        /// 
        /// </summary>
        public AccountManager() : this(1500 * 10000) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialLiquidAssets"></param>
        public AccountManager(long initialLiquidAssets)
        {
            this._liquidAssets = initialLiquidAssets;
        }

        private void transact(long delta, AccountGenre genre)
        {
            _liquidAssets -= delta;
            if (_liquidAssets < 0)
            {
                // TODO: go bunkrupt
                MessageBox.Show(MainWindow.mainWindow, "You are bankrupt. Proceeding with more funds.");
                //! MessageBox.Show( MainWindow.mainWindow, "破産しました。お金を増やして続行します" );
                _liquidAssets += 100000000;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="delta"></param>
        /// <param name="genre"></param>
        public void spend(long delta, AccountGenre genre)
        {
            transact(delta, genre);
            genre.History.spend(delta);
            if (genre.onUpdate != null) genre.onUpdate();
            if (onAccountChange != null) onAccountChange();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="delta"></param>
        /// <param name="genre"></param>
        public void earn(long delta, AccountGenre genre)
        {
            transact(-delta, genre);
            genre.History.earn(delta);
            if (genre.onUpdate != null) genre.onUpdate();
            if (onAccountChange != null) onAccountChange();
        }

        internal void addDebt(Debt debt)
        {
            debts.add(debt);
            updateTotalDebts();
        }

        internal void removeDebt(Debt debt)
        {
            debts.remove(debt);
            updateTotalDebts();
        }

        private void updateTotalDebts()
        {
            long sum = 0;
            foreach (Debt d in debts)
                sum += d.amount;

            _totalDebts = sum;
            if (onAccountChange != null) onAccountChange();
        }
    }
}
