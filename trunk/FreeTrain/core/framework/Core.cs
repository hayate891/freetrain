using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
using freetrain.framework.plugin;
using freetrain.framework.sound;
using freetrain.util;

namespace freetrain.framework
{
	/// <summary>
	/// Entry point to other static instances in the FreeTrain framework.
	/// </summary>
	public sealed class Core
	{
		private Core() {}		// no instantiation


		/// <summary>
		/// Registry key where the per-user application setting
		/// should be stored.
		/// </summary>
		public static RegistryKey userRegistry {
			get {
				return Registry.CurrentUser.CreateSubKey(@"Software\FreeTrain");
			}
		}

		/// <summary>
		/// Installation directory of the FreeTrain framework.
		/// </summary>
		public static string installationDirectory {
			get {
				return Directory.GetParent(Application.ExecutablePath).FullName;
			}
			set {
				userRegistry.SetValue("installationDirectory",value);
			}
		}


		/// <summary> Plug-ins. </summary>
		public static readonly PluginManager plugins = new PluginManager();

		/// <summary> Global options. </summary>
		public static readonly GlobalOptions options = new GlobalOptions().load();
		
		/// <summary> Game mode </summary>
		public static bool isConstructionMode {
			get {
				return _isConstructionMode;
			}
		}

		/// <summary>
		/// Handles BGM playback.
		/// Should be instanciated by attaching the main window.
		/// </summary>
		public static BGMManager bgmManager { get { return _bgmManager; } }

		/// <summary>
		/// Handles SFX.
		/// Should be instanciated by attaching the main window.
		/// </summary>
		public static SoundEffectManager soundEffectManager { get { return _soundEffectManager; } }
		

		private static SoundEffectManager _soundEffectManager;
		private static BGMManager _bgmManager;
		private static bool _isConstructionMode;



		/// <summary>
		/// Initializes the framework.
		/// Should be called once and only once.
		/// </summary>
		/// <param name="additionalPluginDirs">
		/// additional directories from which plug-ins are loaded.
		/// </param>
		/// <param name="owner">application's main window.</param>
		/// <param name="bgmMenuItem">"BGM" sub-menu</param>
		/// <param name="progressHandler">
		/// Receives initializtion progress report. Can be null.
		/// </param>
		public static void init( string[] args, Control owner, MenuItem bgmMenuItem,ProgressHandler progressHandler, bool constructionMode ) {
			
			_isConstructionMode = constructionMode;

			if(progressHandler==null)
				progressHandler = new ProgressHandler(silentProgressHandler);
			
			// load plug-ins
			Core.plugins.init(
				args.Length==0?getDefaultProfile():parseProfile(args[0]),
				progressHandler);

			_soundEffectManager = new SoundEffectManager(owner);
			_bgmManager = new BGMManager(bgmMenuItem);
		}

		private static void silentProgressHandler( string msg ) {
			Trace.WriteLine(msg);
		}
		
		/// <summary>
		/// Determines the plug-ins to be used from a profile.
		/// </summary>
		private static IList parseProfile( string profileFileName ) {
			// TODO: better error check
			Set r = new Set();

			XmlDocument doc = new XmlDocument();
			doc.Load(profileFileName);

			foreach( XmlElement e in doc.DocumentElement.SelectNodes("plugin") ) {
				string baseDir = Path.Combine(
					Directory.GetParent(profileFileName).ToString(), e.Attributes["dir"].Value );
				
				string[] subdirs = Directory.GetDirectories(
					baseDir, e.Attributes["includes"].Value );

				foreach( string subdir in subdirs )
					r.add(Path.Combine(baseDir,subdir));
			}

			return new ArrayList(r);
		}

		/// <summary>
		/// Determines the default set of plug-ins. Used when no profile is specified.
		/// </summary>
		/// <returns></returns>
		private static IList getDefaultProfile() {
			IList r = new ArrayList();
			
			string baseDir = PluginManager.getDefaultPluginDirectory();
			foreach( string subdir in Directory.GetDirectories(baseDir) )
				r.Add(Path.Combine(baseDir,subdir));
			
			return r;
		}
	}

	/// <summary>
	/// Function that receives a progress in initialization.
	/// </summary>
	public delegate void ProgressHandler( string msg );
}
