using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Xml;

namespace nft.util
{
	/// <summary>
	/// Utility methods to help parsing XML documents.
	/// </summary>
	public class XmlUtil
	{
		/// <summary>
		/// Performs a node selection and throws an exception if it's not found.
		/// </summary>
		/// <exception cref="XmlException"></exception>
		public static XmlNode selectSingleNode( XmlNode node, string xpath ) {
			XmlNode n = node.SelectSingleNode(xpath);
			if(n==null)
				throw new XmlException("unable to find "+xpath,null);
			return n;
		}

		/// <summary>
		/// select single node and retruns InnerText.
		/// returns defaultVal if it's not found.
		/// </summary>
		/// <exception cref="XmlException"></exception>
		public static string getSingleNodeText( XmlNode node, string xpath, string defaultVal) 
		{
			XmlNode n = node.SelectSingleNode(xpath);
			if(n==null)
				return defaultVal;
			else
				return n.InnerText;
		}

		/// <summary>
		/// get value of specified attribute.
		/// returns defaultVal if it's not found.
		/// </summary>
		/// <exception cref="XmlException"></exception>
		public static string getAttributeValue( XmlNode node, string name, string defaultVal) 
		{
			XmlAttribute a = node.Attributes[name];
			if(a==null)
				return defaultVal;
			else
				return a.Value;
		}

		/// <summary>
		/// Resolves a relative URI.
		/// </summary>
		public static Uri resolve( XmlNode context, string relative ) 
		{
			return new Uri(new Uri(context.BaseURI),relative);
		}

		public static Point parsePoint( string text ) {
			try {
				int idx = text.IndexOf(',');
				return new Point( int.Parse(text.Substring(0,idx)), int.Parse(text.Substring(idx+1)) );
			} catch( Exception e ) {
				throw new FormatException("Unable to parse "+text+" as point",e);
			}
		}

//		public static SIZE parseSize( string text ) {
//			try {
//				int idx = text.IndexOf(',');
//				return new SIZE( int.Parse(text.Substring(0,idx)), int.Parse(text.Substring(idx+1)) );
//			} catch( Exception e ) {
//				throw new FormatException("Unable to parse "+text+" as size",e);
//			}
//		}

		public static string getAttribute( XmlNode context, string name, string _default )
		{
			XmlAttribute a = context.Attributes[name];
			if(a==null)
			{
				if(_default!=null)
					return _default;
				else
					throw new FormatException("the node ["+context.Name+"] must have attribute ["+name+"].");
			}
			return a.Value;	
		}

		/// <summary>
		/// Loads plugin.xml file from the directory.
		/// </summary>
		public static XmlDocument loadFile( string path ) {
			using( FileStream file = new FileStream(path,FileMode.Open,FileAccess.Read,FileShare.ReadWrite) ) {
				XmlDocument doc = new XmlDocument();
				XmlValidatingReader reader = new XmlValidatingReader(new XmlTextReader(path,file));
				reader.ValidationType = ValidationType.None;
				doc.Load(reader);
				return doc;
			}
		}

	}
}
