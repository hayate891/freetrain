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
using freetrain.framework.plugin;
using freetrain.framework.graphics;

namespace freetrain.contributions.common
{
    /// <summary>
    /// Picture can be contributed.
    /// </summary>
    [Serializable]
    public class PictureContribution : Contribution
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly Picture picture;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public PictureContribution(XmlElement e)
            : base(e)
        {
            picture = new Picture(
                (XmlElement)XmlUtil.selectSingleNode(e, "picture"),
                this.id);
            // picture object will register itself to the manager.
        }
    }
}
