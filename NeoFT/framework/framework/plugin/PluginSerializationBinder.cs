using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;
using nft.framework;

namespace nft.framework.plugin
{
	/// <summary>
	/// Allows objects from plug-ins to be de-serialized.
	/// </summary>
	public class PluginSerializationBinder : SerializationBinder
	{
		static public readonly PluginSerializationBinder theInstance;

		static PluginSerializationBinder()
		{
			theInstance = new PluginSerializationBinder();
		}

		private PluginSerializationBinder(){}
		
		protected Hashtable assemblies = new Hashtable();

		static public void registerAssembly(Assembly asm)
		{
			theInstance.addAssembly(asm);
		}
		
		protected void addAssembly(Assembly asm)
		{
			if(!assemblies.ContainsKey(asm.GetName().Name))
				assemblies.Add(asm.GetName().Name,asm);
		}

		public override Type BindToType(string assemblyName, string typeName) 
		{
			string shortName = assemblyName.Substring(0,assemblyName.IndexOf(','));
			Assembly assem = (Assembly)assemblies[shortName];
			Type t = null;
			if(assem!=null)
				t = assem.GetType(typeName);
			if( t!=null )
				return t;
			Trace.WriteLine("search for all assemblies.("+shortName+","+typeName+")");
			foreach( Assembly asm in assemblies.Values )
			{
				t = asm.GetType(typeName);
				if(t!=null)	return t;
			}
			Trace.WriteLine("type ["+typeName+"] is not found.");
			return null;
		}

		//		public override System.Type BindToType(string assemblyName, string typeName) {
		//			Type t;
		//			
		//			t = Type.GetType(typeName);
		//			if(t!=null)		return t;
		//
		//			Trace.WriteLine("binding "+typeName);
		//
		//			// try assemblies of plug-ins
		//			foreach( Contribution cont in Core.plugins.contributions ) {
		//				 Assembly asm = cont.assembly;
		//				if(asm!=null) {
		//					t = asm.GetType(typeName);
		//					if(t!=null)	return t;
		//				}
		//			}
		//			Trace.WriteLine("not found");
		//			return null;
		//		}		public override System.Type BindToType(string assemblyName, string typeName) {
	}
}
