using System;
using System.Drawing;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using freetrain.controls;
using freetrain.controllers;
using freetrain.contributions;
using freetrain.contributions.common;
using freetrain.contributions.population;
using freetrain.contributions.structs;
using freetrain.contributions.land;
using freetrain.framework;
using freetrain.framework.graphics;
using freetrain.framework.plugin;
using freetrain.views;
using freetrain.views.map;
using freetrain.world;
using freetrain.world.structs;

namespace freetrain.framework.plugin.generic
{
	/// <summary>
	/// MultiSelectorController の概要の説明です。
	/// </summary>
	public class MultiSelectorController : AbstractControllerImpl, MapOverlay, LocationDisambiguator
	{
		#region generated by form designer

		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.PictureBox previewBox;
		private freetrain.controls.IndexSelector selectorDir;
		private freetrain.controls.IndexSelector selectorColor;
		private System.Windows.Forms.NumericUpDown numHeight;
		private System.Windows.Forms.Button btnDetail;
		private System.Windows.Forms.ComboBox typeBox;
		private freetrain.controls.CostBox costBox;
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label labelDir;
		private System.Windows.Forms.Label labelHeight;
		private System.Windows.Forms.Label labelInfo;
		private freetrain.controls.IndexSelector selectorDesign;
		private System.Windows.Forms.CheckBox cbRandColor;
		private System.Windows.Forms.CheckBox cbRandDesign;
		private freetrain.controls.IndexSelector selectorCol2;
		private System.Windows.Forms.CheckBox cbRandCol2;
		private System.Windows.Forms.ListBox typeList;
		private System.Windows.Forms.GroupBox groupColor;
		private System.Windows.Forms.GroupBox groupDesign;
		private System.Windows.Forms.GroupBox groupCol2;

		#endregion		

		protected static readonly Location UNPLACED = world.Location.UNPLACED;
		private Bitmap previewBitmap;
		private bool bShowDetail;
		private Hashtable typeMap = new Hashtable();
		private ArrayList designMap;
		//private Hashtable subMap;
		private string[] subNames;
		private ArrayList currentList;
		private GenericStructureContribution current;
		private Random rnd = new Random();
		private GenericStructureContribution dummy;

		#region Singleton instance management
		private static MultiSelectorController theInstance;
		private KeyEventHandler keyHandler;

		/// <summary>
		/// Creates a new controller window, or active the existing one.
		/// </summary>
		public static void create() 
		{
			if(theInstance==null)
				theInstance = new MultiSelectorController();
			theInstance.Show();
			theInstance.Activate();
		}


		protected override void OnClosing(System.ComponentModel.CancelEventArgs e) 
		{
			base.OnClosing(e);
			theInstance = null;
		}
		#endregion

		public MultiSelectorController()
		{
			CategoryTreeWnd.ShowForm();
			CategoryTreeWnd.OnNodeSelected+=new NodeSelectedEvent(OnNodeSelected);
			dummy = (GenericStructureContribution)PluginManager.theInstance.getContribution("null-contrib");
				//
				// Windows フォーム デザイナ サポートに必要です。
				//
			InitializeComponent();
			World.world.viewOptions.OnViewOptionChanged+=new OptionChangedHandler(this.redrawPreview);
			OnNodeSelected(CategoryTreeWnd.getSelectedCategory(),0);
			if(typeBox.Items.Count!=0)
				typeBox_SelectedIndexChanged(this,null);
			//SetAllCategories();
			bShowDetail = false;
			btnDetail_Click(this,null);
		}

		// callback from StructureCategoryWindow
		internal void OnNodeSelected(StructCategory cat, int option)
		{
			typeBox.Items.Clear();
			typeList.Items.Clear();
			typeMap.Clear();
			SetCategoryEntries(cat,typeMap);
				
			if(typeBox.Items.Count>0)
			{
				typeBox.SelectedIndex = 0;
				typeList.SelectedIndex = 0;
			}
			else
				current = dummy;
		}

		public override LocationDisambiguator disambiguator { get { return this; } }

		public bool isSelectable( Location loc ) {
			return GroundDisambiguator.theInstance.isSelectable(loc);
		}

