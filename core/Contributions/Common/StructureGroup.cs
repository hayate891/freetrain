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
using System.ComponentModel;

namespace FreeTrain.Contributions.Common
{
    /// <summary>
    /// Group of StructureContributions.
    /// </summary>
    public class StructureGroup : CollectionBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public StructureGroup(string name)
        {
            this.name = name;
        }

        /// <summary> Name of this group. </summary>
        private string name;

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sc"></param>
        public void Add(StructureContribution sc)
        {
            base.List.Add(sc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public StructureContribution Get(int idx)
        {
            return (StructureContribution)base.List[idx];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sc"></param>
        public void Remove(StructureContribution sc)
        {
            base.List.Remove(sc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() { return name; }
    }
}
