using System;
using System.Drawing;
using System.Windows.Forms;
using freetrain.controllers;
using freetrain.framework;
using freetrain.views.map;
using freetrain.world.rail;
using org.kohsuke.directdraw;

namespace freetrain.world.development
{
	/// <summary>
	/// Controller that checks the land value.
	/// </summary>
	internal class LandValueInspector : AbstractControllerImpl/*, MapOverlay*/
	{
		public LandValueInspector() {
			InitializeComponent();

			Show();
			Activate();
		}

		protected override void Dispose( bool disposing ) {
			if( disposing && components != null)
				components.Dispose();
			base.Dispose( disposing );
		}


		public override void onMouseMove( MapViewWindow view, Location loc, Point ab ) {
			MainWindow.mainWindow.statusText = "�n���F" + World.world.landValue[loc];
		}

		//
		// Disambiguator implementation
		//
		public override LocationDisambiguator disambiguator { get { return GroundDisambiguator.theInstance; } }



//		//
//		// MapOverlay implementation
//		//
//		public void drawVoxel( MapViewWindow view, DrawContextEx context, Location loc, Point pt ) {
//		}
//
//		public void drawBefore( MapViewWindow view, DrawContextEx context ) {}
//		public void drawAfter( MapViewWindow view, DrawContextEx context ) {}

		#region Designer generated code
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label1;

		/// <summary>
		/// Designer �T�|�[�g�ɕK�v�ȃ��\�b�h�ł��B�R�[�h �G�f�B�^��
		/// ���̃��\�b�h�̃R���e���c��ύX���Ȃ��ł��������B
		/// </summary>
		private void InitializeComponent() {
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 64);
			this.label1.TabIndex = 0;
			this.label1.Text = "�J�[�\�����ړ����Ēn����\��";
			// 
			// LandValueInspector
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(120, 75);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.label1});
			this.Name = "LandValueInspector";
			this.Text = "�n���̌���";
			this.ResumeLayout(false);

		}
		#endregion
	
	}
}
