using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace nft.util
{
	/// <summary>
	/// Base implementation for persistent configuration information
	/// via XML serialization.
	/// 
	/// Derived class should add actual data members. See GlobalOptions for example.
	/// It should also override the load method.
	/// 
	/// For some reason, the derived class needs to be public.
	/// 
	/// To load a persistent configuration, do
	/// <code>new DerivedClass().load()</code>.
	/// An instance of the derived class is necessary because it determined where and
	/// how an XML file is loaded.
	/// </summary>
	public abstract class PersistentOptions
	{
		public PersistentOptions() {}

		/// <summary>
		/// Compute the stem of the file name to be used.
		/// Default implementation should be fine for the most cases.
		/// </summary>
		protected virtual string Stem {
			get {
				string stem = "."+this.GetType().FullName;
				return stem.Replace('+','.');
			}
		}

		private string FileName {
			get {
				return Application.ExecutablePath+Stem+".options";
			}
		}

		public void Save() {
			using(Stream s = new FileStream( FileName, FileMode.Create ))
				SaveTo(s);
		}
		protected void SaveTo( Stream stream ) {
			XmlSerializer s = new XmlSerializer(this.GetType());
			s.Serialize(stream,this);
		}

		protected PersistentOptions Load() {
			try {
				using(Stream s = new FileStream( FileName, FileMode.Open ))
					return LoadFrom(s);
			} catch( Exception e ) {
				Debug.WriteLine(e);
				// unable to load. use default.
				return this;
			}
		}
		protected PersistentOptions LoadFrom( Stream stream ) {
			XmlSerializer serializer = new XmlSerializer(this.GetType());
			return (PersistentOptions)serializer.Deserialize(stream);
		}
	}
}
