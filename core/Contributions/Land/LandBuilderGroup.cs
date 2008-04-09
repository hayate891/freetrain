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
using FreeTrain.Contributions.Common;

namespace FreeTrain.Contributions.Land
{
    /// <summary>
    /// Group of LandContribution.
    /// </summary>
    public class LandBuilderGroup : StructureGroup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public LandBuilderGroup(string name)
            : base(name)
        {
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="idx"></param>
        ///// <returns></returns>
        //public LandBuilderContribution Get(int idx)
        //{
        //    return (LandBuilderContribution)base.List[idx];
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sc"></param>
        public void Remove(LandBuilderContribution sc)
        {
            base.List.Remove(sc);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() { return Name; }
    }

    /// <summary>
    /// Group of LandGroup.
    /// 
    /// This object implements IListSource to support data-binding.
    /// </summary>
    public class LandBuilderGroupGroup : StructureGroupGroup
    {
        /// <summary>
        /// 
        /// </summary>
        public LandBuilderGroupGroup() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public new LandBuilderGroup this[string name]
        {
            get
            {
                LandBuilderGroup g = (LandBuilderGroup)core[name];
                if (g == null)
                {
                    core[name] = g = new LandBuilderGroup(name);
                    list.Add(g);
                }

                return g;
            }
        }
    }
}
