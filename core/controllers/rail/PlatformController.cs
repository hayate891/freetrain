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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FreeTrain.Contributions.Common;
using FreeTrain.Contributions.Rail;
using FreeTrain.Framework;
using FreeTrain.Framework.Graphics;
using FreeTrain.Framework.Plugin;
using FreeTrain.Util;
using FreeTrain.World;
using FreeTrain.World.Rail;
using FreeTrain.Views;
using FreeTrain.Views.Map;
using FreeTrain.Controls;

namespace FreeTrain.Controllers.Rail
{
    /// <summary>
    /// 
    /// </summary>
    public partial class PlatformController : AbstractControllerImpl, IMapOverlay, ILocationDisambiguator
    {
        private FreeTrain.Controls.IndexSelector indexSelector;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private FreeTrain.Controls.IndexSelector indexSelector1;
        private System.Windows.Forms.Label label4;
        private FreeTrain.Controls.IndexSelector indexSelector2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private Bitmap bitmapN, bitmapS, bitmapE, bitmapW;
        private Bitmap stationPreviewBitmap;
        /// <summary>
        /// 
        /// </summary>
        public PlatformController()
        {
            // この呼び出しは Windows フォーム デザイナで必要です。
            InitializeComponent();
            //			colorPickButton1.colorLibraries = new IColorLibrary[]{
            //				(IColorLibrary)PluginManager.theInstance.getContribution("{COLORLIB-RAINBOW}"),
            //				(IColorLibrary)PluginManager.theInstance.getContribution("{COLORLIB-STONES}")//,
            ////				(IColorLibrary)PluginManager.theInstance.getContribution("{COLORLIB-WOODS}"),
            ////				(IColorLibrary)PluginManager.theInstance.getContribution("{COLORLIB-METALS}"),
            ////				(IColorLibrary)PluginManager.theInstance.getContribution("{COLORLIB-BRICKS}"),
            ////				(IColorLibrary)PluginManager.theInstance.getContribution("{COLORLIB-DIRTS}"),
            ////				(IColorLibrary)PluginManager.theInstance.getContribution("{COLORLIB-PASTEL}"),
            ////				(IColorLibrary)PluginManager.theInstance.getContribution("{COLORLIB-COLPLATE}"),
            ////				(IColorLibrary)PluginManager.theInstance.getContribution("{COLORLIB-ROOF}")
            //			};

            //do translation, if you put this in InitialiseComponent then it will get overridden by the
            //visual studio when you edit the form in designer view...

            //this.lblTitle.Text = Translation.GetString("CONTROLLER_STATION_TITLE");
            this.stationPage.Text = Translation.GetString("CONTROLLER_STATION_PAGE");
            this.label3.Text = Translation.GetString("CONTROLLER_STATION_DESIGN");
            this.columnHeader1.Text = Translation.GetString("CONTROLLER_STATION_NAME");
            this.columnHeader2.Text = Translation.GetString("CONTROLLER_STATION_SCALE");
            this.columnHeader3.Text = Translation.GetString("CONTROLLER_STATION_MATERIAL");
            this.columnHeader4.Text = Translation.GetString("CONTROLLER_STATION_SIZE");
            this.columnHeader5.Text = Translation.GetString("CONTROLLER_STATION_COST");
            this.columnHeader6.Text = Translation.GetString("CONTROLLER_STATION_MAINTENANCE");
            this.label4.Text = Translation.GetString("CONTROLLER_STATION_DIRECTION");
            this.label5.Text = Translation.GetString("CONTROLLER_STATION_COLOUR");
            this.platformPage.Text = Translation.GetString("CONTROLLER_STATION_PLATFORM");
            this.checkSlim.Text = Translation.GetString("CONTROLLER_STATION_SLIMPLATFORM");
            this.label1.Text = Translation.GetString("CONTROLLER_STATION_LENGTH");
            this.Text = Translation.GetString("CONTROLLER_STATION_CONSTRUCTION");
            this.buttonRemove.Text = Translation.GetString("CONTROLLER_REMOVE_BUTTON");
            this.buttonPlace.Text = Translation.GetString("CONTROLLER_BUILD_BUTTON");

            dirN.Tag = Direction.NORTH;
            dirE.Tag = Direction.EAST;
            dirS.Tag = Direction.SOUTH;
            dirW.Tag = Direction.WEST;

            // load pictures
            bitmapN = ResourceUtil.LoadSystemBitmap("PlatformN.bmp");
            dirN.Image = bitmapN;

            bitmapE = ResourceUtil.LoadSystemBitmap("PlatformN.bmp");
            bitmapE.RotateFlip(RotateFlipType.Rotate90FlipNone);
            dirE.Image = bitmapE;

            bitmapS = ResourceUtil.LoadSystemBitmap("PlatformN.bmp");
            bitmapS.RotateFlip(RotateFlipType.Rotate180FlipNone);
            dirS.Image = bitmapS;

            bitmapW = ResourceUtil.LoadSystemBitmap("PlatformN.bmp");
            bitmapW.RotateFlip(RotateFlipType.Rotate270FlipNone);
            dirW.Image = bitmapW;

            // load station type list
            stationType.DataSource = Core.Plugins.stationGroup;
            stationType.DisplayMember = "name";

            updateAfterResize(null, null);
            onDirChange(dirN, null);
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

            bitmapN.Dispose();
            bitmapS.Dispose();
            bitmapE.Dispose();
            bitmapW.Dispose();
            stationPreviewBitmap.Dispose();
            if (alphaSprites != null)
                alphaSprites.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        public override ILocationDisambiguator Disambiguator { get { return this; } }

        /// <summary> LocationDisambiguator implementation </summary>
        public bool IsSelectable(Location loc)
        {
            if (currentMode == Mode.Station)
            {
                return GroundDisambiguator.theInstance.IsSelectable(loc);
            }

            if (isPlacing)
            {
                // align to RRs or the ground

                if (currentMode == Mode.FatPlatform)
                    loc += direction.right90;

                if (GroundDisambiguator.theInstance.IsSelectable(loc))
                    return true;

                RailRoad rr = RailRoad.get(loc);
                if (rr == null) return false;
                return rr.hasRail(direction) && rr.hasRail(direction.opposite);
            }
            else
            {
                return Platform.get(loc) != null;
            }
        }

        /// <summary> The direction of the platform </summary>
        private Direction direction;

        private RailPattern railPattern { get { return RailPattern.get(direction, direction.opposite); } }

        /// <summary>
        /// Called when the direction of a platform is changed.
        /// </summary>
        private void onDirChange(object sender, System.EventArgs e)
        {
            updatePlatformBox(sender, dirN);
            updatePlatformBox(sender, dirS);
            updatePlatformBox(sender, dirE);
            updatePlatformBox(sender, dirW);
            updateAlphaSprites();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void updateAfterResize(object sender, System.EventArgs e)
        {
            this.buttonPlace.Width = ((this.tabControl.Left + this.tabControl.Width) - (buttonPlace.Left * 2)) / 2;
            this.buttonRemove.Left = (this.buttonPlace.Left + this.buttonPlace.Width);
            this.buttonRemove.Width = this.buttonPlace.Width;
            this.dirN.Width = (this.tabControl.Width - 30) / 2;
            this.dirN.Height = (this.checkSlim.Top - 20) / 2;
            this.dirE.Size = this.dirN.Size;
            this.dirS.Size = this.dirN.Size;
            this.dirW.Size = this.dirN.Size;
            this.dirE.Left = this.dirN.Left + this.dirN.Width + 5;
            this.dirS.Left = this.dirE.Left;
            this.dirS.Top = this.dirN.Top + this.dirN.Height + 5;
            this.dirW.Top = this.dirS.Top;
            UpdatePreview();
        }

        private void updatePlatformBox(object sender, PictureBox pic)
        {
            if (pic == sender)
            {
                direction = (Direction)pic.Tag;
                pic.BorderStyle = BorderStyle.Fixed3D;
            }
            else
            {
                pic.BorderStyle = BorderStyle.None;
            }
        }

        private bool isPlacing { get { return buttonPlace.Checked; } }

        private enum Mode
        {
            Station,
            ThinPlatform,
            FatPlatform
        }

        /// <summary>
        /// Returns true if the current page is the station page.
        /// </summary>
        private Mode currentMode
        {
            get
            {
                if (tabControl.SelectedIndex == 0) return Mode.Station;
                if (checkSlim.Checked) return Mode.ThinPlatform;
                else return Mode.FatPlatform;
            }
        }

        private Location baseLoc = World.Location.Unplaced;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        public override void OnMouseMove(MapViewWindow view, Location loc, Point ab)
        {
            WorldDefinition w = WorldDefinition.World;

            if (baseLoc != loc)
            {
                // update the screen
                updateVoxels();
                baseLoc = loc;
                updateVoxels();
            }
        }

        private void updateVoxels()
        {
            Location loc2 = baseLoc;
            if (currentMode == Mode.Station)
            {
                loc2 += selectedStation.size;
            }
            else
            {
                // platform
                loc2.x += direction.offsetX * length;
                loc2.y += direction.offsetY * length;
                loc2 += direction.right90;		// width 1 by default
                loc2.z++;
                if (currentMode == Mode.FatPlatform)
                {
                    loc2 += direction.right90;	// for the attached rail road, width is two
                }
            }
            WorldDefinition.World.OnVoxelUpdated(Cube.createExclusive(baseLoc, loc2));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        public override void OnClick(MapViewWindow view, Location loc, Point ab)
        {
            switch (currentMode)
            {
                case Mode.Station:
                    if (isPlacing)
                    {
                        if (!selectedStation.canBeBuilt(loc, ControlMode.Player))
                        {
                            MessageBox.Show(Translation.GetString("CONSTRUCTION_CANNOT_BUILD"));
                        }
                        else
                        {
                            selectedStation.create(loc, true);
                        }
                    }
                    else
                    {
                        Station s = Station.get(loc);
                        if (s != null) s.remove();
                    }
                    return;

                case Mode.FatPlatform:
                    if (isPlacing)
                    {
                        if (!FatPlatform.canBeBuilt(loc, direction, length))
                        {
                            MessageBox.Show(Translation.GetString("CONSTRUCTION_CANNOT_BUILD"));
                            return;
                        }
                        new FatPlatform(loc, direction, length);
                    }
                    else
                    {
                        Platform p = Platform.get(loc);
                        if (p != null)
                        {
                            if (p.canRemove)
                                p.remove();
                            else
                                MessageBox.Show(Translation.GetString("CONSTRUCTION_CANNOT_REMOVE"));
                        }
                    }
                    return;

                case Mode.ThinPlatform:
                    if (isPlacing)
                    {
                        if (!ThinPlatform.canBeBuilt(loc, direction, length))
                        {
                            MessageBox.Show(Translation.GetString("CONSTRUCTION_CANNOT_BUILD"));
                            return;
                        }
                        new ThinPlatform(loc, direction, length);
                    }
                    else
                    {
                        Platform p = Platform.get(loc);
                        if (p != null)
                        {
                            if (p.canRemove)
                                p.remove();
                            else
                                MessageBox.Show(Translation.GetString("CONSTRUCTION_CANNOT_REMOVE"));
                        }
                    }
                    return;
            }
        }

        private void validateLength(object sender, CancelEventArgs e)
        {
            // only allow a value longer than 1
            try
            {
                e.Cancel = lengthBox.Value < 1;
            }
            catch (Exception)
            {
                e.Cancel = true;
            }
        }

        /// <summary> Length of the platform to be built. </summary>
        private int length { get { return (int)lengthBox.Value; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="dc"></param>
        /// <param name="loc"></param>
        /// <param name="pt"></param>
        public void DrawVoxel(QuarterViewDrawer view, DrawContext dc, Location loc, Point pt)
        {
            if (loc.z != baseLoc.z || !isPlacing) return;

            Surface canvas = dc.Surface;

            switch (this.currentMode)
            {
                case Mode.Station:
                    if (Cube.createExclusive(baseLoc, alphaSprites.size).contains(loc))
                        alphaSprites.getSprite(loc - baseLoc).drawAlpha(canvas, pt);
                    break;

                case Mode.ThinPlatform:
                    // adjustment
                    if (direction == Direction.NORTH) loc.y += length - 1;
                    if (direction == Direction.WEST) loc.x += length - 1;

                    if (Cube.createExclusive(baseLoc, alphaSprites.size).contains(loc))
                        alphaSprites.getSprite(loc - baseLoc).drawAlpha(canvas, pt);
                    break;

                case Mode.FatPlatform:
                    // left-top corner of the platform to be drawn
                    Location ptLT = baseLoc;
                    switch (direction.index)
                    {
                        case 0:	// NORTH
                            ptLT.y -= length - 1;
                            break;
                        case 2: // EAST
                            break;	// no adjustment
                        case 4: // SOUTH
                            ptLT.x -= 1;
                            break;
                        case 6: // WEST
                            ptLT.x -= length - 1;
                            ptLT.y -= 1;
                            break;
                    }


                    if (direction.isParallelToX)
                    {
                        int y = ptLT.y;
                        if ((loc.y == y || loc.y == y + 1)
                        && ptLT.x <= loc.x && loc.x < ptLT.x + length)
                            alphaSprites.sprites[loc.x - ptLT.x, loc.y - y, 0].drawAlpha(canvas, pt);
                    }
                    else
                    {
                        int x = ptLT.x;
                        if ((loc.x == x || loc.x == x + 1)
                        && ptLT.y <= loc.y && loc.y < ptLT.y + length)
                            alphaSprites.sprites[loc.x - x, loc.y - ptLT.y, 0].drawAlpha(canvas, pt);
                    }
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="surface"></param>
        public void DrawBefore(QuarterViewDrawer view, DrawContext surface) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="surface"></param>
        public void DrawAfter(QuarterViewDrawer view, DrawContext surface) { }

        private void onGroupChanged(object sender, System.EventArgs e)
        {
            indexSelector.dataSource = (StructureGroup)stationType.SelectedItem;
            onStationChanged(null, null);
        }

        /// <summary>
        /// Called when a selection of the station has changed.
        /// </summary>
        private void onStationChanged(object sender, EventArgs e)
        {
            // Builds a new preview bitmap and set it to the picture box
            PreviewDrawer drawer;
            if (selectedStation == null)
            {
                drawer = new PreviewDrawer(stationPicture.ClientSize, new Distance());
            }
            else
            {
                drawer = new PreviewDrawer(stationPicture.ClientSize, selectedStation.size);
                drawer.drawCenter(selectedStation.sprites);
            }

            if (stationPreviewBitmap != null) stationPreviewBitmap.Dispose();
            stationPicture.Image = stationPreviewBitmap = drawer.createBitmap();

            drawer.Dispose();

            updateAlphaSprites();
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdatePreview()
        {
            if (this.currentMode == Mode.Station)
                onStationChanged(null, null);
        }

        private StationContribution selectedStation { get { return (StationContribution)indexSelector.currentItem; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            onStationChanged(null, null);
            updateAlphaSprites();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnDetached()
        {
            // TODO: update voxels correctly
            WorldDefinition.World.OnAllVoxelUpdated();
        }

        private void onLengthChanged(object sender, EventArgs e)
        {
            // TODO: performance slow down when the length is very long. Check why.
            updateAlphaSprites();
        }

        private AlphaBlendSpriteSet alphaSprites;

        /// <summary>
        /// Re-builds an alpha-blending preview.
        /// </summary>
        private void updateAlphaSprites()
        {
            if (direction == null) return;	// during the initialization, this method can be called in a wrong timing.
            if (alphaSprites != null)
                alphaSprites.Dispose();


            ISprite[, ,] alphas = null;

            switch (this.currentMode)
            {
                case Mode.Station:
                    // builds a new alpha blended preview
                    alphas = selectedStation.sprites;
                    break;

                case Mode.ThinPlatform:
                    ISprite spr = ThinPlatform.getSprite(direction, false);

                    // build sprite set
                    // TODO: use the correct sprite
                    if (direction == Direction.NORTH || direction == Direction.SOUTH)
                    {
                        alphas = new ISprite[1, length, 1];
                        for (int i = 0; i < length; i++)
                            alphas[0, i, 0] = spr;
                    }
                    else
                    {
                        alphas = new ISprite[length, 1, 1];
                        for (int i = 0; i < length; i++)
                            alphas[i, 0, 0] = spr;
                    }

                    alphaSprites = new AlphaBlendSpriteSet(alphas);
                    break;

                case Mode.FatPlatform:
                    RailPattern rp = this.railPattern;


                    // build sprite set
                    if (direction == Direction.NORTH || direction == Direction.SOUTH)
                    {
                        alphas = new ISprite[2, length, 1];
                        int j = direction == Direction.SOUTH ? 1 : 0;
                        for (int i = 0; i < length; i++)
                        {
                            alphas[j, i, 0] = FatPlatform.getSprite(direction);
                            alphas[j ^ 1, i, 0] = railPattern;
                        }
                    }
                    else
                    {
                        alphas = new ISprite[length, 2, 1];
                        int j = direction == Direction.WEST ? 1 : 0;
                        for (int i = 0; i < length; i++)
                        {
                            alphas[i, j, 0] = FatPlatform.getSprite(direction);
                            alphas[i, j ^ 1, 0] = railPattern;
                        }
                    }
                    break;
            }

            alphaSprites = new AlphaBlendSpriteSet(alphas);
            WorldDefinition.World.OnAllVoxelUpdated();	// completely redraw the window
        }

        private void onModeChanged(object sender, System.EventArgs e)
        {
            updateAlphaSprites();
        }
    }
}