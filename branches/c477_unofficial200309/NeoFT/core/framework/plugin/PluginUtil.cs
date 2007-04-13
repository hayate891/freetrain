using System;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Xml;
using nft.util;
using nft.ui.command;

namespace nft.framework.plugin
{
	/// <summary>
	/// Utility code
	/// </summary>
	public class PluginUtil
	{
		/// <summary>
		/// Parse a color from a string of the form "100,53,26"
		/// </summary>
		public static Color parseColor( string value ) {
			string[] cmp = value.Split(',');
			return Color.FromArgb( int.Parse(cmp[0]), int.Parse(cmp[1]), int.Parse(cmp[2]) );
		}

		/// <summary>
		/// Load a new object by reading a type from the manifest XML element.
		/// The "codeBase" attribute and the "name" attribute of
		/// a class element are used to determine the class to be loaded.
		/// </summary>
		public static object loadObjectFromManifest( XmlElement contrib ) {
			XmlElement el = (XmlElement)XmlUtil.selectSingleNode(contrib,"class");
			Type t = loadTypeFromManifest(el);

			try {
				// give XmlNode as first argument of constructor.
				object result = Activator.CreateInstance(t,new object[]{contrib});
				if( result==null )
				{
					string templ = Core.resources["xml.class_load_error"].stringValue;
					throw new Exception(string.Format(templ,t.FullName,contrib.OwnerDocument.BaseURI));
				}
				return result;
			} catch( TargetInvocationException e ) {
				Debug.WriteLine(e.Message);
				Debug.WriteLine(e.StackTrace);
				string templ = Core.resources["xml.class_load_error"].stringValue;
				throw new Exception(string.Format(templ,t.FullName,contrib.OwnerDocument.BaseURI),e);
			}
		}
		
		/// <summary>
		/// Load a type from the name attribute and the codebase attribute .
		/// </summary>
		/// <param name="e">Typically a "class" element</param>
		public static Type loadTypeFromManifest( XmlElement e ) {
			string typeName = e.Attributes["name"].Value;

			Assembly a;

			if( e.Attributes["codebase"]==null ) {
				// load the class from the FreeTrain.Core.dll
				a = Assembly.GetExecutingAssembly();
			} else {
				// load the class from the specified assembly
				Uri codeBase = XmlUtil.resolve( e, e.Attributes["codebase"].Value );

				if( !codeBase.IsFile )
					throw new FormatException("指定されたコードベースはファイル名ではありません:"+codeBase);

				a = Assembly.LoadFrom( codeBase.LocalPath );
			}

			return a.GetType(typeName,true);
		}

		public static void RegisterCommand( string id, ICommandEntity entity, XmlNode commandNode )
		{
			Debug.Assert(entity!=null&&id!=null&&commandNode!=null,"Invalid Command is going to regist!");
			string pid = PluginUtil.GetPruginDirName(commandNode);
			string bar = XmlUtil.getAttributeValue(commandNode,"toolbar","MAIN");
			string bid = XmlUtil.getAttributeValue(commandNode,"button",null);
			string mpath = XmlUtil.getAttributeValue(commandNode,"menupath",null);
			if( bid != null )
				Core.mainFrame.SetToolButtonCommand(id,entity,bar,bid);
			if( mpath != null )
				Core.mainFrame.SetMenuCommand(id,entity,mpath);
		}

		public static string GetPruginDirName( XmlNode node )
		{
			return Path.GetFileName(Path.GetDirectoryName(node.OwnerDocument.BaseURI));
		}

		public static string GetPruginFullPath( XmlNode node )
		{
			return Path.GetDirectoryName(node.OwnerDocument.BaseURI);
		}

	}
}
