using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using freetrain.framework;
using freetrain.views;
using freetrain.views.map;
using freetrain.world;
using freetrain.world.rail;

namespace freetrain.controllers.rail
{
	/// <summary>
	/// StationBridgeController の概要の説明です。
	/// </summary>
	public class StationPassagewayController : AbstractControllerImpl, MapOverlay, LocationDisambiguator
	{
		#region Singleton instance management
		/// <summary>
		/// Creates a new controller window, or active the existing one.
		/// </summary>
		public static void create() {
			if(theInstance==null)
				theInstance = new StationPassagewayController();
			theInstance.Show();
			theInstance.Activate();
		}

		private static StationPassagewayController theInstance;

		protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
			base.OnClosing(e);
			theInstance = null;
		}
		#endregion

		private StationPassagewayController() {
			InitializeComponent();

			typeCombo.Items.Add("コンクリート");
			typeCombo.Items.Add("煉瓦");
		}


		protected override void Dispose( bool disposing ) {
			if( disposing && components != null)
				components.Dispose();
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox typeCombo;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radioPassage;
		private System.Windows.Forms.RadioButton radioStair;
		private System.Windows.Forms.RadioButton buttonPlace;
		private System.Windows.Forms.RadioButton buttonRemove;
		private System.ComponentModel.Container components = null;
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.buttonPlace = new System.Windows.Forms.RadioButton();
			this.buttonRemove = new System.Windows.Forms.RadioButton();
			this.typeCombo = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radioStair = new System.Windows.Forms.RadioButton();
			this.radioPassage = new System.Windows.Forms.RadioButton();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label1.Dock = System.Windows.Forms.DockStyle.Right;
			this.label1.Location = new System.Drawing.Point(122, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 144);
			this.label1.TabIndex = 1;
			this.label1.Text = "薄いホーム専用です。通路をホームと直角に設置してから階段を設置してください。";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// buttonPlace
			// 
			this.buttonPlace.Appearance = System.Windows.Forms.Appearance.Button;
			this.buttonPlace.Checked = true;
			this.buttonPlace.Location = new System.Drawing.Point(8, 112);
			this.buttonPlace.Name = "buttonPlace";
			this.buttonPlace.Size = new System.Drawing.Size(48, 24);
			this.buttonPlace.TabIndex = 1;
			this.buttonPlace.TabStop = true;
			this.buttonPlace.Text = "設置";
			this.buttonPlace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// buttonRemove
			// 
			this.buttonRemove.Appearance = System.Windows.Forms.Appearance.Button;
			this.buttonRemove.Location = new System.Drawing.Point(64, 112);
			this.buttonRemove.Name = "buttonRemove";
			this.buttonRemove.Size = new System.Drawing.Size(48, 24);
			this.buttonRemove.TabIndex = 2;
			this.buttonRemove.Text = "撤去";
			this.buttonRemove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// typeCombo
			// 
			this.typeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.typeCombo.Location = new System.Drawing.Point(8, 8);
			this.typeCombo.Name = "typeCombo";
			this.typeCombo.Size = new System.Drawing.Size(104, 20);
			this.typeCombo.TabIndex = 3;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.radioStair,
																					this.radioPassage});
			this.groupBox1.Location = new System.Drawing.Point(8, 40);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(104, 64);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "部品";
			// 
			// radioStair
			// 
			this.radioStair.Location = new System.Drawing.Point(8, 40);
			this.radioStair.Name = "radioStair";
			this.radioStair.Size = new System.Drawing.Size(88, 16);
			this.radioStair.TabIndex = 1;
			this.radioStair.Text = "階段";
			// 
			// radioPassage
			// 
			this.radioPassage.Checked = true;
			this.radioPassage.Location = new System.Drawing.Point(8, 16);
			this.radioPassage.Name = "radioPassage";
			this.radioPassage.Size = new System.Drawing.Size(88, 16);
			this.radioPassage.TabIndex = 0;
			this.radioPassage.TabStop = true;
			this.radioPassage.Text = "通路";
			// 
			// StationBridgeController
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(242, 144);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.groupBox1,
																		  this.typeCombo,
																		  this.label1,
																		  this.buttonPlace,
																		  this.buttonRemove});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "StationBridgeController";
			this.Text = "駅連絡通路";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private bool isPlacing { get { return buttonPlace.Checked; } }
		private bool isPassage { get { return radioPassage.Checked; } }
		private bool isStair { get { return !isPassage; } }

		public override LocationDisambiguator disambiguator { get { return this; } }

		/// <summary> LocationDisambiguator implementation </summary>
		public bool isSelectable( Location loc ) {
			// align to platforms or the ground
			if( anchor!=UNPLACED )
				// must be the same height with the anchor
				return loc.z == anchor.z;
			else
				return ThinPlatform.get(loc)!=null || GroundDisambiguator.theInstance.isSelectable(loc);
		}

		private static readonly Location UNPLACED = world.Location.UNPLACED;

		/// <summary> Used when we are placing a passageway. </summary>
		private Location anchor = UNPLACED;
		private Location location = UNPLACED;

		public override void onMouseMove(MapViewWindow view, Location loc, Point ab ) {
			World w = World.world;

			if(isStair) {
				location = loc;
				w.onAllVoxelUpdated();
			} else
			if(anchor!=UNPLACED) {
				loc = loc.align4To(anchor);
				if(loc!=location) {
					location = loc;
					w.onAllVoxelUpdated();
				}
			}
		}

		public override void onClick(MapViewWindow view, Location loc, Point ab ) {
			if(isStair) {
				if( isPlacing ) {
					if( !createStair(loc,false) )
						MainWindow.showError("設置できません");
				} else
					removeStair(loc);
			} else {
				// passageway
				if(anchor==UNPLACED) {
					// place an anchor
					anchor = loc;
				} else {
					loc = loc.align4To(anchor);
					if( isPlacing ) {
						if( canBuildPassageway( anchor, loc ) )
							buildPassageway( anchor, loc );
						else
							MainWindow.showError("設置できません");
					} else {
						removePassageway( anchor, loc );
					}
					anchor = UNPLACED;
				}
			}
		}
		public override void onRightClick( MapViewWindow source, Location loc, Point ab ) {
			if( anchor==UNPLACED || isStair)
				Close();	// cancel
			else {
				// cancel the anchor
				World.world.onAllVoxelUpdated();
				anchor = UNPLACED;
			}
		}




		public void drawBefore( QuarterViewDrawer view, DrawContextEx canvas ) {
			if( anchor!=UNPLACED && isPlacing && isPassage )
				canvas.tag = canBuildPassageway(anchor,location);
		}

		public void drawVoxel( QuarterViewDrawer view, DrawContextEx canvas, Location loc, Point pt ) {
			object tag = canvas.tag;

			if( tag!=null && (bool)tag && loc.inBetween( anchor, location ) ) {
				PassagewayRail.getFloatingSprite( anchor.getDirectionTo(location) )
					.drawAlpha( canvas.surface, pt );
			}
		}

		public void drawAfter( QuarterViewDrawer view, DrawContextEx canvas ) {
		}





		// remove any passageway 
		private static void removePassageway( Location loc1, Location loc2 ) {
			// on each voxel along the way
			for( Location loc=loc1; ; loc=loc.toward(loc2) ) {

				TrafficVoxel tv = TrafficVoxel.get(loc);
				if(tv!=null &&  tv.railRoad is ThinPlatform.RailRoadImpl) {
					ThinPlatform.RailRoadImpl rr = (ThinPlatform.RailRoadImpl)tv.railRoad;
					
					if( rr.outlook is ThinPlatform.PassagewayPlatform ) {
						// retore the normal platform.
						rr.outlook = ThinPlatform.plainPlatform;
					}
				} else {
					// TODO: open-ended bridge
				}

				if(loc==loc2) {
					// TODO: correctly updated voxels
					World.world.onAllVoxelUpdated();
					return;
				}
			}
		}


		private static bool canBuildPassageway( Location loc1, Location loc2 ) {
			if(loc1==loc2)		return false;

			Direction dd = loc1.getDirectionTo(loc2);	// direction
			Debug.Assert(dd.isSharp);

			while( true ) {

				ThinPlatform.RailRoadImpl rr = ThinPlatform.RailRoadImpl.get(loc1);
				
				if( World.world[loc1]==null		// unused voxel == can be used in any way
				||	(rr!=null && Direction.angle( rr.direction, dd )==2)) {	// orthogonal platform.
					
					if(loc1==loc2)	return true;	// all voxels satisfy the constraint
					
					loc1 = loc1.toward(loc2);
					continue;
				}

				return false;	// otherwise fail
			}
		}

		private static void buildPassageway( Location loc1, Location loc2 ) {
			Direction dd = loc1.getDirectionTo(loc2);	// direction
			Debug.Assert(dd.isSharp);

			// on each voxel along the way
			for( Location loc=loc1; ; loc=loc.toward(loc2) ) {

				TrafficVoxel tv = TrafficVoxel.get(loc);
				if(tv!=null) {
					if(tv.railRoad is ThinPlatform.RailRoadImpl) {
						ThinPlatform.RailRoadImpl rr = (ThinPlatform.RailRoadImpl)tv.railRoad;
						
						if( rr.outlook is ThinPlatform.PassagewayPlatform ) {
							ThinPlatform.PassagewayPlatform ppp = (ThinPlatform.PassagewayPlatform)rr.outlook;

							if( ppp.hasBridge ) {
								// if the passageway always has a bridge, keep it.
							} else {
								// if it has a partial passageway, make a bridge for it.
								rr.outlook = new ThinPlatform.PassagewayPlatform(true);
							}
						} else {
							// leave the end un-bridged.
							rr.outlook = new ThinPlatform.PassagewayPlatform(
								!((loc==loc1)&&(dd.right90==rr.direction) || (loc==loc2)&&(dd.left90==rr.direction)));
						}
					} else {
						// TODO: open-ended bridge
					}
				} else {
					// TODO: allow passageway to go over unused grounds
				}

				if(loc==loc2)
					return;
			}
		}



		//
		//
		// stairs
		//
		//
		/// <summary>
		/// Builds a new stair
		/// </summary>
		/// <param name="loc"></param>
		/// <param name="test">true to just check if it can be built.</param>
		/// <returns>true if it was/can be built.</returns>
		private bool createStair( Location loc, bool test) {
			ThinPlatform.RailRoadImpl rr = ThinPlatform.RailRoadImpl.get(loc);
			if( rr==null )	return false;

			ThinPlatform.RailRoadImpl nrr,prr;
			nrr = ThinPlatform.RailRoadImpl.get(loc+rr.direction);
			prr = ThinPlatform.RailRoadImpl.get(loc-rr.direction);

			if(nrr!=null && nrr.outlook is ThinPlatform.PassagewayPlatform) {
				// TODO correctly compute the roof
				if(!test)
					rr.outlook = new ThinPlatform.StairPlatform( true );
				return true;
			}

			if(prr!=null && prr.outlook is ThinPlatform.PassagewayPlatform) {
				// TODO correctly compute the roof
				if(!test)
					rr.outlook = new ThinPlatform.StairPlatform( false );
				return true;
			}

			return false;
		}
		private void removeStair( Location loc ) {
			ThinPlatform.RailRoadImpl rr = ThinPlatform.RailRoadImpl.get(loc);
			if(rr!=null && rr.outlook is ThinPlatform.StairPlatform)
				rr.outlook = ThinPlatform.plainPlatform;
		}
	}
}
