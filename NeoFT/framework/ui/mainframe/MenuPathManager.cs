using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using nft.ui.command;
using nft.framework.plugin;

namespace nft.ui.mainframe
{
	/// <summary>
	/// MenuPathManager ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class MenuPathManager
	{
		public const char PathSepalator='\\';
		public const char GroupSepalator='|';
		protected readonly MainMenu root;
		protected readonly Hashtable table;

		public MenuPathManager(MainMenu root)
		{
			this.root = root;
			table = new Hashtable();
			//table.Add(root,new MenuCreationInfo("ROOT",null,null));
		}

		/// <summary>
		/// Do not use "ROOT" but ""
		/// </summary>
		/// <param name="info"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		public string AddMenu( MenuCreationInfo info, string path, string after, string before )
		{
			MenuItem newitem = new MenuItem(info.caption);
			Menu parent =FindOrCreate(path,info.caption);
			int n = FindNearestNeighbor(parent,info.id, after, before);
			parent.MenuItems.Add(n,newitem);
			table.Add(newitem,info);
			if(path.Trim().Length>0)
				return path+PathSepalator+info.id;
			else
				return info.id;
		}

		public bool SetCommand( string cmdID, ICommandEntity entity, string pathname )
		{
			Contribution c = PluginManager.theInstance.GetContribution(cmdID);
			Menu m = FindOrCreate(pathname,c!=null?c.Name:cmdID);
			MenuItem item = m as MenuItem;
			if( item==null || entity==null) return false;

			CommandUI cmdUI = CommandUI.GetCommandUI( cmdID );
			if( cmdUI ==null )
				cmdUI = new CommandUI( cmdID, entity );
			cmdUI.AddTrigger(CommandTriggerFactory.Create(item));
			return true;
		}

		public Menu FindOrCreate(string path, string newcap)
		{
			string[] elms = path.Split(new char[]{PathSepalator});
			Menu p = root;
			if( !path.Equals("") )
				for(int i=0; i<elms.Length; i++)
				{
					p = FindOrCreate(p,elms[i],newcap);
				}
			return p;
		}

		protected MenuItem FindOrCreate(Menu parent, string mid, string newcap)
		{
			MenuItem lastitem = null;
			foreach( MenuItem item in parent.MenuItems)
			{
				lastitem = item;
				if(!table.ContainsKey(item)) continue;
				if(getMenuID(item).Equals(mid))
					return item;
			}
			MenuItem newone = new MenuItem(newcap);
			int n = FindNearestNeighbor(parent,mid);
			parent.MenuItems.Add(n,newone);
			return newone;
		}

		protected string getMenuID(MenuItem item)
		{
			return ((MenuCreationInfo)table[item]).id;
		}

		protected string getGroupName(MenuItem item)
		{
			return getGroupName(getMenuID(item));
		}

		protected string getGroupName(string mid)
		{
			string[] s = mid.Split(new char[]{GroupSepalator},2);
			if(s.Length==1)
				return "";
			else
				return s[0];
		}

		protected int FindNearestNeighbor(Menu parent, string mid, string after, string before)
		{
			int idx=0;	
			string gn = getGroupName(mid);

			MenuItem ai=null;
			MenuItem bi=null;
			for(int n=0; n<parent.MenuItems.Count; n++)
			{
				MenuItem item = parent.MenuItems[n];
				if(!table.ContainsKey(item)) continue;

				if(getMenuID(item).Equals(after))
					ai = item;
				if(getMenuID(item).Equals(before))
					bi = item;				
			}
			int start = 0;
			int end = parent.MenuItems.Count;
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
				MenuItem item = parent.MenuItems[i];
				if(!table.ContainsKey(item)) 
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
				if( idx != parent.MenuItems.Count )
					parent.MenuItems.Add(idx,new MenuItem("-"));
				// insert sepalator before?
				if( idx != 0 && gabf )
					parent.MenuItems.Add(idx++,new MenuItem("-"));
			}
			return idx;
		}		

		protected int FindNearestNeighbor(Menu parent, string mid)
		{
			int idx=0;	
			bool find = false;
			string gn = getGroupName(mid);

			for(int  i=0; i<parent.MenuItems.Count; i++)
			{
				MenuItem item = parent.MenuItems[i];
				if(!table.ContainsKey(item)) continue;

				if(getGroupName(item).Equals(gn))
				{
					find = true;
					idx = i;
				}
				else if( find )			
					return idx;				
			}
			// insert sepalator
			if( !find && parent.MenuItems.Count > 0 )
				parent.MenuItems.Add(new MenuItem("-"));
			idx = parent.MenuItems.Count;
			return idx;
		}		

	}
}
