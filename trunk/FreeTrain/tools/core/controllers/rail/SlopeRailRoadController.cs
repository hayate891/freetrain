using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using freetrain.framework;
using freetrain.framework.graphics;
using freetrain.util;
using freetrain.world;
using freetrain.world.rail;
using freetrain.views;
using freetrain.views.map;
using org.kohsuke.directdraw;

namespace freetrain.controllers.rail
{
	/// <summary>
	/// Controller to place/remove slope RRs.
	/// </summary>
	public class SlopeRailRoadController : AbstractControllerImpl, LocationDisambiguator, MapOverlay
	{
		#region Singleton instance management
		/// <summary>
		/// Creates a new controller window, or active the existing one.
		/// </summary>
		public static void create() {
			if(theInstance==null)
				theInstance = new SlopeRailRoadController();
			theInstance.Show();
			theInstance.Activate();
		}

		private freetrain.controls.CostBox costBox;

		private static SlopeRailRoadController theInstance;

		protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
			base.OnClosing(e);
			theInstance = null;
		}
		#endregion


		public SlopeRailRoadController() {
			// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
			InitializeComponent();

			pictureN.Tag = Direction.get(0);
			pictureE.Tag = Direction.get(2);
			pictureS.Tag = Direction.get(4);
			pictureW.Tag = Direction.get(6);

			update( pictureN, pictureN );	// select N first

			PreviewDrawer drawer;

			// TODO: locations of the previews are uttely incorrect. fix them

			// direction N
			using(drawer = new PreviewDrawer( pictureN.ClientSize, new Size(2,4), 0 )) {
				drawer.draw( RailPattern.getSlope( Direction.NORTH, 3 ), 1, -1 );
				drawer.draw( RailPattern.getSlope( Direction.NORTH, 2 ), 1, 0 );
				drawer.draw( RailPattern.getSlope( Direction.NORTH, 1 ), 0, 2 );
				drawer.draw( RailPattern.getSlope( Direction.NORTH, 0 ), 0, 3 );
				pictureN.Image = drawer.createBitmap();
			}

			// direction S
			using(drawer = new PreviewDrawer( pictureS.ClientSize, new Size(2,4), 0 )) {
				drawer.draw( RailPattern.getSlope( Direction.SOUTH, 0 ), 0, 0 );
				drawer.draw( RailPattern.getSlope( Direction.SOUTH, 1 ), 0, 1 );
				drawer.draw( RailPattern.getSlope( Direction.SOUTH, 2 ), 1, 1 );
				drawer.draw( RailPattern.getSlope( Direction.SOUTH, 3 ), 1, 2 );
				pictureS.Image = drawer.createBitmap();
			}

			// direction E
			using(drawer = new PreviewDrawer( pictureE.ClientSize, new Size(4,2), 0 )) {
				drawer.draw( RailPattern.getSlope( Direction.EAST, 3 ),  3, 0 );
				drawer.draw( RailPattern.getSlope( Direction.EAST, 2 ),  2, 0 );
				drawer.draw( RailPattern.getSlope( Direction.EAST, 1 ),  0, 1 );
				drawer.draw( RailPattern.getSlope( Direction.EAST, 0 ), -1, 1 );
				pictureE.Image = drawer.createBitmap();
			}

			// direction W
			using(drawer = new PreviewDrawer( pictureW.ClientSize, new Size(4,2), 0 )) {
				drawer.draw( RailPattern.getSlope( Direction.WEST, 3 ), 1, 0 );
				drawer.draw( RailPattern.getSlope( Direction.WEST, 2 ), 2, 0 );
				drawer.draw( RailPattern.getSlope( Direction.WEST, 1 ), 2, 1 );
				drawer.draw( RailPattern.getSlope( Direction.WEST, 0 ), 3, 1 );
				pictureW.Image = drawer.createBitmap();
			}
		}

