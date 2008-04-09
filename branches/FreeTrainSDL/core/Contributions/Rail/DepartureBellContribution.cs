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
using FreeTrain.Contributions.Common;
using FreeTrain.Framework;
using FreeTrain.Framework.Plugin;
using FreeTrain.Framework.Sound;
using FreeTrain.World;

namespace FreeTrain.Contributions.Rail
{
    /// <summary>
    /// Departure bell for trains.
    /// </summary>
    [Serializable]
    public class DepartureBellContribution : Contribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public DepartureBellContribution(XmlElement e)
            : base(e)
        {

            name = XmlUtil.SelectSingleNode(e, "name").InnerText;

            string href = XmlUtil.SelectSingleNode(e, "sound/@href").InnerText;
            sound = new RepeatableSoundEffectImpl(
                ResourceUtil.LoadSound(XmlUtil.Resolve(e, href)));
        }

        /// <summary> name of this sound </summary>
        public readonly string name;

        /// <summary> Bell sound </summary>
        public readonly ISoundEffect sound;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return name;
        }




        /// <summary> Gets all the departure bell contributions in the system. </summary>
        public static DepartureBellContribution[] all
        {
            get
            {
                return (DepartureBellContribution[])
                    PluginManager.ListContributions(typeof(DepartureBellContribution));
            }
        }

        /// <summary> Default bell sound. </summary>
        public static DepartureBellContribution DEFAULT
        {
            get
            {
                return (DepartureBellContribution)PluginManager
                    .GetContribution("{9B087AEA-6E9C-48cd-A1F3-1B774500752E}");
            }
        }
    }
}
