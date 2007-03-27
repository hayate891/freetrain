using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using freetrain.util;
using freetrain.contributions.sound;
using freetrain.contributions.others;
using freetrain.contributions.rail;
using freetrain.contributions.land;
using freetrain.contributions.train;
using freetrain.contributions.common;
using freetrain.contributions.structs;
using freetrain.contributions.road;

namespace freetrain.framework.plugin
{
	/// <summary>
	/// Loads plug-ins.
	/// </summary>
	public class PluginManager
	{
		/// <summary> The singleton instance. </summary>
		public static PluginManager theInstance;

		/// <summary>
		/// All loaded plug-ins.
		/// </summary>
		private Plugin[] plugins;

		/// <summary>
		/// Plugins keyed by their names.
		/// </summary>
		private readonly IDictionary pluginMap = new Hashtable();
		
		/// <summary>
		/// Contribution factories that are used to load contributions.
		/// </summary>
		private readonly IDictionary contributionFactories = new Hashtable();

		/// <summary>
		/// Contributions keyed by their IDs.
		/// </summary>
		private readonly IDictionary contributionMap = new Hashtable();



		public PluginManager() {
			theInstance = this;

			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(onAssemblyResolve);
		}

		private static string getModuleName( string name ) {
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

		/// <summary>
		/// This method should be called after the object is created.
		/// </summary>
		/// <param name="dirs">
		/// collection of strings (directory names)
		/// for each directory in this collection, its sub-directories
		/// are scanned for plugin.xml
		/// </param>
		public void init( ICollection dirs, ProgressHandler progressHandler ) {

			Set pluginSet = new Set();
			int errCount=0;
			
			// locate plugins
			foreach( string dir in dirs ) {
				progressHandler("Searching for plugins "+dir);
				//! progressHandler("プラグインを検索中 "+dir);
				
				if( !File.Exists(Path.Combine(dir,"plugin.xml")) )
					continue;	// this directory doesn't have the plugin.xml file.

				try {
					Plugin p = new Plugin(dir);
					pluginSet.add( p );
					if( pluginMap.Contains(p.name) ) {
						// loaded more than once
						throw new Exception( string.Format(
							"Plugin 「{0}」 is loaded from more than one place ({1} and {2})",
							//! "プラグイン「{0}」は{1}と{2}の二箇所からロードされています",
							p.name, p.dirName, ((Plugin)pluginMap[p.name]).dirName) );
					}
					pluginMap.Add( p.name, p );
				} catch( Exception e ) {
					if( errCount++ <= 5 )
						MessageBox.Show(e.ToString(),
							"プラグイン"+Path.GetFileName(dir)+"がロードできません");
							//! "Plugin "+Path.GetFileName(dir)+" can not be loaded");
					if( errCount==5 )
						MessageBox.Show("There are too many plugin loading errors");
						//! MessageBox.Show("プラグインのロードエラーが多すぎます");
				}
			}

			progressHandler("Sorting dependencies");
			//! progressHandler("依存関係を整理中");
			{// convert it to an array by sorting them in the order of dependency
				this.plugins = new Plugin[pluginSet.count];
				int ptr=0;

				while( !pluginSet.isEmpty ) {
					Plugin p = (Plugin)pluginSet.getOne();
					try {
						while(true) {
							Plugin[] deps = p.getDependencies();
							int i;
							for( i=0; i<deps.Length; i++ )
								if( pluginSet.contains(deps[i]) )
									break;
							if(i==deps.Length)
								break;
							else
								p = deps[i];
						}
					} catch( Exception e ) {
						if( errCount++ <= 5 )
							MessageBox.Show(e.ToString(),
								"Plugin "+p.name+" can not be loaded");
								//! "プラグイン"+p.name+"がロードできません");
						if( errCount==5 )
							MessageBox.Show("There are too many plugin loading errors");
							//! MessageBox.Show("プラグインのロードエラーが多すぎます");
					}

					pluginSet.remove(p);
					plugins[ptr++] = p;
				}
			}

			//	 load all the contributions
			progressHandler("Loading contributions");
			//! progressHandler("コントリビューションをロード中");
			foreach( Plugin p in plugins ) {
				try {
					p.loadContributions();
				} catch( Exception e ) {
					if( errCount++ <= 5 )
						ErrorMessageBox.show( null, 
							"Plugin "+p.name+" can not be loaded",e);
							//! "プラグイン"+p.name+"がロードできません",e);
					if( errCount==5 )
						MessageBox.Show("There are too many plugin loading errors");
						//! MessageBox.Show("プラグインのロードエラーが多すぎます");
				}
			}

			// initialize contributions
			progressHandler("Initializing contributions");
			//! progressHandler("コントリビューションを初期化中");
			foreach( Contribution contrib in contributions ) {
				try {
					contrib.onInitComplete();
				} catch( Exception e ) {
					ErrorMessageBox.show(null,"Contribution "+contrib.id+" that is contained in plugin "+contrib.parent.name+" can not be initialized",e);
					//! ErrorMessageBox.show(null,"プラグイン"+contrib.parent.name+"内のコントリビューション"+contrib.id+"が初期化できません",e);
					errCount++;
					break;	// abort
				}
			}

			{// make sure there's no duplicate id
				IDictionary dic = new Hashtable();
				foreach( Contribution contrib in contributions ) {
					if( dic[contrib.id]!=null ) {
						throw new FormatException("ID:"+contrib.id+" is not unique");
						//! throw new FormatException("ID:"+contrib.id+"が一意ではありません");
					}
					dic[contrib.id] = contrib;
				}
			}

			if( errCount!=0 ) {
				// error during the initialization
				Environment.Exit(errCount);
			}
		}


		/// <summary>
		/// Gets the default plug-in directory.
		/// </summary>
		/// <returns></returns>
		public static string getDefaultPluginDirectory() {
			// try the IDE directory first
			string pluginDir = Path.GetFullPath(Path.Combine(
				Core.installationDirectory, @"..¥..¥plugins" ));
			if(Directory.Exists(pluginDir))
				return pluginDir;

			// if we can't find it, try the directory under the executable directory
			pluginDir = Path.GetFullPath(Path.Combine(
				Core.installationDirectory, @"plugins" ));
			if(Directory.Exists(pluginDir))
				return pluginDir;
				
			throw new IOException("unable to find the plug-in directory: "+pluginDir);
		}

		/// <summary>
		/// Registers a <c>ContributionFactory</c>.
		/// This method has to be called before the initialization.
		/// Normally, this method is called by <c>Plugin</c> but the caller
		/// can invoke this method before calling the init method.
		/// </summary>
		public void addContributionFactory( string name, ContributionFactory factory ) {
			if( contributionFactories.Contains(name) )
				throw new Exception(string.Format(
					"contribution type ¥"{0}¥" is already registered.",name));

			contributionFactories.Add(name,factory);
		}

		public ContributionFactory getContributionFactory( string name ) {
			ContributionFactory factory = (ContributionFactory)
				contributionFactories[name];

			if(factory==null)
				throw new Exception(name+" is an unknown contribution");
				//! throw new Exception(name+"は未知のコントリビューションです");
			else
				return factory;
		}


		/// <summary>
		/// Enumerates all plug-in objects.
		/// </summary>
		public IEnumerator GetEnumerator() {
			return new ArrayEnumerator(plugins);
		}





		/// <summary>
		/// Gets all the station contributions.
		/// </summary>
		public StationContribution[] stations {
			get {
				return (StationContribution[])listContributions(typeof(StationContribution));
			}
		}
		public readonly StructureGroupGroup stationGroup = new StructureGroupGroup();

		/// <summary>
		/// Gets all the special rail contributions.
		/// </summary>
		public SpecialRailContribution[] specialRails {
			get {
				return (SpecialRailContribution[])listContributions(typeof(SpecialRailContribution));
			}
		}

		/// <summary>
		/// Gets all the rail stationary contributions
		/// </summary>
		public RailStationaryContribution[] railStationaryStructures {
			get {
				return (RailStationaryContribution[])listContributions(typeof(RailStationaryContribution));
			}
		}
		public readonly StructureGroupGroup railStationaryGroup = new StructureGroupGroup();

		/// <summary>
		/// Gets all the commercial structure contributions.
		/// </summary>
		public CommercialStructureContribution[] commercialStructures {
			get {
				return (CommercialStructureContribution[])listContributions(typeof(CommercialStructureContribution));
			}
		}
		public readonly StructureGroupGroup commercialStructureGroup = new StructureGroupGroup();

		/// <summary>
		/// Gets all the special structure contributions.
		/// </summary>
		public SpecialStructureContribution[] specialStructures {
			get {
				return (SpecialStructureContribution[])listContributions(typeof(SpecialStructureContribution));
			}
		}

		/// <summary>
		/// Gets all the road contributions.
		/// </summary>
		public RoadContribution[] roads {
			get {
				return (RoadContribution[])listContributions(typeof(RoadContribution));
			}
		}


		/// <summary>
		/// Gets all the BGM contributions.
		/// </summary>
		public BGMContribution[] bgms {
			get {
				return (BGMContribution[])listContributions(typeof(BGMContribution));
			}
		}

		/// <summary>
		/// Gets all the menu item contributions.
		/// </summary>
		public MenuContribution[] menus {
			get {
				return (MenuContribution[])listContributions(typeof(MenuContribution));
			}
		}

		/// <summary>
		/// Gets all the train contributions.
		/// </summary>
		public TrainContribution[] trains {
			get {
				return (TrainContribution[])listContributions(typeof(TrainContribution));
			}
		}

		/// <summary>
		/// Gets all the train controller contributions.
		/// </summary>
		public TrainControllerContribution[] trainControllers {
			get {
				return (TrainControllerContribution[])listContributions(typeof(TrainControllerContribution));
			}
		}

		public VarHeightBuildingContribution[] varHeightBuildings {
			get {
				return (VarHeightBuildingContribution[])listContributions(typeof(VarHeightBuildingContribution));
			}
		}
		public readonly StructureGroupGroup varHeightBuildingsGroup = new StructureGroupGroup();



		public LandBuilderContribution[] landBuilders {
			get {
				return (LandBuilderContribution[])listContributions(typeof(LandBuilderContribution));
			}
		}
		public readonly LandBuilderGroupGroup landBuilderGroup = new LandBuilderGroupGroup();



		/// <summary>
		/// Lists up contributions of the given type.
		/// </summary>
		public Array listContributions( Type contributionType ) {
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

		public void addContribution( Contribution contrib ) {
			contributionMap.Add( contrib.id, contrib );
		}

		/// <summary>
		/// Gets the contribution with a given ID, or null if not found.
		/// </summary>
		public Contribution getContribution( string id ) {
			return (Contribution)contributionMap[id];
		}

		/// <summary>
		/// Get the plug-in of the specified name, or null if not found.
		/// </summary>
		public Plugin getPlugin( string name ) {
			return (Plugin)pluginMap[name];
		}
	}
}