		protected override void Dispose( bool disposing ) {
			if( disposing && components != null)
					components.Dispose();
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		private System.Windows.Forms.PictureBox pictureN;
		private System.Windows.Forms.PictureBox pictureE;
		private System.Windows.Forms.PictureBox pictureS;
		private System.Windows.Forms.PictureBox pictureW;
		private System.Windows.Forms.RadioButton buttonPlace;
		private System.Windows.Forms.RadioButton buttonRemove;
		/// <summary>
		/// �K�v�ȃf�U�C�i�ϐ��ł��B
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// �f�U�C�i �T�|�[�g�ɕK�v�ȃ��\�b�h�ł��B���̃��\�b�h�̓��e��
		/// �R�[�h �G�f�B�^�ŕύX���Ȃ��ł��������B
		/// </summary>
		private void InitializeComponent()
		{
			this.pictureN = new System.Windows.Forms.PictureBox();
			this.pictureE = new System.Windows.Forms.PictureBox();
			this.pictureS = new System.Windows.Forms.PictureBox();
			this.pictureW = new System.Windows.Forms.PictureBox();
			this.buttonPlace = new System.Windows.Forms.RadioButton();
			this.buttonRemove = new System.Windows.Forms.RadioButton();
			this.costBox = new freetrain.controls.CostBox();
			this.SuspendLayout();
			// 
			// pictureN
			// 
			this.pictureN.Location = new System.Drawing.Point(8, 8);
			this.pictureN.Name = "pictureN";
			this.pictureN.Size = new System.Drawing.Size(96, 48);
			this.pictureN.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureN.TabIndex = 0;
			this.pictureN.TabStop = false;
			this.pictureN.Click += new System.EventHandler(this.picture_Click);
			// 
			// pictureE
			// 
			this.pictureE.Location = new System.Drawing.Point(8, 64);
			this.pictureE.Name = "pictureE";
			this.pictureE.Size = new System.Drawing.Size(96, 48);
			this.pictureE.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureE.TabIndex = 1;
			this.pictureE.TabStop = false;
			this.pictureE.Click += new System.EventHandler(this.picture_Click);
			// 
			// pictureS
			// 
			this.pictureS.Location = new System.Drawing.Point(8, 120);
			this.pictureS.Name = "pictureS";
			this.pictureS.Size = new System.Drawing.Size(96, 48);
			this.pictureS.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureS.TabIndex = 2;
			this.pictureS.TabStop = false;
			this.pictureS.Click += new System.EventHandler(this.picture_Click);
			// 
			// pictureW
			// 
			this.pictureW.Location = new System.Drawing.Point(8, 176);
			this.pictureW.Name = "pictureW";
			this.pictureW.Size = new System.Drawing.Size(96, 48);
			this.pictureW.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureW.TabIndex = 3;
			this.pictureW.TabStop = false;
			this.pictureW.Click += new System.EventHandler(this.picture_Click);
			// 
			// buttonPlace
			// 
			this.buttonPlace.Appearance = System.Windows.Forms.Appearance.Button;
			this.buttonPlace.Checked = true;
			this.buttonPlace.Location = new System.Drawing.Point(8, 256);
			this.buttonPlace.Name = "buttonPlace";
			this.buttonPlace.Size = new System.Drawing.Size(48, 24);
			this.buttonPlace.TabIndex = 4;
			this.buttonPlace.TabStop = true;
			this.buttonPlace.Text = "�~��";
			this.buttonPlace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// buttonRemove
			// 
			this.buttonRemove.Appearance = System.Windows.Forms.Appearance.Button;
			this.buttonRemove.Location = new System.Drawing.Point(56, 256);
			this.buttonRemove.Name = "buttonRemove";
			this.buttonRemove.Size = new System.Drawing.Size(48, 24);
			this.buttonRemove.TabIndex = 5;
			this.buttonRemove.Text = "�P��";
			this.buttonRemove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// costBox
			// 
			this.costBox.cost = 0;
			this.costBox.label = "��p�F";
			this.costBox.Location = new System.Drawing.Point(8, 224);
			this.costBox.Name = "costBox";
			this.costBox.Size = new System.Drawing.Size(96, 32);
			this.costBox.TabIndex = 6;
			// 
			// SlopeRailRoadController
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(112, 284);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.costBox,
																		  this.buttonRemove,
																		  this.buttonPlace,
																		  this.pictureW,
																		  this.pictureS,
																		  this.pictureE,
																		  this.pictureN});
			this.Name = "SlopeRailRoadController";
			this.Text = "���z���H";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary> Selected direction. </summary>
		private Direction direction;

		/// <summary>
		/// construction mode. Are we placing a new one, or removing an existing one?
		/// </summary>
		private bool isPlacing { get { return buttonPlace.Checked; } }

