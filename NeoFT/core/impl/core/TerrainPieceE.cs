using System;
using nft.core.geometry;
using nft.core.game;

namespace nft.impl.game {
	/// <summary>
	/// TerrainPiece の概要の説明です。
	/// 地形エディタで使用するため、メモリ容量を喰うが、操作は高速。
	/// </summary>
	public class TerrainPieceE : ITerrainPiece {
		protected int height;
		protected int[] slope = new int[4];
		protected bool convex;
		protected int c_min;
		protected int c_max;
		protected int v_max;

		public TerrainPieceE(short baseHight, int ne, int se, int sw, int nw, bool convex) {
			this.height = (short)(convex?1:0);
			setSlope(ne,se,sw,nw);
			checkMinMax();
			this.BaseHeight = baseHight;
			Convex = convex;
		}

		public TerrainPieceE(ITerrainMap map, int x, int y) : this(0,
				map.Height(x+1,y,InterCardinalDirection.NORTHEAST),
				map.Height(x+1,y+1,InterCardinalDirection.SOUTHEAST),
				map.Height(x,y+1,InterCardinalDirection.SOUTHWEST),
				map.Height(x,y,InterCardinalDirection.NORTHWEST),
				false) 
		{
			int ave = map.Height(x+1,y,InterCardinalDirection.SOUTHWEST)+
				map.Height(x+1,y+1,InterCardinalDirection.NORTHWEST)+
				map.Height(x,y+1,InterCardinalDirection.NORTHEAST)+
				map.Height(x,y,InterCardinalDirection.SOUTHEAST);
			ave /= 4;
			this.Convex = (AverageHeight>=ave);
		}

		public TerrainPieceE(ITerrainPiece org) {
			this.Convex = org.Convex;
			this.BaseHeight = org.BaseHeight;
			for(int i=0; i<4; i++)
				slope[i] = org[InterCardinalDirection.NORTHEAST+i];
			checkMinMax();
		}

		public int BaseHeight {
			get { return height; }
			set { 
				int tmp = value;
				if((tmp&0x4000) !=0 ) 
					tmp=value|0x3fff;
				height = tmp;
			}
		}

		public int MaxHeight { 
			get{ return BaseHeight + v_max;	} 
		}		

		public int AverageHeight { 
			get{
				return height+((slope[0]+slope[1]+slope[2]+slope[3])>>2);
			} 
		}		

		public bool Convex {
			get{ return convex; } 
			set{ convex = value; } 
		}

		public int this[ InterCardinalDirection dir ]{
			get{ return getHeight(dir); }
		}

		public int getHeight( InterCardinalDirection dir ){
			return BaseHeight + getSlopeHeight(dir);
		}

		public int getSlopeHeight( InterCardinalDirection dir ) {
			int i = dir - InterCardinalDirection.NORTHEAST;
			return slope[i];
		}

		public void vertexUp( InterCardinalDirection dir ) {
			int i = dir - InterCardinalDirection.NORTHEAST;
			int v = slope[i];
			int v2;
			if(0 == v) {
				if( v_max <= 1)	{
					slope[i]++;
					c_max++;
					c_min--;
				}
				else {
					v2 = v_max>>1;
					slope[i] += v2;
					if(c_min==1) {
						for(int j=0; j<4; j++ )
							slope[j] -= v2;
						height += v2;
						c_min = 4-c_max;
					}
					else
						c_min--;			
				}
			}
			else if(v_max == v) {
				c_max = 1;
				v_max <<=1;
				if(v!=1) {
					v2 = v>>1;
					for(int j=0; j<4; j++ ) 
						slope[j]=(slope[j]&v2)<<1;
					slope[i]=v<<1;
				}
				else 
					slope[i] = 2;				
			}
			else { // medium height
				c_max++;
				slope[i] <<=1;
			}
		}

		public void vertexDown( InterCardinalDirection dir ) {
			int i = dir - InterCardinalDirection.NORTHEAST;
			int v = slope[i];
			int v2;
			if(0 == v) {
				c_min = 1;


				if( v_max == 0)	{
					slope[i]--;
					for(int j=0; j<4; j++ )
						slope[j]++;
					height--;
					v2 = 1;
					v_max = 1;
				}
				else {
					slope[i] -= v_max;
					v2 = v_max>>1;
					for(int j=0; j<4; j++ )
						slope[j] += v_max - (slope[j]&v2);
					height -= v_max;
					v_max<<=1;
				}
			}
			else if(v_max == v) {
				v2 = v>>1;
				slope[i] = v2;
				c_max--;
				if( v==1 )
					c_min = 4 - c_max;
				else if(c_max==0)
					c_max = 4-c_min;				
			}
			else { // medium height
				c_min++;
				slope[i] >>=1;
			}
		}

		public void setSlope( int ne, int se, int sw, int nw ) {
			slope[0] = ne;
			slope[1] = se;
			slope[2] = sw;
			slope[3] = nw;
			validate();
		}

		virtual protected void validate() {
			int b = TerrainUtil.CorrectVoxelVertices(ref slope);
			if(b!=0)
				BaseHeight += b;
		}

		virtual protected void checkMinMax() {
			v_max = slope[0];
			for(int i=1; i<4; i++) {
				if(v_max<slope[i])
					v_max = slope[i];
			}
			for(int i=0; i<4; i++) {
				if(0 == slope[i])
					c_min++;
				if(v_max == slope[i])
					c_max++;
			}
			if(v_max==0)
				c_max=0;
		}
	}
}
