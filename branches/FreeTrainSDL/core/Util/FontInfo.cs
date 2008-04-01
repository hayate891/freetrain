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
using System.Runtime.InteropServices;
using System.Drawing;
using System.Reflection;
using System.Xml.Serialization;

namespace FreeTrain.Util
{
    /// <summary>
    /// LOGFONT structure that keeps all the information of Font.
    /// 
    /// <c>Font</c> doesn't work with XML serialization, but this one does.
    /// </summary>
    public class FontInfo
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute()]
        public string fontName;
        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute()]
        public GraphicsUnit unit;
        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute()]
        public float size;
        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute()]
        public FontStyle style;
        /// <summary>
        /// 
        /// </summary>
        public FontInfo() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        public FontInfo(Font f)
        {
            this.fontName = f.FontFamily.Name;
            this.unit = f.Unit;
            this.size = f.Size;
            this.style = f.Style;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Font createFont()
        {
            return new Font(fontName, size, style, unit);
        }
    }
}