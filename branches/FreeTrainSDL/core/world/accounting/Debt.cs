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

namespace FreeTrain.World.Accounting
{
    /// <summary>
    /// Payable.
    /// </summary>
    [Serializable]
    public class Debt
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="due"></param>
        /// <param name="genre"></param>
        public Debt(long amount, Time due, AccountGenre genre)
        {
            this.amount = amount;
            this.due = due;
            this.genre = genre;

            manager.addDebt(this);
            WorldDefinition.world.clock.registerOneShot(new ClockHandler(onDue), span);
        }


        /// <summary> Amount due. </summary>
        public readonly long amount;

        /// <summary> Due date. </summary>
        public readonly Time due;

        /// <summary> Genre. </summary>
        public readonly AccountGenre genre;

        /// <summary> TimeLength before the due date </summary>
        public TimeLength span
        {
            get { return due - WorldDefinition.world.clock; }
        }

        /// <summary>
        /// Called automatically by the clock when the time comes to
        /// return the debt.
        /// </summary>
        public virtual void onDue()
        {
            manager.spend(amount, genre);
            manager.removeDebt(this);
        }

        private AccountManager manager { get { return WorldDefinition.world.account; } }
    }
}
