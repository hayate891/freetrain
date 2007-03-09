using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using freetrain.framework;

namespace freetrain.controllers
{
	/// <summary>
	/// Pop-up tool window to host modal controllers.
	/// </summary>
	public class AbstractControllerForm : Form
	{
		public AbstractControllerForm() {
			InitializeComponent();

		}

		protected override void Dispose( bool disposing ) {
			if( disposing && components != null)
					components.Dispose();
			base.Dispose( disposing );
		}



		protected override void OnLoad(System.EventArgs e) {
			// attach this window.
			MainWindow.mainWindow.AddOwnedForm(this);
			// move this window to the left-top position of the parent window
			this.Left = MainWindow.mainWindow.Left;
			this.Top  = MainWindow.mainWindow.Top;
		}

		#region Windows Form Designer generated code
		private System.ComponentModel.Container components = null;

		private void InitializeComponent()
		{
			// 
			// AbstractControllerImpl
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(292, 271);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AbstractControllerImpl";
			this.ShowInTaskbar = false;

		}
		#endregion
	}
}
