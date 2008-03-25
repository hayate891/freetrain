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
using System.Windows.Forms;
using FreeTrain.Views;
using FreeTrain.Views.Map;
using FreeTrain.World;
using FreeTrain.World.Rail;
using FreeTrain.Framework;
using FreeTrain.Framework.Sound;
using FreeTrain.Util;
using FreeTrain.Util.Controls;
using FreeTrain.Util.Command;
using FreeTrain.Framework.Graphics;

namespace FreeTrain.Controllers.Rail
{
    /// <summary>
    /// Controller that allows the user to
    /// place/remove trains.
    /// </summary>
    public partial class TrainPlacementController : AbstractControllerImpl, MapOverlay, LocationDisambiguator
    {
        #region singleton instance management
        /// <summary>
        /// Creates a new controller window, or active the existing one.
        /// </summary>
        public static void create()
        {
            if (theInstance == null)
                theInstance = new TrainPlacementController();
            theInstance.Show();
            theInstance.Activate();
        }

        private System.Windows.Forms.ComboBox controllerCombo;
        private System.Windows.Forms.MenuItem miSell;
        private Button cmdTrading;

        private static TrainPlacementController theInstance;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            theInstance = null;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public TrainPlacementController()
        {
            // Windows フォーム デザイナ サポートに必要です。
            InitializeComponent();

            this.lblTitle.Text = Translation.GetString("CONTROLLER_TRAINPLACE_TITLE");
            this.buttonRemove.Text = Translation.GetString("CONTROLLER_REMOVE_BUTTON");
            this.buttonPlace.Text = Translation.GetString("CONTROLLER_BUILD_BUTTON");
            this.miAddGroup.Text = Translation.GetString("CONTROLLER_TRAIN_ADDNEWGROUP");
            this.miSell.Text = Translation.GetString("CONTROLLER_TRAIN_SELL");
            this.cmdTrading.Text = Translation.GetString("CONTROLLER_TRAIN_TRADING");
            this.label3.Text = Translation.GetString("CONTROLLER_TRAIN_TYPE");
            this.label2.Text = Translation.GetString("CONTROLLER_TRAIN_DIAGRAM");
            this.label1.Text = Translation.GetString("CONTROLLER_TRAIN_NAME");
            this.Text = Translation.GetString("CONTROLLER_TRAIN_PLACE");

            controllerCombo.DataSource = WorldDefinition.World.trainControllers;
            tree.ItemMoved = new ItemMovedHandler(onItemDropped);

            //this.FormBorderStyle = FormBorderStyle.SizableToolWindow;

            reset();

            // register command manager
            new Command(commands)
                .addUpdateHandler(new CommandHandler(updateAddGroup))
                .addExecuteHandler(new CommandHandlerNoArg(executeAddGroup))
                .commandInstances.AddAll(miAddGroup);
            new Command(commands)
                .addUpdateHandler(new CommandHandler(updateSell))
                .addExecuteHandler(new CommandHandlerNoArg(executeSell))
                .commandInstances.AddAll(miSell);
        }

        private readonly CommandManager commands = new CommandManager();

        /// <summary>
        /// Location of the arrow.
        /// </summary>
        private readonly LocationStore arrowLoc = new LocationStore();

        /// <summary>
        /// Resets the contents of the list.
        /// </summary>
        private void reset()
        {
            tree.BeginUpdate();
            tree.Nodes.Clear();

            TreeNode root = createNode(WorldDefinition.World.rootTrainGroup);
            tree.Nodes.Add(root);
            populate(WorldDefinition.World.rootTrainGroup, root.Nodes);

            tree.ExpandAll();
            tree.EndUpdate();
        }

        private TreeNode createNode(TrainItem item)
        {
            DDTreeNode n = new DDTreeNode(item.name);
            n.Tag = item;
            n.canAcceptDrop = (item is TrainGroup);
            n.ImageIndex = n.SelectedImageIndex = (item is Train) ? 2 : 0;
            return n;
        }

