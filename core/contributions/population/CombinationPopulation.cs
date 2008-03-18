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
using FreeTrain.world;

namespace FreeTrain.Contributions.population
{
    /// <summary>
    /// Populaion that additively combines other populations.
    /// 
    /// Syntax in XML would be:
    /// &lt;population>
    ///   &lt;class name="...CombinationPopulation"/>
    ///   &lt;population>
    ///     ...
    ///   &lt;/population>
    ///   &lt;population>
    ///     ...
    ///   &lt;/population>
    ///   ...
    /// &lt;/population>
    /// </summary>
    public class CombinationPopulation : Population
    {
        private readonly Population[] children;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public CombinationPopulation(XmlElement e)
        {
            XmlNodeList nl = e.SelectNodes("population");
            children = new Population[nl.Count];
            for (int i = 0; i < nl.Count; i++)
                children[i] = Population.load((XmlElement)nl[i]);
        }
        /// <summary>
        /// 
        /// </summary>
        public override int residents
        {
            get
            {
                int r = 0;
                foreach (Population p in children)
                    r += p.residents;
                return r;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentTime"></param>
        /// <returns></returns>
        public override int calcPopulation(Time currentTime)
        {
            int r = 0;
            foreach (Population p in children)
                r += p.calcPopulation(currentTime);
            return r;
        }
    }
}
