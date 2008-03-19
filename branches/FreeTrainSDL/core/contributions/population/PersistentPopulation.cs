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
using FreeTrain.world;

namespace FreeTrain.Contributions.Population
{
    /// <summary>
    /// Population implementation that wraps another Population and
    /// provides persistence support.
    /// 
    /// During deserialization, reference to this object is re-connected
    /// to the existing PersistentPopulation object.
    /// </summary>
    [Serializable]
    public class PersistentPopulation : BasePopulation, ISerializable
    {
        private readonly BasePopulation core;

        /// <summary>
        /// Object used to restore the reference to this Population object.
        /// </summary>
        private IObjectReference resolver;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_core"></param>
        /// <param name="_ref">
        /// Object that returns a reference to the existing same object.
        /// This object needs to be serializable, and will be used to
        /// restore reference correctly.
        /// </param>
        public PersistentPopulation(BasePopulation _core, IObjectReference _ref)
        {
            this.core = _core;
            this.resolver = _ref;
        }
        /// <summary>
        /// 
        /// </summary>
        public override int residents { get { return core.residents; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentTime"></param>
        /// <returns></returns>
        public override int calcPopulation(Time currentTime)
        {
            return core.calcPopulation(currentTime);
        }

        //
        // serialization
        //
        private void setResolver(IObjectReference resolver)
        {
            this.resolver = resolver;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.SetType(typeof(SerializationHelper));
            // it seems to me that the resolver is fully resolved to the target object
            // before it's assigned to the reference field.
            // so the type of the reference field is Population, not IObjectReference to
            // a Population.
            info.AddValue("reference", resolver);
        }

        [Serializable]
        internal class SerializationHelper : IObjectReference
        {
            private BasePopulation reference = null;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="context"></param>
            /// <returns></returns>
            public object GetRealObject(StreamingContext context)
            {
                return reference;
            }
        }
    }
}