        // populate a tree control with trains
        private void populate(TrainGroup group, TreeNodeCollection col)
        {
            foreach (TrainItem ti in group.items)
            {
                TreeNode node = createNode(ti);
                col.Add(node);
                if (ti is TrainGroup)
                    populate((TrainGroup)ti, node.Nodes);
            }
        }

        /// <summary>
        /// 使用されているリソースに後処理を実行します。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private bool isPlacingMode { get { return buttonPlace.Checked; } }

        private TrainItem selectedItem
        {
            get
            {
                TreeNode n = tree.SelectedNode;
                if (n == null) return null;
                return (TrainItem)n.Tag;
            }
        }
        /// <summary>
        /// Gets the currently selected train, if any
        /// </summary>
        private Train selectedTrain
        {
            get
            {
                TrainItem ti = selectedItem;
                if (ti is Train) return (Train)ti;
                else return null;
            }
        }

        /// <summary>
        /// Gets the currently selected group, if any.
        /// </summary>
        private TrainGroup selectedGroup
        {
            get
            {
                TrainItem ti = selectedItem;
                if (ti is TrainGroup) return (TrainGroup)ti;
                else return null;
            }
        }

        /// <summary>
        /// Re-computes the arrow location correctly
        /// </summary>
        private void resetArrowLocation()
        {
            Train tr = this.selectedTrain;
            if (tr == null || !tr.head.state.isInside)
            {
                arrowLoc.location = World.Location.UNPLACED;
            }
            else
            {
                arrowLoc.location = tr.head.state.asInside().location;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        public override void OnClick(MapViewWindow source, Location loc, Point ab)
        {
            if (isPlacingMode)
            {
                if (this.selectedTrain == null) return;

                // place
                Train tr = this.selectedTrain;
                if (tr.isPlaced)
                {
                    // see if the user has clicked the same train
                    Car c = Car.get(loc);
                    if (c is Train.TrainCar && ((Train.TrainCar)c).parent == tr)
                    {
                        // clicking the same train will be considered to reverse its direction
                        // and change the position of arrow
                        tr.reverse();
                        resetArrowLocation();
                        return;
                    }
                    else
                    {
                        MainWindow.showError(Translation.GetString("CONTROLLER_TRAIN_ALREADYPLACED"));
                        return;
                    }
                }

                RailRoad rr = RailRoad.get(loc);
                if (rr == null)
                {
                    MainWindow.showError(Translation.GetString("CONTROLLER_TRAIN_CANNOTPLACE_NOTRACKS"));
                    return;
                }

                if (!tr.place(loc))
                {
                    MainWindow.showError(Translation.GetString("CONSTRUCTION_CANNOT_PLACE"));
                }
                else
                    playSound();

            }
            else
            {
                // remove
                RailRoad rr = RailRoad.get(loc);
                if (rr == null)
                {
                    MainWindow.showError(Translation.GetString("CONTROLLER_TRAIN_NORAIL"));
                    return;
                }
                if (!(rr.Voxel.car is Train.TrainCar))
                {
                    MainWindow.showError(Translation.GetString("CONTROLLER_TRAIN_NOCARS"));
                    return;
                }
                ((Train.TrainCar)rr.Voxel.car).parent.remove();
                playSound();
                // successfully removed
            }
        }

        private void playSound()
        {
            SoundEffectManager.PlayAsynchronousSound(
                ResourceUtil.findSystemResource("vehiclePurchase.wav"));
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnAttached()
        {
            reset();
        }

        // place or remove button is clicked.
        private void onModeChange(object sender, EventArgs e)
        {
        }

        // when a selected train is changed
        private void onTrainChange(object sender, TreeViewEventArgs e)
        {
            // update controls
            if (selectedItem != null)
            {
                nameBox.Enabled = true;
                nameBox.Text = selectedItem.name;
                controllerCombo.Enabled = true;
                if (selectedItem.controller != null)
                {
                    controllerCombo.SelectedItem = selectedItem.controller;
                }
            }
            else
            {
                nameBox.Enabled = false;
                controllerCombo.Enabled = false;
                controllerCombo.SelectedIndex = -1;	// unset selection
            }

            if (selectedTrain != null)
            {
                typeBox.Enabled = true;
                typeBox.Text = selectedTrain.type.name;
            }
            else
            {
                typeBox.Enabled = false;
                typeBox.Text = "";
            }

            resetArrowLocation();
        }

        /// <summary>
        /// 
        /// </summary>
        public override LocationDisambiguator Disambiguator { get { return this; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public bool isSelectable(Location loc)
        {
            RailRoad rr = RailRoad.get(loc);
            if (rr == null) return false;

            if (isPlacingMode)
            {
                if (rr.Voxel.car == null) return true;

                Train.TrainCar car = rr.Voxel.car as Train.TrainCar;
                if (car != null && car.parent == selectedTrain)
                    // allow selecting the same train to reverse the direction
                    return true;

                return false;
            }
            else
            {
                return rr.Voxel.car != null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="surface"></param>
        public void DrawBefore(QuarterViewDrawer view, DrawContextEx surface) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="canvas"></param>
        /// <param name="loc"></param>
        /// <param name="pt"></param>
        public void DrawVoxel(QuarterViewDrawer view, DrawContextEx canvas, Location loc, Point pt) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="dc"></param>
        public void DrawAfter(QuarterViewDrawer view, DrawContextEx dc)
        {
            Train tr = this.selectedTrain;
            if (tr == null || !tr.head.state.isInside) return;

            // draw an arrow that indicates the direction of the train
            CarState.Inside ci = tr.head.state.asInside();

            Point pt = view.fromXYZToClient(ci.location);
            pt.Y -= 12;

            ci.direction.drawArrow(dc.surface, pt);
        }

        private void onDoubleClick(object sender, EventArgs e)
        {

        }

        private void onNameChanged(object sender, EventArgs e)
        {
            string name = nameBox.Text;
            selectedItem.name = name;
            tree.SelectedNode.Text = name;
        }

        private TreeNode rightSelectedItem;

        // this method is called before the context menu pops up.
        private void treeMenu_Popup(object sender, System.EventArgs e)
        {
            rightSelectedItem = tree.GetNodeAt(tree.PointToClient(Cursor.Position));
            commands.updateAll();
        }

        private void onNodeExpanded(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            e.Node.SelectedImageIndex = e.Node.ImageIndex = 1;
        }

        private void onNodeCollapsed(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            e.Node.SelectedImageIndex = e.Node.ImageIndex = 0;
        }

        private void onItemDropped(TreeNode node, TreeNode newParent)
        {
            // move this train under a new group.
            node.Remove();
            newParent.Nodes.Add(node);

            TrainItem item = (TrainItem)node.Tag;
            TrainGroup newGroup = (TrainGroup)newParent.Tag;

            item.moveUnder(newGroup);
        }

        private void controllerCombo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            TrainItem item = selectedItem;
            if (item != null)
                item.controller = (TrainController)controllerCombo.SelectedItem;
        }

        private void updateAddGroup(Command cmd)
        {
            cmd.Enabled = (rightSelectedItem != null && rightSelectedItem.Tag is TrainGroup);
        }

        private void executeAddGroup()
        {
            TrainGroup newGroup = new TrainGroup((TrainGroup)rightSelectedItem.Tag);
            TreeNode node = createNode(newGroup);
            rightSelectedItem.Nodes.Add(node);
            tree.SelectedNode = node;
        }

        private void updateSell(Command cmd)
        {
            bool b = false;
            if (rightSelectedItem != null)
            {
                Train tr = rightSelectedItem.Tag as Train;
                if (tr != null && !tr.isPlaced)
                    b = true;
            }

            cmd.Enabled = b;
        }

        private void executeSell()
        {
            if (MessageBox.Show(this, Translation.GetString("CONTROLLER_TRAIN_CONFIRMSELL"), Application.ProductName,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {

                Train tr = (Train)rightSelectedItem.Tag;
                tr.sell();

                rightSelectedItem.Remove();
            }
        }

        private void cmdTrading_Click(object sender, EventArgs e)
        {
            TrainTradingDialog tr = new TrainTradingDialog();
            tr.ShowDialog();
        }
    }
}