		private Location baseLoc = UNPLACED;
		private Location anchor = UNPLACED;
		private Direction mouseDir;
		public override void onMouseMove(MapViewWindow view, Location loc, Point ab ) 
		{
			if(anchor==UNPLACED)
			{
				if(baseLoc!=loc) 
				{
					// update the screen
					baseLoc = loc;
					// TODO: we need to correctly update the screen
					World.world.onAllVoxelUpdated();
				}
			}
			else
			{
				world.Location l = loc.align4To(anchor);
				mouseDir = anchor.getDirectionTo(l);			
				World.world.onVoxelUpdated(anchor);
			}
		}

		public override void onClick(MapViewWindow view, Location loc, Point ab ) 
		{			
			if( !canBeBuilt(loc) )
                MainWindow.showError("Can not build");
                //! MainWindow.showError("設置できません");
			else 
			{
				if( current.current is VarHeightBuildingContribution ) 
				{
					VarHeightBuildingContribution vhContrib = (VarHeightBuildingContribution)current.current;
					CompletionHandler handler = new CompletionHandler(vhContrib,loc,(int)numHeight.Value,true);
					new ConstructionSite( loc, new EventHandler(handler.handle),
						new Distance( vhContrib.size, (int)numHeight.Value ) );
				}
				else
				{
					CommercialStructureContribution csContrib = (CommercialStructureContribution)current.current;
					if( csContrib.size.volume>0 ) // eliminate dummy contribution
					{
						CompletionHandler handler = new CompletionHandler(csContrib,loc, current.maxHeight ,true);
						new ConstructionSite( loc, new EventHandler(handler.handle), csContrib.size );
					}
				}
			}
			randomize();
		}

		/// <summary>
		/// Returns true iff this structure can be built at the specified location.
		/// </summary>
		public bool canBeBuilt( Location baseLoc ) 
		{
			int height;
			SIZE size;
			if(  current.current is VarHeightBuildingContribution )
			{
				size = ((VarHeightBuildingContribution)current.current).size;
				height = (int)numHeight.Value;
			}
			else
			{
				Distance d =((CommercialStructureContribution)current.current).size;
				size = new SIZE(d.x,d.y);
				height = current.maxHeight;
			}
			for( int z=0; z<height; z++ )
				for( int y=0; y<size.y; y++ )
					for( int x=0; x<size.x; x++ )
						if( World.world[ baseLoc.x+x, baseLoc.y+y, baseLoc.z+z ]!=null )
							return false;

			return true;
		}

//		protected override void OnLoad(System.EventArgs e) 
//		{
//			base.OnLoad(e);
//			updateAlphaSprites();
//		}

		#region draw as MapOverlay
		private AlphaBlendSpriteSet alphaSprites;

		/// <summary>
		/// Re-builds an alpha-blending preview.
		/// </summary>
		protected void updateAlphaSprites() 
		{
			if(alphaSprites!=null)
				alphaSprites.Dispose();

			// builds a new alpha blended preview
			alphaSprites = createAlphaSprites();
		}

		/// <summary>
		/// Implemented by the derived class to provide a sprite set used
		/// to draw a preview of this structure on MapView.
		/// </summary>
		protected AlphaBlendSpriteSet createAlphaSprites()
		{
			IEntityBuilder contrib = current.current;
			if( contrib is LandBuilderContribution )
				return null;
			Sprite[,,] temp;
			if( contrib is VarHeightBuildingContribution ) 
			{
				VarHeightBuildingContribution vhcontrib = (VarHeightBuildingContribution)contrib;
				Size sz = vhcontrib.size;
				int pHeight = 3;
				temp = new Sprite[sz.Width,sz.Height,pHeight];
				for( int z=0; z<pHeight; z++ )
					for( int y=0; y<sz.Height; y++ )
						for( int x=0; x<sz.Width; x++ ) 
							temp[x,y,z] = vhcontrib.getSprites(x,y,z,pHeight)[0];
			}
			else
				temp = ((CommercialStructureContribution)contrib).sprites;
			return new AlphaBlendSpriteSet( temp );
		}


		public void drawBefore( QuarterViewDrawer view, DrawContextEx surface ) {}

