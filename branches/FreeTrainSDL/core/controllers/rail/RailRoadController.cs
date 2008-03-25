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
using FreeTrain.Framework.Graphics;
using FreeTrain.Util;
using FreeTrain.World;
using FreeTrain.World.Rail;
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
    public partial class RailRoadController : AbstractControllerImpl, MapOverlay
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

            this.message.Text = Translation.GetString("CONTROLLER_RAIL_INITIAL_MESSAGE");
            this.buttonPlace.Text = Translation.GetString("CONTROLLER_PLACE_BUTTON");
            this.buttonRemove.Text = Translation.GetString("CONTROLLER_REMOVE_BUTTON");
            this.costBox.label = Translation.GetString("CONTROLLER_COST_LABEL");
            this.Text = Translation.GetString("CONTROLLER_RAIL_TOOLTIP");
            this.lblTitle.Text = Translation.GetString("CONTROLLER_RAIL_TITLE");

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
                Translation.GetString("CONTROLLER_RAIL_END_POINT") : 
                Translation.GetString("CONTROLLER_RAIL_START_POINT");
        }

        /// <summary>
        /// The first location selected by the user.
        /// </summary>
        private Location anchor = UNPLACED;

        /// <summary>
        /// Current mouse position. Used only when anchor!=UNPLACED
        /// </summary>
        private Location currentPos = UNPLACED;

        private static Location UNPLACED = FreeTrain.World.Location.UNPLACED;

        private bool isPlacing { get { return buttonPlace.Checked; } }

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
                            MainWindow.showError(Translation.GetString("CONTROLLER_RAIL_OBSTACLES"));
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
                    WorldDefinition.world.onVoxelUpdated(Cube.createInclusive(anchor, currentPos));
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
                    WorldDefinition.world.onVoxelUpdated(Cube.createInclusive(anchor, currentPos));
                currentPos = loc;
                WorldDefinition.world.onVoxelUpdated(Cube.createInclusive(anchor, currentPos));

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
        public override LocationDisambiguator Disambiguator
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
        public void DrawBefore(QuarterViewDrawer view, DrawContextEx canvas)
        {
            if (anchor != UNPLACED && isPlacing)
            {
                int cost;
                canvas.Tag = SingleRailRoad.comupteRoute(anchor, currentPos, out cost);
                if (canvas.Tag != null)
                    Debug.WriteLine(((IDictionary)canvas.Tag).Count);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="canvas"></param>
        /// <param name="loc"></param>
        /// <param name="pt"></param>
        public void DrawVoxel(QuarterViewDrawer view, DrawContextEx canvas, Location loc, Point pt)
        {
            IDictionary dic = (IDictionary)canvas.Tag;
            if (dic != null)
            {
                RailPattern rp = (RailPattern)dic[loc];
                if (rp != null)
                {
                    for (int j = WorldDefinition.world.getGroundLevel(loc); j < loc.z; j++)
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
        public void DrawAfter(QuarterViewDrawer view, DrawContextEx canvas)
        {
        }

        private void RailRoadController_Load(object sender, EventArgs e)
        {
            this.lblTitle.Text = Translation.GetString("CONTROLLER_RAIL_TITLE");
        }
    }
}
