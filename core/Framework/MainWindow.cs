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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using FreeTrain.World;
using FreeTrain.Controllers;
using FreeTrain.Controllers.Rail;
using FreeTrain.Controllers.Terrain;
using FreeTrain.Controllers.Structs;
using FreeTrain.Controllers.Land;
using FreeTrain.Framework.Plugin;

namespace FreeTrain.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MainWindow : Form
    {
        private int childFormNumber = 0;

        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            mainWindow = this;
            InitializeComponent();
            
            RailRoadController railRoadController = new RailRoadController();
            railRoadController.MdiParent = this;
            railRoadController.WindowState = FormWindowState.Maximized;
            railRoadController.Show();
            PlatformController platformController = new PlatformController();
            platformController.MdiParent = this;
            platformController.WindowState = FormWindowState.Maximized;
            platformController.Show();
            TrainPlacementController trainPlacementController = new TrainPlacementController();
            trainPlacementController.MdiParent = this;
            trainPlacementController.WindowState = FormWindowState.Maximized;
            trainPlacementController.Show();
            MountainController mountainController = new MountainController();
            mountainController.MdiParent = this;
            mountainController.WindowState = FormWindowState.Maximized;
            mountainController.Show();
            VarHeightBuildingController varHeightBuildingController = new VarHeightBuildingController();
            varHeightBuildingController.MdiParent = this;
            varHeightBuildingController.WindowState = FormWindowState.Maximized;
            varHeightBuildingController.Show();
            LandController landController = new LandController();
            landController.MdiParent = this;
            landController.WindowState = FormWindowState.Maximized;
            landController.Show();
            LandPropertyController landPropertyController = new LandPropertyController();
            landPropertyController.MdiParent = this;
            landPropertyController.WindowState = FormWindowState.Maximized;
            landPropertyController.Show();
            StationPassagewayController stationPassagewayController = new StationPassagewayController();
            stationPassagewayController.MdiParent = this;
            stationPassagewayController.WindowState = FormWindowState.Maximized;
            stationPassagewayController.Show();
            SlopeRailRoadController slopeRailRoadController = new SlopeRailRoadController();
            slopeRailRoadController.MdiParent = this;
            slopeRailRoadController.WindowState = FormWindowState.Maximized;
            slopeRailRoadController.Show();
            
            //RoadController roadController = new RoadController();
            //roadController.MdiParent = mainWindowMDI;
            //roadController.Show();
            BulldozeController bulldozeController = new BulldozeController();
            bulldozeController.MdiParent = this;
            bulldozeController.Show();
            BGMPlaylist bgmplaylist = new BGMPlaylist();
            bgmplaylist.MdiParent = this;
            bgmplaylist.Show();
        }

        /// <summary> Reference to the single instance of the main window. </summary>
        public static MainWindow mainWindow;

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
            {
                return;	// already activated
            }
            if (controller != null)
            {
                DetachController();	// deactive the current handler first
            }

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
            if (controller == null)
            {
                return;
            }

            controller.OnDetached();
            controller = null;

            // update all the views
            // TODO: update voxels correctly
            WorldDefinition.World.OnVoxelUpdated(World.Location.Unplaced);
        }
        #endregion

        private void ShowNewForm(object sender, EventArgs e)
        {
            // Create a new instance of the child form.
            NewWorldDialog childForm = new NewWorldDialog();
            // Make it a child of this MDI form before showing it.
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
                // TODO: Add code here to open the file.
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
                // TODO: Add code here to save the current contents of the form to a file.
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Use System.Windows.Forms.Clipboard to insert the selected text or images into the clipboard
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Use System.Windows.Forms.Clipboard to insert the selected text or images into the clipboard
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Use System.Windows.Forms.Clipboard.GetText() or System.Windows.Forms.GetData to retrieve information from the clipboard.
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void MainWindowMDI_Load(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutDialog aboutDialog = new AboutDialog();
            aboutDialog.ShowDialog();
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void playButton_Click(object sender, EventArgs e)
        {
            MainWindow.mainWindow.DetachController();
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            //MainWindowMDI.mainWindow.CurrentController = MainWindowMDI.mainWindow.br 
            //MainWindowMDI.mainWindow.AttachController(MainWindowMDI.mainWindow.CurrentController);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigDialog configDialog = new ConfigDialog();
            configDialog.MdiParent = this;
            configDialog.Show();
        }

        private void plugingListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PluginListDialog pluginListDialog = new PluginListDialog();
            pluginListDialog.MdiParent = this;
            pluginListDialog.WindowState = FormWindowState.Maximized;
            pluginListDialog.Show();
        }
    }
}
