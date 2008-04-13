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
    public class StructCategoryTree //: Contribution
    {
        /// <summary>
        /// 
        /// </summary>
        static public StructCategoryTree theInstance;
        /// <summary>
        /// 
        /// </summary>
        static string baseDir;
        /// <summary>
        /// 
        /// </summary>
        static public string BaseDir { get { return baseDir; } }
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
            baseDir = p.dirName;
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
            theInstance = this;
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
