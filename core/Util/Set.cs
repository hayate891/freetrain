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
    /// java.util.Set
    /// </summary>
    [Serializable]
    public sealed class Set : ICollection
    {
        private readonly Hashtable core = new Hashtable();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool contains(object o) { return core.Contains(o); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool add(object o)
        {
            if (!core.ContainsKey(o))
            {
                core.Add(o, o);
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        public void remove(object o) { core.Remove(o); }
        /// <summary>
        /// 
        /// </summary>
        public void clear() { core.Clear(); }
        /// <summary>
        /// 
        /// </summary>
        public int Count { get { return core.Count; } }
        /// <summary>
        /// 
        /// </summary>
        public bool IsSynchronized { get { return false; } }
        /// <summary>
        /// 
        /// </summary>
        public object SyncRoot { get { return this; } }
        /// <summary>
        /// 
        /// 
        /// </summary>
        public bool isEmpty { get { return Count == 0; } }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return core.Keys.GetEnumerator();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object getOne()
        {
            IEnumerator e = GetEnumerator();
            e.MoveNext();
            return e.Current;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Array toArray(Type type)
        {
            Array r = Array.CreateInstance(type, Count);
            int idx = 0;

            foreach (object o in this)
                r.SetValue(o, idx++);
            return r;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Array array, int index)
        {
            foreach (object o in this)
                array.SetValue(o, index++);
        }
    }
}
