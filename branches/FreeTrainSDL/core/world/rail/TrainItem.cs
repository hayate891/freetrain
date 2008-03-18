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

namespace FreeTrain.world.Rail
{
    /// <summary>
    /// Common aspect of Train and TrainGroup.
    /// </summary>
    [Serializable]
    public abstract class TrainItem
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="_name"></param>
        protected TrainItem(TrainGroup group, string _name)
        {
            this._ownerGroup = group;
            this.name = _name;
            if (group != null)
                group.items.add(this);
        }

        /// <summary> Display name of this train. </summary>
        public string name;

        /// <summary>
        /// TrainController that controls this train/train group.
        /// 
        /// TrainController for train groups are not directly used.
        /// Rather, they will be used only when the train controller
        /// that delegates the call to its parent is used.
        /// </summary>
        public TrainController controller;

        private TrainGroup _ownerGroup;

        /// <summary> TrainGroup to which this train belong. </summary>
        public TrainGroup ownerGroup { get { return _ownerGroup; } }

        /// <summary> Move this group to a new train group </summary>
        public void moveUnder(TrainGroup newGroup)
        {
            if (ownerGroup != null)
                _ownerGroup.items.remove(this);

            _ownerGroup = newGroup;
            _ownerGroup.items.add(this);
        }
    }
}
