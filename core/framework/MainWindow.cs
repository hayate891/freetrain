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
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Data;

using FreeTrain.Contributions.Common;
using FreeTrain.Contributions.Rail;
using FreeTrain.Contributions.Road;
using FreeTrain.Contributions.Structs;
using FreeTrain.Contributions.Others;
using FreeTrain.Controls;
using FreeTrain.Controllers;
using FreeTrain.Controllers.Land;
using FreeTrain.Controllers.Rail;
using FreeTrain.Controllers.Road;
using FreeTrain.Controllers.Terrain;
using FreeTrain.Controllers.Structs;
using FreeTrain.Framework.Plugin;
using FreeTrain.Framework.Sound;
using FreeTrain.Framework.Graphics;
using FreeTrain.Views;
using FreeTrain.Views.Map;
using FreeTrain.World;
using FreeTrain.World.Accounting;
using FreeTrain.World.Rail;
using FreeTrain.Util;
using FreeTrain.Util.Command;

namespace FreeTrain.Framework
{
    /// <summary>
    /// MDI Container Window
    /// </summary>
    public class MainWindow : Form
    {
        //なぜか知らないが起動直後にセーブデータを読み込むと、
        //この画像ファイルがPictureManagerに登録されていないのでエラーになる。
        //このクラスでは使わないけど、ここで参照すれば、起動時には常に登録される。
        //"RailRoads.bmp"なんかはそんなことないのに不思議。
        public MainWindow(string[] args, bool constructionMode)
        {
            mainWindow = this;	// set the instance to this field
        }

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(mainWindow);
        }

        /// <summary> Reference to the single instance of the main window. </summary>
        public static MainWindow mainWindow;

        #region error message
        /// <summary>
        /// Reports an error.
        /// Depending on the configuration, this will either pop up a message box
        /// or just send the message to the status bar
        /// </summary>
        public static void showError(string msg)
        {
            MessageBox.Show(mainWindow, msg, Application.ProductName,
                MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        /// <summary>
        /// The time when the current status message should be cleared.
        /// </summary>
        //private DateTime statusBarTime = DateTime.MaxValue;
        #endregion

        #region Controller management
        private IModalController controller;

        /// <summary>
        /// Currently activated controller, if any. Or null.
        /// </summary>
        public IModalController CurrentController { get { return controller; } }

        /// <summary>
        /// Activates a new ModalController.
        /// </summary>
        public void AttachController(IModalController newHandler)
        {
            if (controller == newHandler)
                return;	// already activated
            if (controller != null)
                DetachController();	// deactive the current handler first

            controller = newHandler;
            controller.OnAttached();

            // update all the views
            // TODO: update voxels correctly
            WorldDefinition.World.OnAllVoxelUpdated();
        }

        /// <summary>
        /// Deactivates the current ModalController, if any.
        /// </summary>
        public void DetachController()
        {
            if (controller == null) return;

            controller.OnDetached();
            controller = null;
            //statusBar_Controller.Text = null;

            // update all the views
            // TODO: update voxels correctly
            WorldDefinition.World.OnVoxelUpdated(World.Location.Unplaced);
        }
        #endregion
    }
}
