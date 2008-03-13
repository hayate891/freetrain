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

namespace freetrain.world.accounting
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public abstract class TransactionSummary
    {
        /// <summary>
        /// 
        /// </summary>
        public abstract long sales { get; }
        /// <summary>
        /// 
        /// </summary>
        public abstract long expenditures { get; }
        /// <summary>
        /// 
        /// </summary>
        public long balance { get { return sales - expenditures; } }
    }

    /// <summary>
    /// Records the summary of past transactions.
    /// </summary>
    [Serializable]
    public class TransactionHistory
    {
        [Serializable]
        private class Recorder
        {
            private long _dayTotal;
            private long _monthTotal;
            private long _yearTotal;
            private readonly Clock clock = World.world.clock;

            internal Recorder()
            {
                // align the clock to 0:00am
                clock.registerRepeated(
                    new ClockHandler(onClock),
                    TimeLength.fromMinutes(
                        TimeLength.ONEDAY.totalMinutes - (clock.totalMinutes % TimeLength.ONEDAY.totalMinutes)),
                    TimeLength.ONEDAY);
            }

            internal long dayTotal { get { return _dayTotal; } }
            internal long monthTotal { get { return _dayTotal + _monthTotal; } }
            internal long yearTotal { get { return monthTotal + _yearTotal; } }

            internal void add(long delta)
            {
                _dayTotal += delta;
            }
            /// <summary>
            /// 
            /// </summary>
            public void onClock()
            {
                _monthTotal += _dayTotal;
                _dayTotal = 0;
                if (clock.day == 1)
                {
                    _yearTotal += _monthTotal;
                    _monthTotal = 0;
                    if (clock.month == 4)
                    {
                        _yearTotal = 0;
                    }
                }
            }
        }

        // used to record sales and expenditures
        private readonly Recorder sales = new Recorder();
        private readonly Recorder expenditures = new Recorder();

        // expose those information to outside
        /// <summary>
        /// 
        /// </summary>
        public readonly TransactionSummary day;
        /// <summary>
        /// 
        /// </summary>
        public readonly TransactionSummary month;
        /// <summary>
        /// 
        /// </summary>
        public readonly TransactionSummary year;

        /// <summary>
        /// Record transactions of the given genre.
        /// </summary>
        public TransactionHistory()
        {
            day = new DayTransactionSummary(this);
            month = new MonthTransactionSummary(this);
            year = new YearTransactionSummary(this);
        }

        internal void earn(long delta)
        {
            sales.add(delta);
        }

        internal void spend(long delta)
        {
            expenditures.add(delta);
        }

        [Serializable]
        private class DayTransactionSummary : TransactionSummary
        {
            private readonly TransactionHistory history;

            internal DayTransactionSummary(TransactionHistory _history)
            {
                this.history = _history;
            }
            /// <summary>
            /// 
            /// </summary>
            public override long sales { get { return history.sales.dayTotal; } }
            /// <summary>
            /// 
            /// </summary>
            public override long expenditures { get { return history.expenditures.dayTotal; } }
        }

        [Serializable]
        private class MonthTransactionSummary : TransactionSummary
        {
            private readonly TransactionHistory history;

            internal MonthTransactionSummary(TransactionHistory _history)
            {
                this.history = _history;
            }
            /// <summary>
            /// 
            /// </summary>
            public override long sales { get { return history.sales.monthTotal; } }
            /// <summary>
            /// 
            /// </summary>
            public override long expenditures { get { return history.expenditures.monthTotal; } }
        }

        [Serializable]
        private class YearTransactionSummary : TransactionSummary
        {
            private readonly TransactionHistory history;

            internal YearTransactionSummary(TransactionHistory _history)
            {
                this.history = _history;
            }
            /// <summary>
            /// 
            /// </summary>
            public override long sales { get { return history.sales.yearTotal; } }
            /// <summary>
            /// 
            /// </summary>
            public override long expenditures { get { return history.expenditures.yearTotal; } }
        }

    }
}
