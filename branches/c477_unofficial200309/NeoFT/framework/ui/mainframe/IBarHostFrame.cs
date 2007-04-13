using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using nft.ui.command;

namespace nft.ui.mainframe
{
	public delegate void FileDroppedHandler( string filename );
	/// <summary>
	/// IMainFrame ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public interface IBarHostFrame
	{
		void AddFileDroppedHandler(FileDroppedHandler handler);

		string RegisterMenuNode(string idname, string path, string caption, string after, string before);

		/// <summary>
		/// Create ToolBarButton.
		/// if the bar specified name is not found, create new one.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="barname">name of the owner toolbar</param>
		/// <param name="after">the button after witch this button placed</param>
		/// <param name="before">the button before witch this button placed</param>
		void AddToolButton(ButtonCreationInfo info, string barname, string after, string before);

		/// <summary>
		/// Create MenuItem.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="path">the fullpath name of the parent menu node</param>
		/// <param name="after">the menuitem after witch this item placed</param>
		/// <param name="before">the menuitem before witch this item placed</param>
		void AddMenuItem(MenuCreationInfo info, string path, string after, string before);

		/// <summary>
		/// Set ICommandEntity as a click event handler of specified tool button.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="barname"></param>
		/// <param name="bid"></param>
		/// <returns></returns>
		CommandUI SetToolButtonCommand( string cmdID, ICommandEntity entity, string barname, string bid );

		/// <summary>
		/// Set ICommandEntity as a click event handler of specified menu item.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		CommandUI SetMenuCommand( string cmdID, ICommandEntity entity, string path );

		/// <summary>
		/// Register the form as MDI child frame of MainFrame.
		/// </summary>
		/// <param name="frame"></param>
		void RegisterChild(Form frame);

		Form[] MdiChildren{get;}
	}

	public class MenuCreationInfo
	{
		#region implementation
//		static protected readonly Hashtable table = new Hashtable();
//
//		static public MenuCreationInfo getInfo(string id)
//		{
//			return (MenuCreationInfo)table[id];
//		}		

		public readonly string id;
		public readonly string caption;
		public readonly string description;

		public MenuCreationInfo( string _id, string _caption, string _statustext)
		{
//			if( table.ContainsKey(_id) )
//				throw new Exception("menu id ["+_id+"] is already registerd.");
			id = _id;
			caption = _caption;
			description = _statustext;
//			table.Add(_id,this);
		}
		#endregion
	}

	public class ButtonCreationInfo
	{
		#region implementation
//		static protected readonly Hashtable table = new Hashtable();
//
//		static public ButtonCreationInfo getInfo(string id)
//		{
//			return (ButtonCreationInfo)table[id];
//		}		

		public readonly string id;
		public readonly string tooltip;
		internal readonly string image_path="";
		internal readonly int imgidx_offset;
		internal int imgidx_real = -1;

		public ButtonCreationInfo( string _id, string _tooltip, string imgPath)
			: this( _id, _tooltip, imgPath, 0 )
		{
		}

		public ButtonCreationInfo( string _id, string _tooltip, string imgPath, int idx)
		{
//			if( table.ContainsKey(_id) )
//				throw new Exception("button id ["+_id+"] is already registerd.");
			if( imgPath==null )
				throw new Exception("null image is not allowed for button["+_id+"].");
			id = _id;
			tooltip = _tooltip;
			image_path = imgPath;
			imgidx_offset = idx;
//			table.Add(_id,this);
		}
		#endregion
	}

	public class CyclicButtonCreationInfo : ButtonCreationInfo
	{
		#region implementation
		internal readonly string[] images_path;
		internal readonly int[] imgidcs_offset;
		internal int[] imgidcs_real;

		public CyclicButtonCreationInfo(string _id, string _tooltip, string[] imgPaths, int[] indices)
			:base(_id,_tooltip,imgPaths[0],indices[0])
		{
			imgidcs_offset = indices;
			if(imgPaths.Length != indices.Length)
				images_path = new string[indices.Length];
			else
				images_path = imgPaths;
			imgidcs_real = new int[indices.Length];
			
			for(int i=0; i<indices.Length; i++ )
			{
				imgidcs_real[i]=-1;
				if( imgPaths.Length>i && imgPaths[i]!=null)
					images_path[i] = imgPaths[i];
				else
					images_path[i] = images_path[i-1];
			}
		}
		#endregion
	}

}
