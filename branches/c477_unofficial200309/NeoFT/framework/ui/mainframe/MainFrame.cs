using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using UtilityLibrary.CommandBars;
using nft.ui.docking;
using nft.framework;
using nft.ui.command;

namespace nft.ui.mainframe
{
	/// <summary>
	/// MainFrame �̊T�v�̐����ł��B
	/// </summary>
	public class MainFrame : System.Windows.Forms.Form, IBarHostFrame
	{
		private nft.ui.mainframe.CustomStatusBar statusBar;
		/// <summary>
		/// �K�v�ȃf�U�C�i�ϐ��ł��B
		/// </summary>
		private System.ComponentModel.Container components = null;

		internal readonly DockingManagerEx dockManager;
		internal readonly MenuPathManager menuManager;
		internal readonly ToolBarExManager toolbarManager;
		private System.Windows.Forms.MainMenu mainMenu;
		private UtilityLibrary.CommandBars.ReBar reBar;
		
		public MainFrame(string[] args)
		{
			//
			// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
			//
			InitializeComponent();
			
			IsMdiContainer = true;

			dockManager = new DockingManagerEx(this);
			dockManager.OuterControl = statusBar;
			menuManager = new MenuPathManager(mainMenu);
			toolbarManager = new ToolBarExManager(reBar);

			Show();
			Main.init(args, this);
		}

		/// <summary>
		/// �g�p����Ă��郊�\�[�X�Ɍ㏈�������s���܂��B
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
			dockManager.Dispose();
			base.Dispose( disposing );
		}
		protected override void OnClosing( CancelEventArgs e ) 
		{
			base.OnClosing(e);
			//Main.options.save();
			if( dockManager.Contents.Count > 0 )
				dockManager.SaveConfigToFile("layout.config");
		}

		#region Windows �t�H�[�� �f�U�C�i�Ő������ꂽ�R�[�h 
		/// <summary>
		/// �f�U�C�i �T�|�[�g�ɕK�v�ȃ��\�b�h�ł��B���̃��\�b�h�̓��e��
		/// �R�[�h �G�f�B�^�ŕύX���Ȃ��ł��������B
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainFrame));
			this.statusBar = new nft.ui.mainframe.CustomStatusBar();
			this.reBar = new UtilityLibrary.CommandBars.ReBar();
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.SuspendLayout();
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 315);
			this.statusBar.Name = "statusBar";
			this.statusBar.ShowPanels = true;
			this.statusBar.Size = new System.Drawing.Size(488, 22);
			this.statusBar.TabIndex = 1;
			this.statusBar.Text = "customStatusBar1";
			// 
			// reBar
			// 
			this.reBar.Dock = System.Windows.Forms.DockStyle.Top;
			this.reBar.Location = new System.Drawing.Point(0, 0);
			this.reBar.Name = "reBar";
			this.reBar.Size = new System.Drawing.Size(488, 4);
			this.reBar.TabIndex = 3;
			this.reBar.TabStop = false;
			this.reBar.Text = "reBar1";
			// 
			// MainFrame
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(488, 337);
			this.Controls.Add(this.reBar);
			this.Controls.Add(this.statusBar);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu;
			this.Name = "MainFrame";
			this.Text = "MainFrame";
			this.ResumeLayout(false);

		}
		#endregion
		
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			// load the screen layout
			try 
			{
				dockManager.LoadConfigFromFile("layout.config");
			} 
			catch( Exception ex ) 
			{
				// exception will be thrown if the file doesn't exist.
				Debug.WriteLine(ex.Message);
				Debug.WriteLine(ex.StackTrace);
			}			
		}

		public void SetSubTitle(string subtitle) 
		{
			string long_title = Main.resources["mainframe.long_title"].stringValue;
			string short_title = Main.resources["mainframe.short_title"].stringValue;
			if( subtitle==null || subtitle.Trim().Length==0 )
				this.Text = long_title;
			else
				this.Text = short_title+" - "+subtitle;
		}

		#region child view management
		public void RegisterChild(Form frame)
		{
			frame.MdiParent=this;
			//frame.Show();
		}
		#endregion

		#region IBarHostFrame �����o

		public void AddFileDroppedHandler(nft.ui.mainframe.FileDroppedHandler handler)
		{
			// TODO:  MainFrame.AddFileDroppedHandler ������ǉ����܂��B
		}

		public string RegisterMenuNode(string idname, string path, string caption, string after, string before)
		{
			return menuManager.AddMenu( new MenuCreationInfo(idname,caption,null), path, after, before);
		}

		public void AddToolButton(ButtonCreationInfo info, string barname, string after, string before)
		{
			toolbarManager.AddNewButton(info,barname,after,before);
		}

		public void AddMenuItem(MenuCreationInfo info, string path, string after, string before)
		{
			menuManager.AddMenu(info, path, after, before);
		}

		public CommandUI SetToolButtonCommand( string cmdID, ICommandEntity entity, string barname, string bid )
		{
			if(toolbarManager.SetCommand( cmdID, entity, barname, bid ))
				return CommandUI.GetCommandUI( cmdID );
			else
				return null;
		}

		public CommandUI SetMenuCommand(  string cmdID, ICommandEntity entity, string path )
		{
			if(	menuManager.SetCommand( cmdID, entity, path ))
				return CommandUI.GetCommandUI( cmdID );
			else
				return null;
		}		
		#endregion

	}
}
