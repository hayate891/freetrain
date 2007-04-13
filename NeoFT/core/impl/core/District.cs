using System;
using System.Collections;
using System.Drawing;
using System.Diagnostics;
using nft.framework;
using nft.core;
using nft.core.game;

namespace nft.impl.game
{
	/// <summary>
	/// A region in the world, the map that can play at one time.
	/// </summary>
	public class District : IDistrict
	{
		public District(World w, ITerrainMap map, Rectangle area )
		{
			world = w;
			worldPos = area.Location;
			int us = world.UnitSize;
			mapSize = new Size(area.Width*us,area.Height*us);
		}

		#region IHasNameAndID ÉÅÉìÉo
		public string ID 
		{
			get	{ return string.Format("District:{0}-{1:00}{2:00}-{3}", world.ShortID, worldPos.X, worldPos.Y, ShortID);	}
		}
		protected readonly string id;
		public string ShortID 
		{
			get	{ return string.Format("{0:X}", this.GetHashCode());	}
		}

		public string Name
		{
			get	{ return name; }
		}
		protected string name;
		#endregion

		#region IDistrict ÉÅÉìÉo
		public World OwnerWorld { get{ return world; }}
		protected World world;

		public IOffGameProxy Proxy { get{ return null; }}

		public Point WorldLocation { get{ return worldPos; }}
		protected Point worldPos;

		public Size SizeInGrid 
		{ 
			get
			{ 
				int us = world.UnitSize;
				Debug.Assert(mapSize.Width%us==0 && mapSize.Height%us==0,"World Unit Size is not Matched!");
				return new Size(mapSize.Width/us,mapSize.Height/us); 
			}
		}

		public Size SizeInVoxel { get{ return mapSize; }}
		protected readonly Size mapSize;

		public void SetWorld(World w, int wx, int wy )
		{
			if( world !=null )
				throw new InvalidOperationException("Can't set more than one world on "+Name);
			world = w;
			worldPos = new Point(wx,wy);
		}
		#endregion

	}
}
