using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using freetrain.controllers;
using freetrain.contributions.common;
using freetrain.views;
using freetrain.views.map;
using freetrain.world;
using freetrain.world.terrain;
using freetrain.framework;
using freetrain.framework.graphics;
using freetrain.framework.plugin;
using freetrain.util;
using org.kohsuke.directdraw;

namespace freetrain.world.road.dummycar
{
	/// <summary>
	/// Controller that allows the user to
	/// place/remove cars.
	/// </summary>
	public class ControllerForm : ControllerHostForm
	{
		private freetrain.controls.CostBox costBox;
		private System.Windows.Forms.RadioButton buttonRemove;
		private System.Windows.Forms.RadioButton buttonPlace;
		private System.Windows.Forms.ComboBox typeBox;
		private freetrain.controls.IndexSelector colSelector;

		private Bitmap previewBitmap;

		public ControllerForm() {
			InitializeComponent();

			// load list of lands
			typeBox.DataSource = Core.plugins.listContributions(typeof(DummyCarContribution));
			typeBox.DisplayMember="name";
		}

		protected override void Dispose( bool disposing ) {
			if( disposing && components != null)
				components.Dispose();
			base.Dispose( disposing );
			
			if( previewBitmap!=null )
				previewBitmap.Dispose();
		}

		#region Designer generated code
		private System.Windows.Forms.PictureBox preview;
		private System.ComponentModel.IContainer components = null;
		private freetrain.controls.IndexSelector indexSelector;

		private void InitializeComponent()
		{
			this.typeBox = new System.Windows.Forms.ComboBox();
			this.preview = new System.Windows.Forms.PictureBox();
			//this.costBox = new freetrain.controls.CostBox();
			this.buttonRemove = new System.Windows.Forms.RadioButton();
			this.buttonPlace = new System.Windows.Forms.RadioButton();
			this.colSelector = new freetrain.controls.IndexSelector();
			this.SuspendLayout();
			// 
			// typeBox
			// 
			this.typeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.typeBox.Location = new System.Drawing.Point(8, 8);
			this.typeBox.Name = "typeBox";
			this.typeBox.Size = new System.Drawing.Size(112, 20);
			this.typeBox.Sorted = true;
			this.typeBox.TabIndex = 2;
			this.typeBox.SelectedIndexChanged += new System.EventHandler(this.onTypeChanged);
			// 
			// colSelector
			// 
			this.colSelector.count = 10;
			this.colSelector.current = 0;
			this.colSelector.Cursor = System.Windows.Forms.Cursors.Default;
			this.colSelector.dataSource = null;
			this.colSelector.Location = new System.Drawing.Point(8, 32);
			this.colSelector.Name = "colSelector";
			this.colSelector.Size = new System.Drawing.Size(112, 20);
			this.colSelector.TabIndex = 6;
			this.colSelector.indexChanged += new System.EventHandler(this.onColorChanged);
			// 
			// preview
			// 
			this.preview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.preview.Location = new System.Drawing.Point(8, 64);
			this.preview.Name = "preview";
			this.preview.Size = new System.Drawing.Size(112, 80);
			this.preview.TabIndex = 1;
			this.preview.TabStop = false;
			/*
			// 
			// costBox
			// 
			this.costBox.cost = 0;
			this.costBox.label = "Cost:";
			//! this.costBox.label = "費用：";
			this.costBox.Location = new System.Drawing.Point(8, 128);
			this.costBox.Name = "costBox";
			this.costBox.Size = new System.Drawing.Size(112, 32);
			this.costBox.TabIndex = 10;
			*/
			// 
			// buttonRemove
			// 
			this.buttonRemove.Appearance = System.Windows.Forms.Appearance.Button;
			this.buttonRemove.Location = new System.Drawing.Point(64, 160);
			this.buttonRemove.Name = "buttonRemove";
			this.buttonRemove.Size = new System.Drawing.Size(56, 24);
			this.buttonRemove.TabIndex = 9;
			this.buttonRemove.Text = "Remove";
			//! this.buttonRemove.Text = "撤去";
			this.buttonRemove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.buttonRemove.CheckedChanged += new System.EventHandler(this.onColorChanged);
			// 
			// buttonPlace
			// 
			this.buttonPlace.Appearance = System.Windows.Forms.Appearance.Button;
			this.buttonPlace.Checked = true;
			this.buttonPlace.Location = new System.Drawing.Point(8, 160);
			this.buttonPlace.Name = "buttonPlace";
			this.buttonPlace.Size = new System.Drawing.Size(56, 24);
			this.buttonPlace.TabIndex = 8;
			this.buttonPlace.TabStop = true;
			this.buttonPlace.Text = "Place";
			//! this.buttonPlace.Text = "設置";
			this.buttonPlace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.buttonPlace.CheckedChanged += new System.EventHandler(this.onColorChanged);
			// 
			// ControllerForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(128, 190);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  /*this.costBox,*/
																		  this.buttonRemove,
																		  this.buttonPlace,
																		  this.typeBox,
																		  this.colSelector,
																		  this.preview});
			this.Name = "ControllerForm";
			this.Text = "Automobile";
			//! this.Text = "自動車";
			this.ResumeLayout(false);

		}
		#endregion
		
		public bool isPlacing {
			get {
				return buttonPlace.Checked;
			}
		}

		private int currentColor 
		{
			get 
			{
				return colSelector.current;
			}
		}

		private void onTypeChanged(object sender, System.EventArgs e) 
		{
			DummyCarContribution builder = (DummyCarContribution)typeBox.SelectedItem;
			int colors = builder.colorVariations;
			if( this.colSelector.current > colors ) this.colSelector.current=colors;
			this.colSelector.count=colors;
			builder.currentColor=colors;
			updatePreview(builder);
		}

		protected virtual void onColorChanged(object sender, System.EventArgs e) 
		{
			DummyCarContribution builder = (DummyCarContribution)typeBox.SelectedItem;
			builder.currentColor = this.currentColor;
			updatePreview(builder);
		}

		/// <summary>
		/// Called when a selection of the structure has changed.
		/// </summary>
		protected virtual void updatePreview(DummyCarContribution builder) 
		{
			//DummyCarContribution builder = (DummyCarContribution)typeBox.SelectedItem;
			using( PreviewDrawer drawer = new PreviewDrawer( preview.Size, new Size(10,1), 0 ) ) {
				
				drawer.draw( builder.getSprites(), 5,0 );

				if( previewBitmap!=null )	previewBitmap.Dispose();
				preview.Image = previewBitmap = drawer.createBitmap();
			}
			if(isPlacing)
				currentController = builder.createBuilder(this.siteImpl);
			else
				currentController = builder.createRemover(this.siteImpl);
		}
	}
}
