using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using freetrain.contributions.road;
using freetrain.framework;
using freetrain.framework.plugin;
using freetrain.views.map;
using freetrain.world;
using freetrain.world.road;
using freetrain.contributions.common;

namespace freetrain.controllers.road
{
	/// <summary>
	/// Controller to place/remove roads
	/// </summary>
	public class RoadController : AbstractLineController
	{
		public RoadController() : base(null) {
			
		}

		private RoadContribution currentContrib;
		private TreeNode lastValidNode;
		private int currentPattern = 0;
		
		protected override LineContribution type
		{	get { return currentContrib; }	}


		protected override void draw( Direction d, DrawContextEx canvas, Point pt ) 
		{
			ResourceUtil.emptyChip.drawShape( canvas.surface, pt, Color.Blue );
//			RoadPattern.getStraight(d).drawAlpha( canvas.surface, pt );
		}

		#region Windows Form Designer generated code
		private System.Windows.Forms.Label level;
		private System.Windows.Forms.Label description;
		private System.Windows.Forms.TreeView contribTree;

		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		protected override void InitializeComponent()
		{
			base.InitializeComponent();
			this.SuspendLayout();
			this.buttonRemove.Location = new System.Drawing.Point(192, 132);
			this.buttonPlace.Location = new System.Drawing.Point(136, 132);
			this.picture.Location = new System.Drawing.Point(136, 40);
			this.picture.Size = new System.Drawing.Size(104, 88);
			this.picture.Click+=new EventHandler(picture_Click);
			this.toolTip.SetToolTip(this.picture, "クリックで別のパターンを表示");
			// 
			// contribTree
			// 
			this.contribTree = new System.Windows.Forms.TreeView();
			this.contribTree.FullRowSelect = true;
			this.contribTree.HideSelection = false;
			this.contribTree.ImageIndex = -1;
			this.contribTree.Location = new System.Drawing.Point(0, 0);
			this.contribTree.Name = "contribTree";
			// set contribution tree on TreeView
			makeContribTree();
			this.contribTree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.contribTree.SelectedImageIndex = -1;
			this.contribTree.ShowLines = false;
			this.contribTree.ShowRootLines = true;
			this.contribTree.Indent = 10;
			this.contribTree.Sorted = true;
			this.contribTree.Size = new System.Drawing.Size(128, 159);
			this.contribTree.TabIndex = 0;
			this.contribTree.AfterSelect+=new TreeViewEventHandler(contribTree_AfterSelect);
			// 
			// description
			// 
			this.description = new Label();
			this.description.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.description.Location = new System.Drawing.Point(132, 4);
			this.description.Name = "description";
			this.description.Size = new System.Drawing.Size(112, 16);
			this.description.TabIndex = 9;
			this.description.Text = currentContrib.oneLineDescription;
			this.description.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTip.SetToolTip(this.description, this.description.Text);
			// 
			// level
			// 
			this.level = new System.Windows.Forms.Label();
			this.level.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.level.Location = new System.Drawing.Point(132, 22);
			this.level.Name = "level";
			this.level.Size = new System.Drawing.Size(112, 16);
			this.level.TabIndex = 10;
			this.level.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// RoadController
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(248, 160);
			this.Controls.Add(this.description);
			this.Controls.Add(this.level);
			this.Controls.Add(this.contribTree);
			this.Text = "道路工事";
			this.ResumeLayout(false);

		}
		#endregion

		private void picture_Click(object sender, EventArgs e)
		{
			currentPattern++;
			if( currentPattern > 2 ) currentPattern = 0;
			currentContrib.PreviewPatternIdx = currentPattern;
			updatePreview();
		}

		private void makeContribTree()
		{
			RoadContribution[] contribs = Core.plugins.roads;
			if(contribs.Length>0)
			{
				currentContrib = contribs[0];

				for(int idx = 0; idx<contribs.Length; idx++ )
				{
					RoadContribution rc = contribs[idx];
					string[] path = rc.name.Split(new char[]{'(',')','（','）','/','\\'});
					TreeNodeCollection parent = contribTree.Nodes;
					TreeNode node = null;
					int m = path.Length-1;
					for(int i = 0; i<=m; i++ )
					{
						string label = path[i].Trim();
						if(label.Length==0) continue;
						bool find = false;
						foreach(TreeNode n in parent)
						{
							if(n.Text.Equals(label))
							{
								find = true;
								node = n;
								break;
							}
						}
						if(!find)
						{
							node = new TreeNode(label);
							parent.Add(node);
						}
						if(node.Tag==null || m==i)
							node.Tag = rc;
						parent = node.Nodes;
					}					
					lastValidNode = node;
				}
			}
		}

		private void contribTree_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if( contribTree.SelectedNode == null )
			{
				contribTree.SelectedNode = lastValidNode;
			}
			int idxnew;
			if(contribTree.SelectedNode.Tag == null )
				idxnew = contribTree.SelectedNode.Nodes[0].Index;
			else
			{
				lastValidNode = contribTree.SelectedNode;
				idxnew = lastValidNode.Index;
			}
			if( currentContrib == lastValidNode.Tag )
				return;
			currentContrib = (RoadContribution)lastValidNode.Tag;
			description.Text = currentContrib.oneLineDescription;
			level.Text = ToStyleDescription(currentContrib.style);
			toolTip.SetToolTip(this.description, this.description.Text);
			currentContrib.PreviewPatternIdx = currentPattern;
			this.Text = type.name;			
			updatePreview();
		}

		static private string ToStyleDescription(RoadStyle style)
		{
			string text = new string[]{"未定義","小道","街路","高速道"}[(int)style.Type];
			if(style.Type==MajorRoadType.street||style.Type==MajorRoadType.highway)
			{
				if(style.CarLanes>0)
					text = string.Format("{0}車線{1}",style.CarLanes,text);
				if(style.Sidewalk==SidewalkType.pavement)
					text = "歩道付き"+text;
			}
			return text;
		}
	}
}
