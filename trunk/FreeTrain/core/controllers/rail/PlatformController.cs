using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using freetrain.contributions.common;
using freetrain.contributions.rail;
using freetrain.framework;
using freetrain.framework.graphics;
using freetrain.framework.plugin;
using freetrain.util;
using freetrain.world;
using freetrain.world.rail;
using freetrain.views;
using freetrain.views.map;
using org.kohsuke.directdraw;


namespace freetrain.controllers.rail
{
	public class PlatformController : AbstractControllerImpl, MapOverlay, LocationDisambiguator
	{
		#region Singleton instance management
		/// <summary>
		/// Creates a new controller window, or active the existing one.
		/// </summary>
		public static void create() {
			if(theInstance==null)
				theInstance = new PlatformController();
			theInstance.Show();
			theInstance.Activate();
		}

		private freetrain.controls.IndexSelector indexSelector;

		private static PlatformController theInstance;

		protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
			base.OnClosing(e);
			theInstance = null;
		}
		#endregion


		private Bitmap bitmapN,bitmapS,bitmapE,bitmapW;
		private Bitmap stationPreviewBitmap;

		public PlatformController() {
			// この呼び出しは Windows フォーム デザイナで必要です。
			InitializeComponent();

			dirN.Tag = Direction.NORTH;
			dirE.Tag = Direction.EAST;
			dirS.Tag = Direction.SOUTH;
			dirW.Tag = Direction.WEST;

			// load pictures
			bitmapN=ResourceUtil.loadSystemBitmap("PlatformN.bmp");
			dirN.Image = bitmapN;

			bitmapE=ResourceUtil.loadSystemBitmap("PlatformN.bmp");
			bitmapE.RotateFlip( RotateFlipType.Rotate90FlipNone );
			dirE.Image = bitmapE;

			bitmapS=ResourceUtil.loadSystemBitmap("PlatformN.bmp");
			bitmapS.RotateFlip( RotateFlipType.Rotate180FlipNone );
			dirS.Image = bitmapS;

			bitmapW=ResourceUtil.loadSystemBitmap("PlatformN.bmp");
			bitmapW.RotateFlip( RotateFlipType.Rotate270FlipNone );
			dirW.Image = bitmapW;

			// load station type list
			stationType.DataSource = Core.plugins.stationGroup;
			stationType.DisplayMember="name";

			onDirChange(dirN,null);
		}

		protected override void Dispose( bool disposing ) {
			if( disposing && components != null)
					components.Dispose();
			base.Dispose( disposing );

			bitmapN.Dispose();
			bitmapS.Dispose();
			bitmapE.Dispose();
			bitmapW.Dispose();
			stationPreviewBitmap.Dispose();
			if(alphaSprites!=null)
				alphaSprites.Dispose();
		}

		public override LocationDisambiguator disambiguator { get { return this; } }

		/// <summary> LocationDisambiguator implementation </summary>
		public bool isSelectable( Location loc ) {
			if( currentMode==Mode.Station ) {
				return GroundDisambiguator.theInstance.isSelectable(loc);
			}

			if(isPlacing) {
				// align to RRs or the ground

				if( currentMode==Mode.FatPlatform )
					loc += direction.right90;

				if( GroundDisambiguator.theInstance.isSelectable(loc) )
					return true;

				RailRoad rr = RailRoad.get(loc);
				if(rr==null)	return false;
				return rr.hasRail(direction) && rr.hasRail(direction.opposite);
			} else {
				return Platform.get(loc)!=null;
			}
		}

