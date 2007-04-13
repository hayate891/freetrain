
using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using nft.framework;
using nft.util;
using nft.ui;
using nft.framework.drawing;

namespace nft.debug.ddraw
{
	/// <summary>
	/// TestFormDDraw の概要の説明です。
	/// </summary>
	public class TestFormDDraw : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TabPage tabPage1;
		private TestDXDynamicViewControl dxViewControl1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabControl tabControl1;
		private TestDXStaticViewControl dxStaticViewControl1;
		private System.Windows.Forms.TabPage tabPage3;
		private nft.framework.drawing.DrawablePanel drawable;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label l_t1;
		private System.Windows.Forms.Label l_t2;
		private System.Windows.Forms.Label l_t3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button btn_go;
		private System.Windows.Forms.ComboBox combo_filter;
		private System.Windows.Forms.ComboBox combo_zoom;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private bool calced;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TestFormDDraw()
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();
			combo_filter.SelectedIndex = 0;
			combo_filter.Items.Add(graphic.GetHalfAlphaDrawer());
			combo_filter.Items.Add(graphic.GetIntersectDrawer());
			combo_filter.Items.Add(graphic.GetBrighterDrawer());
			combo_filter.Items.Add(graphic.GetColorBurnDrawer(Color.OrangeRed,0.5f));
			combo_zoom.SelectedIndex = 1;
			prepareDrawing();
		}

		protected IGraphicManager graphic { get { return GlobalModules.GraphicManager; } }
		protected ISprite[] sprite;
		protected ISurface bg;
		protected ISurface buffer;
		protected ISurfaceDrawer currentDrawer;

		private void prepareDrawing(){
			string path = Path.Combine(Directories.AppBaseDir,@"res\SpriteSamples.bmp");
			sprite = new ISprite[3];
			Rectangle r = new Rectangle(0,0,32,32);
			for(int i=0; i<3; i++){
				ISimpleTexture t = graphic.CreateSimpleTexture(path,r,new Point(0));
				t.PickColorKeyFromSource(0,31);
				sprite[i] = graphic.CreateSprite(t);
				r.X += 32;
			}
			path = Path.Combine(Directories.AppBaseDir,@"res\SceneSample.bmp");
			bg = graphic.CreateSurfaceFromBitmap(new Bitmap(path),graphic.CurrentColorMode,SurfaceAlloc.SystemMem);
			buffer = graphic.CreateOffscreenSurface(drawable.Canvas.Size,PixelColorMode.Default,SurfaceAlloc.SystemMem);
			drawable.Canvas.MouseMove+=new MouseEventHandler(drawableControl1_MouseMove);
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

	 

		#region Windows フォーム デザイナで生成されたコード 
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.dxViewControl1 = new nft.debug.ddraw.TestDXDynamicViewControl();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.dxStaticViewControl1 = new nft.debug.ddraw.TestDXStaticViewControl();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.combo_filter = new System.Windows.Forms.ComboBox();
			this.btn_go = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.l_t1 = new System.Windows.Forms.Label();
			this.drawable = new nft.framework.drawing.DrawablePanel();
			this.l_t2 = new System.Windows.Forms.Label();
			this.l_t3 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.combo_zoom = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.dxViewControl1);
			this.tabPage1.Location = new System.Drawing.Point(4, 21);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(400, 367);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Dynamic";
			// 
			// dxViewControl1
			// 
			this.dxViewControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.dxViewControl1.AutoScroll = true;
			this.dxViewControl1.AutoScrollMinSize = new System.Drawing.Size(320, 240);
			this.dxViewControl1.Location = new System.Drawing.Point(0, 0);
			this.dxViewControl1.Name = "dxViewControl1";
			this.dxViewControl1.ScrollLocation = new System.Drawing.Point(0, 0);
			this.dxViewControl1.Size = new System.Drawing.Size(400, 368);
			this.dxViewControl1.TabIndex = 2;
			this.dxViewControl1.WorldPosition = new System.Drawing.Point(0, 0);
			this.dxViewControl1.WorldSize = new System.Drawing.Size(320, 240);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.dxStaticViewControl1);
			this.tabPage2.Location = new System.Drawing.Point(4, 21);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(400, 367);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Static";
			// 
			// dxStaticViewControl1
			// 
			this.dxStaticViewControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.dxStaticViewControl1.Location = new System.Drawing.Point(0, 0);
			this.dxStaticViewControl1.Name = "dxStaticViewControl1";
			this.dxStaticViewControl1.Size = new System.Drawing.Size(392, 360);
			this.dxStaticViewControl1.TabIndex = 0;
			this.dxStaticViewControl1.ViewHAlign = nft.ui.DXStaticViewControl.HorizontalAlign.CENTER;
			this.dxStaticViewControl1.ViewPosition = new System.Drawing.Point(196, 180);
			this.dxStaticViewControl1.ViewVAlign = nft.ui.DXStaticViewControl.VerticalAlign.CENTER;
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.ItemSize = new System.Drawing.Size(53, 17);
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(408, 392);
			this.tabControl1.TabIndex = 1;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.combo_filter);
			this.tabPage3.Controls.Add(this.btn_go);
			this.tabPage3.Controls.Add(this.label5);
			this.tabPage3.Controls.Add(this.l_t1);
			this.tabPage3.Controls.Add(this.drawable);
			this.tabPage3.Controls.Add(this.l_t2);
			this.tabPage3.Controls.Add(this.l_t3);
			this.tabPage3.Controls.Add(this.label6);
			this.tabPage3.Controls.Add(this.label7);
			this.tabPage3.Controls.Add(this.combo_zoom);
			this.tabPage3.Controls.Add(this.label2);
			this.tabPage3.Controls.Add(this.label3);
			this.tabPage3.Location = new System.Drawing.Point(4, 21);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(400, 367);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "NewLib";
			// 
			// combo_filter
			// 
			this.combo_filter.Items.AddRange(new object[] {
															  "Normal"});
			this.combo_filter.Location = new System.Drawing.Point(40, 8);
			this.combo_filter.Name = "combo_filter";
			this.combo_filter.Size = new System.Drawing.Size(176, 20);
			this.combo_filter.TabIndex = 4;
			this.combo_filter.Text = "comboBox1";
			this.combo_filter.SelectedIndexChanged += new System.EventHandler(this.combo_filter_SelectedIndexChanged);
			// 
			// btn_go
			// 
			this.btn_go.Location = new System.Drawing.Point(352, 5);
			this.btn_go.Name = "btn_go";
			this.btn_go.Size = new System.Drawing.Size(40, 24);
			this.btn_go.TabIndex = 3;
			this.btn_go.Text = "GO!";
			this.btn_go.Click += new System.EventHandler(this.btn_go_Click);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(96, 40);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(24, 16);
			this.label5.TabIndex = 2;
			this.label5.Text = "T1:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// l_t1
			// 
			this.l_t1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.l_t1.Location = new System.Drawing.Point(120, 40);
			this.l_t1.Name = "l_t1";
			this.l_t1.Size = new System.Drawing.Size(64, 16);
			this.l_t1.TabIndex = 1;
			this.l_t1.Text = "--";
			this.l_t1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// drawable
			// 
			this.drawable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.drawable.BackColor = System.Drawing.SystemColors.Window;
			this.drawable.Location = new System.Drawing.Point(0, 64);
			this.drawable.Name = "drawable";
			this.drawable.Size = new System.Drawing.Size(400, 300);
			this.drawable.TabIndex = 0;
			// 
			// l_t2
			// 
			this.l_t2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.l_t2.Location = new System.Drawing.Point(224, 40);
			this.l_t2.Name = "l_t2";
			this.l_t2.Size = new System.Drawing.Size(64, 16);
			this.l_t2.TabIndex = 1;
			this.l_t2.Text = "--";
			this.l_t2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// l_t3
			// 
			this.l_t3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.l_t3.Location = new System.Drawing.Point(328, 40);
			this.l_t3.Name = "l_t3";
			this.l_t3.Size = new System.Drawing.Size(64, 16);
			this.l_t3.TabIndex = 1;
			this.l_t3.Text = "--";
			this.l_t3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(200, 40);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(24, 16);
			this.label6.TabIndex = 2;
			this.label6.Text = "T2:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(304, 40);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(24, 16);
			this.label7.TabIndex = 2;
			this.label7.Text = "T3:";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// combo_zoom
			// 
			this.combo_zoom.Items.AddRange(new object[] {
															"x2",
															"x1",
															"x0.5",
															"x0.25"});
			this.combo_zoom.Location = new System.Drawing.Point(272, 8);
			this.combo_zoom.Name = "combo_zoom";
			this.combo_zoom.Size = new System.Drawing.Size(64, 20);
			this.combo_zoom.TabIndex = 4;
			this.combo_zoom.Text = "comboBox1";
			this.combo_zoom.SelectedIndexChanged += new System.EventHandler(this.combo_filter_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(0, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 24);
			this.label2.TabIndex = 2;
			this.label2.Text = "Filter:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(232, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(40, 24);
			this.label3.TabIndex = 2;
			this.label3.Text = "Zoom:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.TabIndex = 0;
			this.label1.Text = "DrawableControl";
			// 
			// TestFormDDraw
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(408, 389);
			this.Controls.Add(this.tabControl1);
			this.Name = "TestFormDDraw";
			this.Text = "TestFormDDraw";
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void drawableControl1_MouseMove(object sender, MouseEventArgs e) {
			if(calced) return;
			drawSceneImage();
			ISprite sp = sprite[2];
			Size sz = sp.Texture.Boundary.Size;
			int zoom =  1-combo_zoom.SelectedIndex;
			Point pt = new Point(e.X, e.Y);
			int n = -zoom;
			if(zoom==1)
				pt.Offset(-sz.Width<<1,-sz.Height<<1);
			else
				pt.Offset(-sz.Width>>n,-sz.Height>>n);
			pt = ((Control)sender).PointToScreen(pt);
			switch(combo_filter.SelectedIndex ){
				case 0: // Normal
					sp.Draw(drawable.Surface, pt, zoom, 0);
					break;
//				case 1: // HalfAlpha
//					sp.DrawEx(drawable.Surface, pt, zoom, currentDrawer, 0);
//					break;
				case -1: // nothing selected
					break;
				default: // Normal
					currentDrawer = combo_filter.SelectedItem as ISurfaceDrawer;
					sp.DrawEx(drawable.Surface, pt, zoom, currentDrawer, 0);
					break;
			}
		}

		private void combo_filter_SelectedIndexChanged(object sender, System.EventArgs e) {
			const string def = "--";
			l_t1.Text = def;
			l_t2.Text = def;
			l_t3.Text = def;
			calced = false;
			drawSceneImage();			
		}

		private void drawSceneImage(){
			if(bg!=null) {
				bg.Draw(drawable.Surface, drawable.PointToScreen(new Point(0)),0,0);
				drawable.Surface.ColorKey = Color.FromArgb(0x00,0x72,0xbc);
			}
		}

//		NFT.DirectDraw描画スピードテスト
//		■ノーマル：
//		x2:	186,199,197
//		x1:	071,073,074
//		xH:	074,074,077
//		xQ: 070,070,070
//		■AlphaBlenderClass(比較用)：
//		x1:	100,123,116
		private void btn_go_Click(object sender, System.EventArgs e) {
			calced = true;
			switch(combo_filter.SelectedIndex ){
				case 0: // Normal
					doTestNormal();
					break;
//				case 1: // HalfAlpha
//					doTestDrawer(currentDrawer);
//					break;
				case -1: // nothing selected
					break;
				default: // Normal
					currentDrawer = combo_filter.SelectedItem as ISurfaceDrawer;
					doTestDrawer(currentDrawer);
					break;
			}
		}

		private void doTestNormal(){
			Label[] labels = new Label[] { l_t1,l_t2,l_t3 };
			int width = drawable.Canvas.Width;
			int height = drawable.Canvas.Height;
			int zs = 1-combo_zoom.SelectedIndex;
			for(int i=0; i<3; i++ ){
				int x=0,y=0;
				DateTime start = DateTime.Now;
				for(int n=0; n<100; n++){
					int m = 255-n;
					buffer.Clear(Rectangle.Empty, Color.FromArgb(m,m,m));
					y = 0;
					while(y<height){
						x = 0;
						while(x<width) {
							sprite[i].Draw(buffer,new Point(x,y),zs,0);
							x+=16;
						}
						y+=16;
					}
					buffer.Draw(drawable.Surface, drawable.Canvas.PointToScreen(new Point(0,0)), 0, 0);
				}
				TimeSpan span = DateTime.Now - start;
				labels[i].Text = string.Format("{0} ns", (int)(span.TotalMilliseconds*10000/(x*y)));
			}
		}

		private void doTestDrawer(ISurfaceDrawer drawer){
			Label[] labels = new Label[] { l_t1,l_t2,l_t3 };
			int width = drawable.Canvas.Width;
			int height = drawable.Canvas.Height;
			int zs = 1-combo_zoom.SelectedIndex;
			for(int i=0; i<3; i++ ){
				int x=0,y=0;
				DateTime start = DateTime.Now;
				for(int n=0; n<100; n++){
					int m = 255-n;
					buffer.Clear(Rectangle.Empty, Color.FromArgb(m,m,m));
					y = 0;
					while(y<height){
						x = 0;
						while(x<width) {
							sprite[i].DrawEx(buffer,new Point(x,y),zs,drawer,0);
							x+=16;
						}
						y+=16;
					}
					buffer.Draw(drawable.Surface, drawable.Canvas.PointToScreen(new Point(0,0)), 0, 0);
				}
				TimeSpan span = DateTime.Now - start;
				labels[i].Text = string.Format("{0} ns", (int)(span.TotalMilliseconds*10000/(x*y)));
			}
		}

		private void doTestFilter(IPixelFilter pf){
			Label[] labels = new Label[] { l_t1,l_t2,l_t3 };
			int width = drawable.Canvas.Width;
			int height = drawable.Canvas.Height;
			int zs = 1-combo_zoom.SelectedIndex;
			for(int i=0; i<3; i++ ){
				int x=0,y=0;
				DateTime start = DateTime.Now;
				for(int n=0; n<100; n++){
					int m = 255-n;
					buffer.Clear(Rectangle.Empty, Color.FromArgb(m,m,m));
					y = 0;
					while(y<height){
						x = 0;
						while(x<width) {
							sprite[i].DrawEx(buffer,new Point(x,y),zs,pf,0);
							x+=16;
						}
						y+=16;
					}
					buffer.Draw(drawable.Surface, drawable.Canvas.PointToScreen(new Point(0,0)), 0, 0);
				}
				TimeSpan span = DateTime.Now - start;
				labels[i].Text = string.Format("{0} ns", (int)(span.TotalMilliseconds*10000/(x*y)));
			}
		}

	}
}
