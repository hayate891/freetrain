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
using System.IO;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;

namespace FreeTrain.Framework.Plugin.Generic
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class StructCategories
    {
        /// <summary>
        /// 
        /// </summary>
        protected ArrayList array = new ArrayList();

        internal StructCategories() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cat"></param>
        public StructCategories(StructCategory cat)
        {
            this.Add(cat);
        }

        // give <structure> tag, which contains <category> as children.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="idContrib"></param>
        public StructCategories(XmlNode node, string idContrib)
        {
            foreach (XmlNode cn in node)
            {
                if (cn.Name.Equals("category"))
                {
                    XmlAttribute a = cn.Attributes["byid"];
                    StructCategory cat = null;
                    if (a != null)
                        cat = StructCategoryTree.theInstance[int.Parse(a.Value, NumberStyles.HexNumber)];
                    else
                    {
                        a = cn.Attributes["byname"];
                        if (a != null)
                            cat = StructCategoryTree.theInstance[a.Value];
                    }
                    if (cat == null) cat = StructCategory.Root;
                    if (!array.Contains(cat.idnum))
                        array.Add(cat.idnum);
                    a = cn.Attributes["hide"];
                    if (a == null || !a.Value.Equals("true"))
                        cat.Entries.Add(idContrib);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count { get { return array.Count; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        public int Add(StructCategory cat)
        {
            return array.Add(cat.idnum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public StructCategory this[int i]
        {
            get { return StructCategoryTree.theInstance[(int)array[i]]; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        public bool Contains(StructCategory cat)
        {
            return array.Contains(cat.idnum);
        }
    }
}
