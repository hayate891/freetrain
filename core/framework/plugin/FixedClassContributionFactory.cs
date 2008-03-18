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

namespace FreeTrain.Framework.plugin
{
    /// <summary>
    /// Loads a Contribution from a designated class by passing the XmlElement
    /// to its constructor
    /// </summary>
    public class FixedClassContributionFactory : ContributionFactory
    {
        /// <param name="concreteType">
        /// Type of the class to be used to load the class.
        /// </param>
        public FixedClassContributionFactory(Type concreteType)
        {
            this.concreteType = concreteType;
        }
        /// <summary>
        /// Constructor for the use in plugin.xml
        /// </summary>
        public FixedClassContributionFactory(XmlElement e)
            :
            this(PluginUtil.loadTypeFromManifest(
                    (XmlElement)XmlUtil.selectSingleNode(e, "implementation"))) { }

        private readonly Type concreteType;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public Contribution load(Plugin owner, XmlElement e)
        {
            return (Contribution)Activator.CreateInstance(concreteType, new object[] { e });
        }
    }
}
