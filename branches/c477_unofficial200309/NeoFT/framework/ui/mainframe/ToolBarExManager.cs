using System;
using System.Collections;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using UtilityLibrary.CommandBars;
using nft.ui.command;

namespace nft.ui.mainframe
{
	/// <summary>
	/// ToolBarManager ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class ToolBarExManager
	{
		static public readonly Color DefaultTranspalentColor = Color.Magenta;
		public const char GroupSepalator='|';
		protected readonly Hashtable table;
		protected readonly ReBar reBar;

		public ToolBarExManager(ReBar bar)
		{
			this.reBar = bar;
			table = new Hashtable();
		}

		/// <summary>
		/// Add new tool button
		/// </summary>
		/// <param name="info"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		public ToolBarItem AddNewButton( ButtonCreationInfo info, string barname, string after, string before )
		{
			
			ToolBarEx target;
			if( table.ContainsKey(barname) )
				target = FindBar(barname);
			else
				target = CreateNewToolBar(barname, DefaultTranspalentColor, new Size(16,16) );

			// create and add to tool bar.
			ToolBarItem newitem = CreateToolBarItem(info,target);
			int n = FindNearestNeighbor(target, info.id, after, before);
			target.Items.Insert(n,newitem);
			return newitem;
		}

		public bool SetCommand( string cmdID, ICommandEntity entity, string barname, string bid )
		{
			if( !table.ContainsKey(barname) || entity==null ) return false;
			foreach(ToolBarItem item in this[barname].bar.Items)
			{
				if( item.Tag== null ) continue;
				if( item.Tag.ToString().Equals(bid) )
				{
					CommandUI cmdUI = CommandUI.GetCommandUI( cmdID );
					if( cmdUI ==null )
						cmdUI = new CommandUI( cmdID, entity );
					cmdUI.AddTrigger(CommandTriggerFactory.Create(item));
					return true;
				}
			}
			return false;
		}

		public ToolBarEx FindBar( string name )
		{
			return this[name].bar;
		}

		public void AddNewToolBar( ToolBarEx bar )
		{
			table.Add(bar.Name,new BarInfo(bar));
			if(!reBar.Bands.Contains(bar))
				reBar.Bands.Add(bar);
		}

		public ToolBarEx CreateNewToolBar( string barname, Color transpalent, Size buttonsize )
		{
			ToolBarEx bar = new ToolBarEx(BarType.ToolBar,true);
			reBar.Bands.Add(bar);
			bar.Name = barname;
			bar.Dock = DockStyle.Top;
			bar.ImageList = new ImageList();
			bar.ImageList.TransparentColor = transpalent;
			bar.ImageList.ImageSize = buttonsize;
			bar.Disposed+=new EventHandler(bar_Disposed);
			table.Add(barname,new BarInfo(bar));
			return bar;
		}

		/// <summary>
		/// create tool button.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="bar"></param>
		/// <returns></returns>
		protected ToolBarItem CreateToolBarItem(ButtonCreationInfo info,ToolBarEx bar)
		{
			ToolBarItem newitem = new ToolBarItem();
			newitem.ToolTip = info.tooltip;
			newitem.Tag = info.id;
			info.imgidx_real = this[bar.Name].resolver.GetRealImageIndex( info.image_path, info.imgidx_offset );
			newitem.ImageListIndex = info.imgidx_real;
			//bar.Items.Add(newitem);
			return newitem;
		}

		protected BarInfo this[string name]
		{
			get{ return (BarInfo)table[name]; }
			set{ table.Add(name, value); }
		}


		protected string getGroupName(ToolBarItem item)
		{
			return getGroupName(item.Tag.ToString());
		}

		protected string getGroupName(string bid)
		{
			string[] s = bid.Split(new char[]{GroupSepalator},2);
			if(s.Length==1)
				return "";
			else
				return s[0];
		}

		protected int FindNearestNeighbor(ToolBarEx parent, string bid, string after, string before)
		{
			int idx=0;	
			string gn = getGroupName(bid);

			ToolBarItem ai=null;
			ToolBarItem bi=null;
			for(int n=0; n<parent.Items.Count; n++)
			{
				ToolBarItem item = parent.Items[n];
				if(item.Tag==null) continue;
				if(item.Tag.ToString().Equals(after))
					ai = item;
				if(item.Tag.ToString().Equals(before))
					bi = item;				
			}
			int start = 0;
			int end = parent.Items.Count;
			bool gaf = false;
			bool gbf = false;
			string gna = null;
			string gnb = null;
			bool gabf = (gna==null&&gnb==null);
			if( ai!=null )
			{
				start = ai.Index;
				gna = getGroupName(ai);
				gaf = gna.Equals(gn);
			}
			if( bi!=null )
			{
				end = bi.Index;
				gnb = getGroupName(bi);
				gbf = gnb.Equals(gn);
				if( ai!=null )
					gabf = gnb.Equals(gna);
			}

			int i;
			bool find = gaf;
			idx = end;
			for( i=start; i<end; i++)
			{
				ToolBarItem item = parent.Items[i];
				if(item.Tag==null) 
					if(find)
					{		
						idx = i;
						break;
					}
					else
						continue;

				if(getGroupName(item).Equals(gn))
					find = true;
				else if( find )	
				{		
					idx = i;
					break;
				}
			}

			if( !find && !gbf )
			{
				// insert sepalator after?
				if( idx != parent.Items.Count )
					parent.Items.Insert(idx,NewSepalator());
				// insert sepalator before?
				if( idx != 0 && gabf )
					parent.Items.Insert(idx++,NewSepalator());
			}
			return idx;
		}		

		protected int FindNearestNeighbor(ToolBarEx parent, string bid)
		{
			int idx=0;	
			bool find = false;
			string gn = getGroupName(bid);

			for(int  i=0; i<parent.Items.Count; i++)
			{
				ToolBarItem item = parent.Items[i];
				if(item.Tag==null) continue;

				if(getGroupName(item).Equals(gn))
				{
					find = true;
					idx = i;
				}
				else if( find )			
					return idx;				
			}
			// insert sepalator
			if( !find && parent.Items.Count > 0 )
				parent.Items.Add(NewSepalator());
			idx = parent.Items.Count;
			return idx;
		}

		protected ToolBarItem NewSepalator()
		{
			ToolBarItem item = new ToolBarItem();
			item.Style = ToolBarItemStyle.Separator;
			return item;
		}
		private void bar_Disposed(object sender, EventArgs e)
		{
			ToolBarEx bar = (ToolBarEx)sender;
			bar.ImageList.Dispose();
		}
		protected class BarInfo
		{
			public readonly ToolBarEx bar;
			public readonly ImageListIndexResolver resolver;

			public BarInfo(ToolBarEx bar)
			{
				this.bar = bar;
				if(bar.ImageList==null)
					throw new Exception("the Bar must have ImageList");
				resolver = new ImageListIndexResolver(bar.ImageList);
			}
		}
	}
}
