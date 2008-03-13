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

namespace freetrain.world.rail
{
    /// <summary>
    /// Group of trains and child train groups.
    /// </summary>
    [Serializable]
    public class TrainGroup : TrainItem
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="name"></param>
        public TrainGroup(TrainGroup group, string name) : base(group, name) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        public TrainGroup(TrainGroup group)
            : this(group, string.Format("Group {0}", iota++))
        {
            //! public TrainGroup(TrainGroup group) : this(group,string.Format("グループ{0}",iota++)) {
            controller = DelegationTrainControllerImpl.theInstance;
        }
        private static int iota = 1;

        /// <summary>
        /// 
        /// </summary>
        public readonly TrainCollection items = new TrainCollection();

        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        public class TrainCollection : CollectionBase
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="t"></param>
            public void add(TrainItem t)
            {
                base.List.Add(t);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="idx"></param>
            /// <returns></returns>
            public TrainItem get(int idx)
            {
                return (TrainItem)base.List[idx];
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="t"></param>
            public void remove(TrainItem t)
            {
                base.List.Remove(t);
            }
        }
    }
}
