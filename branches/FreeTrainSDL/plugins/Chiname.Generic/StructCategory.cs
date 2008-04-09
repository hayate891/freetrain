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

    /// <summary>
    /// 
    /// </summary>
    public class StructCategoryTree //: Contribution
    {
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        static protected StructCategoryTree _theInstance;
        /// <summary>
        /// 
        /// </summary>
        static public StructCategoryTree theInstance { get { return _theInstance; } }
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        static protected string _baseDir;
        /// <summary>
        /// 
        /// </summary>
        static public string BaseDir { get { return _baseDir; } }
        static private StructCategory hidden = new StructCategory(-1, "--N/A--", 0);

        /// <summary>
        /// 
        /// </summary>
        protected Hashtable idTbl = new Hashtable();
        /// <summary>
        /// 
        /// </summary>
        protected Hashtable nameTbl = new Hashtable();

        /// <summary>
        /// 
        /// </summary>
        static public void loadDefaultTree()
        {
            PluginDefinition p = PluginManager.GetPlugin("Chiname.Generic");
            _baseDir = p.dirName;
            string filename = Path.Combine(p.dirName, "CategoryTree.xml");
            using (Stream file = p.loadStream(filename))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(file);
                new StructCategoryTree(doc);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public StructCategoryTree(XmlDocument e)//:base(e)
        {
            _theInstance = this;
            XmlNode root = e.SelectSingleNode("tree");
            makeRoot(e);
            if (root != null)
                parseTree(root, 0);
        }

        private void makeRoot(XmlDocument e)
        {
            String fn = Path.Combine(BaseDir, "icons.bmp");
            StructCategory.ImageBaseInt(fn);
            StructCategory cat = new StructCategory(0, "Uncategorized", 0);
            //! StructCategory cat = new StructCategory(0, "未分類", 0);
            idTbl.Add(cat.idnum, cat);
            nameTbl.Add(cat.name, cat);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nd"></param>
        /// <param name="level"></param>
        protected void parseTree(XmlNode nd, int level)
        {
            XmlNodeList xnl = nd.ChildNodes;
            foreach (XmlNode cn in xnl)
            {
                if (cn.Name.Equals("category"))
                {
                    StructCategory cat = new StructCategory(cn);
                    idTbl.Add(cat.idnum, cat);
                    nameTbl.Add(cat.name, cat);
                    parseTree(cn, level + 1);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idnum"></param>
        /// <returns></returns>
        public StructCategory this[int idnum]
        {
            get
            {
                if (idnum == -1)
                    return hidden;
                else
                    return (StructCategory)idTbl[idnum];
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public StructCategory this[String name]
        {
            get { return (StructCategory)nameTbl[name]; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        public StructCategory getParent(StructCategory cat)
        {
            int pid = cat.idnum >> 8;
            return this[pid];
        }

        /// <summary>
        /// 
        /// </summary>
        public ICollection Categories
        {
            get { return idTbl.Values; }
        }
    }

}
