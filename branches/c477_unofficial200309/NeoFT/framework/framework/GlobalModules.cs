using System;
using System.Collections;
using System.Diagnostics;
using System.Xml;
using nft.framework.drawing;
using nft.framework.plugin;
using nft.util;

namespace nft.framework
{
	/// <summary>
	/// load core services
	/// </summary>
	public sealed class GlobalModules
	{
		static private Hashtable map = new Hashtable();

		// primitive modules.
		static private IGraphicManager gmanager = null;

		static public IGraphicManager GraphicManager { get { return gmanager;} }

		static public void Initialize(){
			XmlDocument doc = XmlUtil.loadFile(Directories.AppBaseDir+"core_modules.xml");
			XmlNode root = XmlUtil.selectSingleNode( doc, "modules");
			foreach(XmlNode cn in root.SelectNodes("module") ){
				try{
					IGlobalModule gm = loadModule(cn);
					if(gm!=null){
						Debug.WriteLine(gm.Name+" loaded.");
						Register(gm);
					}
				}catch(Exception e){
					string txt = Main.resources["global_modules.load_error"].stringValue;
					UIUtil.ShowException(txt,e,UIInformLevel.severe);
				}
			}
		}

		/// <summary>
		/// Load a new object by reading a type from the manifest XML element.
		/// The "codeBase" attribute and the "name" attribute of
		/// a class element are used to determine the class to be loaded.
		/// </summary>
		private static IGlobalModule loadModule( XmlNode node ) {
			XmlElement el = (XmlElement)XmlUtil.selectSingleNode(node,"class");
			Type t =  PluginUtil.loadTypeFromManifest(el);
				
			object result = null;
			try {
				// give XmlNode as first argument of constructor.
				result = Activator.CreateInstance(t,new object[]{node});
			} catch( Exception e ) {
				Debug.WriteLine(e.Message);
				Debug.WriteLine(e.StackTrace);
				string templ = Main.resources["xml.class_load_error"].stringValue;
				throw new Exception(string.Format(templ,t.FullName,node.OwnerDocument.BaseURI),e);
			}
			if(!(result is IGlobalModule)){
				string templ = Main.resources["xml.class_cast_error"].stringValue;
				object[] args = new object[]{t.FullName,"IGlobalModule",node.OwnerDocument.BaseURI};
				throw new InvalidCastException(string.Format(templ,args));
			}
			return result as IGlobalModule;
		}

		
		static public void Register(IGlobalModule module)
		{
			map.Add(module.RegistType,module);
			if( module is IGraphicManager )
				gmanager = (IGraphicManager)module;
		}

		static public void Unregister(IGlobalModule module) {
			if(GetModule(module.RegistType)==module)
				map.Remove(module.RegistType);
			if( gmanager == module )
				gmanager = null;
		}

		static public IGlobalModule GetModule(string typeName){
			Type type = Type.GetType(typeName);
			if(type != null)
				return (IGlobalModule)map[type];
			else
				return null;
		}

		static public IGlobalModule GetModule(Type type){
			return  (IGlobalModule)map[type];
		}
	}

	public interface IGlobalModule {
		Type RegistType { get; }
		string Name { get; }
		string Description { get; }
	}
}
