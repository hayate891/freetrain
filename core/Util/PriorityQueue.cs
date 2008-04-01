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
using System.Collections;

namespace FreeTrain.Util
{
    /// <summary>
    /// Priority queue implementation
    /// </summary>
    [Serializable]
    public class PriorityQueue
    {
        /// <summary>
        /// 
        /// </summary>
        public PriorityQueue() : this(Comparer.Default) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="comp"></param>
        public PriorityQueue(IComparer comp)
        {
            this.comparer = comp;
        }

        /// <summary>
        /// Actual data structure that realizes the priority queue.
        /// </summary>
        private readonly SortedList core = new SortedList();

        private readonly IComparer comparer;

        /// <summary>
        /// Inserts a new object into the queue.
        /// </summary>
        public void insert(object priority, object value)
        {
            Entry e = new Entry(priority, value, comparer, idGen++);
            core.Add(e, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public delegate bool ValueComparer(object o1, object o2);

        private ValueComparer valueComparer;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vc"></param>
        public void setValueComparer(ValueComparer vc)
        {
            this.valueComparer = vc;
        }

        /// <summary>
        /// Removes all the items that has the given value.
        /// </summary>
        public void remove(object value)
        {
            for (int i = core.Count - 1; i >= 0; i--)
            {
                Entry e = (Entry)core.GetKey(i);
                if (e.value.Equals(value))
                    core.RemoveAt(i);
            }
        }

        /// <summary>
        /// Gets the object with the lowest priority.
        /// </summary>
        public object minValue
        {
            get
            {
                return ((Entry)core.GetKey(0)).value;
            }
        }

        /// <summary>
        /// Gets the lowest priority in the queue.
        /// </summary>
        public object minPriority
        {
            get
            {
                return ((Entry)core.GetKey(0)).priority;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int count { get { return core.Count; } }

        /// <summary>
        /// Removes the object with the lowest priority.
        /// </summary>
        public void removeMin()
        {
            core.RemoveAt(0);
        }

        /// <summary>Used to generate unique id numbers.</summary>
        private int idGen = 0;


        /// <summary>
        /// This object will be stored into the SortedList.
        /// </summary>
        [Serializable]
        protected class Entry : IComparable
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="p"></param>
            /// <param name="v"></param>
            /// <param name="c"></param>
            /// <param name="_id"></param>
            public Entry(object p, object v, IComparer c, int _id)
            {
                this.priority = p; this.value = v;
                this.comparar = c; this.id = _id;
            }
            /// <summary>
            /// 
            /// </summary>
            public readonly object priority;
            /// <summary>
            /// 
            /// </summary>
            public readonly object value;

            /// <summary>
            /// Unique id that is used to introduce the order relationship between
            /// two objects with the same priority.
            /// </summary>
            private readonly int id;

            private readonly IComparer comparar;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="o"></param>
            /// <returns></returns>
            public int CompareTo(object o)
            {
                Entry rhs = (Entry)o;

                int r;
                if (comparar != null)
                    r = comparar.Compare(priority, rhs.priority);
                else
                    r = ((IComparable)priority).CompareTo(rhs.priority);

                if (r != 0) return r;

                // if two priorities are the same, use the id number
                // so that no two objects will be considered equal.
                return id - rhs.id;
            }
        }
    }
}
