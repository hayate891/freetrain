using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace nft.ui.mainframe
{
	/// <summary>
	/// Manages Image index in attached ImageList control.
	/// 
	/// </summary>
	public class ImageListIndexResolver
	{
		protected ImageList images;
		protected Hashtable img_table;

		public ImageListIndexResolver(ImageList ilist)
		{
			images = ilist;
			img_table = new Hashtable();
		}

		public int GetRealImageIndex(string imagepath, int idx_offset)
		{
			if( !img_table.ContainsKey(imagepath) )
			{
				try
				{
					Image newimage = Image.FromFile(imagepath);
					int n = images.Images.AddStrip(newimage);
					img_table.Add(imagepath,n);
					return n+idx_offset;
				}
				catch(Exception e)
				{
					Console.WriteLine(e.Message);
					Console.WriteLine(e.StackTrace);
					return -1;
				}
			}
			else
			{
				int n = (int)img_table[imagepath];
				return n+idx_offset;
			}
		}
	}
}
