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
using freetrain.contributions.common;
using freetrain.framework.plugin;
using freetrain.world;
using freetrain.world.structs;

namespace freetrain.contributions.structs
{
    /// <summary>
    /// commercial structure.
    /// 
    /// Including everything from convenience stores (like Seven-eleven)
    /// to shopping malls like Walmart.
    /// </summary>
    [Serializable]
    public class CommercialStructureContribution : FixedSizeStructureContribution
    {
        /// <summary>
        /// Parses a commercial structure contribution from a DOM node.
        /// </summary>
        /// <exception cref="XmlException">If the parsing fails</exception>
        public CommercialStructureContribution(XmlElement e) : base(e) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="master"></param>
        /// <param name="pic"></param>
        /// <param name="main"></param>
        /// <param name="opposite"></param>
        public CommercialStructureContribution(AbstractExStructure master, XmlElement pic, XmlElement main, bool opposite)
            : base(master, pic, main, opposite) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override StructureGroup getGroup(string name)
        {
            return PluginManager.theInstance.commercialStructureGroup[name];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wLoc"></param>
        /// <param name="initiallyOwned"></param>
        /// <returns></returns>
        public override Structure create(WorldLocator wLoc, bool initiallyOwned)
        {
            return new Commercial(this, wLoc, initiallyOwned);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseLoc"></param>
        /// <param name="cm"></param>
        /// <returns></returns>
        public override bool canBeBuilt(Location baseLoc, ControlMode cm)
        {
            return Commercial.canBeBuilt(baseLoc, size, cm);
        }

        // TODO: additional parameters, like population and attractiveness.
    }
}
