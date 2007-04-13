using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using DxVBLib;
using nft.framework.drawing;

namespace nft.drawing.ddraw7
{
	/// <summary>
	/// DD7Control の概要の説明です。
	/// </summary>
	public class DD7Control : DrawableControl
	{
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;
		private WindowedDDraw7 winDDraw7;

		public DD7Control()
		{
			// この呼び出しは、Windows.Forms フォーム デザイナで必要です。
			InitializeComponent();			

		}

		protected override void OnLoad(EventArgs e) {
			base.OnLoad (e);
			winDDraw7 = new WindowedDDraw7(this);
		}

		public override ISurface Surface { get { return winDDraw7.PrimarySurface; } }

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(winDDraw7!=null)
				winDDraw7.Dispose();
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region コンポーネント デザイナで生成されたコード 
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion
	}

	/// <summary>
	/// DirectDraw with the primary surface to the window of
	/// the specified control.
	/// </summary>
	/// <remarks>this class is originaly implemented by K.Kawaguchi</remarks>
	class WindowedDDraw7 : DDraw7 {
		private PrimarySurface primary;
		public DD7Surface PrimarySurface { get { return primary; } }

		public WindowedDDraw7( Control control ) {
			DDSURFACEDESC2 sd= new DDSURFACEDESC2();
			sd.lFlags = CONST_DDSURFACEDESCFLAGS.DDSD_CAPS;
			sd.ddsCaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_PRIMARYSURFACE;
			primary = new PrimarySurface(handle.CreateSurface(ref sd), control);
			// attach window clipper
			DirectDrawClipper cp = handle.CreateClipper(0);
			cp.SetHWnd( control.Handle.ToInt32() );
			primary.NativeSurface.SetClipper(cp);
		}

		public override void Dispose() {
			primary.Dispose();
			primary=null;
		}

		private void control_BoundsChanged(object sender, EventArgs e) {
			Control c = sender as Control;
			Point p = c.PointToScreen(new Point(0));
			primary.ClipRect = new Rectangle(p,c.Size);
		}

	}
	class PrimarySurface : DD7Surface {
		private Control control;

		public PrimarySurface(DirectDrawSurface7 surface, Control c) : base(surface){
			this.control = c;
		}

		protected Rectangle controlBorder(){
			Point p = control.PointToScreen(new Point(0));
			Debug.WriteLine(new Rectangle(p,control.Size));
			return new Rectangle(p,control.Size);
		}

		public override RECT ClipRECT {
			get {
				return Util.toRECT(controlBorder());
			}
			set {
				// invalid operation
				base.ClipRECT = value;
			}
		}

		public override Rectangle ClipRect {
			get {
				return controlBorder();
			}
			set {
				// invalid operation
				base.ClipRect = value;
			}
		}
		
		public override void Dispose() {
			base.Dispose ();
			control = null;
		}

	}
}
