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
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using FreeTrain.Util;
using SDL.net;

namespace FreeTrain.Framework
{
    /// <summary>
    /// Global Configuration.
    /// 
    /// This is an application-wide configuration, which will be used across
    /// all the games.
    /// 
    /// Use freetrain.framework.Core.options to access the instance.
    /// </summary>
    [XmlTypeAttribute(Namespace = "http://www.kohsuke.org/freetrain/globalConfig")]
    [XmlRootAttribute(Namespace = "http://www.kohsuke.org/freetrain/globalConfig", IsNullable = false)]
    public class GlobalOptions : PersistentOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public GlobalOptions() { }

        /// <summary>
        /// If true, show a message box for errors.
        /// If false, show a message into the status bar.
        /// </summary>
        public bool showErrorMessageBox = false;

        /*public DDSurfaceAllocation surfaceAlloc = DirectDraw.SurfeceAllocation;
        public DDSurfaceAllocation SurfaceAlloc
        {
            get{ return surfaceAlloc; }
            set{ DirectDraw.SurfeceAllocation = value; 
                 surfaceAlloc = value; }
        }*/
        /// <summary>
        /// 
        /// </summary>
        public double[] devParams = new double[11];

        /// <summary>
        /// Length of the time (in seconds) 
        /// while a message is displayed.
        /// </summary>
        public int messageDisplayTime = 3;
        /// <summary>
        /// 
        /// </summary>
        public bool enableSoundEffect = true;

        /// <summary>
        /// Debug option to draw the bounding box
        /// </summary>
        public bool drawBoundingBox = false;

        private bool _drawStationNames = true;
        /// <summary>
        /// 
        /// </summary>
        public bool drawStationNames
        {
            get
            {
                return _drawStationNames;
            }
            set
            {
                if (_drawStationNames != value && World.WorldDefinition.world != null)
                    World.WorldDefinition.world.onAllVoxelUpdated();	// redraw
                _drawStationNames = value;
            }
        }

        /// <summary>
        /// If false, draw trees.
        /// If true, speed up drawing by ignore drawing trees.
        /// </summary>
        private bool _hideTrees = false;
        /// <summary>
        /// 
        /// </summary>
        public bool hideTrees
        {
            get
            {
                return _hideTrees;
            }
            set
            {
                if (_hideTrees != value && World.WorldDefinition.world != null)
                    World.WorldDefinition.world.onAllVoxelUpdated();	// redraw
                _hideTrees = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public new GlobalOptions load()
        {
            GlobalOptions opt = (GlobalOptions)base.load();
            //DirectDraw.SurfeceAllocation = opt.SurfaceAlloc; 
            return opt;
        }

        // Maintain backward-compatibility
        /// <summary>
        /// 
        /// </summary>
        protected override string Stem { get { return ""; } }
    }
}
