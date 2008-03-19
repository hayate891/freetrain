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
using FreeTrain.Framework.Plugin;

namespace FreeTrain.Contributions.Train
{
    /// <summary>
    /// LoadsTrainContributionFactory.
    /// 
    /// When a class element is present, it is used. Otherwise
    /// defaults to ParamTrainImpl
    /// </summary>
    public class TrainContributionFactory : ContributionFactory
    {
        /// <summary>
        /// 
        /// </summary>
        public TrainContributionFactory() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public TrainContributionFactory(XmlElement e) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public Contribution load(PluginDefinition owner, XmlElement e)
        {
            if (e.SelectSingleNode("class") == null)
                // default to ParamTrainImpl
                return new ParamTrainImpl(e);
            else
                // use the class element
                return (Contribution)PluginUtil.loadObjectFromManifest(e);
        }
    }
}
