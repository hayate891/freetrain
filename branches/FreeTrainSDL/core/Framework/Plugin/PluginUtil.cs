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
using System.Reflection;
using System.Xml;
using FreeTrain.Framework.Graphics;

namespace FreeTrain.Framework.Plugin
{
    /// <summary>
    /// Utility code
    /// </summary>
    public class PluginUtil
    {
        /// <summary>
        /// Parse a color from a string of the form "100,53,26"
        /// </summary>
        public static Color parseColor(string value)
        {
            string[] cmp = value.Split(',');
            return Color.FromArgb(int.Parse(cmp[0]), int.Parse(cmp[1]), int.Parse(cmp[2]));
        }

        /// <summary>
        /// Load a new object by reading a type from the manifest XML element.
        /// The "codeBase" attribute and the "name" attribute of
        /// a class element are used to determine the class to be loaded.
        /// </summary>
        public static object loadObjectFromManifest(XmlElement contrib)
        {
            XmlElement el = (XmlElement)XmlUtil.SelectSingleNode(contrib, "class");
            Type t = loadTypeFromManifest(el);

            try
            {
                object result = Activator.CreateInstance(t, new object[] { contrib });
                if (result == null)
                    throw new FormatException("Designated class can not be loaded: " + t.FullName);
                //! throw new FormatException("指定されたクラスはロードできません:"+t.FullName);

                return result;
            }
            catch (TargetInvocationException e)
            {
                throw new FormatException("Designated class can not be loaded: " + t.FullName, e);
                //! throw new FormatException("指定されたクラスはロードできません:"+t.FullName,e);
            }
        }

        /// <summary>
        /// Load a type from the name attribute and the codebase attribute .
        /// </summary>
        /// <param name="e">Typically a "class" element</param>
        public static Type loadTypeFromManifest(XmlElement e)
        {
            string typeName = e.Attributes["name"].Value;

            Assembly a;

            if (e.Attributes["codebase"] == null)
            {
                // load the class from the FreeTrain.Core.dll
                a = Assembly.GetExecutingAssembly();
            }
            else
            {
                // load the class from the specified assembly
                Uri codeBase = XmlUtil.Resolve(e, e.Attributes["codebase"].Value);

                if (!codeBase.IsFile)
                    throw new FormatException("Designated codebase is not a filename: " + codeBase);
                //! throw new FormatException("指定されたコードベースはファイル名ではありません:"+codeBase);

                a = Assembly.LoadFrom(codeBase.LocalPath);
            }

            return a.GetType(typeName, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sprite"></param>
        /// <returns></returns>
        public static SpriteLoaderContribution getSpriteLoader(XmlElement sprite)
        {
            string loader;

            if (sprite.Attributes["loader"] != null)
                loader = sprite.Attributes["loader"].Value;
            else
                loader = "default";

            SpriteLoaderContribution contrib = (SpriteLoaderContribution)
                PluginManager.GetContribution("spriteLoader:" + loader);
            if (contrib == null)
                throw new Exception("unable to find spriteLoader:" + loader);
            return contrib;
        }
    }
}
