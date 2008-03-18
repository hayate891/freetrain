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
using System.Runtime.Serialization;
using System.Xml;
using FreeTrain.Framework.plugin;
using FreeTrain.world;
using FreeTrain.world.Structs;

namespace FreeTrain.Contributions.population
{
    /// <summary>
    /// Computes the population from the base population.
    /// 
    /// The Population class has a special code to support de-serizliation.
    /// We'd like the de-serialization of Population not to create a fresh
    /// instance of Population, but we'd like it to resolve to the existing
    /// instance.
    /// 
    /// However, Population object doesn't know how to resolve to its running
    /// instance. Thus it takes an IObjectReference as a parameter, which should
    /// know how to resolve to the actual instance.
    /// 
    /// During the serialization, this resolver is stored in place of the
    /// population object and then asked to restore the reference.
    /// </summary>
    [Serializable]
    public abstract class Population
    {
        /// <summary>
        /// Number of population that is counted toward the total population of the world.
        /// </summary>
        public abstract int residents { get; }

        /// <summary>
        /// Computes the population of the given structure at the given time.
        /// </summary>
        public abstract int calcPopulation(Time currentTime);

        /// <summary>
        /// Loads a population from the plug-in manifest file.
        /// </summary>
        public static Population load(XmlElement e)
        {
            return (Population)PluginUtil.loadObjectFromManifest(e);
        }
    }
}
