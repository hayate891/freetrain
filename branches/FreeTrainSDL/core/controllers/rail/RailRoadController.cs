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
    public partial class RailRoadController : AbstractControllerImpl, IMapOverlay
    {
        /// <summary>
        /// 
        /// </summary>
        public RailRoadController()
        {
            InitializeComponent();

            this.message.Text = Translation.GetString("CONTROLLER_RAIL_INITIAL_MESSAGE");
            this.buttonPlace.Text = Translation.GetString("CONTROLLER_PLACE_BUTTON");
            this.buttonRemove.Text = Translation.GetString("CONTROLLER_REMOVE_BUTTON");
            this.costBox.label = Translation.GetString("CONTROLLER_COST_LABEL");
        }

        /// <summary>
        /// Updates the message in the dialog box.
        /// </summary>
        private void updateDialog()
        {
            message.Text = anchor != unplaced ?
                Translation.GetString("CONTROLLER_RAIL_END_POINT") : 
                Translation.GetString("CONTROLLER_RAIL_START_POINT");
        }

        /// <summary>
        /// The first location selected by the user.
        /// </summary>
        private Location anchor = unplaced;

        /// <summary>
        /// Current mouse position. Used only when anchor!=UNPLACED
        /// </summary>
        private Location currentPosition = unplaced;

        private static Location unplaced = FreeTrain.World.Location.Unplaced;

        private bool isPlacing { get { return buttonPlace.Checked; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        public override void OnClick(MapViewWindow source, Location loc, Point ab)
        {
            if (anchor == unplaced)
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
                        if (!SingleRailRoad.Build(anchor, loc))
                            MainWindow.showError(Translation.GetString("CONTROLLER_RAIL_OBSTACLES"));
                    }
                    else
                        // remove existing ones
                        SingleRailRoad.Remove(anchor, loc);
                }
                anchor = unplaced;
            }

            updateDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        public override void OnRightClick(MapViewWindow source, Location loc, Point ab)
        {
            if (anchor == unplaced)
                Close();	// cancel
            else
            {
                // cancel the anchor
                if (currentPosition != unplaced)
                    WorldDefinition.World.OnVoxelUpdated(Cube.createInclusive(anchor, currentPosition));
                anchor = unplaced;
                updateDialog();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        public override void OnMouseMove(MapViewWindow view, Location loc, Point ab)
        {
            if (anchor != unplaced && isPlacing && currentPosition != loc)
            {
                // update the screen
                if (currentPosition != unplaced)
                    WorldDefinition.World.OnVoxelUpdated(Cube.createInclusive(anchor, currentPosition));
                currentPosition = loc;
                WorldDefinition.World.OnVoxelUpdated(Cube.createInclusive(anchor, currentPosition));

                int cost;
                SingleRailRoad.ComputeRoute(anchor, currentPosition, out cost);
                costBox.cost = cost;
            }
            if (anchor != unplaced && !isPlacing)
            {
                costBox.cost = SingleRailRoad.CalcCostOfRemoving(anchor, loc);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnDetached()
        {
            anchor = unplaced;
        }

        /// <summary>
        /// 
        /// </summary>
        public override ILocationDisambiguator Disambiguator
        {
            get
            {
                // the 2nd selection must go to the same height as the anchor.
                if (anchor == unplaced) return RailRoadDisambiguator.theInstance;
                else return sameLevelDisambiguator;
            }
        }

        private ILocationDisambiguator sameLevelDisambiguator;

        // "place" or "remove" button was clicked. reset the anchor
        private void modeChanged(object sender, EventArgs e)
        {
            anchor = unplaced;
            updateDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="canvas"></param>
        public void DrawBefore(QuarterViewDrawer view, DrawContext canvas)
        {
            if (anchor != unplaced && isPlacing)
            {
                int cost;
                canvas.Tag = SingleRailRoad.ComputeRoute(anchor, currentPosition, out cost);
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
        public void DrawVoxel(QuarterViewDrawer view, DrawContext canvas, Location loc, Point pt)
        {
            IDictionary dic = (IDictionary)canvas.Tag;
            if (dic != null)
            {
                RailPattern rp = (RailPattern)dic[loc];
                if (rp != null)
                {
                    for (int j = WorldDefinition.World.getGroundLevel(loc); j < loc.z; j++)
                        // TODO: ground level handling
                        BridgePierVoxel.defaultSprite.drawAlpha(
                            canvas.Surface,
                            view.fromXYZToClient(loc.x, loc.y, j));

                    rp.drawAlpha(canvas.Surface, pt);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="canvas"></param>
        public void DrawAfter(QuarterViewDrawer view, DrawContext canvas)
        {
        }
    }
}
