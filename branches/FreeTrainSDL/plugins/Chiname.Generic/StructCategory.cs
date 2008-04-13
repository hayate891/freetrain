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
    /// StructCategory
    /// </summary>
    [Serializable]
    public class StructCategory
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly int idnum;
        /// <summary>
        /// 
        /// </summary>
        public readonly int iconIdx;
        /// <summary>
        /// 
        /// </summary>
        public readonly String name;
        /// <summary>
        /// 
        /// </summary>
        static protected ImageList images = new ImageList();
        /// <summary>
        /// 
        /// </summary>
        static protected Hashtable files = new Hashtable();
        /// <summary>
        /// 
        /// </summary>
        static public ImageList icons { get { return images; } }

        /// <summary>
        /// 
        /// </summary>
        public StructCategories Subcategories = new StructCategories();
        internal ArrayList Entries = new ArrayList();

        internal StructCategory(int idnum, String name, int icon)
        {
            this.idnum = idnum;
            this.name = name;
            this.iconIdx = icon;
        }

        internal static int ImageBaseInt(String filename)
        {
            if (!files.ContainsKey(filename))
            {
                int i = images.Images.Count;
                if (i == 0)
                {
                    images.ImageSize = new Size(16, 15);
                    images.TransparentColor = Color.White;
                }
                images.Images.AddStrip(new System.Drawing.Bitmap(filename));
                files.Add(filename, i);
                return i;
            }
            else
                return (int)files[filename];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xn"></param>
        public StructCategory(XmlNode xn)
        {
            name = XmlUtil.SelectSingleNode(xn, "name").InnerText;
            XmlAttribute att = xn.Attributes["id"];
            if (att != null)
                idnum = int.Parse(att.Value, NumberStyles.HexNumber);

            XmlNode ni = xn.SelectSingleNode("icon");
            if (ni == null)
                iconIdx = Parent.iconIdx;
            else
            {
                String fn = ni.Attributes["src"].Value;
                String path;
                if (xn.BaseURI.Length > 0)
                    path = new Uri(new Uri(xn.BaseURI), fn).LocalPath;
                else
                    path = Path.Combine(StructCategoryTree.BaseDir, fn);
                int idx = int.Parse(ni.Attributes["index"].Value);
                iconIdx = idx + ImageBaseInt(path);
            }
            if (HasParent())
                Parent.Subcategories.Add(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool HasParent()
        { return (idnum & 0xffffff00) != 0; }
        /// <summary>
        /// 
        /// </summary>
        public StructCategory Parent
        {
            get
            {
                StructCategory cat = StructCategoryTree.theInstance.getParent(this);
                if (cat == null)
                    throw new FormatException("no parent for " + name + " : maybe ID is wrong." + idnum);
                return cat;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        static public StructCategory Root { get { return StructCategoryTree.theInstance[0]; } }
    }
}
