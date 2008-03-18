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
using FreeTrain.Framework.plugin;

namespace FreeTrain.Contributions.Structs
{
    /// <summary>
    /// Contribution that adds a special kind of structures, like airports.
    /// </summary>
    [Serializable]
    public abstract class SpecialStructureContribution : Contribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected SpecialStructureContribution(XmlElement e) : base("specialStructure", e.Attributes["id"].Value) { }

        /// <summary>
        /// Gets the name used for the menu item.
        /// </summary>
        public abstract string name { get; }

        /// <summary>
        /// Gets a one line description of this rail.
        /// </summary>
        public abstract string oneLineDescription { get; }

        /// <summary>
        /// This method is called when the menu item is selected by the user.
        /// </summary>
        public abstract void showDialog();
    }
}
