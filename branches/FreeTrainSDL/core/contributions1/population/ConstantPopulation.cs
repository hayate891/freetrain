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
using FreeTrain.World;
using FreeTrain.World.Structs;
using FreeTrain.Framework.Plugin;

namespace FreeTrain.Contributions.Population
{
    /// <summary>
    /// Always the same population
    /// </summary>
    [Serializable]
    public class ConstantPopulation : BasePopulation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        public ConstantPopulation(int p)
        {
            this.population = p;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public ConstantPopulation(XmlElement e)
        {
            this.population = int.Parse(XmlUtil.selectSingleNode(e, "base").InnerText);
        }

        private readonly int population;
        /// <summary>
        /// 
        /// </summary>
        public override int residents { get { return population; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentTime"></param>
        /// <returns></returns>
        public override int calcPopulation(Time currentTime)
        {
            return population;
        }
    }
}
