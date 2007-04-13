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

		/// <summary>
		/// Contribution Types keyed by their CtbTypes (string specified a Type of Contribution).
		/// </summary>
		private readonly Hashtable ctbTypeMap = new Hashtable();

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

			if( Main.plugins.plugins==null )	return null;

			// try assemblies of plug-ins
			foreach( Contribution cont in Main.plugins.Contributions ) {
				Assembly asm = cont.Assembly;

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
						if( pluginMap.Contains(p.ID) ) 
						{
							p._state = InstallationState.FatalError;
							// loaded more than once
							// maybe same subdir name in different plugin dirs.
							throw new Exception( string.Format(
								"プラグイン「{0}」は{1}と{2}の二箇所からロードされています",
								p.ID, p.dirName, ((Plugin)pluginMap[p.ID]).dirName) );
						}
						pluginMap.Add( p.ID, p );
					} 
					catch( Exception e ) 
					{
						Debug.WriteLine(e.Message);
						if(p!=null)
							p._state = InstallationState.FatalError;

						string templ = Main.resources["plugin.plugin_load_error"].stringValue;
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
				monitor.Progress(2,1,p.ID);
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
				monitor.Progress(2,1,p.ID);
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
				monitor.Progress(2,1,p.ID);
				try 
				{
					// this will call AddContribution method
					p.loadContributions();
					p._state = InstallationState.Ready;
				} 
				catch( Exception e ) 
				{
					if(p._state != InstallationState.FatalError)
						p._state = InstallationState.PartialError;
					ReportError(e.Message,e);
				}
			}
		}

		private void InitContributions(ProgressMonitor monitor)
		{
			// initialize contributions
			monitor.Progress(1,1,"コントリビューションを初期化中");
			monitor.SetMaximum(2,Contributions.Length);
			foreach( Contribution contrib in Contributions ) 
			{
				monitor.Progress(2,1,contrib.ID);
				try 
				{
					contrib.onInitComplete();
					contrib._state = InstallationState.Ready;
				} 
				catch( Exception e ) 
				{
					contrib._state = InstallationState.FatalError;
					string templ = Main.resources["plugin.contrib_init_error"].stringValue;
					ReportError(string.Format(templ,contrib.parent.ID,"contrib.name",contrib.ID),e);
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
			string templ = Main.resources["plugin.too_many_errors"].stringValue;
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
		public void AddContributionFactory( string name, ContributionFactory factory ) 
		{
			if( contributionFactories.Contains(name) )
				throw new Exception(string.Format(
					"contribution type \"{0}\" is already registered.",name));

			contributionFactories.Add(name,factory);
		}

		// 
		public ContributionFactory GetContributionFactory( string name ) {
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
		public Array ListContributions( Type contributionType, bool hideDisabled ) 
		{
			ArrayList list = new ArrayList();
			foreach( Plugin p in plugins ) {
				foreach( Contribution contrib in p.contributions ) {
					if( contributionType.IsInstanceOfType(contrib) )
						if( !hideDisabled || contrib.IsAttached )
							list.Add(contrib);
				}
			}

			return list.ToArray(contributionType);
		}


		/// <summary>
		/// Gets all contributions.
		/// </summary>
		public Contribution[] Contributions {
			get {
				ArrayList list = new ArrayList();
				foreach( Plugin p in plugins )
					foreach( Contribution contrib in p.contributions )
						list.Add(contrib);

				return (Contribution[])list.ToArray(typeof(Contribution));
			}
		}

		public void AddContribution( Contribution contrib ) {
			if(contributionMap.ContainsKey(contrib.ID))
			{
				// TODO:
				Debug.WriteLine("Duplicate contribution id found:"+contrib.ID);
			}
			contributionMap.Add( contrib.ID, contrib );
		}

		/// <summary>
		/// Gets the contribution with a given ID, or null if not found.
		/// </summary>
		public Contribution GetContribution( string id ) {
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
		public Plugin GetPlugin( string name ) 
		{
			return (Plugin)pluginMap[name];
		}

		public Type GetDefinedType( string ctbType )
		{
			ContributionFactory factory = GetContributionFactory(ctbType);
			if( factory!=null )
				return factory.OutputType;
			else
				return null;
		}

		public string GetInstallInfo()
		{
			if(plugins == null )
				return "";
			string output = "Installed Plugins (except for system plugins).";
			foreach(Plugin p in plugins)
			{
				if(!p.ID.StartsWith("system"))
				{
					output+=Environment.NewLine;
					output+=string.Format("{0}[{1}] {2:yyyyMMdd-HHmm}",p.Title,p.ID,p.lastModifiedTime);
				}
			}
			return output;
		}
	}
}
