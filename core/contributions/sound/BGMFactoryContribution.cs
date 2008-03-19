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
using System.Net;
using System.Xml;
using FreeTrain.Framework.Plugin;

namespace FreeTrain.Contributions.Sound
{
    /// <summary>
    /// Code that lists up BGMContribution programatically.
    /// </summary>
    [Serializable]
    public abstract class BGMFactoryContribution : Contribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected BGMFactoryContribution(XmlElement e) : base(e) { }

        /// <summary>
        /// Lists up the BGM contributions, which will be incorporated
        /// into the set of BGMs.
        /// </summary>
        /// <returns></returns>
        public abstract BGMContribution[] listContributions();

        /// <summary>
        /// Gets the group name of these BGMContributions.
        /// </summary>
        public abstract string title { get; }
    }

    internal class BGMFactoryContributionFactory : ContributionFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public BGMFactoryContributionFactory(XmlElement e) { }
        /// <summary>
        /// 
        /// </summary>
        public BGMFactoryContributionFactory() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public Contribution load(PluginDefinition owner, XmlElement e)
        {
            BGMFactoryContribution contrib =
                (BGMFactoryContribution)PluginUtil.loadObjectFromManifest(e);

            // let the factory load BGMs
            foreach (BGMContribution bgm in contrib.listContributions())
            {
                owner.contributions.Add(bgm);
                bgm.init(owner, new Uri(owner.dirName));
            }

            return contrib;
        }
    }
}