		public void drawVoxel( QuarterViewDrawer view, DrawContextEx canvas, Location loc, Point pt ) 
		{
			if( Cube.createExclusive( baseLoc, alphaSprites.size ).contains(loc) )
				alphaSprites.getSprite( loc-baseLoc ).drawAlpha( canvas.surface, pt );
		}

		public void drawAfter( QuarterViewDrawer view, DrawContextEx surface ) {}
		#endregion

		protected void redrawPreview() 
		{
			if( current.current is VarHeightBuildingContribution )
			{
				using( PreviewDrawer drawer = ((VarHeightBuildingContribution)current.current).createPreview(previewBox.Size,(int)numHeight.Value) ) 
				{
					if( previewBitmap!=null )	previewBitmap.Dispose();
					previewBox.Image = previewBitmap = drawer.createBitmap();
				}
			}
			else 
			{
				using( PreviewDrawer drawer = current.current.createPreview(previewBox.Size) ) 
				{

					if( previewBitmap!=null )	previewBitmap.Dispose();
					previewBox.Image = previewBitmap = drawer.createBitmap();
				}
			}
			updateAlphaSprites();
			if( bShowDetail ) 
				 labelInfo.Text = getDetailText();

			//描画異常がおこるため
			Invalidate();				
			Update();
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			World.world.viewOptions.OnViewOptionChanged-=new OptionChangedHandler(this.redrawPreview);
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );

			if( previewBitmap!=null )
				previewBitmap.Dispose();
			
			if(alphaSprites!=null)
				alphaSprites.Dispose();
		}


		protected bool isPlacing { get { return true; } }

