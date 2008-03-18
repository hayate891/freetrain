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
using System.IO;
using FreeTrain.Framework.plugin;
using FreeTrain.world;

namespace FreeTrain.Framework
{
    /// <summary>
    /// SilentInitializer の概要の説明です。
    /// </summary>
    public class ExternToolsHelper
    {
        private static bool initialized;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="plugindirs"></param>
        /// <param name="progressHandler"></param>
        /// <param name="errorHandler"></param>
        public static void InitializePlugins(string[] plugindirs, ProgressHandler progressHandler, PluginErrorHandler errorHandler)
        {
            if (initialized)
                Clear();
            if (MainWindow.mainWindow == null)
                MainWindow.mainWindow = new MainWindow(plugindirs, false);
            if (progressHandler == null)
                progressHandler = new ProgressHandler(SilentProgress);

            IList r = new ArrayList();
            string baseDir = PluginManager.getDefaultPluginDirectory();
            foreach (string subdir in Directory.GetDirectories(baseDir))
                r.Add(Path.Combine(baseDir, subdir));
            // load plug-ins
            Core.plugins.init(r, progressHandler, errorHandler);
            if (World.world == null)
                World.world = new World(new Distance(5, 5, 5), 0);
            initialized = true;
        }
        /// <summary>
        /// 
        /// </summary>
        public static void Clear()
        {
            // TODO:
            //Core.plugins.
            initialized = false;
        }

        static void SilentProgress(string msg, float progress)
        {
        }
    }
}
