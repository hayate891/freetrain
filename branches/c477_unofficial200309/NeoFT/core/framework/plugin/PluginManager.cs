using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using nft.util;

namespace nft.framework.plugin
{
	public delegate void ParseEventHandler(Plugin p, XmlElement e);

	/// <summary>
	/// Loads plug-ins.
	/// </summary>
	public class PluginManager
	{
		/// <summary> The singleton instance. </summary>
		public static PluginManager theInstance;

		/// <summary>
		/// Called before a contribution parsed.
		/// </summary>
		public ParseEventHandler BeforeContributionParse;

		/// <summary>
		/// All loaded plug-ins.
		/// </summary>
		private Plugin[] plugins;

		/// <summary>
		/// Plugins keyed by their names.
		/// </summary>
		private readonly Hashtable pluginMap = new Hashtable();
		
		/// <summary>
		/// Contribution factories that are used to load contributions.
		/// </summary>
		private readonly Hashtable contributionFactories = new Hashtable();

		/// <summary>
		/// Contributions keyed by their IDs.
		/// </summary>
		private readonly Hashtable contributionMap = new Hashtable();

		private int errCount=0;

		public PluginManager() {
			theInstance = this;
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(onAssemblyResolve);
		}

		#region resolve assembly on deserialize
		private static string getModuleName( string name ) 
		{
			return name.Substring(0,name.IndexOf(','));
		}

		private static Assembly onAssemblyResolve( object sender, ResolveEventArgs args) {
			// TODO: improve performance by having a dictionary from name to Assemblies.
			// TODO: what is the correct way to use an application specific logic to resolve assemblies
			Trace.WriteLine("onAssemblyResolve resolving "+args.Name);
			
			string name = getModuleName(args.Name);

			if( Core.plugins.plugins==null )	return null;

			// try assemblies of plug-ins
			foreach( Contribution cont in Core.plugins.contributions ) {
				Assembly asm = cont.assembly;

				if(getModuleName(asm.FullName)==name)
					return asm;
			}
			
			Trace.WriteLine("onAssemblyResolve failed");
			return null;
		}
		#endregion

		/// <summary>
		/// This method should be called after the object is created.
		/// </summary>
		/// <param name="dirs">
		/// collection of strings (directory names)
		/// for each directory in this collection, its sub-directories
		/// are scanned for plugin.xml
		/// </param>
		public void init( ICollection dirs, ProgressMonitor monitor ) 
		{
			errCount=0;
			ArrayList pluginSet=SeekPluginDirectories(dirs,monitor);

			SolveDependency(pluginSet, monitor);
			LoadContributions(monitor);
			InitContributions(monitor);

//			if( errCount!=0 ) {
//				// error during the initialization
//				Environment.Exit(errCount);
//			}
		}

		// called from Plugin on initialization
		internal void NotifyStartParse(Plugin p, XmlElement e)
		{
			if( BeforeContributionParse!=null )
				BeforeContributionParse(p,e);
		}
	
		#region initialization processes
		private ArrayList SeekPluginDirectories(ICollection directories,ProgressMonitor monitor)
		{
			ArrayList pluginSet = new ArrayList();

			monitor.Progress(1,1,"プラグインを検索中");

			string[][] subdirs = new string[directories.Count][];
			int n=0;
			int count=0;
			foreach( string dir in directories )
			{
				subdirs[n] = Directory.GetDirectories( dir );
				count += subdirs[n++].Length;
			}

			// locate plugins
			monitor.SetMaximum(2,count);
			
			foreach( string[] dirarray in subdirs )
			{
				foreach( string dir in dirarray )
				{								
					monitor.Progress(2,1,dir);
					if( !File.Exists(Path.Combine(dir,"plugin.xml")) )
						continue;	// this directory doesn't have the plugin.xml file.

					Plugin p = null;
					try 
					{
						p = new Plugin(dir);
						pluginSet.Add( p );
						if( pluginMap.Contains(p.name) ) 
						{
							p._state = ModuleState.FatalError;
							// loaded more than once
							// maybe same subdir name in different plugin dirs.
							throw new Exception( string.Format(
								"プラグイン「{0}」は{1}と{2}の二箇所からロードされています",
								p.name, p.dirName, ((Plugin)pluginMap[p.name]).dirName) );
						}
						pluginMap.Add( p.name, p );
					} 
					catch( Exception e ) 
					{
						Debug.WriteLine(e.Message);
						if(p!=null)
							p._state = ModuleState.FatalError;

						string templ = Core.resources["plugin.plugin_load_error"].stringValue;
						templ+="\n"+e.Message;
						ReportError(string.Format(templ,Path.GetFileName(dir)),e);
					}
				}
			}
			return pluginSet;
		}

		private void SolveDependency(ArrayList pluginSet, ProgressMonitor monitor)
		{// convert it to an array by sorting them in the order of dependency
			monitor.Progress(1,1,"依存関係を整理中");
			monitor.SetMaximum(2,pluginSet.Count);

			this.plugins = new Plugin[pluginSet.Count];
			int ptr=0;

			while( pluginSet.Count>0 ) 
			{
				Plugin p = (Plugin)pluginSet[0];
				monitor.Progress(2,1,p.name);
				try 
				{
					while(true) 
					{
						Plugin[] deps = p.getDependencies();
						int i;
						for( i=0; i<deps.Length; i++ )
							if( pluginSet.Contains(deps[i]) )
								break;
						if(i==deps.Length)
							break;
						else
							p = deps[i];
					}
				} 
				catch( Exception e ) 
				{					
					ReportError(e.Message,e);
				}

				pluginSet.Remove(p);
				plugins[ptr++] = p;
			}
		}