		#region Windows Form Designer generated code
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
            this.nameLabel = new System.Windows.Forms.Label();
            this.previewBox = new System.Windows.Forms.PictureBox();
            this.selectorDesign = new freetrain.controls.IndexSelector();
            this.selectorDir = new freetrain.controls.IndexSelector();
            this.labelDir = new System.Windows.Forms.Label();
            this.selectorColor = new freetrain.controls.IndexSelector();
            this.typeBox = new System.Windows.Forms.ComboBox();
            this.numHeight = new System.Windows.Forms.NumericUpDown();
            this.labelHeight = new System.Windows.Forms.Label();
            this.btnDetail = new System.Windows.Forms.Button();
            this.labelInfo = new System.Windows.Forms.Label();
            this.costBox = new freetrain.controls.CostBox();
            this.groupColor = new System.Windows.Forms.GroupBox();
            this.cbRandColor = new System.Windows.Forms.CheckBox();
            this.groupDesign = new System.Windows.Forms.GroupBox();
            this.cbRandDesign = new System.Windows.Forms.CheckBox();
            this.groupCol2 = new System.Windows.Forms.GroupBox();
            this.selectorCol2 = new freetrain.controls.IndexSelector();
            this.cbRandCol2 = new System.Windows.Forms.CheckBox();
            this.typeList = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.previewBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).BeginInit();
            this.groupColor.SuspendLayout();
            this.groupDesign.SuspendLayout();
            this.groupCol2.SuspendLayout();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            this.nameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nameLabel.BackColor = System.Drawing.Color.White;
            this.nameLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.nameLabel.Location = new System.Drawing.Point(4, 32);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(152, 16);
            this.nameLabel.TabIndex = 2;
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // previewBox
            // 
            this.previewBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.previewBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.previewBox.Location = new System.Drawing.Point(0, 128);
            this.previewBox.Name = "previewBox";
            this.previewBox.Size = new System.Drawing.Size(184, 224);
            this.previewBox.TabIndex = 3;
            this.previewBox.TabStop = false;
            this.previewBox.Click += new System.EventHandler(this.previewBox_Click);
            // 
            // selectorDesign
            // 
            this.selectorDesign.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.selectorDesign.count = 10;
            this.selectorDesign.current = 0;
            this.selectorDesign.dataSource = null;
            this.selectorDesign.Location = new System.Drawing.Point(24, 16);
            this.selectorDesign.Name = "selectorDesign";
            this.selectorDesign.Size = new System.Drawing.Size(112, 14);
            this.selectorDesign.TabIndex = 1;
            this.selectorDesign.indexChanged += new System.EventHandler(this.selectorDesign_indexChanged);
            // 
            // selectorDir
            // 
            this.selectorDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectorDir.count = 10;
            this.selectorDir.current = 0;
            this.selectorDir.dataSource = null;
            this.selectorDir.Location = new System.Drawing.Point(253, 152);
            this.selectorDir.Name = "selectorDir";
            this.selectorDir.Size = new System.Drawing.Size(88, 16);
            this.selectorDir.TabIndex = 5;
            this.selectorDir.indexChanged += new System.EventHandler(this.selectorDir_indexChanged);
            // 
            // labelDir
            // 
            this.labelDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDir.Location = new System.Drawing.Point(192, 152);
            this.labelDir.Name = "labelDir";
            this.labelDir.Size = new System.Drawing.Size(59, 16);
            this.labelDir.TabIndex = 4;
            this.labelDir.Text = "Direction:";
            //! this.labelDir.Text = "方向：";
            this.labelDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // selectorColor
            // 
            this.selectorColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.selectorColor.count = 10;
            this.selectorColor.current = 0;
            this.selectorColor.dataSource = null;
            this.selectorColor.Location = new System.Drawing.Point(24, 16);
            this.selectorColor.Name = "selectorColor";
            this.selectorColor.Size = new System.Drawing.Size(112, 14);
            this.selectorColor.TabIndex = 1;
            this.selectorColor.indexChanged += new System.EventHandler(this.selectorColor_indexChanged);
            // 
            // typeBox
            // 
            this.typeBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.typeBox.Location = new System.Drawing.Point(192, 0);
            this.typeBox.Name = "typeBox";
            this.typeBox.Size = new System.Drawing.Size(160, 20);
            this.typeBox.TabIndex = 0;
            this.typeBox.SelectedIndexChanged += new System.EventHandler(this.typeBox_SelectedIndexChanged);
            // 
            // numHeight
            // 
            this.numHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numHeight.Location = new System.Drawing.Point(272, 172);
            this.numHeight.Name = "numHeight";
            this.numHeight.Size = new System.Drawing.Size(56, 19);
            this.numHeight.TabIndex = 7;
            this.numHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numHeight.ValueChanged += new System.EventHandler(this.numHeight_ValueChanged);
            // 
            // labelHeight
            // 
            this.labelHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelHeight.Location = new System.Drawing.Point(232, 173);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(40, 16);
            this.labelHeight.TabIndex = 6;
            this.labelHeight.Text = "Height:";
            //! this.labelHeight.Text = "高さ：";
            this.labelHeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnDetail
            // 
            this.btnDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDetail.Font = new System.Drawing.Font("Wingdings 3", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnDetail.Location = new System.Drawing.Point(336, 200);
            this.btnDetail.Name = "btnDetail";
            this.btnDetail.Size = new System.Drawing.Size(20, 20);
            this.btnDetail.TabIndex = 9;
            this.btnDetail.Text = "q";
            this.btnDetail.Click += new System.EventHandler(this.btnDetail_Click);
            // 
            // labelInfo
            // 
            this.labelInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelInfo.Location = new System.Drawing.Point(192, 232);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(160, 120);
            this.labelInfo.TabIndex = 9;
            this.labelInfo.UseMnemonic = false;
            // 
            // costBox
            // 
            this.costBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.costBox.cost = 0;
            this.costBox.label = "Cost:";
            //! this.costBox.label = "費用：";
            this.costBox.Location = new System.Drawing.Point(192, 192);
            this.costBox.Name = "costBox";
            this.costBox.Size = new System.Drawing.Size(136, 32);
            this.costBox.TabIndex = 8;
            // 
            // groupColor
            // 
            this.groupColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupColor.Controls.Add(this.selectorColor);
            this.groupColor.Controls.Add(this.cbRandColor);
            this.groupColor.Location = new System.Drawing.Point(192, 76);
            this.groupColor.Name = "groupColor";
            this.groupColor.Size = new System.Drawing.Size(160, 34);
            this.groupColor.TabIndex = 2;
            this.groupColor.TabStop = false;
            this.groupColor.Text = "Color 1:";
            //! this.groupColor.Text = "色：";
            // 
            // cbRandColor
            // 
            this.cbRandColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbRandColor.BackColor = System.Drawing.SystemColors.Control;
            this.cbRandColor.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbRandColor.Location = new System.Drawing.Point(80, 0);
            this.cbRandColor.Name = "cbRandColor";
            this.cbRandColor.Size = new System.Drawing.Size(64, 16);
            this.cbRandColor.TabIndex = 0;
            this.cbRandColor.Text = "Random";
            //! this.cbRandColor.Text = "ランダム";
            this.cbRandColor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbRandColor.UseVisualStyleBackColor = false;
            // 
            // groupDesign
            // 
            this.groupDesign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupDesign.Controls.Add(this.cbRandDesign);
            this.groupDesign.Controls.Add(this.selectorDesign);
            this.groupDesign.Controls.Add(this.nameLabel);
            this.groupDesign.Location = new System.Drawing.Point(192, 24);
            this.groupDesign.Name = "groupDesign";
            this.groupDesign.Size = new System.Drawing.Size(160, 52);
            this.groupDesign.TabIndex = 1;
            this.groupDesign.TabStop = false;
            this.groupDesign.Text = "Design:";
            //! this.groupDesign.Text = "デザイン：";
            // 
            // cbRandDesign
            // 
            this.cbRandDesign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbRandDesign.BackColor = System.Drawing.SystemColors.Control;
            this.cbRandDesign.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbRandDesign.Location = new System.Drawing.Point(80, -1);
            this.cbRandDesign.Name = "cbRandDesign";
            this.cbRandDesign.Size = new System.Drawing.Size(64, 16);
            this.cbRandDesign.TabIndex = 0;
            this.cbRandDesign.Text = "Random";
            //! this.cbRandDesign.Text = "ランダム";
            this.cbRandDesign.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbRandDesign.UseVisualStyleBackColor = false;
            // 
            // groupCol2
            // 
            this.groupCol2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupCol2.Controls.Add(this.selectorCol2);
            this.groupCol2.Controls.Add(this.cbRandCol2);
            this.groupCol2.Location = new System.Drawing.Point(192, 112);
            this.groupCol2.Name = "groupCol2";
            this.groupCol2.Size = new System.Drawing.Size(160, 34);
            this.groupCol2.TabIndex = 3;
            this.groupCol2.TabStop = false;
            this.groupCol2.Text = "Color 2:";
            //! this.groupCol2.Text = "色2：";
            // 
            // selectorCol2
            // 
            this.selectorCol2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.selectorCol2.count = 10;
            this.selectorCol2.current = 0;
            this.selectorCol2.dataSource = null;
            this.selectorCol2.Location = new System.Drawing.Point(24, 16);
            this.selectorCol2.Name = "selectorCol2";
            this.selectorCol2.Size = new System.Drawing.Size(112, 14);
            this.selectorCol2.TabIndex = 1;
            this.selectorCol2.indexChanged += new System.EventHandler(this.selectorCol2_indexChanged);
            // 
            // cbRandCol2
            // 
            this.cbRandCol2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbRandCol2.BackColor = System.Drawing.SystemColors.Control;
            this.cbRandCol2.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbRandCol2.Location = new System.Drawing.Point(80, 0);
            this.cbRandCol2.Name = "cbRandCol2";
            this.cbRandCol2.Size = new System.Drawing.Size(64, 16);
            this.cbRandCol2.TabIndex = 0;
            this.cbRandCol2.Text = "Random";
            //! this.cbRandCol2.Text = "ランダム";
            this.cbRandCol2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbRandCol2.UseVisualStyleBackColor = false;
            // 
            // typeList
            // 
            this.typeList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.typeList.ItemHeight = 12;
            this.typeList.Location = new System.Drawing.Point(0, 0);
            this.typeList.Name = "typeList";
            this.typeList.Size = new System.Drawing.Size(184, 124);
            this.typeList.TabIndex = 10;
            this.typeList.SelectedIndexChanged += new System.EventHandler(this.typeList_SelectedIndexChanged);
            // 
            // MultiSelectorController
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
            this.ClientSize = new System.Drawing.Size(362, 355);
            this.Controls.Add(this.costBox);
            this.Controls.Add(this.numHeight);
            this.Controls.Add(this.groupDesign);
            this.Controls.Add(this.selectorDir);
            this.Controls.Add(this.labelDir);
            this.Controls.Add(this.btnDetail);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.groupColor);
            this.Controls.Add(this.groupCol2);
            this.Controls.Add(this.typeList);
            this.Controls.Add(this.typeBox);
            this.Controls.Add(this.previewBox);
            this.Controls.Add(this.labelHeight);
            this.Name = "MultiSelectorController";
            ((System.ComponentModel.ISupportInitialize)(this.previewBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).EndInit();
            this.groupColor.ResumeLayout(false);
            this.groupDesign.ResumeLayout(false);
            this.groupCol2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region event handlers
		private void btnDetail_Click(object sender, System.EventArgs e)
		{
			bShowDetail = !bShowDetail;
			int width = this.ClientSize.Width;
			if( bShowDetail ) 
			{
				this.ClientSize = new Size(width, 355);
				btnDetail.Text = "p";
			}
			else
			{
				this.ClientSize = new Size(width, 227);
				btnDetail.Text = "q";
			}
			redrawPreview();
		}

		private void typeList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			typeBox.SelectedIndex = typeList.SelectedIndex;		
			UpdateContribution();
			redrawPreview();
		}

		private void typeBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			typeList.SelectedIndex = typeBox.SelectedIndex;		
			UpdateContribution();
			redrawPreview();
		}

		private void selectorDesign_indexChanged(object sender, System.EventArgs e)
		{
			UpdateDesign();
			redrawPreview();
		}

		private void selectorColor_indexChanged(object sender, System.EventArgs e)
		{
			UpdateColor();
			redrawPreview();
		}

		private void selectorCol2_indexChanged(object sender, System.EventArgs e)
		{
			UpdateCol2();
			redrawPreview();
		}

		private void selectorDir_indexChanged(object sender, System.EventArgs e)
		{
			UpdateDir();
			redrawPreview();
		}

		private void numHeight_ValueChanged(object sender, System.EventArgs e)
		{
			numHeight.Refresh();
			UpdateHeight();
			redrawPreview();
		}

		private void previewBox_Click(object sender, System.EventArgs e)
		{
			if(cbRandDesign.Checked)
			{
				selectorDesign.current = rnd.Next(selectorDesign.count);
				UpdateDesign();
			}
			if(cbRandColor.Checked)
			{
				selectorColor.current = rnd.Next(selectorColor.count);
				UpdateColor();
			}
			if(cbRandCol2.Checked)
			{
				selectorCol2.current = rnd.Next(selectorCol2.count);
				UpdateCol2();
			}
			redrawPreview();		
		}
		#endregion
		
		#region set current data to selector controls
		protected void SetCategoryEntries(StructCategory cat, Hashtable worktable)
		{
			int n = cat.Entries.Count;
			for( int i=0; i<n; i++ )
			{
				GenericStructureContribution contrib 
					= (GenericStructureContribution)PluginManager.theInstance.getContribution((string)cat.Entries[i]);
				string key = contrib.name;
				if( !worktable.ContainsKey(key) ) 
				{
					typeBox.Items.Add(contrib);
					typeList.Items.Add(contrib);
					worktable.Add(key,new ArrayList());
				}
				designMap = (ArrayList)worktable[key];
				designMap.Add(contrib);
			}
			n = cat.Subcategories.Count;
			for(int i=0; i<n; i++)
				SetCategoryEntries(cat.Subcategories[i],typeMap);
		}

		protected void SetAllCategories()
		{
			GenericStructureContribution[] arrCont =(GenericStructureContribution[])
				PluginManager.theInstance.listContributions(typeof(GenericStructureContribution));
			int n = arrCont.Length;
			typeMap.Clear();
			for( int i=0; i<n; i++ )
			{
				GenericStructureContribution contrib = arrCont[i];
				string key = contrib.name;
				if( !typeMap.ContainsKey(key) ) 
				{
					typeBox.Items.Add(contrib);
					typeList.Items.Add(contrib);
					typeMap.Add(key,new Hashtable());
				}
				designMap = (ArrayList)typeMap[key];
				designMap.Add(contrib);
			}
			typeBox.SelectedIndex = 0;
			typeList.SelectedIndex = 0;
		}
		
		protected void updatePrice() 
		{
			int p = 0;
			if(  current.current is VarHeightBuildingContribution )
			{
				VarHeightBuildingContribution vhb = (VarHeightBuildingContribution)current.current;
				p = vhb.price * (int)numHeight.Value;
			}
			else
				p = current.unitPrice;
			costBox.cost = p;
		}

		protected String getDetailText() 
		{
			string buf="";
			
			int n = current.categories.Count;
			buf+=current.categories[0].name;
			for(int i=1; i<n; i++)
				buf+=";"+current.categories[i].name;
			buf+="\nMax population:";
            //! buf += "\n最大人口:";
			if( current.population==null )
				buf+="N/A";
			else
			{
				if(  current.current is VarHeightBuildingContribution ) 
					buf += current.population.residents*(int)numHeight.Value;
				else
					buf += current.population.residents;
			}

			return buf;
		}

		private void UpdateContribution()
		{
			string key = typeBox.SelectedItem.ToString();
			designMap = (ArrayList)typeMap[key];
			subNames = new String[designMap.Count];
			IEnumerator ie = designMap.GetEnumerator();
			int i=0;
			while(ie.MoveNext())
				subNames[i++]=((GenericStructureContribution)ie.Current).design;

			selectorDesign.count = designMap.Count;
			bool b =( selectorDesign.count > 1 );
			groupDesign.Enabled = b;

			UpdateDesign();
		}

		private void UpdateDesign()
		{
			int n = selectorDesign.current;

			string subkey = subNames[n];
			nameLabel.Text = subkey;
			currentList = new ArrayList();//(ArrayList)designMap[n];
			currentList.Add(designMap[n]);

			//Update Color, Col2, Dir, Height
			bool b;

			selectorCol2.count = 1;//currentList.Count;
			b =( selectorCol2.count > 1 );
			groupCol2.Enabled = b;
			UpdateCol2();

			selectorColor.count = current.colorVariations;
			b =( selectorColor.count > 1 );
			groupColor.Enabled = b;
			UpdateColor();

			selectorDir.count = current.directionVariations;
			b =( selectorDir.count > 1 );
			selectorDir.Enabled = b;
			labelDir.Enabled = b;			
			UpdateDir();

			b = ( current.current is VarHeightBuildingContribution );
			labelHeight.Enabled = b;
			numHeight.Enabled = b;
			if( b )
			{
				numHeight.Minimum = ((VarHeightBuildingContribution)current.current).minHeight;
				numHeight.Maximum = ((VarHeightBuildingContribution)current.current).maxHeight;
			}
			UpdateHeight();

		}

		private void UpdateColor()
		{
			current.colorIndex = selectorColor.current;
		}

		private void UpdateCol2()
		{
			current = (GenericStructureContribution)currentList[selectorCol2.current];
		}

		private void UpdateDir()
		{
			current.dirIndex = selectorDir.current;
		}

		private void UpdateHeight()
		{
			redrawPreview();
			updatePrice();
		}

		#endregion

		internal void randomize()
		{
			previewBox_Click(this,null);
		}

		private void safeSetSelector(IndexSelector selector, int current) 
		{
			if(selector.count <= current ) selector.current = selector.count-1;
			else selector.current = current;
		}

//		public void OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
//		{
//			switch(e.KeyCode)
//			{
//				case Keys.Space:
//					randomize();
//					break;
//				default:
//					Debug.WriteLine("key catch:"+e.KeyCode.ToString());
//					break;
//			}
//		}

		[Serializable]
		private class CompletionHandler 
		{
			internal CompletionHandler( StructureContribution contribution,
				Location loc, int height, bool initiallyOwned ) 
			{
				
				this.contribution = contribution;
				this.loc = loc;
				this.height = height;
				this.owned = initiallyOwned;
			}
			private readonly StructureContribution contribution;
			private readonly Location loc;
			private readonly int height;
			private readonly bool owned;
			public void handle( object sender, EventArgs args ) 
			{
				Debug.WriteLine("called handler");
				if( contribution is VarHeightBuildingContribution )
					((VarHeightBuildingContribution)contribution).create(loc,height,owned);
				else
					((CommercialStructureContribution)contribution).create(loc,owned);
			}
		}

	
	}
}
