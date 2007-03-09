using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using freetrain.contributions.common;
using freetrain.contributions.rail;
using freetrain.framework.plugin;
using freetrain.views;
using freetrain.views.map;
using freetrain.world;

namespace freetrain.controllers
{
	/// <summary>
	/// Controller that places/removes lines, such as roads or rail roads.
	/// </summary>
	public abstract class AbstractLineController : AbstractControllerImpl, MapOverlay
	{
		public AbstractLineController( LineContribution _type ) {
			InitializeComponent();
			this.type = _type;
			this.Text = type.name;
			Bitmap bmp = type.previewBitmap;
			this.picture.Image = bmp;
			this.picture.BackColor = bmp.GetPixel(0,bmp.Size.Height-1);
		}

		private readonly LineContribution type;

		protected override void Dispose( bool disposing ) {
			if( disposing && components != null)
				components.Dispose();
			
			this.picture.Image.Dispose();	// I'm not sure if I really need to do this or not.

			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		private System.Windows.Forms.RadioButton buttonRemove;
		private System.Windows.Forms.RadioButton buttonPlace;
		protected System.Windows.Forms.PictureBox picture;
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.buttonRemove = new System.Windows.Forms.RadioButton();
			this.buttonPlace = new System.Windows.Forms.RadioButton();
			this.picture = new System.Windows.Forms.PictureBox();
			this.SuspendLayout();
			// 
			// buttonRemove
			// 
			this.buttonRemove.Appearance = System.Windows.Forms.Appearance.Button;
			this.buttonRemove.Location = new System.Drawing.Point(56, 88);
			this.buttonRemove.Name = "buttonRemove";
			this.buttonRemove.Size = new System.Drawing.Size(48, 24);
			this.buttonRemove.TabIndex = 7;
			this.buttonRemove.Text = "撤去";
			this.buttonRemove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.buttonRemove.CheckedChanged += new System.EventHandler(this.modeChanged);
			// 
			// buttonPlace
			// 
			this.buttonPlace.Appearance = System.Windows.Forms.Appearance.Button;
			this.buttonPlace.Checked = true;
			this.buttonPlace.Location = new System.Drawing.Point(8, 88);
			this.buttonPlace.Name = "buttonPlace";
			this.buttonPlace.Size = new System.Drawing.Size(48, 24);
			this.buttonPlace.TabIndex = 6;
			this.buttonPlace.TabStop = true;
			this.buttonPlace.Text = "敷設";
			this.buttonPlace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.buttonPlace.CheckedChanged += new System.EventHandler(this.modeChanged);
			// 
			// picture
			// 
			this.picture.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
			this.picture.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.picture.Location = new System.Drawing.Point(8, 8);
			this.picture.Name = "picture";
			this.picture.Size = new System.Drawing.Size(96, 72);
			this.picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.picture.TabIndex = 4;
			this.picture.TabStop = false;
			// 
			// RoadController
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(112, 115);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.buttonRemove,
																		  this.buttonPlace,
																		  this.picture});
			this.Name = "RoadController";
			this.Text = "道路工事";
			this.ResumeLayout(false);

		}
		#endregion

		private bool isPlacing { get { return buttonPlace.Checked; } }




		/// <summary>
		/// The first location selected by the user.
		/// </summary>
		private Location anchor = UNPLACED;

		/// <summary>
		/// Current mouse position. Used only when anchor!=UNPLACED
		/// </summary>
		private Location currentPos = UNPLACED;

		private static Location UNPLACED = freetrain.world.Location.UNPLACED;

		/// <summary>
		/// Aligns the given location to the anchor so that
		/// the location will become straight.
		/// </summary>
		private Location align( Location loc ) {
			loc.z = anchor.z;

			if( type.directionMode == SpecialRailContribution.DirectionMode.FourWay )
				return loc.align4To(anchor);

			if( type.directionMode == SpecialRailContribution.DirectionMode.EightWay )
				return loc.align8To(anchor);

			Debug.Assert(false);
			return UNPLACED;
		}





		public override void onMouseMove( MapViewWindow view, Location loc, Point ab ) {
			if( anchor!=UNPLACED && isPlacing && currentPos!=loc ) {
				if( currentPos!=UNPLACED )
					World.world.onVoxelUpdated(Cube.createInclusive(anchor,currentPos));
				currentPos = align(loc);
				World.world.onVoxelUpdated(Cube.createInclusive(anchor,currentPos));
			}
		}

		public override void onClick( MapViewWindow source, Location loc, Point ab ) {
			if(anchor==UNPLACED) {
				anchor = loc;
				sameLevelDisambiguator = new SameLevelDisambiguator(anchor.z);
			} else {
				loc = align(loc);
				if(anchor!=loc) {
					if(isPlacing) {
						if( type.canBeBuilt( anchor, loc ) )
							// build new railroads.
							type.build( anchor, loc );
					} else
						// remove existing ones
						type.remove( anchor, loc );
					World.world.onVoxelUpdated(Cube.createInclusive(anchor,loc));
				}
				anchor = UNPLACED;
			}
		}
		public override void onRightClick( MapViewWindow source, Location loc, Point ab ) {
			if( anchor==UNPLACED )
				Close();	// cancel
			else {
				// cancel the anchor
				if( currentPos!=UNPLACED )
					World.world.onVoxelUpdated(Cube.createInclusive(anchor,currentPos));
				anchor = UNPLACED;
			}
		}

		public override LocationDisambiguator disambiguator {
			get {
				// the 2nd selection must go to the same height as the anchor.
				if(anchor==UNPLACED)	return RailRoadDisambiguator.theInstance;
				else					return sameLevelDisambiguator;
			}
		}
		private LocationDisambiguator sameLevelDisambiguator;


		private void modeChanged( object sender, EventArgs e ) {
			anchor = UNPLACED;
		}






		private bool inBetween( Location loc, Location lhs, Location rhs ) {
			if( !loc.inBetween(lhs,rhs) )	return false;

			if(( lhs.x==rhs.x && rhs.x==loc.x )
			|| ( lhs.y==rhs.y && rhs.y==loc.y ) )	return true;

			if( Math.Abs(loc.x-lhs.x)==Math.Abs(loc.y-lhs.y) )	return true;

			return false;
		}


		public void drawBefore( QuarterViewDrawer view, DrawContextEx canvas ) {
			if( anchor!=UNPLACED && isPlacing )
				canvas.tag = type.canBeBuilt(anchor,currentPos);
		}

		public void drawVoxel( QuarterViewDrawer view, DrawContextEx canvas, Location loc, Point pt ) {
			object tag = canvas.tag;

			if( tag!=null && (bool)tag && inBetween( loc, anchor, currentPos ) ) {
				Direction d = anchor.getDirectionTo(currentPos);
				draw( d, canvas, pt );
			}
		}

		public void drawAfter( QuarterViewDrawer view, DrawContextEx canvas ) {
		}

		/// <summary>
		/// Draw the preview on the given point.
		/// </summary>
		protected abstract void draw( Direction d, DrawContextEx canvs, Point pt );
	}
}
