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
using FreeTrain.Framework;
using FreeTrain.Framework.graphics;
using FreeTrain.Util;
using FreeTrain.world;
using FreeTrain.world.Rail;
using FreeTrain.Views;
using FreeTrain.Views.Map;

namespace FreeTrain.Controllers.Rail
{
    /// <summary>
    /// Railroad construction dialog
    /// </summary>
    /// This controller has two states.
    /// In one state, we expect the user to select one voxel.
    /// In the other state, we expect the user to select next voxel,
    /// so that we can build railroads.
    public class RailRoadController : AbstractControllerImpl, MapOverlay
    {
        #region Singleton instance management
        /// <summary>
        /// Creates a new controller window, or active the existing one.
        /// </summary>
        public static void create()
        {
            if (theInstance == null)
                theInstance = new RailRoadController();
            theInstance.Show();
            theInstance.Activate();
        }

        private static RailRoadController theInstance;
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



        private RailRoadController()
        {
            InitializeComponent();
            updatePreview();
        }
        /// <summary>
        /// 
        /// </summary>
        public override void updatePreview()
        {
            /*using( PreviewDrawer drawer = new PreviewDrawer( picture.Size, new Size(1,10), 0 ) ) 
            {
                for( int i=0; i<10; i++ )
                    drawer.draw( RailPattern.get( Direction.NORTH, Direction.SOUTH ), 0, i );
                if(picture.Image!=null) picture.Image.Dispose();
                picture.Image = drawer.createBitmap();
            }*/
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Updates the message in the dialog box.
        /// </summary>
        private void updateDialog()
        {
            message.Text = anchor != UNPLACED ?
                "Select end point" : "Select starting point";
            //! "終点を選んでください":"始点を選んでください";
        }

        /// <summary>
        /// The first location selected by the user.
        /// </summary>
        private Location anchor = UNPLACED;

        /// <summary>
        /// Current mouse position. Used only when anchor!=UNPLACED
        /// </summary>
        private Location currentPos = UNPLACED;

        private static Location UNPLACED = FreeTrain.world.Location.UNPLACED;

        private bool isPlacing { get { return buttonPlace.Checked; } }

        #region Windows Form Designer generated code
        private System.Windows.Forms.RadioButton buttonPlace;
        private System.Windows.Forms.RadioButton buttonRemove;
        private FreeTrain.controls.CostBox costBox;
        private System.Windows.Forms.Label message;
        private System.ComponentModel.Container components = null;

        private void InitializeComponent()
        {
            this.message = new System.Windows.Forms.Label();
            this.buttonPlace = new System.Windows.Forms.RadioButton();
            this.buttonRemove = new System.Windows.Forms.RadioButton();
            this.costBox = new FreeTrain.controls.CostBox();
            this.SuspendLayout();
            // 
            // message
            // 
            this.message.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.message.Location = new System.Drawing.Point(12, 46);
            this.message.Name = "message";
            this.message.Size = new System.Drawing.Size(105, 26);
            this.message.TabIndex = 1;
            this.message.Text = "Click on two points on the map to place tracks";
            this.message.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.message.MouseDown += new MouseEventHandler(this.AbstractControllerForm_MouseDown);
            this.message.MouseMove += new MouseEventHandler(this.AbstractControllerForm_MouseMove);
            // 
            // buttonPlace
            // 
            this.buttonPlace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPlace.Appearance = System.Windows.Forms.Appearance.Button;
            this.buttonPlace.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(236)))));
            this.buttonPlace.Checked = true;
            this.buttonPlace.Location = new System.Drawing.Point(12, 115);
            this.buttonPlace.Name = "buttonPlace";
            this.buttonPlace.Size = new System.Drawing.Size(46, 26);
            this.buttonPlace.TabIndex = 2;
            this.buttonPlace.TabStop = true;
            this.buttonPlace.Text = "Place";
            this.buttonPlace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonPlace.UseVisualStyleBackColor = false;
            this.buttonPlace.CheckedChanged += new System.EventHandler(this.modeChanged);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemove.Appearance = System.Windows.Forms.Appearance.Button;
            this.buttonRemove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(236)))));
            this.buttonRemove.Location = new System.Drawing.Point(58, 115);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(59, 26);
            this.buttonRemove.TabIndex = 3;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonRemove.UseVisualStyleBackColor = false;
            this.buttonRemove.CheckedChanged += new System.EventHandler(this.modeChanged);
            // 
            // costBox
            // 
            this.costBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.costBox.cost = 0;
            this.costBox.label = "Cost:";
            this.costBox.Location = new System.Drawing.Point(12, 84);
            this.costBox.Name = "costBox";
            this.costBox.Size = new System.Drawing.Size(105, 25);
            this.costBox.TabIndex = 4;
            this.costBox.MouseDown += new MouseEventHandler(this.AbstractControllerForm_MouseDown);
            this.costBox.MouseMove += new MouseEventHandler(this.AbstractControllerForm_MouseMove);

            // 
            // RailRoadController
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(129, 153);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonPlace);
            this.Controls.Add(this.costBox);
            this.Controls.Add(this.message);
            this.Name = "RailRoadController";
            this.Text = "Track construction";
            this.lblTitle.Text = "RAILROAD";
            this.Resize += new System.EventHandler(this.updateAfterResize);
            this.Load += new System.EventHandler(this.RailRoadController_Load);
            this.Controls.SetChildIndex(this.message, 0);
            this.Controls.SetChildIndex(this.costBox, 0);
            this.Controls.SetChildIndex(this.buttonPlace, 0);
            this.Controls.SetChildIndex(this.buttonRemove, 0);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        public override void onClick(MapViewWindow source, Location loc, Point ab)
        {
            if (anchor == UNPLACED)
            {
                anchor = loc;
                sameLevelDisambiguator = new SameLevelDisambiguator(anchor.z);
            }
            else
            {
                if (anchor != loc)
                {
                    if (isPlacing)
                    {
                        // build new railroads.
                        if (!SingleRailRoad.build(anchor, loc))
                            MainWindow.showError("There are obstacles");
                        //! MainWindow.showError("障害物があります");
                    }
                    else
                        // remove existing ones
                        SingleRailRoad.remove(anchor, loc);
                }
                anchor = UNPLACED;
            }

            updateDialog();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        public override void onRightClick(MapViewWindow source, Location loc, Point ab)
        {
            if (anchor == UNPLACED)
                Close();	// cancel
            else
            {
                // cancel the anchor
                if (currentPos != UNPLACED)
                    World.world.onVoxelUpdated(Cube.createInclusive(anchor, currentPos));
                anchor = UNPLACED;
                updateDialog();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void updateAfterResize(object sender, System.EventArgs e)
        {
            //this.buttonPlace.Width = this.picture.Width / 2;
            //this.buttonRemove.Left = (this.buttonPlace.Left + this.buttonPlace.Width);
            //this.buttonRemove.Width = this.buttonPlace.Width;
            //updatePreview();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        public override void onMouseMove(MapViewWindow view, Location loc, Point ab)
        {
            if (anchor != UNPLACED && isPlacing && currentPos != loc)
            {
                // update the screen
                if (currentPos != UNPLACED)
                    World.world.onVoxelUpdated(Cube.createInclusive(anchor, currentPos));
                currentPos = loc;
                World.world.onVoxelUpdated(Cube.createInclusive(anchor, currentPos));

                int cost;
                SingleRailRoad.comupteRoute(anchor, currentPos, out cost);
                costBox.cost = cost;
            }
            if (anchor != UNPLACED && !isPlacing)
            {
                costBox.cost = SingleRailRoad.calcCostOfRemoving(anchor, loc);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void onDetached()
        {
            anchor = UNPLACED;
        }
        /// <summary>
        /// 
        /// </summary>
        public override LocationDisambiguator disambiguator
        {
            get
            {
                // the 2nd selection must go to the same height as the anchor.
                if (anchor == UNPLACED) return RailRoadDisambiguator.theInstance;
                else return sameLevelDisambiguator;
            }
        }

        private LocationDisambiguator sameLevelDisambiguator;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnVisibleChanged(System.EventArgs e)
        {
            updateDialog();
        }




        // "place" or "remove" button was clicked. reset the anchor
        private void modeChanged(object sender, EventArgs e)
        {
            anchor = UNPLACED;
            updateDialog();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="canvas"></param>
        public void drawBefore(QuarterViewDrawer view, DrawContextEx canvas)
        {
            if (anchor != UNPLACED && isPlacing)
            {
                int cost;
                canvas.tag = SingleRailRoad.comupteRoute(anchor, currentPos, out cost);
                if (canvas.tag != null)
                    Debug.WriteLine(((IDictionary)canvas.tag).Count);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="canvas"></param>
        /// <param name="loc"></param>
        /// <param name="pt"></param>
        public void drawVoxel(QuarterViewDrawer view, DrawContextEx canvas, Location loc, Point pt)
        {
            IDictionary dic = (IDictionary)canvas.tag;
            if (dic != null)
            {
                RailPattern rp = (RailPattern)dic[loc];
                if (rp != null)
                {
                    for (int j = World.world.getGroundLevel(loc); j < loc.z; j++)
                        // TODO: ground level handling
                        BridgePierVoxel.defaultSprite.drawAlpha(
                            canvas.surface,
                            view.fromXYZToClient(loc.x, loc.y, j));

                    rp.drawAlpha(canvas.surface, pt);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="canvas"></param>
        public void drawAfter(QuarterViewDrawer view, DrawContextEx canvas)
        {
        }

        private void RailRoadController_Load(object sender, EventArgs e)
        {
            this.lblTitle.Text = "Railroad";
        }
    }

}
