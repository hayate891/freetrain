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

namespace FreeTrain.Contributions.sound
{
    /// <summary>
    /// Background music.
    /// </summary>
    [Serializable]
    public class BGMContribution : Contribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public BGMContribution(XmlElement e)
            : base(e)
        {
            this.name = XmlUtil.selectSingleNode(e, "name").InnerText;

            XmlElement href = (XmlElement)XmlUtil.selectSingleNode(e, "href");
            fileName = XmlUtil.resolve(href, href.InnerText).LocalPath;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileName"></param>
        /// <param name="id"></param>
        public BGMContribution(string name, string fileName, string id)
            : base("bgm", id)
        {
            this.name = name;
            this.fileName = fileName;
        }

        /// <summary> Title of the music. </summary>
        public readonly string name;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.name;
        }

        /// <summary> File name of the music. </summary>
        public readonly string fileName;
    }
}