		#region Designer generated code
		private System.Windows.Forms.TabPage stationPage;
		private System.Windows.Forms.ComboBox stationType;
		private System.Windows.Forms.PictureBox stationPicture;
		private System.Windows.Forms.TabPage platformPage;
		private System.Windows.Forms.PictureBox dirS;
		private System.Windows.Forms.PictureBox dirW;
		private System.Windows.Forms.PictureBox dirE;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown lengthBox;
		private System.Windows.Forms.PictureBox dirN;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.CheckBox checkSlim;
		private System.Windows.Forms.RadioButton buttonRemove;
		private System.Windows.Forms.RadioButton buttonPlace;
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Designer サポートに必要なメソッドです。コード エディタで
		/// このメソッドのコンテンツを変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.buttonRemove = new System.Windows.Forms.RadioButton();
			this.buttonPlace = new System.Windows.Forms.RadioButton();
			this.stationPage = new System.Windows.Forms.TabPage();
			this.stationType = new System.Windows.Forms.ComboBox();
			this.stationPicture = new System.Windows.Forms.PictureBox();
			this.platformPage = new System.Windows.Forms.TabPage();
			this.checkSlim = new System.Windows.Forms.CheckBox();
			this.dirS = new System.Windows.Forms.PictureBox();
			this.dirW = new System.Windows.Forms.PictureBox();
			this.dirE = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lengthBox = new System.Windows.Forms.NumericUpDown();
			this.dirN = new System.Windows.Forms.PictureBox();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.indexSelector = new freetrain.controls.IndexSelector();
			this.stationPage.SuspendLayout();
			this.platformPage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.lengthBox)).BeginInit();
			this.tabControl.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonRemove
			// 
			this.buttonRemove.Appearance = System.Windows.Forms.Appearance.Button;
			this.buttonRemove.Location = new System.Drawing.Point(64, 200);
			this.buttonRemove.Name = "buttonRemove";
			this.buttonRemove.Size = new System.Drawing.Size(56, 24);
			this.buttonRemove.TabIndex = 1;
			this.buttonRemove.Text = "Remove";
			//! this.buttonRemove.Text = "撤去";
			this.buttonRemove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// buttonPlace
			// 
			this.buttonPlace.Appearance = System.Windows.Forms.Appearance.Button;
			this.buttonPlace.Checked = true;
			this.buttonPlace.Location = new System.Drawing.Point(8, 200);
			this.buttonPlace.Name = "buttonPlace";
			this.buttonPlace.Size = new System.Drawing.Size(56, 24);
			this.buttonPlace.TabIndex = 0;
			this.buttonPlace.TabStop = true;
			this.buttonPlace.Text = "Build";
			//! this.buttonPlace.Text = "設置";
			this.buttonPlace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// stationPage
			// 
			this.stationPage.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.indexSelector,
																					  this.stationType,
																					  this.stationPicture});
			this.stationPage.Location = new System.Drawing.Point(4, 21);
			this.stationPage.Name = "stationPage";
			this.stationPage.Size = new System.Drawing.Size(128, 167);
			this.stationPage.TabIndex = 1;
			this.stationPage.Text = "Station";
			//! this.stationPage.Text = "駅舎";
			// 
			// stationType
			// 
			this.stationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.stationType.ItemHeight = 12;
			this.stationType.Location = new System.Drawing.Point(8, 8);
			this.stationType.Name = "stationType";
			this.stationType.Size = new System.Drawing.Size(112, 20);
			this.stationType.Sorted = true;
			this.stationType.TabIndex = 2;
			this.stationType.SelectedIndexChanged += new System.EventHandler(this.onGroupChanged);
			// 
			// stationPicture
			// 
			this.stationPicture.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.stationPicture.Location = new System.Drawing.Point(8, 64);
			this.stationPicture.Name = "stationPicture";
			this.stationPicture.Size = new System.Drawing.Size(112, 80);
			this.stationPicture.TabIndex = 1;
			this.stationPicture.TabStop = false;
			// 
			// platformPage
			// 
			this.platformPage.Controls.AddRange(new System.Windows.Forms.Control[] {
																					   this.checkSlim,
																					   this.dirS,
																					   this.dirW,
																					   this.dirE,
																					   this.label1,
																					   this.lengthBox,
																					   this.dirN});
			this.platformPage.Location = new System.Drawing.Point(4, 21);
			this.platformPage.Name = "platformPage";
			this.platformPage.Size = new System.Drawing.Size(128, 167);
			this.platformPage.TabIndex = 0;
			this.platformPage.Text = "Platform";
			//! this.platformPage.Text = "ホーム";
			// 
			// checkSlim
			// 
			this.checkSlim.Location = new System.Drawing.Point(8, 120);
			this.checkSlim.Name = "checkSlim";
			this.checkSlim.Size = new System.Drawing.Size(104, 16);
			this.checkSlim.TabIndex = 7;
			this.checkSlim.Text = "Slim platform";
			//!this.checkSlim.Text = "スリムなホーム";
			this.checkSlim.CheckedChanged += new System.EventHandler(this.onModeChanged);
			// 
			// dirS
			// 
			this.dirS.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.dirS.Location = new System.Drawing.Point(64, 64);
			this.dirS.Name = "dirS";
			this.dirS.Size = new System.Drawing.Size(48, 48);
			this.dirS.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.dirS.TabIndex = 6;
			this.dirS.TabStop = false;
			this.dirS.Click += new System.EventHandler(this.onDirChange);
			// 
			// dirW
			// 
			this.dirW.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.dirW.Location = new System.Drawing.Point(8, 64);
			this.dirW.Name = "dirW";
			this.dirW.Size = new System.Drawing.Size(48, 48);
			this.dirW.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.dirW.TabIndex = 5;
			this.dirW.TabStop = false;
			this.dirW.Click += new System.EventHandler(this.onDirChange);
			// 
			// dirE
			// 
			this.dirE.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.dirE.Location = new System.Drawing.Point(64, 8);
			this.dirE.Name = "dirE";
			this.dirE.Size = new System.Drawing.Size(48, 48);
			this.dirE.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.dirE.TabIndex = 4;
			this.dirE.TabStop = false;
			this.dirE.Click += new System.EventHandler(this.onDirChange);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 136);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 24);
			this.label1.TabIndex = 2;
			this.label1.Text = "&Length:";
			//! this.label1.Text = "長さ(&L)：";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lengthBox
			// 
			this.lengthBox.Location = new System.Drawing.Point(72, 136);
			this.lengthBox.Name = "lengthBox";
			this.lengthBox.Size = new System.Drawing.Size(40, 19);
			this.lengthBox.TabIndex = 3;
			this.lengthBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.lengthBox.Value = new System.Decimal(new int[] {
																	5,
																	0,
																	0,
																	0});
			this.lengthBox.Validating += new System.ComponentModel.CancelEventHandler(this.validateLength);
			this.lengthBox.TextChanged += new System.EventHandler(this.onLengthChanged);
			// 
			// dirN
			// 
			this.dirN.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.dirN.Location = new System.Drawing.Point(8, 8);
			this.dirN.Name = "dirN";
			this.dirN.Size = new System.Drawing.Size(48, 48);
			this.dirN.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.dirN.TabIndex = 1;
			this.dirN.TabStop = false;
			this.dirN.Click += new System.EventHandler(this.onDirChange);
			// 
			// tabControl
			// 
			this.tabControl.Controls.AddRange(new System.Windows.Forms.Control[] {
																					 this.stationPage,
																					 this.platformPage});
			this.tabControl.ItemSize = new System.Drawing.Size(42, 17);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(136, 192);
			this.tabControl.TabIndex = 0;
			this.tabControl.SelectedIndexChanged += new System.EventHandler(this.onModeChanged);
			// 
			// indexSelector
			// 
			this.indexSelector.count = 10;
			this.indexSelector.current = 0;
			this.indexSelector.dataSource = null;
			this.indexSelector.Location = new System.Drawing.Point(8, 36);
			this.indexSelector.Name = "indexSelector";
			this.indexSelector.Size = new System.Drawing.Size(112, 20);
			this.indexSelector.TabIndex = 3;
			this.indexSelector.indexChanged += new System.EventHandler(this.onStationChanged);
			// 
			// PlatformController
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(128, 232);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tabControl,
																		  this.buttonPlace,
																		  this.buttonRemove});
			this.Name = "PlatformController";
			this.Text = "Station construction";
			//! this.Text = "駅工事";
			this.stationPage.ResumeLayout(false);
			this.platformPage.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.lengthBox)).EndInit();
			this.tabControl.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary> The direction of the platform </summary>
		private Direction direction;

		private RailPattern railPattern { get { return RailPattern.get(direction,direction.opposite); } }

		/// <summary>
		/// Called when the direction of a platform is changed.
		/// </summary>
		private void onDirChange(object sender, System.EventArgs e) {
			updatePlatformBox(sender,dirN);
			updatePlatformBox(sender,dirS);
			updatePlatformBox(sender,dirE);
			updatePlatformBox(sender,dirW);
			updateAlphaSprites();
		}

		private void updatePlatformBox( object sender, PictureBox pic ) {
			if(pic==sender) {
				direction = (Direction)pic.Tag;
				pic.BorderStyle = BorderStyle.Fixed3D;
			} else {
				pic.BorderStyle = BorderStyle.None;
			}
		}

		private bool isPlacing { get { return buttonPlace.Checked; } }
		
		private enum Mode {
			Station,
			ThinPlatform,
			FatPlatform
		}

		/// <summary>
		/// Returns true if the current page is the station page.
		/// </summary>
		private Mode currentMode {
			get {
				if( tabControl.SelectedIndex==0 )	return Mode.Station;
				if( checkSlim.Checked )				return Mode.ThinPlatform;
				else								return Mode.FatPlatform;
			}
		}


		private Location baseLoc = world.Location.UNPLACED;
		public override void onMouseMove(MapViewWindow view, Location loc, Point ab ) {
			World w = World.world;

			if(baseLoc!=loc) {
				// update the screen
				updateVoxels();
				baseLoc = loc;
				updateVoxels();
			}
		}

		private void updateVoxels() {
			Location loc2 = baseLoc;
			if(currentMode==Mode.Station) {
				loc2 += selectedStation.size;
			} else {
				// platform
				loc2.x += direction.offsetX*length;
				loc2.y += direction.offsetY*length;
				loc2 += direction.right90;		// width 1 by default
				loc2.z++;
				if( currentMode==Mode.FatPlatform ) {
					loc2 += direction.right90;	// for the attached rail road, width is two
				}
			} 
			World.world.onVoxelUpdated(Cube.createExclusive(baseLoc,loc2));
		}

		public override void onClick(MapViewWindow view, Location loc, Point ab ) {
			switch( currentMode ) {
			case Mode.Station:
				if( isPlacing ) {
					if(!selectedStation.canBeBuilt(loc)) {
						MainWindow.showError("Can not build");
						//! MainWindow.showError("設置できません");
					} else {
						selectedStation.create(loc,true);
					}
				} else {
					Station s = Station.get(loc);
					if(s!=null)		s.remove();
				}
				return;

			case Mode.FatPlatform:
				if( isPlacing ) {
					if(!FatPlatform.canBeBuilt(loc,direction,length)) {
						MainWindow.showError("Can not build");
						//! MainWindow.showError("設置できません");
						return;
					}
					new FatPlatform(loc,direction,length);
				} else {
					Platform p = Platform.get(loc);
					if(p!=null) {
						if(p.canRemove)
							p.remove();
						else
							MainWindow.showError("Can not remove");
							//! MainWindow.showError("撤去できません");
					}
				}
				return;

			case Mode.ThinPlatform:
				if( isPlacing ) {
					if(!ThinPlatform.canBeBuilt(loc,direction,length)) {
						MainWindow.showError("Can not build");
						//! MainWindow.showError("設置できません");
						return;
					}
					new ThinPlatform(loc,direction,length);
				} else {
					Platform p = Platform.get(loc);
					if(p!=null) {
						if(p.canRemove)
							p.remove();
						else
							MainWindow.showError("Can not remove");
							//! MainWindow.showError("撤去できません");
					}
				}
				return;
			}
		}

		private void validateLength(object sender, CancelEventArgs e) {
			// only allow a value longer than 1
			try {
				e.Cancel = lengthBox.Value < 1;
			} catch( Exception ) {
				e.Cancel = true;
			}
		}

		/// <summary> Length of the platform to be built. </summary>
		private int length { get { return (int)lengthBox.Value; } }

		public void drawVoxel( QuarterViewDrawer view, DrawContextEx dc, Location loc, Point pt ) {
			if( loc.z != baseLoc.z || !isPlacing)	return;

			Surface canvas = dc.surface;
			
			switch( this.currentMode ) {
			case Mode.Station:
				if( Cube.createExclusive( baseLoc, alphaSprites.size ).contains(loc) )
					alphaSprites.getSprite( loc-baseLoc ).drawAlpha( canvas, pt );
				break;

			case Mode.ThinPlatform:
				// adjustment
				if( direction==Direction.NORTH )	loc.y += length-1;
				if( direction==Direction.WEST )		loc.x += length-1;

				if( Cube.createExclusive( baseLoc, alphaSprites.size ).contains(loc) )
					alphaSprites.getSprite( loc-baseLoc ).drawAlpha( canvas, pt );
				break;

			case Mode.FatPlatform:
				// left-top corner of the platform to be drawn
				Location ptLT = baseLoc;
				switch(direction.index) {
				case 0:	// NORTH
					ptLT.y -= length-1;
					break;
				case 2: // EAST
					break;	// no adjustment
				case 4: // SOUTH
					ptLT.x -= 1;
					break;
				case 6: // WEST
					ptLT.x -= length-1;
					ptLT.y -= 1;
					break;
				}
					

				if( direction.isParallelToX ) {
					int y = ptLT.y;
					if( (loc.y==y || loc.y==y+1)
					&&  ptLT.x<=loc.x && loc.x<ptLT.x+length )
						alphaSprites.sprites[ loc.x-ptLT.x, loc.y-y, 0 ].drawAlpha( canvas, pt );
				} else {
					int x = ptLT.x;
					if( (loc.x==x || loc.x==x+1)
					&&  ptLT.y<=loc.y && loc.y<ptLT.y+length )
						alphaSprites.sprites[ loc.x-x, loc.y-ptLT.y, 0 ].drawAlpha( canvas, pt );
				}
				break;
			}
		}

		public void drawBefore( QuarterViewDrawer view, DrawContextEx surface ) {}
		public void drawAfter( QuarterViewDrawer view, DrawContextEx surface ) {}

		private void onGroupChanged(object sender, System.EventArgs e) {
			indexSelector.dataSource = (StructureGroup)stationType.SelectedItem;
			onStationChanged(null,null);
		}

		/// <summary>
		/// Called when a selection of the station has changed.
		/// </summary>
		private void onStationChanged( object sender, EventArgs e ) {
			// Builds a new preview bitmap and set it to the picture box
			PreviewDrawer drawer;
			
			drawer = new PreviewDrawer( stationPicture.ClientSize, selectedStation.size );
			drawer.drawCenter( selectedStation.sprites );

			if( stationPreviewBitmap!=null )	stationPreviewBitmap.Dispose();
			stationPicture.Image = stationPreviewBitmap = drawer.createBitmap();

			drawer.Dispose();

			updateAlphaSprites();
		}

		private StationContribution selectedStation { get { return (StationContribution)indexSelector.currentItem; } }

		protected override void OnLoad(System.EventArgs e) {
			base.OnLoad(e);
			onStationChanged(null,null);
			updateAlphaSprites();
		}

		public override void onDetached() {
			// TODO: update voxels correctly
			World.world.onAllVoxelUpdated();
		}

		private void onLengthChanged(object sender, EventArgs e) {
			// TODO: performance slow down when the length is very long. Check why.
			updateAlphaSprites();
		}


		private AlphaBlendSpriteSet alphaSprites;

		/// <summary>
		/// Re-builds an alpha-blending preview.
		/// </summary>
		private void updateAlphaSprites() {
			if(direction==null)		return;	// during the initialization, this method can be called in a wrong timing.
			if(alphaSprites!=null)
				alphaSprites.Dispose();


			Sprite[,,] alphas=null;

			switch( this.currentMode ) {
			case Mode.Station:
				// builds a new alpha blended preview
				alphas = selectedStation.sprites;
				break;

			case Mode.ThinPlatform:
				Sprite spr = ThinPlatform.getSprite( direction, false );

				// build sprite set
				// TODO: use the correct sprite
				if( direction==Direction.NORTH || direction==Direction.SOUTH ) {
					alphas = new Sprite[1,length,1];
					for( int i=0; i<length; i++ )
						alphas[0,i,0] = spr;
				} else {
					alphas = new Sprite[length,1,1];
					for( int i=0; i<length; i++ )
						alphas[i,0,0] = spr;
				}

				alphaSprites = new AlphaBlendSpriteSet(alphas);
				break;

			case Mode.FatPlatform:
				RailPattern rp = this.railPattern;


				// build sprite set
				if( direction==Direction.NORTH || direction==Direction.SOUTH ) {
					alphas = new Sprite[2,length,1];
					int j = direction==Direction.SOUTH?1:0;
					for( int i=0; i<length; i++ ) {
						alphas[j  ,i,0] = FatPlatform.getSprite(direction);
						alphas[j^1,i,0] = railPattern;
					}
				} else {
					alphas = new Sprite[length,2,1];
					int j = direction==Direction.WEST?1:0;
					for( int i=0; i<length; i++ ) {
						alphas[i,j  ,0] = FatPlatform.getSprite(direction);
						alphas[i,j^1,0] = railPattern;
					}
				}
				break;
			}

			alphaSprites = new AlphaBlendSpriteSet(alphas);
			World.world.onAllVoxelUpdated();	// completely redraw the window
		}

		private void onModeChanged(object sender, System.EventArgs e) {
			updateAlphaSprites();
		}
	}
}