		private void LoadContributions(ProgressMonitor monitor)
		{
			//	 load all the contributions
			monitor.Progress(1,1,"コントリビューションをロード中");
			monitor.SetMaximum(2,plugins.Length*2);

			foreach( Plugin p in plugins ) 
			{
				monitor.Progress(2,1,p.name);
				try 
				{
					p.loadBinaries();
				} 
				catch( Exception e ) 
				{
					ReportError(e.Message,e);
				}
			}

			foreach( Plugin p in plugins ) 
			{
				monitor.Progress(2,1,p.name);
				try 
				{
					// this will call AddContribution method
					p.loadContributions();
					p._state = ModuleState.Ready;
				} 
				catch( Exception e ) 
				{
					if(p._state != ModuleState.FatalError)
						p._state = ModuleState.PartialError;
					ReportError(e.Message,e);
				}
			}
		}

		private void InitContributions(ProgressMonitor monitor)
		{
			// initialize contributions
			monitor.Progress(1,1,"コントリビューションを初期化中");
			monitor.SetMaximum(2,contributions.Length);
			foreach( Contribution contrib in contributions ) 
			{
				monitor.Progress(2,1,contrib.id);
				try 
				{
					contrib.onInitComplete();
					contrib._state = ModuleState.Ready;
				} 
				catch( Exception e ) 
				{
					contrib._state = ModuleState.FatalError;
					string templ = Core.resources["plugin.contrib_init_error"].stringValue;
					ReportError(string.Format(templ,contrib.parent.name,"contrib.name",contrib.id),e);
				}
			
			}
		}

		private void ReportError(string msg, Exception e)
		{
			if( errCount++ <= 5 )
			{
				UIUtil.ShowException(msg,e,UIInformLevel.minor);
				return;
			}
			if( errCount>5 ) return; // skip too many
			string templ = Core.resources["plugin.too_many_errors"].stringValue;
			UIUtil.ShowException(msg,e,UIInformLevel.minor);
		}
		#endregion

		/// <summary>
		/// Gets the default plug-in directory.
		/// </summary>
		/// <returns></returns>
		public static string getDefaultPluginDirectory() 
		{
			return Directories.PluginDir;
		}

		#region called from Plugin constructor
		/// <summary>
		/// Registers a <c>ContributionFactory</c>.
		/// This method has to be called before the initialization.
		/// Normally, this method is called by <c>Plugin</c> but the caller
		/// can invoke this method before calling the init method.
		/// </summary>
		public void addContributionFactory( string name, ContributionFactory factory ) 
		{
			if( contributionFactories.Contains(name) )
				throw new Exception(string.Format(
					"contribution type \"{0}\" is already registered.",name));

			contributionFactories.Add(name,factory);
		}

		// 
		public ContributionFactory getContributionFactory( string name ) {
			ContributionFactory factory = (ContributionFactory)
				contributionFactories[name];
			if(factory==null)
				throw new Exception(name+"は未知のコントリビューションです");
			else
				return factory;
		}
		#endregion

		/// <summary>
		/// Lists up contributions of the given type.
		/// </summary>
		public Array listContributions( Type contributionType ) 
		{
			ArrayList list = new ArrayList();
			foreach( Plugin p in plugins ) {
				foreach( Contribution contrib in p.contributions ) {
					if( contributionType.IsInstanceOfType(contrib) )
						list.Add(contrib);
				}
			}

			return list.ToArray(contributionType);
		}


		/// <summary>
		/// Gets all contributions.
		/// </summary>
		public Contribution[] contributions {
			get {
				ArrayList list = new ArrayList();
				foreach( Plugin p in plugins )
					foreach( Contribution contrib in p.contributions )
						list.Add(contrib);

				return (Contribution[])list.ToArray(typeof(Contribution));
			}
		}

		public void AddContribution( Contribution contrib ) {
			if(contributionMap.ContainsKey(contrib.id))
			{
			}
			contributionMap.Add( contrib.id, contrib );
		}

		/// <summary>
		/// Gets the contribution with a given ID, or null if not found.
		/// </summary>
		public Contribution getContribution( string id ) {
			return (Contribution)contributionMap[id];
		}

		/// <summary>
		/// Enumerates all plug-in objects.
		/// </summary>
		public IEnumerator GetEnumerator() 
		{
			return plugins.GetEnumerator();
		}

		/// <summary>
		/// Get the plug-in of the specified name, or null if not found.
		/// </summary>
		public Plugin getPlugin( string name ) 
		{
			return (Plugin)pluginMap[name];
		}

		public string getInstallInfo()
		{
			string output = "Installed Plugins (except for system plugins).";
			foreach(Plugin p in plugins)
			{
				if(!p.name.StartsWith("system"))
				{
					output+=Environment.NewLine;
					output+=string.Format("{0}[{1}] {2:yyyyMMdd-HHmm}",p.title,p.name,p.lastModifiedTime);
				}
			}
			return output;
		}
	}
}
