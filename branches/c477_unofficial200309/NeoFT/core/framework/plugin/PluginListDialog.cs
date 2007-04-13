using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using freetrain.controls;
using nft.ui.command;

namespace nft.framework.plugin
{
	/// <summary>
	/// PluginListDialog の概要の説明です。
	/// </summary>
	public class PluginListDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ColumnHeader titleColumn;
		private System.Windows.Forms.ColumnHeader authorColumn;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ListView plist;
		private System.Windows.Forms.TreeView tree;
		private System.Windows.Forms.ListView clist;
		private UrlLinkLabel info;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ColumnHeader pathColumn;
		private System.Windows.Forms.ImageList image;
		private System.ComponentModel.IContainer components;

		public PluginListDialog()
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			plist.ListViewItemSorter = new Sorter(0,false);
			clist.ListViewItemSorter = new Sorter(0,false);
			string filename = Directories.SystemResourceDir+"plugin_icons.bmp";
			image.Images.AddStrip(new System.Drawing.Bitmap(filename));
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing && components != null)
				components.Dispose();
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(PluginListDialog));
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.plist = new System.Windows.Forms.ListView();
			this.titleColumn = new System.Windows.Forms.ColumnHeader();
			this.authorColumn = new System.Windows.Forms.ColumnHeader();
			this.pathColumn = new System.Windows.Forms.ColumnHeader();
			this.image = new System.Windows.Forms.ImageList(this.components);
			this.tree = new System.Windows.Forms.TreeView();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.clist = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.info = new freetrain.controls.UrlLinkLabel();
			this.panel1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.button1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 277);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(432, 32);
			this.panel1.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(208, 32);
			this.label1.TabIndex = 1;
			this.label1.Text = "コンピュータに使わせたくないプラグインは チェックをはずしてください。";
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(336, 4);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(88, 24);
			this.button1.TabIndex = 0;
			this.button1.Text = "&OK";
			// 
			// plist
			// 
			this.plist.AllowColumnReorder = true;
			this.plist.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.plist.CheckBoxes = true;
			this.plist.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					this.titleColumn,
																					this.authorColumn,
																					this.pathColumn});
			this.plist.FullRowSelect = true;
			this.plist.HideSelection = false;
			this.plist.Location = new System.Drawing.Point(0, 0);
			this.plist.Name = "plist";
			this.plist.Size = new System.Drawing.Size(424, 152);
			this.plist.SmallImageList = this.image;
			this.plist.TabIndex = 1;
			this.plist.View = System.Windows.Forms.View.Details;
			this.plist.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.list_ColumnClick);
			this.plist.SelectedIndexChanged += new System.EventHandler(this.plist_SelectedIndexChanged);
			this.plist.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.list_ItemCheck);
			// 
			// titleColumn
			// 
			this.titleColumn.Text = "プラグイン";
			this.titleColumn.Width = 150;
			// 
			// authorColumn
			// 
			this.authorColumn.Text = "製作者";
			this.authorColumn.Width = 80;
			// 
			// pathColumn
			// 
			this.pathColumn.Text = "場所";
			this.pathColumn.Width = 500;
			// 
			// image
			// 
			this.image.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit;
			this.image.ImageSize = new System.Drawing.Size(16, 16);
			this.image.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// tree
			// 
			this.tree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tree.CheckBoxes = true;
			this.tree.HideSelection = false;
			this.tree.ImageList = this.image;
			this.tree.Location = new System.Drawing.Point(0, 0);
			this.tree.Name = "tree";
			this.tree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
																			 new System.Windows.Forms.TreeNode("ノード0")});
			this.tree.Size = new System.Drawing.Size(424, 200);
			this.tree.TabIndex = 1;
			this.tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tree_AfterSelect);
			this.tree.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.tree_BeforeCheck);
			this.tree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tree_BeforeExpand);
			// 
			// tabControl1
			// 
			this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.Buttons;
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(432, 277);
			this.tabControl1.TabIndex = 2;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.clist);
			this.tabPage1.Controls.Add(this.plist);
			this.tabPage1.Location = new System.Drawing.Point(4, 24);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(424, 249);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "リスト表示";
			// 
			// clist
			// 
			this.clist.AllowColumnReorder = true;
			this.clist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.clist.CheckBoxes = true;
			this.clist.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					this.columnHeader1,
																					this.columnHeader2,
																					this.columnHeader3});
			this.clist.FullRowSelect = true;
			this.clist.Location = new System.Drawing.Point(0, 153);
			this.clist.Name = "clist";
			this.clist.Size = new System.Drawing.Size(424, 96);
			this.clist.SmallImageList = this.image;
			this.clist.TabIndex = 2;
			this.clist.View = System.Windows.Forms.View.Details;
			this.clist.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.list_ColumnClick);
			this.clist.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.list_ItemCheck);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "コントリビューション";
			this.columnHeader1.Width = 150;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "種類";
			this.columnHeader2.Width = 120;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "ID";
			this.columnHeader3.Width = 400;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.info);
			this.tabPage2.Controls.Add(this.tree);
			this.tabPage2.Location = new System.Drawing.Point(4, 24);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(424, 249);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "ツリー表示";
			// 
			// info
			// 
			this.info.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.info.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.info.Location = new System.Drawing.Point(0, 209);
			this.info.Name = "info";
			this.info.Size = new System.Drawing.Size(424, 40);
			this.info.TabIndex = 2;
			this.info.TargetUrl = null;
			// 
			// PluginListDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(432, 309);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.panel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PluginListDialog";
			this.Text = "インストールされているプラグイン";
			this.Load += new System.EventHandler(this.PluginListDialog_Load);
			this.panel1.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void PluginListDialog_Load(object sender, System.EventArgs e) {
			// populate the list
			clist.Items.Clear();
			plist.Items.Clear();
			foreach( Plugin p in Core.plugins )
				plist.Items.Add( CreateListItem(p) );

			// construct tree
			tree.Nodes.Clear();
			foreach( Plugin p in Core.plugins )
				tree.Nodes.Add( CreateTreeItem(p) );
		}

		private void list_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			
			ListView _list = (ListView)sender;
			Sorter sorter = (Sorter)_list.ListViewItemSorter;
			sorter.index = e.Column;
			sorter.bNumeric = false;
			_list.Sort();		
		}

		private void plist_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			clist.Items.Clear();
			if( plist.SelectedItems.Count == 0 )
				return;
			//foreach( ListViewItem item in plist.SelectedItems )
			ListViewItem item = plist.SelectedItems[0];
			{
				Application.DoEvents();
				Plugin p = (Plugin)item.Tag;
				foreach( ContributionDefiner cd in p.definers )
					clist.Items.Add( CreateListItem(cd) );
				foreach( Contribution c in p.contributions ) 
					clist.Items.Add( CreateListItem(c) );
			}
		}

		private void tree_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			Plugin p = e.Node.Tag as Plugin;
			if(p!=null)
			{
				e.Node.Nodes.Clear();
				foreach( ContributionDefiner c in p.definers ) 
					e.Node.Nodes.Add( CreateTreeItem(c) );
				foreach( Contribution c in p.contributions ) 
					e.Node.Nodes.Add( CreateTreeItem(c) );
			}
		}

		private void tree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			string msg;
			Plugin p = e.Node.Tag as Plugin;
			if(p!=null)
			{
				msg = string.Format("Title:{0}\nAuthor:{1}\nHomepage:{2}",p.title,p.author,p.homepage);
				info.Text = msg;
				if(p.homepage.Equals("N/A")||p.homepage.Trim().Length==0)
				{
					info.LinkArea = new LinkArea(0,0);
					info.TargetUrl="";
				}
				else
				{
					int n = p.homepage.Length;
					info.LinkArea = new LinkArea(msg.Length-n,n);
					info.TargetUrl=p.homepage;
				}
				return;
			}
			Contribution c = e.Node.Tag as Contribution;
			if(c!=null)
			{
				msg = string.Format("Name:{0}\nType:{1}",c.name,c.type);
				info.Text = msg;
				info.LinkArea = new LinkArea(0,0);
				info.TargetUrl="";
				return;
			}			
		}

		#region Item construction helpers.
		private int GetIconIndex(Contribution c)
		{
			switch(c.state)
			{
				case ModuleState.Ready:
					if( c is ContributionDefiner)	
						return 4;
					else
						return 1;
				default:
					return 2;
			}
		}

		private int GetIconIndex(Plugin p)
		{
			switch(p.state)
			{
				case ModuleState.Ready:
					return 0;
				case ModuleState.PartialError:
					return 3;
				default:
					return 2;
			}
		}

		private ListViewItem CreateListItem(Contribution c)
		{
			int icon = GetIconIndex(c);
			ListViewItem item = new ListViewItem(new string[]{ c.name ,c.type, c.id }, icon );
			item.Tag = c;
			if( c is ContributionDefiner)
				item.Checked = true;
			else
				item.Checked = c.ComAvailable;
			return item;
		}
		private ListViewItem CreateListItem(Plugin p)
		{
			int icon = GetIconIndex(p);
			ListViewItem item = new ListViewItem(new string[]{ p.title, p.author, p.name }, icon );
			item.Tag = p;				
			item.Checked = p.ComAvailable;
			return item;
		}
		private TreeNode CreateTreeItem(Contribution c)
		{
			int icon = GetIconIndex(c);
			TreeNode node = new TreeNode(c.id, icon, icon );
			node.Tag = c;				
			if( c is ContributionDefiner)
				node.Checked = true;
			else
				node.Checked = c.ComAvailable;
			return node;
		}

		private void tabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
		{			
			switch(tabControl1.SelectedIndex )
			{
				case 0:
					foreach(ListViewItem item in plist.Items)
					{
						IUserExtension iue =item.Tag as IUserExtension;
						if(iue!=null && item.Checked!=iue.ComAvailable)
							item.Checked = iue.ComAvailable;
					}
					foreach(ListViewItem item in clist.Items)
					{
						IUserExtension iue =item.Tag as IUserExtension;
						if(iue!=null && item.Checked!=iue.ComAvailable)
							item.Checked = iue.ComAvailable;
					}
					break;
			
			
				case 1:
					foreach(TreeNode node in tree.Nodes)
					{
						IUserExtension iue =node.Tag as IUserExtension;
						if(iue!=null  && node.Checked!=iue.ComAvailable)
							node.Checked = iue.ComAvailable;
						foreach(TreeNode node2 in node.Nodes)
						{
							IUserExtension iue2 =node2.Tag as IUserExtension;
							if(iue2!=null  && node2.Checked!=iue2.ComAvailable)
								node2.Checked = iue2.ComAvailable;
						}
					}
					break;
			}

		}

		private void tree_BeforeCheck(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			TreeNode node = e.Node;
			IUserExtension iue = node.Tag as IUserExtension;
			if( iue is ContributionDefiner)
				e.Cancel = true;
			else
				iue.ComAvailable = !node.Checked;
		}

		private void list_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{			
			ListView _list = (ListView)sender;
			ListViewItem item = _list.Items[e.Index];
			IUserExtension iue = item.Tag as IUserExtension;
			if( iue is ContributionDefiner)
				e.NewValue = CheckState.Checked;
			else
				iue.ComAvailable = (e.NewValue==CheckState.Checked);
		}
	
		private TreeNode CreateTreeItem(Plugin p)
		{
			int icon = GetIconIndex(p);
			TreeNode node = new TreeNode(p.name, icon, icon, new TreeNode[]{new TreeNode("")} );
			node.Tag = p;				
			node.Checked = p.ComAvailable;
			return node;
		}
		#endregion
		
		internal class Sorter : IComparer
		{
			public int index;
			public bool bNumeric;

			public Sorter(int idx, bool bNumeric) 
			{
				index = idx;
				this.bNumeric = bNumeric;
			}

			public int Compare( object x, object y)
			{
				string sx = ((ListViewItem)x).SubItems[index].Text;
				string sy = ((ListViewItem)y).SubItems[index].Text;
				if( bNumeric ) 
				{
					return (int)(double.Parse(sy.Replace("%",""))-double.Parse(sx.Replace("%","")));
				}
				else
					return string.Compare(sx,sy);
			}
		}
	}
}
