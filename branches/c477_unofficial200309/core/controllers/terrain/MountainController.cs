using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using freetrain.views.map;
using freetrain.util;
using freetrain.world;
using freetrain.world.terrain;

namespace freetrain.controllers.terrain
{
	/// <summary>
	/// Manipulates mountains
	/// </summary>
	public class MountainController : AbstractControllerImpl
	{
		#region Singleton instance management
		/// <summary>
		/// Creates a new controller window, or active the existing one.
		/// </summary>
		public static void create() {
			if(theInstance==null)
				theInstance = new MountainController();
			theInstance.Show();
			theInstance.Activate();
		}

		private static MountainController theInstance;

		protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
			base.OnClosing(e);
			theInstance = null;
		}
		#endregion

		public MountainController() {
			InitializeComponent();
		}

		protected override void Dispose( bool disposing ) {
			if( disposing && components != null)
				components.Dispose();
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		private System.Windows.Forms.PictureBox preview;
		private System.Windows.Forms.RadioButton buttonUp;
		private System.Windows.Forms.RadioButton buttonDown;
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// �f�U�C�i �T�|�[�g�ɕK�v�ȃ��\�b�h�ł��B���̃��\�b�h�̓��e��
		/// �R�[�h �G�f�B�^�ŕύX���Ȃ��ł��������B
		/// </summary>
		private void InitializeComponent()
		{
			this.buttonUp = new System.Windows.Forms.RadioButton();
			this.buttonDown = new System.Windows.Forms.RadioButton();
			this.preview = new System.Windows.Forms.PictureBox();
			this.SuspendLayout();
			// 
			// buttonUp
			// 
			this.buttonUp.Appearance = System.Windows.Forms.Appearance.Button;
			this.buttonUp.Checked = true;
			this.buttonUp.Location = new System.Drawing.Point(4, 96);
			this.buttonUp.Name = "buttonUp";
			this.buttonUp.Size = new System.Drawing.Size(56, 24);
			this.buttonUp.TabIndex = 2;
			this.buttonUp.TabStop = true;
			this.buttonUp.Text = "���N";
			this.buttonUp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// buttonDown
			// 
			this.buttonDown.Appearance = System.Windows.Forms.Appearance.Button;
			this.buttonDown.Location = new System.Drawing.Point(60, 96);
			this.buttonDown.Name = "buttonDown";
			this.buttonDown.Size = new System.Drawing.Size(56, 24);
			this.buttonDown.TabIndex = 4;
			this.buttonDown.Text = "�@��";
			this.buttonDown.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// preview
			// 
			this.preview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.preview.Location = new System.Drawing.Point(4, 8);
			this.preview.Name = "preview";
			this.preview.Size = new System.Drawing.Size(112, 80);
			this.preview.TabIndex = 3;
			this.preview.TabStop = false;
			// 
			// MountainController
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(120, 123);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.buttonUp,
																		  this.buttonDown,
																		  this.preview});
			this.Name = "MountainController";
			this.Text = "�n�`����";
			this.ResumeLayout(false);

		}
		#endregion

		public override LocationDisambiguator disambiguator { get { return GroundDisambiguator.theInstance; } }


		private bool isRaising { get { return buttonUp.Checked ^ Keyboard.isControlKeyPressed; } }
		


		public override void onMouseMove(MapViewWindow view, Location loc, Point ab) {
			if( Keyboard.isShiftKeyPressed ) {
				loc = selectVoxel(view,loc,ab);

				if( isRaising )		raise( loc );
				else				lower( loc );
			}
		}

		public override void onClick(MapViewWindow view, Location loc, Point ab) {
			loc = selectVoxel(view,loc,ab);

			if( isRaising )		raise( loc );
			else				lower( loc );
		}

		/// <summary>
		/// Selects the south-western voxel of the point selected by the mouse.
		/// The loc parameter and the ab parameter should point to the same location.
		/// </summary>
		private Location selectVoxel( MapViewWindow view, Location loc, Point ab ) {
			// top-left corner of the selected location
			Point vxl = World.world.fromXYZToAB(loc);
			
			Point offset = new Point( ab.X-vxl.X, ab.Y-vxl.Y );

			if( offset.X< 8 )	loc.x--;
			else
			if( offset.X>=24)	loc.y++;
			else {
				MountainVoxel mv = MountainVoxel.get(loc);
				int h0 = (mv!=null)?(int)mv.getHeight(Direction.NORTHEAST):0;
				int h2 = (mv!=null)?(int)mv.getHeight(Direction.SOUTHWEST):0;

				if( offset.Y >= (16-(h0+h2)*4)/2 ) {
					loc.x--; loc.y++;
				}
			}

			return loc;
		}

		/// <summary>
		/// Checks the height agreement of the four corners adjacent to
		/// the north-eastern corner of the given location.
		/// </summary>
		/// <param name="loc"></param>
		/// <returns></returns>
		private bool isFourAdjacentCornersMatched( Location loc ) {
			Direction d = Direction.NORTH;

			for( int i=0; i<4; i++ ) {
				if( !MountainVoxel.isCornerMatched(loc,d.left) )
					return false;

				loc += d;
				d = d.right90;
			}

			return true;
		}

		// clean it up by using MountainVoxel.isCornerMatched

		/// <summary>
		/// Return true iff the north-eastern corner of the given location
		/// can be raised by a quarter height unit.
		/// </summary>
		private bool canBeRaised( Location loc ) {
			World w = World.world;

			if( !isFourAdjacentCornersMatched(loc) )	return false;


			Voxel baseVoxel = w[loc];
			int glevel = w.getGroundLevel(loc);

			if( loc.z != glevel )	return false;	//mountain can be placed only at the ground level

			// true if this ground level is too close to the roof.
			bool nearRoof = ( glevel==World.world.size.z-1 );

			for( int x=0; x<=1; x++ ) {
				for( int y=-1; y<=0; y++ ) {
					Location l = new Location( loc.x+x, loc.y+y, loc.z );
					Direction d = Direction.get( 1-x*2, -y*2-1 );	// corner to modify

					if( w.isOutsideWorld(l) )
						continue;	// it's OK if it's beyond the border
					
					Voxel v = w[l];

					if( glevel != w.getGroundLevel(l) )
						return false;	// different ground level

					if( v==null )
						continue;	// this voxel is unoccupied.

					if( v is MountainVoxel ) {
						int h = ((MountainVoxel)v).getHeight(d);
						if( h==4 )
							return false;	// corner saturated.
						if( nearRoof && h==3 )
							return false;	// too close to the roof

						continue;	// otherwise OK
					}

					return false;	// occupied for another purpose
				}
			}

			if( World.world.isOutsideWorld(loc) )
				return false;

			return true;
		}

		/// <summary>
		/// Raises the north-eastern corner of the specified voxel
		/// </summary>
		/// <returns>false if the operation was unsuccessful.</returns>
		private bool raise( Location loc ) {
			World w = World.world;

			// make sure that four surrounding voxels can be raised,
			// and the ground levels of them are the same
			if(!canBeRaised(loc))
				return false;
			
			// then actually change the terrain
			for( int x=0; x<=1; x++ ) {
				for( int y=-1; y<=0; y++ ) {
					Location l = new Location( loc.x+x, loc.y+y, loc.z );

					Voxel vx = w[l];
					if( vx is World.OutOfWorldVoxel )
						continue;	// this is beyond the border

					MountainVoxel v = vx as MountainVoxel;
					
					Direction d = Direction.get( 1-x*2, -y*2-1 );	// corner to modify

					if( v==null )
						v = new MountainVoxel( l, 0,0,0,0 );
					
					// raise the corner
					v.setHeight( d, v.getHeight(d)+1 );
					
					if( v.isSaturated ) {
						// if the voxel is saturated, raise the ground level
						w.raiseGround(l);
						w.remove(l);	// remove this voxel
					}
				}
			}
			
			return true;
		}


		// clean it up by using MountainVoxel.isCornerMatched
		private bool canBeLowered( ref Location loc ) {
			World world = World.world;

			if( !isFourAdjacentCornersMatched(loc) )	return false;

			MountainVoxel mvBase = MountainVoxel.get(loc);
			if( mvBase!=null ) {
				if( mvBase.getHeight(Direction.NORTHEAST)==0 )
					return false;	// other corners need to be lowered first.
			} else {
				int glevel = world.getGroundLevel(loc);
				if( glevel!=loc.z && glevel!=loc.z-1 )
					return false;
				if( loc.z==0 )
					return false;	// can't dig deeper
				loc.z--;
			}

			// check other voxels
			for( int x=0; x<=1; x++ ) {
				for( int y=-1; y<=0; y++ ) {
					Location l = new Location( loc.x+x, loc.y+y, loc.z );

					if( MountainVoxel.get(l)!=null )
						continue;	// if it's mountain, OK.
					
					// otherwise, make sure that nothing is on it.
					if( World.world[ l.x, l.y, l.z+1 ]!=null )
						return false;
					// and nothing is under it
					if( World.world[ l.x, l.y, l.z ]!=null )
						return false;
				}
			}

			if( World.world.isOutsideWorld(loc) )
				return false;

			return true;
		}

		/// <summary>
		/// Lowers the north-eastern corner of the specified voxel.
		/// </summary>
		/// <returns>false if the operation was unsuccessful.</returns>
		private bool lower( Location loc ) {

			World world = World.world;

			if(!canBeLowered(ref loc))	return false;


			// then actually change the terrain
			for( int x=0; x<=1; x++ ) {
				for( int y=-1; y<=0; y++ ) {
					Location l = new Location( loc.x+x, loc.y+y, loc.z );
					Direction d = Direction.get( 1-x*2, -y*2-1 );	// corner to modify

					MountainVoxel mv = MountainVoxel.get(l);
					if( mv==null ) {
						World.world.lowerGround(l);
						mv = new MountainVoxel( l, 4,4,4,4 );
					}
					
					mv.setHeight( d, mv.getHeight(d)-1 );

					if( mv.isFlattened )	// completely flattened
						world.remove(mv);
				}
			}

			return true;
		}
	}
}