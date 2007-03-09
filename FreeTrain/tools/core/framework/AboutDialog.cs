using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using freetrain.controls;
using org.kohsuke.directdraw;

namespace freetrain.framework
{
	/// <summary>
	/// AboutDialog �̊T�v�̐����ł��B
	/// </summary>
	public class AboutDialog : System.Windows.Forms.Form
	{
		public static void show() {
			AboutDialog dlg = new AboutDialog();
			dlg.ShowDialog(MainWindow.mainWindow);
		}

		public AboutDialog() {
			InitializeComponent();

			browser.navigate("about:blank");
			browser.docHostUIHandler = new DocHostUIHandlerImpl(this);
			browser.navigate(ResourceUtil.findSystemResource("about.html"));
		}

		protected override void OnLoad(System.EventArgs e) {
			using( WindowedDirectDraw dd = new WindowedDirectDraw(this) ) {
				this.size.Text = format(dd.availableVideoMemory)+"/"+format(dd.totalVideoMemory);
				this.displayMode.Text = dd.primarySurface.displayModeName;
				this.progressBar.Value = Math.Min( 10000,
					(int)(10000.0*dd.availableVideoMemory/dd.totalVideoMemory) );
			}
		}

		private string format( long value ) {
			value /= 1024;
			return value+"KB";
		}

		protected override void Dispose( bool disposing ) {
			if( disposing && components != null)
				components.Dispose();
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Label size;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label displayMode;
		private freetrain.controls.WebBrowser browser;
		private System.Windows.Forms.Panel panel1;
		private System.ComponentModel.Container components = null;
		/// <summary>
		/// �f�U�C�i �T�|�[�g�ɕK�v�ȃ��\�b�h�ł��B���̃��\�b�h�̓��e��
		/// �R�[�h �G�f�B�^�ŕύX���Ȃ��ł��������B
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(AboutDialog));
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.label3 = new System.Windows.Forms.Label();
			this.okButton = new System.Windows.Forms.Button();
			this.size = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.displayMode = new System.Windows.Forms.Label();
			this.browser = new freetrain.controls.WebBrowser();
			this.panel1 = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.browser)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(8, 176);
			this.progressBar.Maximum = 10000;
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(296, 16);
			this.progressBar.TabIndex = 3;
			this.progressBar.Value = 30;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 160);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(96, 16);
			this.label3.TabIndex = 4;
			this.label3.Text = "VRAM�󂫗e�ʁF";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// okButton
			// 
			this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.okButton.Location = new System.Drawing.Point(224, 200);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(80, 24);
			this.okButton.TabIndex = 5;
			this.okButton.Text = "&OK";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// size
			// 
			this.size.Location = new System.Drawing.Point(104, 160);
			this.size.Name = "size";
			this.size.Size = new System.Drawing.Size(104, 16);
			this.size.TabIndex = 6;
			this.size.Text = "100KB/64MB";
			this.size.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 144);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(96, 16);
			this.label4.TabIndex = 7;
			this.label4.Text = "��ʃ��[�h�F";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// displayMode
			// 
			this.displayMode.Location = new System.Drawing.Point(104, 144);
			this.displayMode.Name = "displayMode";
			this.displayMode.Size = new System.Drawing.Size(104, 16);
			this.displayMode.TabIndex = 8;
			this.displayMode.Text = "---";
			this.displayMode.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// browser
			// 
			this.browser.ContainingControl = this;
			this.browser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.browser.Enabled = true;
			this.browser.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("browser.OcxState")));
			this.browser.Size = new System.Drawing.Size(296, 136);
			this.browser.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.browser});
			this.panel1.Location = new System.Drawing.Point(8, 8);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(296, 136);
			this.panel1.TabIndex = 9;
			// 
			// AboutDialog
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(314, 232);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.panel1,
																		  this.displayMode,
																		  this.label4,
																		  this.size,
																		  this.okButton,
																		  this.label3,
																		  this.progressBar});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutDialog";
			this.ShowInTaskbar = false;
			this.Text = "FreeTrain�ɂ���";
			((System.ComponentModel.ISupportInitialize)(this.browser)).EndInit();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void okButton_Click(object sender, System.EventArgs e) {
			Close();
		}

		private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e) {
			Process.Start( ((LinkLabel)sender).Text );
		}

	}
}
