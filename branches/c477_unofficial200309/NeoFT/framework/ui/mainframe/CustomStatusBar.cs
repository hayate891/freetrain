using System;
using System.ComponentModel;
using System.Collections;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace nft.ui.mainframe
{
	/// <summary>
	/// CustomStatusBar の概要の説明です。
	/// </summary>
	public class CustomStatusBar : StatusBar
	{
		private Timer timer;

		public CustomStatusBar()
		{
			InitializeComponent();
			timer = new Timer();
			timer.Tick += new EventHandler(timer_Tick);
		}

		public void InsertPanel( int index, string msg , int width, int minwidth, string tooltip )
		{
			StatusBarPanel panel = new StatusBarPanel();
			panel.Text = msg;
			panel.MinWidth = minwidth;
			panel.AutoSize = width<=0?StatusBarPanelAutoSize.Contents:StatusBarPanelAutoSize.None;
			panel.ToolTipText = tooltip;
			try
			{
				Panels.Insert(index,panel);
			}
			catch
			{
			}
			// for safe
			if(!Panels.Contains(panel))
				Panels.Add(panel);
		}

		#region helper method ShowMessage
		public void ShowMessage( int index, string msg, string tooltip, Icon icon )
		{
			StatusBarPanel panel = Panels[index];
			if(panel==null) return;
			panel.Text = msg;
			panel.ToolTipText = tooltip;
			panel.Icon = icon;
		}

		public void ShowMessage( int index, string msg, string tooltip )
		{
			StatusBarPanel panel = Panels[index];
			if(panel==null) return;
			panel.Text = msg;
			panel.ToolTipText = tooltip;
		}

		public void ShowMessage( int index, string msg )
		{
			StatusBarPanel panel = Panels[index];
			if(panel==null) return;
			panel.Text = msg;
		}

		public void ShowTemporaryMessage( string msg, int timecounts )
		{
			if(timecounts>0)
			{
				lock(this)
				{
					if(timer!=null)
					{
						timer.Stop();
						timer.Interval = timecounts*1000;
						timer.Start();
					}
				}
			}
			Panels[0].Text = msg;
		}

		public void ShowTemporaryMessage( string msg, string tooltip, int timecounts )
		{
			ShowTemporaryMessage( msg, timecounts );
			Panels[0].ToolTipText = tooltip;
		}

		public void ShowTemporaryMessage( string msg, string tooltip, Icon icon, int timecounts )
		{
			ShowTemporaryMessage( msg, timecounts );
			Panels[0].ToolTipText = tooltip;
			Panels[0].Icon = icon;
		}
		#endregion

		#region コンポーネント デザイナで生成されたコード 
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// CustomStatusBar
			// 
			this.ShowPanels = true;

		}
		#endregion

		protected override void Dispose( bool disposing ) 
		{
			timer.Stop();
			timer.Dispose();
			base.Dispose( disposing );
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			Panels[0].Text = "";
			Panels[0].Icon = null;
			Panels[0].ToolTipText = null;
		}
	}
}
