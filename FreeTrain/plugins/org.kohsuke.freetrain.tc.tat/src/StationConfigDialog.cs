using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace freetrain.world.rail.tattc
{
	/// <summary>
	/// StationConfigDialog の概要の説明です。
	/// </summary>
	internal class StationConfigDialog : System.Windows.Forms.Form
	{
		internal StationConfigDialog( StationHandler _currentHandler ) {
			InitializeComponent();

			if( _currentHandler==null )
				_currentHandler = StationHandler.defaultHandler;

			if( _currentHandler is PassStationHandler )
				radioPass.Checked = true;
			if( _currentHandler is FixedDurationStationHandler) {
				FixedDurationStationHandler fdsh = (FixedDurationStationHandler)_currentHandler; 
				durationBox.Value = fdsh.duration.totalMinutes;
				checkTurn1.Checked = fdsh.turnAround;
				radioFixedDuration.Checked = true;
			}
			if( _currentHandler is OnceADayStationHandler ) {
				OnceADayStationHandler oash = (OnceADayStationHandler)_currentHandler;
				hourBox.Value = oash.minutes/60;
				minBox.Value = oash.minutes%60;
				checkTurn2.Checked = oash.turnAround;
				radioSimple.Checked = true;
			}
			if( _currentHandler is AdvancedStationHandler ) {
				radioAdvanced.Checked = true;
			}

			this.currentHandler = _currentHandler;
		}

		internal StationHandler currentHandler;

		protected override void Dispose( bool disposing ) {
			if( disposing && components != null)
				components.Dispose();
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		private System.Windows.Forms.CheckBox checkTurn2;
		private System.Windows.Forms.CheckBox checkTurn1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown durationBox;
		private System.Windows.Forms.RadioButton radioFixedDuration;


		private System.Windows.Forms.RadioButton radioSimple;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.RadioButton radioAdvanced;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button buttonAdvanced;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.NumericUpDown hourBox;
		private System.Windows.Forms.NumericUpDown minBox;
		private System.Windows.Forms.RadioButton radioPass;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.radioFixedDuration = new System.Windows.Forms.RadioButton();
			this.radioSimple = new System.Windows.Forms.RadioButton();
			this.hourBox = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.minBox = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.radioAdvanced = new System.Windows.Forms.RadioButton();
			this.buttonAdvanced = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.buttonOk = new System.Windows.Forms.Button();
			this.radioPass = new System.Windows.Forms.RadioButton();
			this.checkTurn2 = new System.Windows.Forms.CheckBox();
			this.checkTurn1 = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.durationBox = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.hourBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.minBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.durationBox)).BeginInit();
			this.SuspendLayout();
			// 
			// radioFixedDuration
			// 
			this.radioFixedDuration.Location = new System.Drawing.Point(8, 32);
			this.radioFixedDuration.Name = "radioFixedDuration";
			this.radioFixedDuration.Size = new System.Drawing.Size(128, 16);
			this.radioFixedDuration.TabIndex = 2;
			this.radioFixedDuration.Text = "一定時間停車(&N)";
			this.radioFixedDuration.CheckedChanged += new System.EventHandler(this.onRadioChanged);
			// 
			// radioSimple
			// 
			this.radioSimple.Location = new System.Drawing.Point(8, 104);
			this.radioSimple.Name = "radioSimple";
			this.radioSimple.Size = new System.Drawing.Size(128, 16);
			this.radioSimple.TabIndex = 3;
			this.radioSimple.Text = "指定時刻発車(&S)";
			this.radioSimple.CheckedChanged += new System.EventHandler(this.onRadioChanged);
			// 
			// hourBox
			// 
			this.hourBox.Location = new System.Drawing.Point(32, 128);
			this.hourBox.Maximum = new System.Decimal(new int[] {
																	24,
																	0,
																	0,
																	0});
			this.hourBox.Name = "hourBox";
			this.hourBox.Size = new System.Drawing.Size(48, 19);
			this.hourBox.TabIndex = 4;
			this.hourBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.hourBox.ValueChanged += new System.EventHandler(this.onTimeChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(80, 128);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(24, 16);
			this.label1.TabIndex = 5;
			this.label1.Text = "時";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// minBox
			// 
			this.minBox.Increment = new System.Decimal(new int[] {
																	 10,
																	 0,
																	 0,
																	 0});
			this.minBox.Location = new System.Drawing.Point(104, 128);
			this.minBox.Maximum = new System.Decimal(new int[] {
																   60,
																   0,
																   0,
																   0});
			this.minBox.Name = "minBox";
			this.minBox.Size = new System.Drawing.Size(48, 19);
			this.minBox.TabIndex = 6;
			this.minBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.minBox.ValueChanged += new System.EventHandler(this.onTimeChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(152, 128);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(24, 16);
			this.label2.TabIndex = 7;
			this.label2.Text = "分";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// radioAdvanced
			// 
			this.radioAdvanced.Location = new System.Drawing.Point(8, 176);
			this.radioAdvanced.Name = "radioAdvanced";
			this.radioAdvanced.Size = new System.Drawing.Size(24, 16);
			this.radioAdvanced.TabIndex = 8;
			this.radioAdvanced.CheckedChanged += new System.EventHandler(this.onRadioChanged);
			// 
			// buttonAdvanced
			// 
			this.buttonAdvanced.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonAdvanced.Location = new System.Drawing.Point(32, 176);
			this.buttonAdvanced.Name = "buttonAdvanced";
			this.buttonAdvanced.Size = new System.Drawing.Size(120, 24);
			this.buttonAdvanced.TabIndex = 9;
			this.buttonAdvanced.Text = "高度な設定(&A)...";
			this.buttonAdvanced.Click += new System.EventHandler(this.buttonAdvanced_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Location = new System.Drawing.Point(184, -8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(2, 280);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			// 
			// buttonOk
			// 
			this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonOk.Location = new System.Drawing.Point(192, 8);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(72, 24);
			this.buttonOk.TabIndex = 10;
			this.buttonOk.Text = "&OK";
			this.buttonOk.Click += new System.EventHandler(this.onOK);
			// 
			// radioPass
			// 
			this.radioPass.Location = new System.Drawing.Point(8, 8);
			this.radioPass.Name = "radioPass";
			this.radioPass.Size = new System.Drawing.Size(128, 16);
			this.radioPass.TabIndex = 1;
			this.radioPass.Text = "通過(&P)";
			this.radioPass.CheckedChanged += new System.EventHandler(this.onRadioChanged);
			// 
			// checkTurn2
			// 
			this.checkTurn2.Location = new System.Drawing.Point(32, 152);
			this.checkTurn2.Name = "checkTurn2";
			this.checkTurn2.Size = new System.Drawing.Size(144, 16);
			this.checkTurn2.TabIndex = 11;
			this.checkTurn2.Text = "折り返す(&T)";
			// 
			// checkTurn1
			// 
			this.checkTurn1.Location = new System.Drawing.Point(32, 80);
			this.checkTurn1.Name = "checkTurn1";
			this.checkTurn1.Size = new System.Drawing.Size(144, 16);
			this.checkTurn1.TabIndex = 14;
			this.checkTurn1.Text = "折り返す(&T)";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(80, 56);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(32, 16);
			this.label3.TabIndex = 13;
			this.label3.Text = "分間";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// durationBox
			// 
			this.durationBox.Increment = new System.Decimal(new int[] {
																		  10,
																		  0,
																		  0,
																		  0});
			this.durationBox.Location = new System.Drawing.Point(32, 56);
			this.durationBox.Maximum = new System.Decimal(new int[] {
																		1215752191,
																		23,
																		0,
																		0});
			this.durationBox.Minimum = new System.Decimal(new int[] {
																		10,
																		0,
																		0,
																		0});
			this.durationBox.Name = "durationBox";
			this.durationBox.Size = new System.Drawing.Size(48, 19);
			this.durationBox.TabIndex = 12;
			this.durationBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.durationBox.Value = new System.Decimal(new int[] {
																	  10,
																	  0,
																	  0,
																	  0});
			// 
			// StationConfigDialog
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(270, 205);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.checkTurn1,
																		  this.label3,
																		  this.durationBox,
																		  this.checkTurn2,
																		  this.radioPass,
																		  this.buttonOk,
																		  this.groupBox1,
																		  this.buttonAdvanced,
																		  this.radioAdvanced,
																		  this.label2,
																		  this.minBox,
																		  this.label1,
																		  this.hourBox,
																		  this.radioSimple,
																		  this.radioFixedDuration});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "StationConfigDialog";
			this.ShowInTaskbar = false;
			this.Text = "発車時刻の設定";
			((System.ComponentModel.ISupportInitialize)(this.hourBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.minBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.durationBox)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void buttonAdvanced_Click(object sender, System.EventArgs e) {
			if(!(currentHandler is AdvancedStationHandler ))
				currentHandler = new AdvancedStationHandler();

			new StationAdvancedDialog( (AdvancedStationHandler)currentHandler ).ShowDialog(this);
		}

		private int getMinutes() {
			return ((int)hourBox.Value)*60 + ((int)minBox.Value);
		}

		private void onTimeChanged(object sender, System.EventArgs e) {
			if( (int)minBox.Value == 60 ) {
				minBox.Value = 0;
				hourBox.Value += 1;
			}
			if( (int)hourBox.Value == 24 ) {
				hourBox.Value = 0;
			}
		}

		private void onRadioChanged( object sender, System.EventArgs e ) {
			durationBox.Enabled = checkTurn1.Enabled = radioFixedDuration.Checked;
			
			hourBox.Enabled = minBox.Enabled = checkTurn2.Enabled = radioSimple.Checked;

			buttonAdvanced.Enabled = radioAdvanced.Checked;
		}

		private void onOK(object sender, System.EventArgs e) {
			if( radioFixedDuration.Checked )
				currentHandler = new FixedDurationStationHandler(TimeLength.fromMinutes((long)durationBox.Value),checkTurn1.Checked);
			if( radioPass.Checked )	
				currentHandler = new PassStationHandler();
			if( radioSimple.Checked )
				currentHandler = new OnceADayStationHandler(getMinutes(),checkTurn2.Checked);
			if( radioAdvanced.Checked ) {
				if(!(currentHandler is AdvancedStationHandler))
					currentHandler = new AdvancedStationHandler();
			}
		}
	}
}
