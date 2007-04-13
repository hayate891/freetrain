using System;
using System.Drawing;
using System.Diagnostics;
using nft.core.game;
using nft.core.geometry;

namespace nft.impl.game
{
	/// <summary>
	/// TerrainMapImplは実際に地形データを配列に展開して保持する。
	/// </summary>
	[Serializable]
	public class TerrainMapImpl : ITerrainMap {
		public TerrainMapImpl(ITerrainMap source, Rectangle region ) {
			heightMax = source.MaxHeight;
			size = region.Size;
			int width = (size.Width>>1)+size.Width&1;
			int height = (size.Height>>1)+size.Height&1;
			hmap = new TerrainPieceE[width,height];
			int ox = region.Left;
			int oy = region.Top;			

			for(int x=0; x<width; x++)
				for(int y=0; y<height; y++) {						
					hmap[x,y] = new TerrainPieceE(source,ox+(x<<1),oy+(y<<1));
				}
		}
		#region ITerrainMap メンバ
		public Size Size { get{ return size; } }
		protected Size size;

		public short MaxHeight { get { return heightMax; } }
		protected short heightMax;

		public short Height(int x, int y, InterCardinalDirection d) {
			int hx = x>>1;
			int hy = y>>1;
			try {
				if((x&1)!=0){
					if((y&1)!=0){
						return (short)hmap[hx,hy].AverageHeight;
					}
					return (short)((hmap[hx,hy][d]+hmap[hx,hy][(InterCardinalDirection)(((int)d)^2)])>>1);
				}else if((y&1)!=0){
					return (short)((hmap[hx,hy][d]+hmap[hx,hy][(InterCardinalDirection)(((int)d)^1)])>>1);
				}else
					return (short)hmap[hx,hy][d];
			}
			catch(IndexOutOfRangeException) {
				string t = string.Format("Height({0},{1}): argument out of range.",x,y);
				Debug.WriteLine(t);
				return -1;
			}
		}
		protected TerrainPieceE[,] hmap;

		public ITerrainPiece this[int x, int y]{
			get{ return hmap[x>>1,y>>1]; }
		}

		public bool IsDetailedHeight { get { return true; } }

		public short WaterDepth(int x, int y)
		{
			try
			{
				return water[x,y];
			}
			catch(IndexOutOfRangeException)
			{
				string t = string.Format("WaterDepth({0},{1}): argument out of range.",x,y);
				Debug.WriteLine(t);
				return -1;
			}
		}
		protected short[,] water;

		public Rectangle[] Districts
		{
			get	{ return districts; }
			set	{ districts = value; }
		}
		protected Rectangle[] districts;
		
		#endregion
	}
}
