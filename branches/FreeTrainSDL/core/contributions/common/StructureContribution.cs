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
using System.Drawing;
using System.Runtime.Serialization;
using System.Xml;
using freetrain.util;
using freetrain.contributions.population;
using freetrain.controllers;
using freetrain.framework.plugin;
using freetrain.framework.graphics;
using freetrain.world;
using freetrain.world.structs;
using SDL.net;

namespace freetrain.contributions.common
{
    /// <summary>
    /// 
    /// </summary>
    public interface AbstractExStructure
    {
        /// <summary>
        /// 
        /// </summary>
        int unitPrice { get; }
        /// <summary>
        /// 
        /// </summary>
        SIZE size { get; }
        /// <summary>
        /// 
        /// </summary>
        int minHeight { get; }
        /// <summary>
        /// 
        /// </summary>
        int maxHeight { get; }
    }

    /// <summary>
    /// Generic structure contribution.
    /// 
    /// Structure is an object that occupies a cubic area in the World,
    /// has sprites to draw it.
    /// </summary>
    [Serializable]
    public abstract class StructureContribution : Contribution, IEntityBuilder
    {

        /// <summary>
        /// Parses a structure contribution from a DOM node.
        /// </summary>
        /// <exception cref="XmlException">If the parsing fails</exception>
        protected StructureContribution(XmlElement e)
            : base(e)
        {
            core = new EntityBuilderInternal(e, this.id);
            XmlNode groupNode = e.SelectSingleNode("group");
            string groupName = (groupNode != null) ? groupNode.InnerText : core.name;
            group = getGroup(groupName);
            group.add(this);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="original"></param>
        /// <param name="type"></param>
        /// <param name="id"></param>
        protected StructureContribution(StructureContribution original, string type, string id)
            : base(type, id)
        {
            core = original;
            group = original.group;
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected IEntityBuilder core;
        /// <summary>
        /// 
        /// </summary>
        public readonly StructureGroup group;

        /// <summary>
        /// Implemented by the derived class and
        /// used to determine which group this structure should go.
        /// </summary>
        protected abstract StructureGroup getGroup(string name);

        /// <summary>
        /// Name of this entity builder. Primarily used as the display name.
        /// Doesn't need to be unique.
        /// </summary>
        public virtual string name { get { return core.name; } }
        /// <summary>
        /// 
        /// </summary>
        public virtual Population population { get { return core.population; } }

        /// <summary>
        /// True if the computer (the development algorithm) is not allowed to
        /// build this structure.
        /// </summary>
        // TODO: make EntityBuilderContribution responsible for creating a new Plan object.
        public bool computerCannotBuild { get { return core.computerCannotBuild; } }

        /// <summary>
        /// True if the player is not allowed to build this structure.
        /// </summary>
        public bool playerCannotBuild { get { return core.playerCannotBuild; } }
        /// <summary>
        /// 
        /// </summary>
        public virtual int price { get { return core.price; } }

        /// <summary>
        /// price par area (minimum).
        /// </summary>
        public virtual double pricePerArea { get { return core.pricePerArea; } }

        /// <summary>
        /// Creates a preview
        /// </summary>
        /// <param name="pixelSize"></param>
        /// <returns></returns>
        public virtual PreviewDrawer createPreview(Size pixelSize) { return core.createPreview(pixelSize); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public virtual ModalController createBuilder(IControllerSite site) { return core.createBuilder(site); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public virtual ModalController createRemover(IControllerSite site) { return core.createRemover(site); }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() { return core.name; }

        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        internal protected class EntityBuilderInternal : IEntityBuilder
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="e"></param>
            /// <param name="ownerId"></param>
            public EntityBuilderInternal(XmlElement e, string ownerId)
            {
                XmlNode nameNode = e.SelectSingleNode("name");
                XmlNode groupNode = e.SelectSingleNode("group");

                _name = (nameNode != null) ? nameNode.InnerText : (groupNode != null ? groupNode.InnerText : null);
                if (name == null)
                    throw new FormatException("<name> and <group> are both missing");
                _price = int.Parse(XmlUtil.selectSingleNode(e, "price").InnerText);
                _computerCannotBuild = (e.SelectSingleNode("computerCannotBuild") != null);
                _playerCannotBuild = (e.SelectSingleNode("playerCannotBuild") != null);

                XmlElement pop = (XmlElement)e.SelectSingleNode("population");
                if (pop != null)
                    _population = new PersistentPopulation(Population.load(pop),
                        new PopulationReferenceImpl(ownerId));
            }


            private readonly Population _population;
            private readonly bool _computerCannotBuild;
            private readonly bool _playerCannotBuild;
            private readonly string _name;
            /// <summary>
            /// 
            /// </summary>
            protected readonly int _price;

            #region IEntityBuilder o
            /// <summary>
            /// 
            /// </summary>
            public Population population { get { return _population; } }
            /// <summary>
            /// 
            /// </summary>
            public bool computerCannotBuild { get { return _computerCannotBuild; } }
            /// <summary>
            /// 
            /// </summary>
            public bool playerCannotBuild { get { return _playerCannotBuild; } }
            /// <summary>
            /// 
            /// </summary>
            public string name { get { return _name; } }
            /// <summary>
            /// 
            /// </summary>
            public int price { get { return _price; } }
            /// <summary>
            /// 
            /// </summary>
            public virtual double pricePerArea { get { return _price; } }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="pixelSize"></param>
            /// <returns></returns>
            public virtual freetrain.framework.graphics.PreviewDrawer createPreview(System.Drawing.Size pixelSize)
            {
                throw new NotImplementedException();
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="site"></param>
            /// <returns></returns>
            public virtual freetrain.controllers.ModalController createBuilder(freetrain.controllers.IControllerSite site)
            {
                throw new NotImplementedException();
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="site"></param>
            /// <returns></returns>
            public virtual freetrain.controllers.ModalController createRemover(freetrain.controllers.IControllerSite site)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        /// <summary>
        /// Used to resolve references to the population object.
        /// </summary>
        [Serializable]
        internal sealed class PopulationReferenceImpl : IObjectReference
        {
            internal PopulationReferenceImpl(string id) { this.id = id; }
            private string id;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="context"></param>
            /// <returns></returns>
            public object GetRealObject(StreamingContext context)
            {
                return ((StructureContribution)PluginManager.theInstance.getContribution(id)).population;
            }
        }
    }
}
