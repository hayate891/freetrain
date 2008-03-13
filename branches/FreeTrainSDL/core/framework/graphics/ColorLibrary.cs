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
using System.Drawing;
using System.Collections;
using freetrain.world;
using freetrain.framework.plugin;
using freetrain;

namespace freetrain.framework.plugin.graphics
{
    /// <summary>
    /// ColorLibrary の概要の説明です。
    /// </summary>
    public class ColorLibrary : Contribution//, IColorLibrary
    {
        /// <summary>
        /// 
        /// </summary>
        protected static readonly string null_id = "{COLORLIB-NULL}";
        /// <summary>
        /// 
        /// </summary>
        protected ArrayList list;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public ColorLibrary(XmlElement e)
            : base(e)
        {
            list = new ArrayList();
            XmlNode nd = e.FirstChild;
            while (nd != null)
            {
                Color c;
                if (nd.Name.Equals("element"))
                {
                    c = PluginUtil.parseColor(nd.Attributes["color"].Value);
                    list.Add(c);
                }
                nd = nd.NextSibling;
            }

            // special code for NullLibrary
            if (id.Equals(null_id))
                list.Add(Color.Transparent);
        }
        /// <summary>
        /// 
        /// </summary>
        static public ColorLibrary NullLibrary
        {
            get { return (ColorLibrary)PluginManager.theInstance.getContribution(null_id); }
        }
        /// <summary>
        /// 
        /// </summary>
        public int size
        {
            get { return list.Count; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Color this[int index]
        {
            get { return (Color)list[index]; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
