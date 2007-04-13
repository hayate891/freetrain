using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using nft.util;

namespace nft.framework.plugin
{
	
	/// <summary>
	/// Common base class of contributions.
	/// 
	/// A contribution is a functionality provided by a plug-in.
	/// </summary>
	[Serializable]
	public abstract class Contribution : ISerializable, IHasNameAndID, IAddable
	{
		static string GenerateID( XmlElement contrib )
		{
			string short_id = XmlUtil.getAttributeValue( contrib, "id", null);
			if( short_id == null )
			{
				string templ = Main.resources["xml.attribute_not_found"].stringValue;
				throw new PluginXmlException(contrib,string.Format(
					templ,contrib.Name,"name",contrib.OwnerDocument.BaseURI));
			}

			string pname = PluginUtil.GetPruginDirName(contrib);
			return pname + ":" + short_id;
		}
		
		protected Contribution( XmlElement contrib )
		{
			this.name = XmlUtil.getSingleNodeText(contrib,"name","<unknown>");

			id = GenerateID( contrib );
			try
			{
				CtbType = contrib.Attributes["type"].Value;
			}
			catch
			{
				string templ = Main.resources["xml.attribute_not_found"].stringValue;
				throw new PluginXmlException(contrib,string.Format(
					templ,contrib.Name,"type",contrib.OwnerDocument.BaseURI));
			}
			XmlNode n = contrib.SelectSingleNode("description");
			if(n!=null)
				description = n.InnerText;
			else
				description = "";
		}

		protected Contribution( string _type, string _id, string _name, string _description ) {
			this.CtbType = _type;
			this.id = _id;
			this.name = _name;
			this.description = _description;
		}

		/// <summary>
		/// This method is a backdoor to configure a contribution.
		/// 
		/// We could just pass this argument through a constructor,
		/// but Contribution will be inherited multiple times, so it would be
		/// little awkward to pass a lot of parameters around.
		/// </summary>
		/// <param name="_baseUri"></param>
		internal void init( Plugin _parent, Uri _baseUri ) {
			this._parent = _parent;
			this._baseUri = _baseUri;
		}

		#region IAddable メンバ
		internal protected InstallationState _state = InstallationState.Uninitialized;
		[NonSerialized]
		protected AttachChangeEvent onDetach;
		[NonSerialized]
		protected AttachChangeEvent onAttach;

		protected bool attached = false;

		public InstallationState State { get{ return _state; } }
		
		public virtual bool IsDetachable { get{ return false; } }

		public virtual bool QueryDetach() {	return IsDetachable; }

		public virtual void Detach()
		{
			if(IsDetachable)
			{
				attached = false;
				if(onAttach!=null)
					onAttach(this);
			}
			else
			{
				string msg = string.Format("This Contribution{0} is not detachable!",ID);
				throw new InvalidOperationException(msg);
			}
		}

		public virtual void Attach()
		{
			attached = true;
			if(onDetach!=null)
				onDetach(this);
		}

		public virtual bool IsAttached{ get{ return attached; } }
		public virtual bool IsPartiallyDetached{ get{ return false; } }
		public AttachChangeEvent OnDetach{ get{ return onDetach; } set{ onDetach = value; } }
		public AttachChangeEvent OnAttach{ get{ return onAttach; } set{ onAttach = value; } }
		#endregion

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
		public readonly string CtbType;

		/// <summary>
		/// Unique ID of this contribution.
		/// 
		/// Either GUID or URI, but can be anything as long
		/// as it's unique.
		/// </summary>
		protected readonly string id;

		/// <summary>
		/// Name of this contribution.
		/// </summary>
		protected readonly string name;

		protected readonly string description;
		public virtual string Description { get { return description; } }

		internal protected bool _hideFromCom;
		internal protected bool _hideFromPlayer;
		public bool hideFromCom{ get{ return _hideFromCom;} }
		public bool hideFromPlayer{ get{ return _hideFromPlayer;} }

		/// <summary>
		/// Base URI for this contribution (which is the same
		/// as the base URI for the plug-in.)
		/// 
		/// This poinst to the plug-in directory.
		/// </summary>
		public Uri baseUri { get { return _baseUri; } }
		private  Uri _baseUri;
		
		/// <summary>
		/// Returns the Plugin object that contains this contribution.
		/// </summary>
		public Plugin parent { get { return _parent; } }
		private Plugin _parent;


		/// <summary>
		/// If a plug-in is implemented by using an assembly,
		/// it should override property and return the Assembly
		/// object, so that obejcts from this assembly can be
		/// de-serialized.
		/// 
		/// Returns null if this contribution doesn't rely on
		/// any assembly.
		/// </summary>
		public virtual Assembly Assembly { get { return this.GetType().Assembly; } }


		#region utility methods (comment outed)
		/*
		protected Picture loadPicture( string name ) {
			return new Picture( this.id+":"+name, new Uri( baseUri,name).LocalPath );
		}

		/// <summary>
		/// Locate the Picture from which sprites should be loaded.
		/// </summary>
		/// <param name="sprite">&lt;sprite> element in the manifest.</param>
		/// <returns>non-null valid object.</returns>
		protected Picture getPicture( XmlElement sprite ) {
			XmlElement pic = (XmlElement)XmlUtil.selectSingleNode(sprite,"picture");
			
			XmlAttribute r = pic.Attributes["ref"];
			if(r!=null)
				// reference to externally defined pictures.
				return PictureManager.get(r.Value);

			// otherwise look for local picture definition
			return new Picture(pic,
				sprite.SelectSingleNode("ancestor-or-self::contribution/@id").InnerText);
		}
		*/
		#endregion

		// serialize this object by reference
		public virtual void GetObjectData( SerializationInfo info, StreamingContext context) 
		{
			info.SetType(typeof(ReferenceImpl));
			info.AddValue("id",id);
		}
		
		[Serializable]
		internal sealed class ReferenceImpl : IObjectReference {
			private string id=null;
			public object GetRealObject(StreamingContext context) {
				object o = Main.plugins.GetContribution(id);
				if(o==null)
					throw new SerializationException(
						"コントリビューション\""+id+"\"を含むプラグインが見つかりません");
				return o;
			}
		}
		#region IHasNameAndID メンバ

		public string ID
		{
			get
			{
				return id;
			}
		}

		public string Name
		{
			get
			{
				return name;
			}
		}

		#endregion

	}
}
