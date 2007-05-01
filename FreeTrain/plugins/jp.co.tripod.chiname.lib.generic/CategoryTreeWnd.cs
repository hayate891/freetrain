﻿using System;
using System.Drawing;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using freetrain.util.docking;
using freetrain.world;
using freetrain.world.accounting;

namespace freetrain.framework.plugin.generic
{
	public delegate void NodeSelectedEvent(StructCategory cat, int option);
	/// <summary>
	/// BankCounterForm の概要の説明です。
	/// </summary>
	public class CategoryTreeWnd : Form
	{
		#region generated by form editor

		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		#endregion
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.TreeView categoryTree;

		static public NodeSelectedEvent OnNodeSelected;
		static private CategoryTreeWnd theInstance = null;

		static public void ShowForm()
		{
			MenuItem parent = MenuItemConstants.VIEW.menuItem;
			MenuItem target = null;
			int n=parent.MenuItems.Count;
			for(int i=0;i<n;i++)
                if (parent.MenuItems[i].Text.Equals("Structure Type Tree"))
                //! if (parent.MenuItems[i].Text.Equals("建物類別一覧"))
				{
					target = parent.MenuItems[i];
					break;
				}
			if(!target.Checked) target.PerformClick();
//			if(theInstance.Parent != null )
//			{
//				Crownwood.Magic.Controls.TabControl t;
//				t = theInstance.Parent as Crownwood.Magic.Controls.TabControl;
//			}
		}	

		static public StructCategory getSelectedCategory(){
			return theInstance.Selected;
		}

		public CategoryTreeWnd()
		{
			theInstance = this;
			InitializeComponent();
			categoryTree.ImageList = StructCategory.icons;
			BuildCategories();
		}

		private void BuildCategories()
		{			
			IEnumerator ie = StructCategoryTree.theInstance.Categories.GetEnumerator();
			while(ie.MoveNext())
			{
				StructCategory cat = (StructCategory)ie.Current;
				GetNode(cat);
			}

		}
		
		public StructCategory Selected { 
			get {
				if( categoryTree.SelectedNode != null )
					return StructCategoryTree.theInstance[(int)categoryTree.SelectedNode.Tag]; 
				else
					return StructCategory.Root;
			} 
		}

		public TreeNode GetNode(StructCategory cat)
		{
			if(cat.HasParent())
			{
				TreeNode p = GetNode(cat.Parent);
				return GetNode(cat,p.Nodes);
			}
			else
			{
				return GetNode(cat,categoryTree.Nodes);	
			}
		}

		protected TreeNode GetNode(StructCategory cat, TreeNodeCollection nodes)
		{
			// Create new node
			TreeNode node = new TreeNode(cat.name,cat.iconIdx,cat.iconIdx);
			node.Tag = cat.idnum;
			for(int i=0; i<nodes.Count; i++ ) 
			{				
				if( (int)nodes[i].Tag==cat.idnum )
					return nodes[i];
			}

			//nodes.Insert(0,node);
			int j=0;
			// find nearest below node
			for(int i=0; i<nodes.Count; i++ ) 
			{				
				if( (int)nodes[i].Tag<(int)cat.idnum)
					j++;
			}
			nodes.Insert(j,node);
			return node;
		}

		private void EraseBlankNodes()
		{
			ArrayList arr = new ArrayList();
			foreach(TreeNode nd in categoryTree.Nodes)
				if((int)nd.Tag!=StructCategory.Root.idnum)
					EraseIfBlank(nd,arr);
			foreach(TreeNode cnd in arr)
				cnd.Remove();
		}

		protected bool EraseIfBlank(TreeNode nd,ArrayList arr)
		{
			int n = 0;
			foreach(TreeNode cnd in nd.Nodes)
				n += EraseIfBlank(cnd,arr)?0:1;
			int id = (int)nd.Tag;
			bool b = (n==0&&StructCategoryTree.theInstance[id].Entries.Count==0);
			if(b)
				arr.Add(nd);
			return b;
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			categoryTree.ImageList = null;	
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.categoryTree = new System.Windows.Forms.TreeView();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// categoryTree
			// 
			this.categoryTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
				| System.Windows.Forms.AnchorStyles.Left)
				| System.Windows.Forms.AnchorStyles.Right)));
			this.categoryTree.FullRowSelect = true;
			this.categoryTree.HideSelection = false;
			this.categoryTree.Location = new System.Drawing.Point(0, 26);
			this.categoryTree.Name = "categoryTree";
			this.categoryTree.Size = new System.Drawing.Size(128, 310);
			this.categoryTree.TabIndex = 0;
			this.categoryTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.categoryTree_AfterSelect);
			// 
			// checkBox1
			// 
			this.checkBox1.BackColor = System.Drawing.Color.Transparent;
			this.checkBox1.Checked = true;
			this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox1.Location = new System.Drawing.Point(8, -4);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(108, 34);
			this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "Show All";
            //! this.checkBox1.Text = "全表示";
			this.checkBox1.UseVisualStyleBackColor = false;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// CategoryTreeWnd
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(128, 333);
			this.Controls.Add(this.categoryTree);
			this.Controls.Add(this.checkBox1);
			this.MaximumSize = new System.Drawing.Size(400, 10832);
			this.MinimumSize = new System.Drawing.Size(100, 108);
			this.Name = "CategoryTreeWnd";
			this.Text = "Structure Type Tree";
            //! this.Text = "建物類別";
			this.ResumeLayout(false);

		}
		#endregion

		private void categoryTree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			MultiSelectorController.create(); //will show if created, create and show if not.
			StructCategory cat = StructCategoryTree.theInstance[(int)categoryTree.SelectedNode.Tag];
			OnNodeSelected(cat, 0);
		}

		private void checkBox1_CheckedChanged(object sender, System.EventArgs e)
		{
			if(checkBox1.Checked)
			{
				foreach(TreeNode nd in  categoryTree.Nodes)
					nd.Nodes.Clear();
				categoryTree.Nodes.Clear();
				BuildCategories();
			}
			else
				EraseBlankNodes();
		}

	}
}
