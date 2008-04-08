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

namespace FreeTrain.Framework.Plugin
{
    /// <summary>
    /// Loads a Contribution class by using a &lt;class> element
    /// in the contribution Xml definition.
    /// </summary>
    public class DynamicContributionFactory : IContributionFactory
    {
        /// <param name="baseType">
        /// Type of the contribution to be loaded.
        /// Loaded class is judged invalid unless it is a subtype
        /// of this type.
        /// </param>
        public DynamicContributionFactory(Type baseType)
        {
            this.baseType = baseType;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public DynamicContributionFactory(XmlElement e)
            :
            this(PluginUtil.loadTypeFromManifest(
                    (XmlElement)XmlUtil.SelectSingleNode(e, "implementation"))) { }


        private readonly Type baseType;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public Contribution Load(PluginDefinition owner, XmlElement e)
        {
            Contribution contrib = (Contribution)PluginUtil.loadObjectFromManifest(e);
            if (baseType.IsInstanceOfType(contrib))
                return contrib;
            else
                throw new Exception(string.Format(
                    "{0} is incorrect for for this contribution (expected:{1})",
                    contrib.GetType().FullName, baseType.FullName));
        }
    }
}
