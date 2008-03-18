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
using System.Drawing;
using System.Xml;
using FreeTrain.Contributions.Common;
using FreeTrain.world;

namespace FreeTrain.Contributions.rail
{
    /// <summary>
    /// Contribution that adds a special kind of railroads, like tunnels or bridges.
    /// </summary>
    [Serializable]
    public abstract class SpecialRailContribution : LineContribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected SpecialRailContribution(XmlElement e) : base("specialRail", e.Attributes["id"].Value) { }
    }
}
