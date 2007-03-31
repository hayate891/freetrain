using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace freetrain.world.rail.tattc
{
	/// <summary>
	/// JunctionAdvancedDialog の概要の説明です。
	/// </summary>
	internal class JunctionAdvancedDialog : System.Windows.Forms.Form
	{
		internal JunctionAdvancedDialog( Junction junction ) {
			this.junction = junction;
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			resetEntryBoxes();
			onSelectionChanged(null,null);

			// populate the list view
			foreach( AdvJunctionRule rule in junction.advancedRules ) {
				ListViewItem lvi = new ListViewItem();
				updateListViewItem(rule,lvi);
				triggerList.Items.Add(lvi);
			}
		}

		private System.Windows.Forms.RadioButton radioStraight;
		private System.Windows.Forms.RadioButton radioCurve;
		private System.Windows.Forms.ColumnHeader columnHeader5;

		private readonly Junction junction;

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing && components != null)
				components.Dispose();
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox monthBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox dayBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox dayOfWeekBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox hourBox;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button buttonAdd;
		private System.Windows.Forms.Button buttonUp;
		private System.Windows.Forms.Button buttonDown;
		private System.Windows.Forms.Button buttonRemove;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.Button buttonReplace;
		private System.Windows.Forms.ListView triggerList;
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radioCurve = new System.Windows.Forms.RadioButton();
			this.radioStraight = new System.Windows.Forms.RadioButton();
			this.buttonReplace = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.hourBox = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.dayOfWeekBox = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.dayBox = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.monthBox = new System.Windows.Forms.ComboBox();
			this.buttonAdd = new System.Windows.Forms.Button();
			this.buttonUp = new System.Windows.Forms.Button();
			this.buttonDown = new System.Windows.Forms.Button();
			this.buttonRemove = new System.Windows.Forms.Button();
			this.buttonOk = new System.Windows.Forms.Button();
			this.triggerList = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.radioCurve,
																					this.radioStraight,
																					this.buttonReplace,
																					this.label4,
																					this.hourBox,
																					this.label3,
																					this.dayOfWeekBox,
																					this.label2,
																					this.dayBox,
																					this.label1,
																					this.monthBox,
																					this.buttonAdd});
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(360, 80);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Requirement";
			//! this.groupBox1.Text = "条件";
			// 
			// radioCurve
			// 
			this.radioCurve.Location = new System.Drawing.Point(104, 48);
			this.radioCurve.Name = "radioCurve";
			this.radioCurve.Size = new System.Drawing.Size(64, 24);
			this.radioCurve.TabIndex = 9;
			this.radioCurve.Text = "Junction";
			//! this.radioCurve.Text = "分岐";
			this.radioCurve.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// radioStraight
			// 
			this.radioStraight.Checked = true;
			this.radioStraight.Location = new System.Drawing.Point(40, 48);
			this.radioStraight.Name = "radioStraight";
			this.radioStraight.Size = new System.Drawing.Size(64, 24);
			this.radioStraight.TabIndex = 8;
			this.radioStraight.TabStop = true;
			this.radioStraight.Text = "Straight";
			//! this.radioStraight.Text = "直進";
			// 
			// buttonReplace
			// 
			this.buttonReplace.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonReplace.Location = new System.Drawing.Point(288, 48);
			this.buttonReplace.Name = "buttonReplace";
			this.buttonReplace.Size = new System.Drawing.Size(64, 24);
			this.buttonReplace.TabIndex = 11;
			this.buttonReplace.Text = "&Replace";
			//! this.buttonReplace.Text = "置換(&R)";
			this.buttonReplace.Click += new System.EventHandler(this.buttonReplace_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(328, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(20, 20);
			this.label4.TabIndex = 7;
			this.label4.Text = "o'clock";
			//! this.label4.Text = "時";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// hourBox
			// 
			this.hourBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.hourBox.Items.AddRange(new object[] {
														 "*",
														 "00",
														 "01",
														 "02",
														 "03",
														 "04",
														 "05",
														 "06",
														 "07",
														 "08",
														 "09",
														 "10",
														 "11",
														 "12",
														 "13",
														 "14",
														 "15",
														 "16",
														 "17",
														 "18",
														 "19",
														 "20",
														 "21",
														 "22",
														 "23"});
			this.hourBox.Location = new System.Drawing.Point(272, 16);
			this.hourBox.MaxDropDownItems = 13;
			this.hourBox.Name = "hourBox";
			this.hourBox.Size = new System.Drawing.Size(48, 20);
			this.hourBox.TabIndex = 6;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(232, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(32, 20);
			this.label3.TabIndex = 5;
			this.label3.Text = "day";
			//! this.label3.Text = "曜日";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// dayOfWeekBox
			// 
			this.dayOfWeekBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.dayOfWeekBox.Items.AddRange(new object[] {
															  "*",
															  "Sun",
															  "Mon",
															  "Tues",
															  "Wednes",
															  "Thurs",
															  "Fri",
															  "Satur"});
															  //!"日",
															  //!"月",
															  //!"火",
															  //!"水",
															  //!"木",
															  //!"金",
															  //!"土"});
			this.dayOfWeekBox.Location = new System.Drawing.Point(176, 16);
			this.dayOfWeekBox.MaxDropDownItems = 13;
			this.dayOfWeekBox.Name = "dayOfWeekBox";
			this.dayOfWeekBox.Size = new System.Drawing.Size(48, 20);
			this.dayOfWeekBox.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(152, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(16, 20);
			this.label2.TabIndex = 3;
			this.label2.Text = "th";
			//! this.label2.Text = "日";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// dayBox
			// 
			this.dayBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.dayBox.Items.AddRange(new object[] {
														"*",
														"1",
														"2",
														"3",
														"4",
														"5",
														"6",
														"7",
														"8",
														"9",
														"10",
														"11",
														"12",
														"13",
														"14",
														"15",
														"16",
														"17",
														"18",
														"19",
														"20",
														"21",
														"22",
														"23",
														"24",
														"25",
														"26",
														"27",
														"28",
														"29",
														"30",
														"31"});
			this.dayBox.Location = new System.Drawing.Point(96, 16);
			this.dayBox.MaxDropDownItems = 13;
			this.dayBox.Name = "dayBox";
			this.dayBox.Size = new System.Drawing.Size(48, 20);
			this.dayBox.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(72, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(16, 20);
			this.label1.TabIndex = 1;
			this.label1.Text = "month";
			//! this.label1.Text = "月";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// monthBox
			// 
			this.monthBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.monthBox.Items.AddRange(new object[] {
														  "*",
														  "1",
														  "2",
														  "3",
														  "4",
														  "5",
														  "6",
														  "7",
														  "8",
														  "9",
														  "10",
														  "11",
														  "12"});
			this.monthBox.Location = new System.Drawing.Point(16, 16);
			this.monthBox.MaxDropDownItems = 13;
			this.monthBox.Name = "monthBox";
			this.monthBox.Size = new System.Drawing.Size(48, 20);
			this.monthBox.TabIndex = 0;
			// 
			// buttonAdd
			// 
			this.buttonAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonAdd.Location = new System.Drawing.Point(216, 48);
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.Size = new System.Drawing.Size(64, 24);
			this.buttonAdd.TabIndex = 10;
			this.buttonAdd.Text = "&Add";
			//! this.buttonAdd.Text = "追加(&A)";
			this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
			// 
			// buttonUp
			// 
			this.buttonUp.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonUp.Location = new System.Drawing.Point(296, 96);
			this.buttonUp.Name = "buttonUp";
			this.buttonUp.Size = new System.Drawing.Size(64, 24);
			this.buttonUp.TabIndex = 13;
			this.buttonUp.Text = "↑";
			this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
			// 
			// buttonDown
			// 
			this.buttonDown.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonDown.Location = new System.Drawing.Point(296, 128);
			this.buttonDown.Name = "buttonDown";
			this.buttonDown.Size = new System.Drawing.Size(64, 24);
			this.buttonDown.TabIndex = 14;
			this.buttonDown.Text = "↓";
			this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
			// 
			// buttonRemove
			// 
			this.buttonRemove.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonRemove.Location = new System.Drawing.Point(296, 160);
			this.buttonRemove.Name = "buttonRemove";
			this.buttonRemove.Size = new System.Drawing.Size(64, 24);
			this.buttonRemove.TabIndex = 15;
			this.buttonRemove.Text = "Delete";
			//! this.buttonRemove.Text = "削除";
			this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
			// 
			// buttonOk
			// 
			this.buttonOk.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonOk.Location = new System.Drawing.Point(272, 256);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(88, 24);
			this.buttonOk.TabIndex = 16;
			this.buttonOk.Text = "&OK";
			// 
			// triggerList
			// 
			this.triggerList.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.triggerList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						  this.columnHeader1,
																						  this.columnHeader2,
																						  this.columnHeader3,
																						  this.columnHeader4,
																						  this.columnHeader5});
			this.triggerList.FullRowSelect = true;
			this.triggerList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.triggerList.HideSelection = false;
			this.triggerList.Location = new System.Drawing.Point(8, 96);
			this.triggerList.MultiSelect = false;
			this.triggerList.Name = "triggerList";
			this.triggerList.Scrollable = false;
			this.triggerList.Size = new System.Drawing.Size(278, 152);
			this.triggerList.TabIndex = 12;
			this.triggerList.View = System.Windows.Forms.View.Details;
			this.triggerList.SelectedIndexChanged += new System.EventHandler(this.onSelectionChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Month";
			//! this.columnHeader1.Text = "月";
			this.columnHeader1.Width = 55;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Date";
			//! this.columnHeader2.Text = "日";
			this.columnHeader2.Width = 55;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Day";
			//! this.columnHeader3.Text = "曜日";
			this.columnHeader3.Width = 55;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Time";
			//! this.columnHeader4.Text = "時";
			this.columnHeader4.Width = 55;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Direction";
			//! this.columnHeader5.Text = "方向";
			this.columnHeader5.Width = 55;
			// 
			// JunctionAdvancedDialog
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(376, 290);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.triggerList,
																		  this.buttonOk,
																		  this.buttonRemove,
																		  this.buttonDown,
																		  this.buttonUp,
																		  this.groupBox1});
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(384, 800);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(384, 298);
			this.Name = "JunctionAdvancedDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "Advanced point settings";
			//! this.Text = "ポイントの詳細設定";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
		
		/// <summary> clear all the boxes back to the default </summary>
		private void resetEntryBoxes() {
			monthBox.SelectedIndex = 0;
			dayBox.SelectedIndex = 0;
			dayOfWeekBox.SelectedIndex = 0;
			hourBox.SelectedIndex = 0;
		}

		private void buttonAdd_Click(object sender, System.EventArgs e) {
			// update the data structure
			AdvJunctionRule tm = createRule();
			junction.advancedRules.add(tm);

			// update the UI
			ListViewItem lvi = new ListViewItem();
			updateListViewItem(tm,lvi);
			triggerList.Items.Add(lvi);
			
			resetEntryBoxes();
		}

		private int selectedIndex { get { return triggerList.SelectedIndices[0]; } }
		private AdvJunctionRule selectedRule {
			get {
				return (AdvJunctionRule)triggerList.Items[selectedIndex].Tag;
			}
		}

		private void buttonReplace_Click(object sender, System.EventArgs e) {
			int idx = selectedIndex;

			// update the data structure
			AdvJunctionRule tm = createRule();
			junction.advancedRules.set( idx, tm );

			// update the UI
			ListViewItem lvi = triggerList.Items[idx];
			updateListViewItem(tm,lvi);
		}

		private void buttonUp_Click(object sender, System.EventArgs e) {
			moveData(-1);
		}

		private void buttonDown_Click(object sender, System.EventArgs e) {
			moveData(+1);
		}

		private void moveData( int offset ) {
			int idx = selectedIndex;
			
			// update the data structure
			AdvJunctionRule rule = selectedRule;
			junction.advancedRules.remove(rule);
			junction.advancedRules.insert(idx+offset,rule);

			// update the UI
			ListViewItem lvi = triggerList.Items[idx];
			triggerList.Items.Remove(lvi);
			triggerList.Items.Insert(idx+offset,lvi);
		}

		private void buttonRemove_Click(object sender, System.EventArgs e) {
			int idx = selectedIndex;

			// update the data structure
			junction.advancedRules.remove( selectedRule );

			// update the UI
			triggerList.Items.RemoveAt(idx);
		}

		private AdvJunctionRule createRule() {
			AdvJunctionRule tm = new AdvJunctionRule();
			
			tm.month = (sbyte)monthBox.SelectedIndex;
			if(tm.month==0)	tm.month = -1;

			tm.day = (sbyte)dayBox.SelectedIndex;
			if(tm.day==0)	tm.day = -1;

			tm.dayOfWeek = (sbyte)(dayOfWeekBox.SelectedIndex-1);

			tm.hour = (sbyte)(hourBox.SelectedIndex-1);

			tm.minutes = -1;

			tm.route = radioStraight.Checked ? JunctionRoute.Straight : JunctionRoute.Curve;

			return tm;
		}

		private void updateListViewItem( AdvJunctionRule rule, ListViewItem lvi ) {
			lvi.SubItems.Clear();

			lvi.Tag = rule;
			lvi.Text = numberToString(rule.month);

			lvi.SubItems.Add(numberToString(rule.day));
			if( rule.dayOfWeek==-1 )
				lvi.SubItems.Add("*");
			else
				lvi.SubItems.Add(""+Clock.dayOfWeekChar(rule.dayOfWeek));
			lvi.SubItems.Add(numberToString(rule.hour));

			lvi.SubItems.Add( rule.route==JunctionRoute.Straight?"Straight":"Junction" );
			//! lvi.SubItems.Add( rule.route==JunctionRoute.Straight?"直進":"分岐" );
		}

		private string numberToString( sbyte i ) {
			if(i==-1)	return "*";
			else		return i.ToString();
		}

		private void onSelectionChanged(object sender, System.EventArgs e) {
			bool b = ( triggerList.SelectedIndices.Count!=0 );
			int idx=-1;
			if( b )	idx = selectedIndex;

			buttonUp.Enabled = b && idx!=0;
			buttonDown.Enabled = b && idx!=triggerList.Items.Count-1;
			buttonRemove.Enabled = b;
			buttonReplace.Enabled = b;

			if(idx!=-1) {
				// update the edit box
				AdvJunctionRule rule = selectedRule;

				if( rule.month== -1 )	monthBox.SelectedIndex = 0;
				else					monthBox.SelectedIndex = rule.month;

				if( rule.day== -1 )		dayBox.SelectedIndex = 0;
				else					dayBox.SelectedIndex = rule.month;

				dayOfWeekBox.SelectedIndex = rule.dayOfWeek+1;

				hourBox.SelectedIndex = rule.hour+1;

				radioCurve.Checked		= (rule.route==JunctionRoute.Curve);
				radioStraight.Checked	= (rule.route==JunctionRoute.Straight);
			}
		}
	}
}
