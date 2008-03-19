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
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using SDL.net;
using FreeTrain.Framework.Graphics;


namespace FreeTrain.Framework.Plugin
{
    /// <summary>
    /// Common base class of contributions.
    /// 
    /// A contribution is a functionality provided by a plug-in.
    /// </summary>
    [Serializable]
    public abstract class Contribution : ISerializable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contrib"></param>
        protected Contribution(XmlElement contrib)
            :
            this(contrib.Attributes["type"].Value, contrib.Attributes["id"].Value) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_id"></param>
        protected Contribution(string _type, string _id)
        {
            this.type = _type;
            this.id = _id;
        }

        /// <summary>
        /// This method is a backdoor to configure a contribution.
        /// 
        /// We could just pass this argument through a constructor,
        /// but Contribution will be inherited multiple times, so it would be
        /// little awkward to pass a lot of parameters around.
        /// </summary>
        /// <param name="_parent"></param>
        /// <param name="_baseUri"></param>
        internal void init(PluginDefinition _parent, Uri _baseUri)
        {
            this._parent = _parent;
            this._baseUri = _baseUri;
        }


        /// <summary>
        /// Notifies the end of the initialization.
        /// 
        /// This method is called after all the contributions are loaded
        /// into memory. This is a good chance to run additional tasks
        /// that need to access other contributions.
        /// </summary>
        protected internal virtual void onInitComplete()
        {
        }

        /// <summary>
        /// Type of this contribution.
        /// This is the value of the type attribute.
        /// </summary>
        public readonly string type;

        /// <summary>
        /// Unique ID of this contribution.
        /// 
        /// Either GUID or URI, but can be anything as long
        /// as it's unique.
        /// </summary>
        public readonly string id;


        /// <summary>
        /// Base URI for this contribution (which is the same
        /// as the base URI for the plug-in.)
        /// 
        /// This poinst to the plug-in directory.
        /// </summary>
        public Uri baseUri { get { return _baseUri; } }
        private Uri _baseUri;

        /// <summary>
        /// Returns the Plugin object that contains this contribution.
        /// </summary>
        public PluginDefinition parent { get { return _parent; } }
        private PluginDefinition _parent;


        /// <summary>
        /// If a plug-in is implemented by using an assembly,
        /// it should override property and return the Assembly
        /// object, so that obejcts from this assembly can be
        /// de-serialized.
        /// 
        /// Returns null if this contribution doesn't rely on
        /// any assembly.
        /// </summary>
        public virtual Assembly assembly { get { return this.GetType().Assembly; } }




        #region utility methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected Picture loadPicture(string name)
        {
            return new Picture(this.id + ":" + name, new Uri(baseUri, name).LocalPath);
        }

        /// <summary>
        /// Locate the Picture from which sprites should be loaded.
        /// </summary>
        /// <param name="sprite">&lt;sprite> element in the manifest.</param>
        /// <returns>non-null valid object.</returns>
        protected Picture getPicture(XmlElement sprite)
        {
            XmlElement pic = (XmlElement)XmlUtil.selectSingleNode(sprite, "picture");

            XmlAttribute r = pic.Attributes["ref"];
            if (r != null)
                // reference to externally defined pictures.
                return PictureManager.get(r.Value);

            // otherwise look for local picture definition
            return new Picture(pic,
                sprite.SelectSingleNode("ancestor-or-self::contribution/@id").InnerText);
        }
        #endregion



        // serialize this object by reference
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.SetType(typeof(ReferenceImpl));
            info.AddValue("id", id);
        }

        [Serializable]
        internal sealed class ReferenceImpl : IObjectReference
        {
            private string id = null;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="context"></param>
            /// <returns></returns>
            public object GetRealObject(StreamingContext context)
            {
                object o = Core.plugins.getContribution(id);
                if (o == null)
                    throw new SerializationException(
                        "Plugin that contains Contribution \"" + id + "\" could not be found");
                //! "コントリビューション¥""+id+"¥"を含むプラグインが見つかりません");
                return o;
            }
        }
    }
}
