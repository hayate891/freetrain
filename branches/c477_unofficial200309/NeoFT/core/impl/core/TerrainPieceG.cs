using System;
using nft.core.geometry;

namespace nft.impl.game
{
	/// <summary>
	/// TerrainPiece の概要の説明です。
	/// ゲーム中で使用するため、編集には時間がかかるが、メモリサイズを節約。
	/// </summary>
	public struct TerrainPieceG : ITerrainPiece
	{
		private const int NE_BIT = 0x000f; 
		private const int SE_BIT = 0x00f0; 
		private const int SW_BIT = 0x0f00; 
		private const int NW_BIT = 0xf000; 
		private const int NE_MASK = 0xfff0; 
		private const int SE_MASK = 0xff0f; 
		private const int SW_MASK = 0xf0ff; 
		private const int NW_MASK = 0x0fff;
		private static readonly int[] shift = new int[]{0,1,2,2,4,4,4,4,8,8,8,8,8,8,8,8};

		private short height;
		private ushort slope;

		public TerrainPieceG(short baseHight, int ne, int se, int sw, int nw, bool convex)
		{
			this.height = (short)(convex?1:0);
			slope = 0;
			setSlope(ne,se,sw,nw);
			this.BaseHeight = baseHight;	
		}

		public TerrainPieceG(ITerrainPiece org) {
			this.height = (short)(org.Convex?1:0);
			int tmp = (org[InterCardinalDirection.NORTHEAST]&NE_BIT)+
				((org[InterCardinalDirection.SOUTHEAST]&SE_BIT)<<4)+
				((org[InterCardinalDirection.SOUTHWEST]&SW_BIT)<<8)+
				((org[InterCardinalDirection.NORTHWEST]&NW_BIT)<<12);
			this.slope = (ushort)tmp;
			this.BaseHeight = org.BaseHeight;
		}

		public int BaseHeight 
		{
			get{ return height>>1; }
			set
			{ 
				int tmp = value;
				if((tmp&0x4000) !=0 ) 
					tmp=value|0x3fff;
				height = (short)(tmp*2 + height&1);
			}
		}

		public int MaxHeight { 
			// Be careful! this formula is ooptimised for only 0,1,2,4,8 values.
			get{
				int s = slope|(slope>>4);
				s |= s>>16;
				return BaseHeight + shift[s&0xf];
			} 
		}		

		public int AverageHeight { 
			get{
				int s = (slope&NE_BIT)+((slope&SE_BIT)>>4)+((slope&SW_BIT)>>8)+((slope&NW_BIT)>>16);
				return BaseHeight+s>>2;
			} 
		}		

		public bool Convex{
			get{ return (height&1)==1; }
			set{ if(value)
					 height =(short)(height|1);
				 else
					 height = (short)(height&0xfffe);
			}
		}

		public int this[ InterCardinalDirection dir ]{
			get { return getHeight(dir); }
		}

		public int getHeight( InterCardinalDirection dir ){
			return BaseHeight + getSlopeHeight(dir);
		}

		public int getSlopeHeight( InterCardinalDirection dir )
		{
			int i = dir - InterCardinalDirection.NORTHEAST;
			return slope&(NE_BIT<<i);
		}

		public void vertexUp( InterCardinalDirection dir ) {
			TerrainPieceE tmp = new TerrainPieceE(this);
			tmp.vertexUp(dir);
			int s = (tmp[InterCardinalDirection.NORTHEAST]&NE_BIT)+
				((tmp[InterCardinalDirection.SOUTHEAST]&SE_BIT)<<4)+
				((tmp[InterCardinalDirection.SOUTHWEST]&SW_BIT)<<8)+
				((tmp[InterCardinalDirection.NORTHWEST]&NW_BIT)<<12);
			this.slope = (ushort)s;
			BaseHeight = tmp.BaseHeight;
		}

		public void vertexDown( InterCardinalDirection dir ) {
			TerrainPieceE tmp = new TerrainPieceE(this);
			tmp.vertexDown(dir);
			int s = (tmp[InterCardinalDirection.NORTHEAST]&NE_BIT)+
				((tmp[InterCardinalDirection.SOUTHEAST]&SE_BIT)<<4)+
				((tmp[InterCardinalDirection.SOUTHWEST]&SW_BIT)<<8)+
				((tmp[InterCardinalDirection.NORTHWEST]&NW_BIT)<<12);
			this.slope = (ushort)s;			
			BaseHeight = tmp.BaseHeight;
		}

		public void setSlope( int ne, int se, int sw, int nw )
		{
			int[] work = new int[]{ne,se,sw,nw};
			int b = TerrainUtil.CorrectVoxelVertices(ref work);
			if( b!=0 )
				BaseHeight+=b;
			int tmp = (work[0]&NE_BIT)+((work[1]&SE_BIT)<<4)+((work[2]&SW_BIT)<<8)+((work[3]&NW_BIT)<<12);
			this.slope = (ushort)tmp;
		}
	}
}
