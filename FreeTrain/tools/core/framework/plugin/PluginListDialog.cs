using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace freetrain.framework.plugin
{
	/// <summary>
	/// PluginListDialog �̊T�v�̐����ł��B
	/// </summary>
	public class PluginListDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ListView list;
		private System.Windows.Forms.ColumnHeader titleColumn;
		private System.Windows.Forms.ColumnHeader authorColumn;
		private System.Windows.Forms.ColumnHeader linkColumn;
		private System.Windows.Forms.ImageList images;
		private System.ComponentModel.IContainer components;

		public PluginListDialog()
		{
			//
			// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent �Ăяo���̌�ɁA�R���X�g���N�^ �R�[�h��ǉ����Ă��������B
			//
		}

		/// <summary>
		/// �g�p����Ă��郊�\�[�X�Ɍ㏈�������s���܂��B
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing && components != null)
				components.Dispose();
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// �f�U�C�i �T�|�[�g�ɕK�v�ȃ��\�b�h�ł��B���̃��\�b�h�̓��e��
		/// �R�[�h �G�f�B�^�ŕύX���Ȃ��ł��������B
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new System.Windows.Forms.ListViewItem.ListViewSubItem[] {
																																								new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "test", System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128))))}, 0);
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(PluginListDialog));
			this.panel1 = new System.Windows.Forms.Panel();
			this.button1 = new System.Windows.Forms.Button();
			this.list = new System.Windows.Forms.ListView();
			this.titleColumn = new System.Windows.Forms.ColumnHeader();
			this.authorColumn = new System.Windows.Forms.ColumnHeader();
			this.linkColumn = new System.Windows.Forms.ColumnHeader();
			this.images = new System.Windows.Forms.ImageList(this.components);
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.button1});
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 235);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(424, 40);
			this.panel1.TabIndex = 0;
			// 
			// button1
			// 
			this.button1.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(328, 8);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(88, 24);
			this.button1.TabIndex = 0;
			this.button1.Text = "&OK";
			// 
			// list
			// 
			this.list.AllowColumnReorder = true;
			this.list.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																				   this.titleColumn,
																				   this.authorColumn,
																				   this.linkColumn});
			this.list.Dock = System.Windows.Forms.DockStyle.Fill;
			this.list.FullRowSelect = true;
			listViewItem1.StateImageIndex = 0;
			this.list.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
																				 listViewItem1});
			this.list.Name = "list";
			this.list.Size = new System.Drawing.Size(424, 235);
			this.list.SmallImageList = this.images;
			this.list.TabIndex = 1;
			this.list.View = System.Windows.Forms.View.Details;
			// 
			// titleColumn
			// 
			this.titleColumn.Text = "���O";
			this.titleColumn.Width = 141;
			// 
			// authorColumn
			// 
			this.authorColumn.Text = "�����";
			// 
			// linkColumn
			// 
			this.linkColumn.Text = "�A����";
			this.linkColumn.Width = 207;
			// 
			// images
			// 
			this.images.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.images.ImageSize = new System.Drawing.Size(16, 16);
			this.images.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("images.ImageStream")));
			this.images.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// PluginListDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(424, 275);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.list,
																		  this.panel1});
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PluginListDialog";
			this.Text = "�C���X�g�[������Ă���v���O�C��";
			this.Load += new System.EventHandler(this.PluginListDialog_Load);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void PluginListDialog_Load(object sender, System.EventArgs e) {
			// populate the list
			list.Items.Clear();
			foreach( Plugin p in Core.plugins ) {
				list.Items.Add( new ListViewItem(
					new string[]{ p.title, p.author, p.homepage }, 0 ));
			}
		}
	}
}
