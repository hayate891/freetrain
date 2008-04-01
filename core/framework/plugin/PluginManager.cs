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
    /// 
    /// </summary>
    public interface IPluginErrorHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_1st"></param>
        /// <param name="p_2nd"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        bool OnNameDuplicated(PluginDefinition p_1st, PluginDefinition p_2nd, Exception e);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        bool OnPluginLoadError(PluginDefinition p, Exception e);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        bool OnContributionInitError(Contribution c, Exception e);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c_1st"></param>
        /// <param name="c_2nd"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        bool OnContribIDDuplicated(Contribution c_1st, Contribution c_2nd, Exception e);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorPlugins"></param>
        /// <param name="totalErrorCount"></param>
        /// <returns></returns>
        bool OnFinal(IDictionary errorPlugins, int totalErrorCount);
    }
    /// <summary>
    /// 
    /// </summary>
    public class SilentPluginErrorHandler : IPluginErrorHandler
    {
        #region PluginErrorHandler o
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_1st"></param>
        /// <param name="p_2nd"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool OnNameDuplicated(PluginDefinition p_1st, PluginDefinition p_2nd, Exception e)
        {
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool OnPluginLoadError(PluginDefinition p, Exception e)
        {
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool OnContributionInitError(Contribution c, Exception e)
        {
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c_1st"></param>
        /// <param name="c_2nd"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool OnContribIDDuplicated(Contribution c_1st, Contribution c_2nd, Exception e)
        {
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorPlugins"></param>
        /// <param name="totalErrorCount"></param>
        /// <returns></returns>
        public bool OnFinal(IDictionary errorPlugins, int totalErrorCount)
        {
            return true;
        }
        #endregion

    }


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
        private PluginDefinition[] plugins;

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


        /// <summary>
        /// 
        /// </summary>
        public PluginManager()
        {
            theInstance = this;

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(onAssemblyResolve);
        }

        private static string getModuleName(string name)
        {
            return name.Substring(0, name.IndexOf(','));
        }

        private static Assembly onAssemblyResolve(object sender, ResolveEventArgs args)
        {
            // TODO: improve performance by having a dictionary from name to Assemblies.
            // TODO: what is the correct way to use an application specific logic to resolve assemblies
            Trace.WriteLine("onAssemblyResolve resolving " + args.Name);

            string name = getModuleName(args.Name);

            if (Core.plugins.plugins == null) return null;

            // try assemblies of plug-ins
            foreach (Contribution cont in Core.plugins.publicContributions)
            {
                Assembly asm = cont.assembly;

                if (getModuleName(asm.FullName) == name)
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
        /// <param name="errorHandler"></param>
        /// <param name="progressHandler"></param>
        public void init(ICollection dirs, ProgressHandler progressHandler, IPluginErrorHandler errorHandler)
        {

            Set pluginSet = new Set();
            Hashtable errorPlugins = new Hashtable();
            int errCount = 0;
            int count = 0;
            float c_max = dirs.Count * 4;
            bool errBreak = false;
            if (errorHandler == null)
                errorHandler = new SilentPluginErrorHandler();

            // locate plugins
            foreach (string dir in dirs)
            {
                progressHandler("Searching for plugins...\n" + Path.GetFileName(dir), ++count / c_max);
                //! progressHandler("プラグインを検索中\n"+Path.GetFileName(dir),++count/c_max);

                if (!File.Exists(Path.Combine(dir, "plugin.xml")))
                    continue;	// this directory doesn't have the plugin.xml file.
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
                        break;
                    else
                        continue;
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
                        break;
                    else
                        continue;
                }
                pluginMap.Add(p.name, p);
                pluginSet.add(p);
            }
            if (errBreak)
                Environment.Exit(-1);

            {// convert it to an array by sorting them in the order of dependency
                this.plugins = new PluginDefinition[pluginSet.Count];
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
                                if (pluginSet.contains(deps[i]))
                                    break;
                            if (i == deps.Length)
                                break;
                            else
                                p = deps[i];
                        }
                    }
                    catch (Exception e)
                    {
                        errCount++;
                        errBreak = errorHandler.OnPluginLoadError(p, e);
                        if (!errorPlugins.ContainsKey(p))
                            errorPlugins.Add(p, e);
                        if (errBreak)
                            break;
                    }
                    pluginSet.remove(p);
                    plugins[ptr++] = p;
                }
            }
            if (errBreak)
                Environment.Exit(-2);

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
                        errorPlugins.Add(p, e);
                    if (errBreak)
                        break;
                }
            }
            if (errBreak)
                Environment.Exit(-3);

            // initialize contributions
            count = (int)c_max;
            c_max += publicContributions.Length;
            foreach (Contribution contrib in publicContributions)
            {
                progressHandler("Initializing contributions...\n" + contrib.baseUri, ++count / c_max);
                //! progressHandler("コントリビューションを初期化中\n"+contrib.baseUri,++count/c_max);
                try
                {
                    contrib.onInitComplete();
                }
                catch (Exception e)
                {
                    errCount++;
                    errBreak = errorHandler.OnContributionInitError(contrib, e);
                    PluginDefinition p = contrib.parent;
                    if (!errorPlugins.ContainsKey(p))
                        errorPlugins.Add(p, e);
                    if (errBreak)
                        break;
                }
            }
            if (errBreak)
                Environment.Exit(-4);

            {// make sure there's no duplicate id
                progressHandler("Checking for duplicate IDs...", 1.0f);
                //! progressHandler("重複IDのチェック中",1.0f);
                IDictionary dic = new Hashtable();
                foreach (Contribution contrib in publicContributions)
                {
                    if (dic[contrib.id] != null)
                    {
                        errCount++;
                        Exception e = new FormatException("ID:" + contrib.id + " is not unique");
                        //! Exception e = new FormatException("ID:"+contrib.id+"が一意ではありません");
                        errBreak = errorHandler.OnContribIDDuplicated(dic[contrib.id] as Contribution, contrib, e);
                        PluginDefinition p = contrib.parent;
                        if (!errorPlugins.ContainsKey(p))
                            errorPlugins.Add(p, e);
                        if (errBreak)
                            break;
                    }
                    else
                        dic[contrib.id] = contrib;
                }
            }
            if (errBreak)
                Environment.Exit(-5);
            if (errCount > 0)
            {
                if (errorHandler.OnFinal(errorPlugins, errCount))
                    Environment.Exit(errCount);
            }
        }


        /// <summary>
        /// Gets the default plug-in directory.
        /// </summary>
        /// <returns></returns>
        public static string getDefaultPluginDirectory()
        {
            // try the IDE directory first
            //string pluginDir = Path.GetFullPath(Path.Combine(Core.installationDirectory, @"..\..\plugins" ));
            string pluginDir = Path.Combine(Application.StartupPath,"plugins");
            if (Directory.Exists(pluginDir))
                return pluginDir;

            pluginDir = Path.Combine(Application.StartupPath, Path.Combine(Path.Combine("..",".."),"plugins"));
            if (Directory.Exists(pluginDir))
                return pluginDir;

            // if we can't find it, try the directory under the executable directory
            pluginDir = Path.GetFullPath(Path.Combine(
                Core.installationDirectory, @"plugins"));
            if (Directory.Exists(pluginDir))
                return pluginDir;

            throw new IOException("unable to find the plug-in directory: " + pluginDir);
        }

        /// <summary>
        /// Registers a <c>ContributionFactory</c>.
        /// This method has to be called before the initialization.
        /// Normally, this method is called by <c>Plugin</c> but the caller
        /// can invoke this method before calling the init method.
        /// </summary>
        public void addContributionFactory(string name, IContributionFactory factory)
        {
            if (contributionFactories.Contains(name))
                throw new Exception(string.Format(
                    "contribution type \"{0}\" is already registered.", name));

            contributionFactories.Add(name, factory);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IContributionFactory getContributionFactory(string name)
        {
            IContributionFactory factory = (IContributionFactory)
                contributionFactories[name];

            if (factory == null)
                throw new Exception(name + " is an unknown contribution");
            //! throw new Exception(name+"は未知のコントリビューションです");
            else
                return factory;
        }


        /// <summary>
        /// Enumerates all plug-in objects.
        /// </summary>
        public IEnumerator GetEnumerator()
        {
            return new ArrayEnumerator(plugins);
        }





        /// <summary>
        /// Gets all the station contributions.
        /// </summary>
        public StationContribution[] stations
        {
            get
            {
                return (StationContribution[])listContributions(typeof(StationContribution));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public readonly StructureGroupGroup stationGroup = new StructureGroupGroup();

        /// <summary>
        /// Gets all the special rail contributions.
        /// </summary>
        public SpecialRailContribution[] specialRails
        {
            get
            {
                return (SpecialRailContribution[])listContributions(typeof(SpecialRailContribution));
            }
        }

        /// <summary>
        /// Gets all the rail stationary contributions
        /// </summary>
        public RailStationaryContribution[] railStationaryStructures
        {
            get
            {
                return (RailStationaryContribution[])listContributions(typeof(RailStationaryContribution));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public readonly StructureGroupGroup railStationaryGroup = new StructureGroupGroup();

        /// <summary>
        /// Gets all the commercial structure contributions.
        /// </summary>
        public CommercialStructureContribution[] commercialStructures
        {
            get
            {
                return (CommercialStructureContribution[])listContributions(typeof(CommercialStructureContribution));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public readonly StructureGroupGroup commercialStructureGroup = new StructureGroupGroup();

        /// <summary>
        /// Gets all the special structure contributions.
        /// </summary>
        public SpecialStructureContribution[] specialStructures
        {
            get
            {
                return (SpecialStructureContribution[])listContributions(typeof(SpecialStructureContribution));
            }
        }

        /// <summary>
        /// Gets all the road contributions.
        /// </summary>
        public RoadContribution[] roads
        {
            get
            {
                return (RoadContribution[])listContributions(typeof(RoadContribution));
            }
        }


        /// <summary>
        /// Gets all the BGM contributions.
        /// </summary>
        public BGMContribution[] bgms
        {
            get
            {
                return (BGMContribution[])listContributions(typeof(BGMContribution));
            }
        }

        /// <summary>
        /// Gets all the menu item contributions.
        /// </summary>
        public MenuContribution[] menus
        {
            get
            {
                return (MenuContribution[])listContributions(typeof(MenuContribution));
            }
        }

        /// <summary>
        /// Gets all the train contributions.
        /// </summary>
        public TrainContribution[] trains
        {
            get
            {
                return (TrainContribution[])listContributions(typeof(TrainContribution));
            }
        }

        /// <summary>
        /// Gets all the train controller contributions.
        /// </summary>
        public TrainControllerContribution[] trainControllers
        {
            get
            {
                return (TrainControllerContribution[])listContributions(typeof(TrainControllerContribution));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public VarHeightBuildingContribution[] varHeightBuildings
        {
            get
            {
                return (VarHeightBuildingContribution[])listContributions(typeof(VarHeightBuildingContribution));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public readonly StructureGroupGroup varHeightBuildingsGroup = new StructureGroupGroup();


        /// <summary>
        /// 
        /// </summary>
        public LandBuilderContribution[] landBuilders
        {
            get
            {
                return (LandBuilderContribution[])listContributions(typeof(LandBuilderContribution));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public readonly LandBuilderGroupGroup landBuilderGroup = new LandBuilderGroupGroup();



        /// <summary>
        /// Lists up contributions of the given type.
        /// </summary>
        public Array listContributions(Type contributionType)
        {
            ArrayList list = new ArrayList();
            foreach (PluginDefinition p in plugins)
            {
                foreach (Contribution contrib in p.contributions)
                {
                    if (contributionType.IsInstanceOfType(contrib))
                        list.Add(contrib);
                }
            }

            return list.ToArray(contributionType);
        }


        /// <summary>
        /// Gets all contributions. except for runtime generated ones.
        /// </summary>
        public Contribution[] publicContributions
        {
            get
            {
                ArrayList list = new ArrayList();
                foreach (PluginDefinition p in plugins)
                    foreach (Contribution contrib in p.contributions)
                        list.Add(contrib);

                return (Contribution[])list.ToArray(typeof(Contribution));
            }
        }

        /// <summary>
        /// Gets all contributions including runtime generat.
        /// </summary>
        public Contribution[] allContributions
        {
            get
            {
                ArrayList list = new ArrayList();
                foreach (Contribution contrib in contributionMap.Values)
                    list.Add(contrib);
                return (Contribution[])list.ToArray(typeof(Contribution));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contrib"></param>
        public void addContribution(Contribution contrib)
        {
            contributionMap.Add(contrib.id, contrib);
        }

        /// <summary>
        /// Gets the contribution with a given ID, or null if not found.
        /// </summary>
        public Contribution getContribution(string id)
        {
            return (Contribution)contributionMap[id];
        }

        /// <summary>
        /// Get the plug-in of the specified name, or null if not found.
        /// </summary>
        public PluginDefinition getPlugin(string name)
        {
            return (PluginDefinition)pluginMap[name];
        }
    }

}
