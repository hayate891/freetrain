using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using freetrain.framework;

namespace freetrain.controllers
{
	/// <summary>
	/// Tool window that hosts <c>ModelController</c>.
	/// 
	/// Derived class should set the currentController appropriately to
	/// associate a controller to the form. The current controller can
	/// be changed while the form is visible.
	/// </summary>
	public class ControllerHostForm : Form
	{
		/// <summary>
		/// Currently active controller.
		/// </summary>
		private ModalController _currentController;

		public readonly IControllerSite siteImpl;

		public ControllerHostForm() {
			InitializeComponent();
			siteImpl = new ControllerSiteImpl(this);
		}

		protected ModalController currentController {
			get {
				return _currentController;
			}
			set {
				if( _currentController!=null ) {
					// deactive the old one if it's still active
					if(MainWindow.mainWindow.currentController==_currentController)
						MainWindow.mainWindow.detachController();
				}
				// activate the new controller
				_currentController = value;
				MainWindow.mainWindow.attachController(value);
			}
		}

		protected override void Dispose( bool disposing ) {
			if( disposing && components != null)
					components.Dispose();
			base.Dispose( disposing );
		}


		protected override void OnActivated( EventArgs e ) {
			base.OnActivated(e);
			// Attach the control when activated.
			MainWindow.mainWindow.attachController(_currentController);
		}

		/// <summary>
		/// Derived class still needs to extend this method and maintain
		/// the singleton.
		/// </summary>
		protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
			// Detach the controller when the form is going to be closed.
			if(MainWindow.mainWindow.currentController==_currentController)
				MainWindow.mainWindow.detachController();
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
			// AbstractControllerForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(106, 112);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AbstractControllerForm";
			this.ShowInTaskbar = false;

		}
		#endregion
	}
}