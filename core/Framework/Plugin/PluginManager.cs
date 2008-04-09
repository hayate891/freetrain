#region LICENSE
/*
 * Copyright (C) 2007 - 2008 FreeTrain Team (http://freetrain.sourceforge.net)
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
#endregion LICENSE

using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using FreeTrain.Util;
using FreeTrain.Contributions.Sound;
using FreeTrain.Contributions.Others;
using FreeTrain.Contributions.Rail;
using FreeTrain.Contributions.Land;
using FreeTrain.Contributions.Train;
using FreeTrain.Contributions.Common;
using FreeTrain.Contributions.Structs;
using FreeTrain.Contributions.Road;

namespace FreeTrain.Framework.Plugin
{
    /// <summary>
    /// Loads plug-ins.
    /// </summary>
    public static class PluginManager
    {
        ///// <summary> The singleton instance. </summary>
        //public static PluginManager theInstance;

        /// <summary>
        /// All loaded plug-ins.
        /// </summary>
        private static PluginDefinition[] plugins;

        /// <summary>
        /// 
        /// </summary>
        public static PluginDefinition[] Plugins
        {
            get { return PluginManager.plugins; }
            set { PluginManager.plugins = value; }
        }

        /// <summary>
        /// Plugins keyed by their names.
        /// </summary>
        private static readonly IDictionary pluginMap = new Hashtable();

        /// <summary>
        /// Contribution factories that are used to load contributions.
        /// </summary>
        private static readonly IDictionary contributionFactories = new Hashtable();

        /// <summary>
        /// Contributions keyed by their IDs.
        /// </summary>
        private static readonly IDictionary contributionMap = new Hashtable();

        /// <summary>
        /// 
        /// </summary>
        static PluginManager()
        {
            //theInstance = this;

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(OnAssemblyResolve);
        }

        private static string GetModuleName(string name)
        {
            return name.Substring(0, name.IndexOf(','));
        }

        private static Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            // TODO: improve performance by having a dictionary from name to Assemblies.
            // TODO: what is the correct way to use an application specific logic to resolve assemblies
            Trace.WriteLine("onAssemblyResolve resolving " + args.Name);

            string name = GetModuleName(args.Name);

            if (PluginManager.plugins == null)
            {
                return null;
            }

            // try assemblies of plug-ins
            foreach (Contribution cont in PluginManager.PublicContributions)
            {
                Assembly asm = cont.Assembly;

                if (GetModuleName(asm.FullName) == name)
                {
                    return asm;
                }
            }

            Trace.WriteLine("OnAssemblyResolve failed");
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
        /// <param name="errorHandler"></param>
        /// <param name="progressHandler"></param>
        public static void Init(ICollection dirs, ProgressHandler progressHandler, IPluginErrorHandler errorHandler)
        {

            Set pluginSet = new Set();
            Hashtable errorPlugins = new Hashtable();
            int errCount = 0;
            int count = 0;
            float c_max = dirs.Count * 4;
            bool errBreak = false;
            if (errorHandler == null)
            {
                errorHandler = new SilentPluginErrorHandler();
            }

            // locate plugins
            foreach (string dir in dirs)
            {
                progressHandler("Searching for plugins...\n" + Path.GetFileName(dir), ++count / c_max);
                //! progressHandler("プラグインを検索中\n"+Path.GetFileName(dir),++count/c_max);

                if (!File.Exists(Path.Combine(dir, "plugin.xml")))
                {
                    continue;	// this directory doesn't have the plugin.xml file.
                }
                PluginDefinition p = null;
                try
                {
                    p = new PluginDefinition(dir);
                    p.loadContributionFactories();
                }
                catch (Exception e)
                {
                    errCount++;
                    p = PluginDefinition.loadFailSafe(dir);
                    errorPlugins.Add(p, e);
                    errBreak = errorHandler.OnPluginLoadError(p, e);
                    if (errBreak)
                    {
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                if (pluginMap.Contains(p.name))
                {
                    errCount++;
                    // loaded more than once
                    Exception e = new Exception(string.Format(
                        "Plugin \"{0}\" is loaded from more than one place ({1} and {2})",
                        //! "プラグイン「{0}」は{1}と{2}の二箇所からロードされています",
                        p.name, p.dirName, ((PluginDefinition)pluginMap[p.name]).dirName));
                    errBreak = errorHandler.OnNameDuplicated(pluginMap[p.name] as PluginDefinition, p, e);
                    errorPlugins.Add(p, e);
                    if (errBreak)
                    {
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                pluginMap.Add(p.name, p);
                pluginSet.add(p);
            }
            if (errBreak)
            {
                Environment.Exit(-1);
            }

            {// convert it to an array by sorting them in the order of dependency
                plugins = new PluginDefinition[pluginSet.Count];
                int ptr = 0;
                PluginDefinition p = null;
                while (!pluginSet.isEmpty)
                {
                    progressHandler("Sorting dependencies...", ++count / c_max);
                    //! progressHandler("依存関係を整理中",++count/c_max);
                    p = (PluginDefinition)pluginSet.getOne();
                    try
                    {
                        while (true)
                        {
                            PluginDefinition[] deps = p.getDependencies();
                            int i;
                            for (i = 0; i < deps.Length; i++)
                            {
                                if (pluginSet.contains(deps[i]))
                                {
                                    break;
                                }
                            }
                            if (i == deps.Length)
                            {
                                break;
                            }
                            else
                            {
                                p = deps[i];
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        errCount++;
                        errBreak = errorHandler.OnPluginLoadError(p, e);
                        if (!errorPlugins.ContainsKey(p))
                        {
                            errorPlugins.Add(p, e);
                        }
                        if (errBreak)
                        {
                            break;
                        }
                    }
                    pluginSet.remove(p);
                    plugins[ptr++] = p;
                }
            }
            if (errBreak)
            {
                Environment.Exit(-2);
            }

            //	 load all the contributions			
            foreach (PluginDefinition p in plugins)
            {
                progressHandler("Loading contributions...\n" + Path.GetFileName(p.dirName), ++count / c_max);
                //! progressHandler("コントリビューションをロード中\n"+Path.GetFileName(p.dirName),++count/c_max);
                try
                {
                    p.loadContributions();
                }
                catch (Exception e)
                {
                    errCount++;
                    errBreak = errorHandler.OnPluginLoadError(p, e);
                    if (!errorPlugins.ContainsKey(p))
                    {
                        errorPlugins.Add(p, e);
                    }
                    if (errBreak)
                    {
                        break;
                    }
                }
            }
            if (errBreak)
                Environment.Exit(-3);

            // initialize contributions
            count = (int)c_max;
            c_max += PublicContributions.Length;
            foreach (Contribution contrib in PublicContributions)
            {
                progressHandler("Initializing contributions...\n" + contrib.BaseUri, ++count / c_max);
                //! progressHandler("コントリビューションを初期化中\n"+contrib.baseUri,++count/c_max);
                try
                {
                    contrib.OnInitComplete();
                }
                catch (Exception e)
                {
                    errCount++;
                    errBreak = errorHandler.OnContributionInitError(contrib, e);
                    PluginDefinition p = contrib.Parent;
                    if (!errorPlugins.ContainsKey(p))
                    {
                        errorPlugins.Add(p, e);
                    }
                    if (errBreak)
                    {
                        break;
                    }
                }
            }
            if (errBreak)
            {
                Environment.Exit(-4);
            }

            {// make sure there's no duplicate id
                progressHandler("Checking for duplicate IDs...", 1.0f);
                //! progressHandler("重複IDのチェック中",1.0f);
                IDictionary dic = new Hashtable();
                foreach (Contribution contrib in PublicContributions)
                {
                    if (dic[contrib.Id] != null)
                    {
                        errCount++;
                        Exception e = new FormatException("ID:" + contrib.Id + " is not unique");
                        //! Exception e = new FormatException("ID:"+contrib.id+"が一意ではありません");
                        errBreak = errorHandler.OnContribIDDuplicated(dic[contrib.Id] as Contribution, contrib, e);
                        PluginDefinition p = contrib.Parent;
                        if (!errorPlugins.ContainsKey(p))
                        {
                            errorPlugins.Add(p, e);
                        }
                        if (errBreak)
                        {
                            break;
                        }
                    }
                    else
                    {
                        dic[contrib.Id] = contrib;
                    }
                }
            }
            if (errBreak)
            {
                Environment.Exit(-5);
            }
            if (errCount > 0)
            {
                if (errorHandler.OnFinal(errorPlugins, errCount))
                {
                    Environment.Exit(errCount);
                }
            }
        }

        /// <summary>
        /// Gets the default plug-in directory.
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultPluginDirectory()
        {
            // try the IDE directory first
            //string pluginDir = Path.GetFullPath(Path.Combine(Core.installationDirectory, @"..\..\plugins" ));
            string pluginDir = Path.Combine(Application.StartupPath,"plugins");
            if (Directory.Exists(pluginDir))
            {
                return pluginDir;
            }

            pluginDir = Path.Combine(Application.StartupPath, Path.Combine(Path.Combine("..",".."),"plugins"));
            if (Directory.Exists(pluginDir))
            {
                return pluginDir;
            }

            // if we can't find it, try the directory under the executable directory
            pluginDir = Path.GetFullPath(Path.Combine(
                Core.InstallationDirectory, @"plugins"));
            if (Directory.Exists(pluginDir))
            {
                return pluginDir;
            }

            throw new IOException("Unable to find the plug-in directory: " + pluginDir);
        }

        /// <summary>
        /// Registers a <c>ContributionFactory</c>.
        /// This method has to be called before the initialization.
        /// Normally, this method is called by <c>Plugin</c> but the caller
        /// can invoke this method before calling the init method.
        /// </summary>
        public static void AddContributionFactory(string name, IContributionFactory factory)
        {
            if (contributionFactories.Contains(name))
            {
                throw new Exception(string.Format(
                    "contribution type \"{0}\" is already registered.", name));
            }

            contributionFactories.Add(name, factory);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IContributionFactory GetContributionFactory(string name)
        {
            IContributionFactory factory = (IContributionFactory)
                contributionFactories[name];

            if (factory == null)
            {
                throw new Exception(name + " is an unknown contribution");
            }
            //! throw new Exception(name+"は未知のコントリビューションです");
            else
            {
                return factory;
            }
        }

        /// <summary>
        /// Enumerates all plug-in objects.
        /// </summary>
        public static IEnumerator GetEnumerator()
        {
            return new ArrayEnumerator(plugins);
        }

        /// <summary>
        /// Gets all the station contributions.
        /// </summary>
        public static StationContribution[] Stations
        {
            get
            {
                return (StationContribution[])ListContributions(typeof(StationContribution));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static readonly StructureGroupGroup stationGroup = new StructureGroupGroup();

        /// <summary>
        /// 
        /// </summary>
        public static StructureGroupGroup StationGroup
        {
            get { return stationGroup; }
        } 

        /// <summary>
        /// Gets all the special rail contributions.
        /// </summary>
        public static SpecialRailContribution[] SpecialRails
        {
            get
            {
                return (SpecialRailContribution[])ListContributions(typeof(SpecialRailContribution));
            }
        }

        /// <summary>
        /// Gets all the rail stationary contributions
        /// </summary>
        public static RailStationaryContribution[] RailStationaryStructures
        {
            get
            {
                return (RailStationaryContribution[])ListContributions(typeof(RailStationaryContribution));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static readonly StructureGroupGroup railStationaryGroup = new StructureGroupGroup();

        /// <summary>
        /// 
        /// </summary>
        public static StructureGroupGroup RailStationaryGroup
        {
            get { return railStationaryGroup; }
        } 

        /// <summary>
        /// Gets all the commercial structure contributions.
        /// </summary>
        public static CommercialStructureContribution[] CommercialStructures
        {
            get
            {
                return (CommercialStructureContribution[])ListContributions(typeof(CommercialStructureContribution));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static readonly StructureGroupGroup commercialStructureGroup = new StructureGroupGroup();

        /// <summary>
        /// 
        /// </summary>
        public static StructureGroupGroup CommercialStructureGroup
        {
            get { return commercialStructureGroup; }
        } 

        /// <summary>
        /// Gets all the special structure contributions.
        /// </summary>
        public static SpecialStructureContribution[] SpecialStructures
        {
            get
            {
                return (SpecialStructureContribution[])ListContributions(typeof(SpecialStructureContribution));
            }
        }

        /// <summary>
        /// Gets all the road contributions.
        /// </summary>
        public static RoadContribution[] Roads
        {
            get
            {
                return (RoadContribution[])ListContributions(typeof(RoadContribution));
            }
        }

        /// <summary>
        /// Gets all the BGM contributions.
        /// </summary>
        public static BGMContribution[] Bgms
        {
            get
            {
                return (BGMContribution[])ListContributions(typeof(BGMContribution));
            }
        }

        /// <summary>
        /// Gets all the menu item contributions.
        /// </summary>
        public static MenuContribution[] Menus
        {
            get
            {
                return (MenuContribution[])ListContributions(typeof(MenuContribution));
            }
        }

        /// <summary>
        /// Gets all the train contributions.
        /// </summary>
        public static TrainContribution[] Trains
        {
            get
            {
                return (TrainContribution[])ListContributions(typeof(TrainContribution));
            }
        }

        /// <summary>
        /// Gets all the train controller contributions.
        /// </summary>
        public static TrainControllerContribution[] TrainControllers
        {
            get
            {
                return (TrainControllerContribution[])ListContributions(typeof(TrainControllerContribution));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static VarHeightBuildingContribution[] VarHeightBuildings
        {
            get
            {
                return (VarHeightBuildingContribution[])ListContributions(typeof(VarHeightBuildingContribution));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly StructureGroupGroup varHeightBuildingsGroup = new StructureGroupGroup();

        /// <summary>
        /// 
        /// </summary>
        public static LandBuilderContribution[] LandBuilders
        {
            get
            {
                return (LandBuilderContribution[])ListContributions(typeof(LandBuilderContribution));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static readonly LandBuilderGroupGroup landBuilderGroup = new LandBuilderGroupGroup();

        /// <summary>
        /// 
        /// </summary>
        public static LandBuilderGroupGroup LandBuilderGroup
        {
            get { return landBuilderGroup; }
        } 

        /// <summary>
        /// Lists up contributions of the given type.
        /// </summary>
        public static Array ListContributions(Type contributionType)
        {
            ArrayList list = new ArrayList();
            foreach (PluginDefinition p in plugins)
            {
                foreach (Contribution contrib in p.contributions)
                {
                    if (contributionType.IsInstanceOfType(contrib))
                    {
                        list.Add(contrib);
                    }
                }
            }

            return list.ToArray(contributionType);
        }

        /// <summary>
        /// Gets all contributions. except for runtime generated ones.
        /// </summary>
        public static Contribution[] PublicContributions
        {
            get
            {
                ArrayList list = new ArrayList();
                foreach (PluginDefinition p in plugins)
                {
                    foreach (Contribution contrib in p.contributions)
                    {
                        list.Add(contrib);
                    }
                }

                return (Contribution[])list.ToArray(typeof(Contribution));
            }
        }

        /// <summary>
        /// Gets all contributions including runtime generat.
        /// </summary>
        public static Contribution[] AllContributions
        {
            get
            {
                ArrayList list = new ArrayList();
                foreach (Contribution contrib in contributionMap.Values)
                {
                    list.Add(contrib);
                }
                return (Contribution[])list.ToArray(typeof(Contribution));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contrib"></param>
        public static void AddContribution(Contribution contrib)
        {
            contributionMap.Add(contrib.Id, contrib);
        }

        /// <summary>
        /// Gets the contribution with a given ID, or null if not found.
        /// </summary>
        public static Contribution GetContribution(string id)
        {
            return (Contribution)contributionMap[id];
        }

        /// <summary>
        /// Get the plug-in of the specified name, or null if not found.
        /// </summary>
        public static PluginDefinition GetPlugin(string name)
        {
            return (PluginDefinition)pluginMap[name];
        }
    }
}
