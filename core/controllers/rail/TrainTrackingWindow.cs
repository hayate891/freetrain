using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using freetrain.framework;
using freetrain.util.command;
using freetrain.world;
using freetrain.world.rail;

namespace freetrain.controllers.rail
{
	/// <summary>
	/// Window that tracks a train.
	/// </summary>
	public class TrainTrackingWindow : Form
	{
		public TrainTrackingWindow() {
			this.train = null;
			InitializeComponent();

			new Command( commands )
				.addUpdateHandler( new CommandHandler(updateTrackButton) )
				.addExecuteHandler( new CommandHandlerNoArg(onMove) )
				.commandInstances.AddAll( buttonTrack );
		}

		private readonly CommandManager commands = new CommandManager();

		private Train train;

		#region Windows Form Designer generated code
		private System.Windows.Forms.Label stateBox;
		private System.Windows.Forms.Label passengerBox;
		private System.Windows.Forms.Label nameBox;
		private System.Windows.Forms.Button buttonSelect;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button buttonTrack;

		/// <summary>
		/// �g�p����Ă��郊�\�[�X�Ɍ㏈�������s���܂��B
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing && components != null)
				components.Dispose();
			base.Dispose( disposing );
		}

		/// <summary>
		/// �K�v�ȃf�U�C�i�ϐ��ł��B
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// �f�U�C�i �T�|�[�g�ɕK�v�ȃ��\�b�h�ł��B���̃��\�b�h�̓��e��
		/// �R�[�h �G�f�B�^�ŕύX���Ȃ��ł��������B
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.stateBox = new System.Windows.Forms.Label();
			this.passengerBox = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.buttonTrack = new System.Windows.Forms.Button();
			this.nameBox = new System.Windows.Forms.Label();
			this.buttonSelect = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "��Ԗ��F";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 16);
			this.label2.TabIndex = 2;
			this.label2.Text = "��ԁF";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// stateBox
			// 
			this.stateBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.stateBox.Location = new System.Drawing.Point(72, 32);
			this.stateBox.Name = "stateBox";
			this.stateBox.Size = new System.Drawing.Size(88, 16);
			this.stateBox.TabIndex = 3;
			this.stateBox.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// passengerBox
			// 
			this.passengerBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.passengerBox.Location = new System.Drawing.Point(72, 56);
			this.passengerBox.Name = "passengerBox";
			this.passengerBox.Size = new System.Drawing.Size(88, 16);
			this.passengerBox.TabIndex = 5;
			this.passengerBox.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 56);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(56, 16);
			this.label5.TabIndex = 4;
			this.label5.Text = "��q���F";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// buttonTrack
			// 
			this.buttonTrack.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonTrack.Enabled = false;
			this.buttonTrack.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonTrack.Location = new System.Drawing.Point(96, 81);
			this.buttonTrack.Name = "buttonTrack";
			this.buttonTrack.Size = new System.Drawing.Size(64, 24);
			this.buttonTrack.TabIndex = 6;
			this.buttonTrack.Text = "�ړ�(&M)";
			// 
			// nameBox
			// 
			this.nameBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.nameBox.Location = new System.Drawing.Point(72, 8);
			this.nameBox.Name = "nameBox";
			this.nameBox.Size = new System.Drawing.Size(72, 16);
			this.nameBox.TabIndex = 1;
			this.nameBox.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// buttonSelect
			// 
			this.buttonSelect.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.buttonSelect.Font = new System.Drawing.Font("Webdings", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(2)));
			this.buttonSelect.Location = new System.Drawing.Point(144, 8);
			this.buttonSelect.Name = "buttonSelect";
			this.buttonSelect.Size = new System.Drawing.Size(16, 16);
			this.buttonSelect.TabIndex = 7;
			this.buttonSelect.Text = "6";
			this.buttonSelect.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
			// 
			// TrainTrackingWindow
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(162, 110);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.buttonSelect,
																		  this.buttonTrack,
																		  this.passengerBox,
																		  this.label5,
																		  this.stateBox,
																		  this.label2,
																		  this.nameBox,
																		  this.label1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(136, 136);
			this.Name = "TrainTrackingWindow";
			this.Text = "��Ԃ̏��";
			this.ResumeLayout(false);

		}
		#endregion


		protected override void OnClosed(EventArgs e) {
			if(train!=null)
				train.nonPersistentStateListeners -= new TrainHandler(onStateChanged);
		}

		private void onStateChanged( Train tr ) {
			Debug.Assert( tr==train );
			stateBox.Text = train.stateDisplayText;
			nameBox.Text = train.name;
			
			string ratio = "-";
			if( train.passengerCapacity!=0 )
				ratio = (train.passenger*100/train.passengerCapacity).ToString();
			passengerBox.Text = string.Format("{0} ({1}%)", train.passenger, ratio );
			commands.updateAll();
		}

		private void buttonSelect_Click(object sender, EventArgs e) {
			ContextMenu m = new ContextMenu();
			populateMenu( m.MenuItems, World.world.rootTrainGroup );
			m.Show( buttonSelect, new Point(0,buttonSelect.Height) );
		}

		private void populateMenu( Menu.MenuItemCollection menu, TrainGroup group ) {
			foreach( TrainItem item in group.items ) {
				MenuItem mi = new MenuItem( item.name );
				menu.Add(mi);

				if( item is TrainGroup ) {
					populateMenu( mi.MenuItems, (TrainGroup)item );
				} else {
					mi.Click += new EventHandler(new MenuHandler(this,(Train)item).handler);
				}
			}
		}

		private class MenuHandler {
			internal MenuHandler( TrainTrackingWindow o, Train tr ) { this.owner=o; this.train=tr; }
			private readonly Train train;
			private readonly TrainTrackingWindow owner;
			internal void handler( object sender, EventArgs e ) {
				owner.selectTrain(train);
			}
		}

		private void selectTrain( Train newTrain ) {
			if(train!=null)
				train.nonPersistentStateListeners -= new TrainHandler(onStateChanged);
			train = newTrain;
			// update the window now.
			onStateChanged(train);
			// make sure that we will update the window in a timely fashion
			train.nonPersistentStateListeners += new TrainHandler(onStateChanged);
		}

		private void updateTrackButton( Command cmd ) {
			cmd.Enabled = train!=null && train.isPlaced;
		}
		private void onMove() {
			if( train.head.state.isUnplaced )		return;
			if( MainWindow.primaryMapView==null )	return;
			
			MainWindow.primaryMapView.moveTo( train.head.state.asPlaced().location );
		}
	}
}