		private void picture_Click(object sender, System.EventArgs e) {
			update( pictureN, sender );
			update( pictureS, sender );
			update( pictureW, sender );
			update( pictureE, sender );
		}

		private void update( PictureBox picBox, object sender ) {
			if(picBox==sender) {
				picBox.BorderStyle = BorderStyle.Fixed3D;
				direction = (Direction)picBox.Tag;
			} else {
				picBox.BorderStyle = BorderStyle.None;
			}
		}



		public override void onClick( MapViewWindow source, Location loc, Point ab ) {
			if(isPlacing) {
				// placing
				if(SlopeRailRoad.canCreateSlope(loc,direction))
					SlopeRailRoad.createSlope(loc,direction);
				else
					MainWindow.showError("�ݒu�ł��܂���");
			} else {
				SlopeRailRoad srr = SlopeRailRoad.get(loc);
				if(srr==null) {
					loc.z++;
					srr = SlopeRailRoad.get(loc);
				}
				if(srr!=null) {
					if(srr.level>=2)	loc.z--;
					for(int i=0; i<srr.level; i++)
						loc -= direction;

					// removing
					if(SlopeRailRoad.canRemoveSlope(loc,direction)) {
						SlopeRailRoad.removeSlope(loc,direction);
						return;
					}
				}
				MainWindow.showError("�P���ł��܂���");
			}
		}

		public override LocationDisambiguator disambiguator { get { return this; } }

		
		/// <summary>
		/// LocationDisambiguator implementation.
		/// Use the base of the slope to disambiguate.
		/// </summary>
		/// <param name="loc"></param>
		/// <returns></returns>
		public bool isSelectable( Location loc ) {
			if(!isPlacing) {
				SlopeRailRoad rr = SlopeRailRoad.get(loc);
				if(rr!=null && rr.level<2)
						return true;
				
				loc.z++;
				rr = SlopeRailRoad.get(loc);
				if(rr!=null && rr.level>=2)
						return true;

				return false;
			} else {
				// it is always allowed to place it on or under ground 
				if( World.world.getGroundLevel(loc)>=loc.z )
					return true;

				// if the new rail road is at the edge of existing rail,
				// allow.
				RailRoad rr = RailRoad.get(loc+direction.opposite);
				if( rr!=null && rr.hasRail(direction))
					return true;

				for( int i=0; i<4; i++ )
					loc += direction;
				loc.z++;

				// run the same test to the other end
				rr = RailRoad.get(loc);
				if( rr!=null && rr.hasRail(direction.opposite))
					return true;

				return false;
			}
		}


		private Location lastMouse;
		public override void onMouseMove( MapViewWindow source, Location loc, Point ab ) {
			if(lastMouse!=loc) {
				// update the image
				invalidateScreen();
				lastMouse = loc;
				invalidateScreen();

				if( isPlacing )
					costBox.cost = SlopeRailRoad.calcCostOfNewSlope(loc,direction);
				else
					costBox.cost = SlopeRailRoad.calcCostOfTearDownSlope(loc,direction);
			}
		}

		private void invalidateScreen() {
			Location loc2 = lastMouse;
			loc2.x += direction.offsetX*3;
			loc2.y += direction.offsetY*3;
			loc2.z ++;

			World.world.onVoxelUpdated( Cube.createInclusive(lastMouse,loc2) );
		}


		public void drawBefore( QuarterViewDrawer view, DrawContextEx surface ) {}
		public void drawVoxel( QuarterViewDrawer view, DrawContextEx canvas, Location loc, Point pt ) {
			// TODO: draw using this method.
		}
		public void drawAfter( QuarterViewDrawer view, DrawContextEx dc ) {
			if(!isPlacing)		return;
			Location loc = lastMouse;
			if(loc==world.Location.UNPLACED) return;
			if(!SlopeRailRoad.canCreateSlope(loc,direction))	return;

			Surface canvas = dc.surface;

			int Z = loc.z;
			for( int i=0; i<4; i++ ) {
				if(i==2)	loc.z++;

				for( int j=World.world.getGroundLevel(loc); j<Z; j++ )
					// TODO: ground level handling
					BridgePierVoxel.defaultSprite.drawAlpha(
						canvas, view.fromXYZToClient(loc.x,loc.y,j) );

				RailPattern.getSlope(direction,i).drawAlpha(
					canvas, view.fromXYZToClient(loc) );
				loc += direction;
			}
		}

	}
}