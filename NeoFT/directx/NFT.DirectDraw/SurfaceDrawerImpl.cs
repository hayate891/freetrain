using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using DxVBLib;
using nft.framework.drawing;

namespace nft.drawing.ddraw7
{

	#region Intersecter
	class IntersectDrawer : ISurfaceDrawer{
		public void Blt(ref DrawingDesc desc ){
			int mask;
			switch(desc.Dest.Mode){
				case PixelColorMode.RGB16Bit565:
					mask = 0xf7de;
					break;
				case PixelColorMode.RGB16Bit555:
					mask = 0x7bde;
					break;
				default:
					mask = 0xfefefe;
					break;
			}
			if(desc.Stretch>0)
				BltIntersectStretch(ref desc);
			else
				BltIntersect(ref desc);
		}

		private static unsafe void BltIntersect(ref DrawingDesc desc){
			Int32 colKey = desc.Src.ColorKey;
			Int32 colKey2 = desc.Dest.ColorKey;
			Int32 pixMask = desc.Src.PixelMask;
			Int32 pixNegMask = ~pixMask;
			int lpSrc = desc.Src.lpOffset;
			int lpDst = desc.Dest.lpOffset;
			int y = 0;
			while(++y<= desc.RowSteps) {
				// 
				// Alpha-blend the pixels in the current row.
				//
				int x = 0;
				int lpSrc2 = lpSrc;
				int lpDst2 = lpDst;
				while ( ++x<= desc.ColumnSteps ) {
					// Read in the next source pixel.
					Int32 vSrc = *(( Int32*)lpSrc2 );
					// If the source pixel is not black ...
					if ( ( vSrc & pixMask ) != colKey ) {
						// ... read in the next target pixel.
						Int32 vDst = *(( Int32*)lpDst2 );
						if( ( vDst & pixMask ) != colKey2 ){ 
							// Write the destination pixel.
							*(( Int32* )lpDst2) = (vDst&pixNegMask)|vSrc;
						}
					}
					// Proceed to the next pixel.
					lpDst2 += desc.Dest.PixelPitch;
					lpSrc2 += desc.Src.PixelPitch;
				}
				// Proceed to the next line.
				lpDst += desc.Dest.RowPitch;
				lpSrc += desc.Src.RowPitch;
			};
		}

		private static readonly int[] patternArray = { 
														 (1<<1)+(1<<3),//1
														 (1<<2)+(1<<5)+(1<<8),//2
														 (1<<3)+(1<<7)+(1<<11)+(1<<15),//3
														 (1<<4)+(1<<9)+(1<<14)+(1<<19)+(1<<24)//4
													 };
		private static unsafe void BltIntersectStretch(ref DrawingDesc desc){
			Int32 colKey = desc.Src.ColorKey;
			Int32 colKey2 = desc.Dest.ColorKey;
			Int32 pixMask = desc.Src.PixelMask;
			Int32 pixNegMask = ~pixMask;
			int n = Math.Min(desc.Stretch,4);
			int pattern = patternArray[n-1];
			n++;
			int dstColPitch = desc.Dest.PixelPitch*n;
			int dstRowPitch = desc.Dest.RowPitch*n;
			int dstPadding = desc.Dest.RowPitch-dstColPitch;
			n*=n;
			int lpSrc = desc.Src.lpOffset;
			int lpDst = desc.Dest.lpOffset;
			int y = 0;
			while(++y<= desc.RowSteps) {
				// 
				// Alpha-blend the pixels in the current row.
				//
				int x = 0;
				int lpSrc2 = lpSrc;
				int lpDst2 = lpDst;
				while ( ++x<= desc.ColumnSteps ) {
					// Read in the next source pixel.
					Int32 vSrc = *(( Int32*)lpSrc2 );
					// If the source pixel is not black ...
					if ( ( vSrc & pixMask ) != colKey ) {
						int lpDst3 = lpDst2;
						int c = n;
						int p = pattern;
						while(c-->0){
							// ... read in the next target pixel.
							Int32 vDst = *(( Int32*)lpDst3 );
							if(( vDst & pixMask ) != colKey2 ) {
								// Write the destination pixel.
								*(( Int32* )lpDst3) = (vDst&pixNegMask)|vSrc;
							}
							lpDst3+=desc.Dest.PixelPitch;
							lpDst3+=dstPadding*(p&1);
							p>>=1;
						}
					}
					// Proceed to the next pixel.
					lpDst2 += dstColPitch;
					lpSrc2 += desc.Src.PixelPitch;
				}
				// Proceed to the next line.
				lpDst += dstRowPitch;
				lpSrc += desc.Src.RowPitch;
			};		}
	}

	#endregion

	#region HalfAlpha
	class HalfAlphaDrawer : ISurfaceDrawer{
		public void Blt(ref DrawingDesc desc ){
			int mask;
			switch(desc.Dest.Mode){
				case PixelColorMode.RGB16Bit565:
					mask = 0xf7de;
					break;
				case PixelColorMode.RGB16Bit555:
					mask = 0x7bde;
					break;
				default:
					mask = 0xfefefe;
					break;
			}
			if(desc.Stretch>0)
				BltStretch(ref desc, mask);
			else
				Blt(ref desc, mask);
		}

		private static unsafe void Blt(ref DrawingDesc desc, int mask){
			Int32 colKey = desc.Src.ColorKey;
			Int32 pixMask = desc.Src.PixelMask;
			Int32 pixNegMask = ~pixMask;
			int lpSrc = desc.Src.lpOffset;
			int lpDst = desc.Dest.lpOffset;
			int y = 0;
			while(++y<= desc.RowSteps) {
				// 
				// Alpha-blend the pixels in the current row.
				//
				int x = 0;
				int lpSrc2 = lpSrc;
				int lpDst2 = lpDst;
				while ( ++x<= desc.ColumnSteps ) {
					// Read in the next source pixel.
					Int32 vSrc = *(( Int32*)lpSrc2 );
					// If the source pixel is not black ...
					if ( ( vSrc & pixMask ) != colKey ) {
						// ... read in the next target pixel.
						Int32 vDst = *(( Int32*)lpDst2 );

						// Calculate the destination pixel.
						vSrc = ( ( vDst & mask ) + ( vSrc & mask ))>> 1; 
						// Write the destination pixel.
						*(( Int32* )lpDst2) = (vDst&pixNegMask)|vSrc;
					}
					// Proceed to the next pixel.
					lpDst2 += desc.Dest.PixelPitch;
					lpSrc2 += desc.Src.PixelPitch;
				}
				// Proceed to the next line.
				lpDst += desc.Dest.RowPitch;
				lpSrc += desc.Src.RowPitch;
			};
		}

		private static readonly int[] patternArray = { 
														 (1<<1)+(1<<3),//1
														 (1<<2)+(1<<5)+(1<<8),//2
														 (1<<3)+(1<<7)+(1<<11)+(1<<15),//3
														 (1<<4)+(1<<9)+(1<<14)+(1<<19)+(1<<24)//4
													 };
		private static unsafe void BltStretch(ref DrawingDesc desc, int mask){
			Int32 colKey = desc.Src.ColorKey;
			Int32 pixMask = desc.Src.PixelMask;
			Int32 pixNegMask = ~pixMask;
			int n = Math.Min(desc.Stretch,4);
			int pattern = patternArray[n-1];
			n++;
			int dstColPitch = desc.Dest.PixelPitch*n;
			int dstRowPitch = desc.Dest.RowPitch*n;
			int dstPadding = desc.Dest.RowPitch-dstColPitch;
			n*=n;
			int lpSrc = desc.Src.lpOffset;
			int lpDst = desc.Dest.lpOffset;
			int y = 0;
			while(++y<= desc.RowSteps) {
				// 
				// Alpha-blend the pixels in the current row.
				//
				int x = 0;
				int lpSrc2 = lpSrc;
				int lpDst2 = lpDst;
				while ( ++x<= desc.ColumnSteps ) {
					// Read in the next source pixel.
					Int32 vSrc = *(( Int32*)lpSrc2 );
					// If the source pixel is not black ...
					if ( ( vSrc & pixMask ) != colKey ) {
						int lpDst3 = lpDst2;
						int c = n;
						int p = pattern;
						vSrc &= mask;
						while(c-->0){
							// ... read in the next target pixel.
							Int32 vDst = *(( Int32*)lpDst3 );
							// Calculate the destination pixel.
							Int32 vTmp = ( ( vDst & mask ) + vSrc)>> 1; 
							// Write the destination pixel.
							*(( Int32* )lpDst3) = (vDst&pixNegMask)|vTmp;
							lpDst3+=desc.Dest.PixelPitch;
							lpDst3+=dstPadding*(p&1);
							p>>=1;
						}
					}
					// Proceed to the next pixel.
					lpDst2 += dstColPitch;
					lpSrc2 += desc.Src.PixelPitch;
				}
				// Proceed to the next line.
				lpDst += dstRowPitch;
				lpSrc += desc.Src.RowPitch;
			};		}
	}

	#endregion

	#region SelectBrighter
	class SelectBrighterDrawer : ISurfaceDrawer{
		public void Blt(ref DrawingDesc desc ){
			int mask;
			int depth;
			switch(desc.Dest.Mode){
				case PixelColorMode.RGB16Bit565:
					mask = 0xf7de;
					depth = 5;
					break;
				case PixelColorMode.RGB16Bit555:
					mask = 0x7bde;
					depth = 5;
					break;
				default:
					mask = 0xfefefe;
					depth = 8;
					break;
			}
			if(desc.Stretch>0)
				BltStretch(ref desc, mask, depth);
			else
				Blt(ref desc, mask,depth);
		}

		private static unsafe void Blt(ref DrawingDesc desc, int mask, int depth){	
			Int32 channelMask = (1<<depth)-1;
			Int32 colKey = desc.Src.ColorKey;
			Int32 pixMask = desc.Src.PixelMask;
			Int32 pixNegMask = ~pixMask;
			Int32 flg = (~mask)&(pixMask<<1);
			int lpSrc = desc.Src.lpOffset;
			int lpDst = desc.Dest.lpOffset;
			int y = 0;
			while(++y<= desc.RowSteps) {
				// 
				// Alpha-blend the pixels in the current row.
				//
				int x = 0;
				int lpSrc2 = lpSrc;
				int lpDst2 = lpDst;
				while ( ++x<= desc.ColumnSteps ) {
					// Read in the next source pixel.
					Int32 vSrc = *(( Int32*)lpSrc2 );
					// If the source pixel is not black ...
					if ( ( vSrc & pixMask ) != colKey ) {
						// ... read in the next target pixel.
						Int32 vDst = *(( Int32*)lpDst2 );

						// Calculate the destination pixel.
						Int32 work = vDst|flg;
						work -= vSrc&mask;
						work &= flg;
						work *= channelMask;
						work >>= depth;

						vSrc = (vDst&work) + (vSrc&~work); 
						// Write the destination pixel.
						*(( Int32* )lpDst2) = (vDst&pixNegMask)|vSrc;
					}
					// Proceed to the next pixel.
					lpDst2 += desc.Dest.PixelPitch;
					lpSrc2 += desc.Src.PixelPitch;
				}
				// Proceed to the next line.
				lpDst += desc.Dest.RowPitch;
				lpSrc += desc.Src.RowPitch;
			};
		}

		private static readonly int[] patternArray = { 
														 (1<<1)+(1<<3),//1
														 (1<<2)+(1<<5)+(1<<8),//2
														 (1<<3)+(1<<7)+(1<<11)+(1<<15),//3
														 (1<<4)+(1<<9)+(1<<14)+(1<<19)+(1<<24)//4
													 };
		private static unsafe void BltStretch(ref DrawingDesc desc, int mask, int depth){
			Int32 channelMask = (1<<depth)-1;
			Int32 colKey = desc.Src.ColorKey;
			Int32 pixMask = desc.Src.PixelMask;
			Int32 pixNegMask = ~pixMask;
			Int32 flg = (~mask)&(pixMask<<1);
			int n = Math.Min(desc.Stretch,4);
			int pattern = patternArray[n-1];
			n++;
			int dstColPitch = desc.Dest.PixelPitch*n;
			int dstRowPitch = desc.Dest.RowPitch*n;
			int dstPadding = desc.Dest.RowPitch-dstColPitch;
			n*=n;
			int lpSrc = desc.Src.lpOffset;
			int lpDst = desc.Dest.lpOffset;
			int y = 0;
			while(++y<= desc.RowSteps) {
				// 
				// Alpha-blend the pixels in the current row.
				//
				int x = 0;
				int lpSrc2 = lpSrc;
				int lpDst2 = lpDst;
				while ( ++x<= desc.ColumnSteps ) {
					// Read in the next source pixel.
					Int32 vSrc = *(( Int32*)lpSrc2 );
					// If the source pixel is not black ...
					if ( ( vSrc & pixMask ) != colKey ) {
						int lpDst3 = lpDst2;
						int c = n;
						int p = pattern;
						vSrc &= mask;
						while(c-->0){
							// ... read in the next target pixel.
							Int32 vDst = *(( Int32*)lpDst3 );
							// Calculate the destination pixel.
							Int32 work = (vDst&mask)|flg;
							work -= vSrc&mask;
							work &= flg;
							work *= channelMask;
							work >>= depth;

							work = (vDst&work) + (vSrc&~work); 
							// Write the destination pixel.
							*(( Int32* )lpDst3) = (vDst&pixNegMask)|work;
							lpDst3+=desc.Dest.PixelPitch;
							lpDst3+=dstPadding*(p&1);
							p>>=1;
						}
					}
					// Proceed to the next pixel.
					lpDst2 += dstColPitch;
					lpSrc2 += desc.Src.PixelPitch;
				}
				// Proceed to the next line.
				lpDst += dstRowPitch;
				lpSrc += desc.Src.RowPitch;
			};		}
	}

	#endregion

	#region ColorBurn
	class ColorBurnDrawer : ISurfaceDrawer{
		protected float apply;
		protected Color color;
		protected PixelColorMode prev = PixelColorMode.Unknown;
		protected Int32[] masks = new Int32[4];

		public ColorBurnDrawer(Color col, float apply){
			this.apply = apply;
			this.color = col;
		}
		
		public ColorBurnDrawer() : this(Color.Black,0.5f){
		}

		public float Apply { 
			get{ return apply; }
			set{ apply = value; prev = PixelColorMode.Unknown; }
		}
		
		public Color BurnColor { 
			get{ return color; }
			set{ color = value; prev = PixelColorMode.Unknown; }
		}

		public void Blt(ref DrawingDesc desc ){
			if(prev != desc.Dest.Mode){
				prev = desc.Dest.Mode;
				int v = (int)(255*apply)&255;
				int R = (int)(color.R*apply);
				int G = (int)(color.G*apply);
				int B = (int)(color.B*apply);
				switch(desc.Dest.Mode){
					case PixelColorMode.RGB16Bit565:
						masks[0] = 0xf7de;
						masks[1] = (v&0xf8)<<3;
						v = (v&0xf)>>3;
						masks[1] |=  (v<<11) + v;
						masks[2] = (G&0xfc)<<3;
						masks[2] |=(R&0xf8)<<8;
						masks[2] |=(B>>3);
						masks[3] = 5;
						break;
					case PixelColorMode.RGB16Bit555:
						masks[0] = 0x7bde;
						v = (v&0xf)>>3;
						masks[1] = v + (v<<6)+ (v<<11);
						masks[2] = (R&0xf8)<<8;
						masks[2] |= (G&0xf8)<<2;
						masks[2] |= (B>>3);
						masks[3] = 5;
						break;
					default:
						masks[0] = 0xfefefe;
						masks[1] = v + (v<<8) + (v<<16);
						masks[1] &= masks[0];
						masks[2] = (R<<16)|(G<<8)|B;
						masks[3] = 8;
						break;
				}
			}
			if(desc.Stretch>0)
				BltStretch(ref desc , ref masks);
			else
				Blt(ref desc, ref masks);
		}

		private static unsafe void Blt(ref DrawingDesc desc, ref Int32[] masks){	
			Int32 channelMask = (1<<masks[3])-1;
			Int32 colKey = desc.Src.ColorKey;
			Int32 pixMask = desc.Src.PixelMask;
			Int32 pixNegMask = ~pixMask;
			Int32 flg = (~masks[0])&(pixMask<<1);
			int lpSrc = desc.Src.lpOffset;
			int lpDst = desc.Dest.lpOffset;
			int y = 0;
			while(++y<= desc.RowSteps) {
				// 
				// Alpha-blend the pixels in the current row.
				//
				int x = 0;
				int lpSrc2 = lpSrc;
				int lpDst2 = lpDst;
				while ( ++x<= desc.ColumnSteps ) {
					// Read in the next source pixel.
					Int32 vSrc = *(( Int32*)lpSrc2 );
					// If the source pixel is not black ...
					if ( ( vSrc & pixMask ) != colKey ) {
						// ... read in the next target pixel.
						Int32 vDst = *(( Int32*)lpDst2 );

						// Calculate the destination pixel.
						vSrc &= masks[0];
						vSrc -= masks[1];
						Int32 work = vSrc&flg;
						work *= channelMask;
						work >>= masks[3];

						vSrc &= ~work;
						vSrc += masks[2]; 
						// Write the destination pixel.
						*(( Int32* )lpDst2) = (vDst&pixNegMask)|vSrc;
					}
					// Proceed to the next pixel.
					lpDst2 += desc.Dest.PixelPitch;
					lpSrc2 += desc.Src.PixelPitch;
				}
				// Proceed to the next line.
				lpDst += desc.Dest.RowPitch;
				lpSrc += desc.Src.RowPitch;
			};
		}

		private static readonly int[] patternArray = { 
														 (1<<1)+(1<<3),//1
														 (1<<2)+(1<<5)+(1<<8),//2
														 (1<<3)+(1<<7)+(1<<11)+(1<<15),//3
														 (1<<4)+(1<<9)+(1<<14)+(1<<19)+(1<<24)//4
													 };
		private static unsafe void BltStretch(ref DrawingDesc desc, ref Int32[] masks){
			Int32 channelMask = (1<<masks[3])-1;
			Int32 colKey = desc.Src.ColorKey;
			Int32 pixMask = desc.Src.PixelMask;
			Int32 pixNegMask = ~pixMask;
			Int32 flg = (~masks[0])&(pixMask<<1);
			int n = Math.Min(desc.Stretch,4);
			int pattern = patternArray[n-1];
			n++;
			int dstColPitch = desc.Dest.PixelPitch*n;
			int dstRowPitch = desc.Dest.RowPitch*n;
			int dstPadding = desc.Dest.RowPitch-dstColPitch;
			n*=n;
			int lpSrc = desc.Src.lpOffset;
			int lpDst = desc.Dest.lpOffset;
			int y = 0;
			while(++y<= desc.RowSteps) {
				// 
				// Alpha-blend the pixels in the current row.
				//
				int x = 0;
				int lpSrc2 = lpSrc;
				int lpDst2 = lpDst;
				while ( ++x<= desc.ColumnSteps ) {
					// Read in the next source pixel.
					Int32 vSrc = *(( Int32*)lpSrc2 );
					// If the source pixel is not black ...
					if ( ( vSrc & pixMask ) != colKey ) {
						int lpDst3 = lpDst2;
						int c = n;
						int p = pattern;
						// Calculate the destination pixel.
						vSrc &= masks[0];
						vSrc -= masks[1];
						Int32 work = vSrc&flg;
						work *= channelMask;
						work >>= masks[3];

						vSrc &= ~work;
						vSrc += masks[2]; 
						while(c-->0){
							// ... read in the next target pixel.
							Int32 vDst = *(( Int32*)lpDst2 );
							// Write the destination pixel.
							*(( Int32* )lpDst3) = (vDst&pixNegMask)|vSrc;
							lpDst3+=desc.Dest.PixelPitch;
							lpDst3+=dstPadding*(p&1);
							p>>=1;
						}
					}
					// Proceed to the next pixel.
					lpDst2 += dstColPitch;
					lpSrc2 += desc.Src.PixelPitch;
				}
				// Proceed to the next line.
				lpDst += dstRowPitch;
				lpSrc += desc.Src.RowPitch;
			};		}
	}

	#endregion

	#region MonoColor
	class MonoColorDrawer : ISurfaceDrawer{
		protected Color color;
		protected PixelColorMode prev = PixelColorMode.Unknown;
		protected Int32[] masks = new Int32[6];
		protected long workval;
		protected long workmask;

		public MonoColorDrawer(Color col){
			this.color = col;
		}
		
		public MonoColorDrawer() : this(Color.White){
		}

		public Color DrawColor { 
			get{ return color; }
			set{ color = value; prev = PixelColorMode.Unknown; }
		}

		public void Blt(ref DrawingDesc desc ){
			if(prev != desc.Dest.Mode){
				prev = desc.Dest.Mode;

				switch(desc.Dest.Mode){
					case PixelColorMode.RGB16Bit565:
						masks[0] = 0xe7dc;
						masks[1] = 11;
						masks[2] = 6;
						masks[3] = 0x1f;
						masks[4] = 5;
						workval = ((long)color.G<<22)+(color.R<<11)+(color.B);
						workmask = 0x7c0f81f;
						break;
					case PixelColorMode.RGB16Bit555:
						masks[0] = 0x73dc;
						masks[1] = 10;
						masks[2] = 5;
						masks[3] = 0x1f;
						masks[4] = 5;
						workval = ((long)color.G<<20)+(color.R<<10)+(color.B);
						workmask = 0x1f07c1f;
						break;
					default:
						masks[0] = 0xfcfefc;
						masks[1] = 16;
						masks[2] = 8;
						masks[3] = 0xff;
						masks[4] = 0xff00ff;
						masks[5] = 8;
						workval = ((long)color.G<<32)+(color.R<<16)+(color.B);
						workmask = 0xff00ff00ff;
						break;
				}
			}
			//Debug.WriteLine(" v: " + workval.ToString("X"));
			if(desc.Stretch>0)
				BltStretch(ref desc , ref masks, workval, workmask);
			else
				Blt(ref desc, ref masks, workval, workmask);
		}

		private static unsafe void Blt(ref DrawingDesc desc, ref Int32[] masks, long val, long mask){	
			Int32 colKey = desc.Src.ColorKey;
			Int32 pixMask = desc.Src.PixelMask;
			Int32 pixNegMask = ~pixMask;
			int shift = (masks[1]<<1) - masks[5];
			int lpSrc = desc.Src.lpOffset;
			int lpDst = desc.Dest.lpOffset;
			int y = 0;
			while(++y<= desc.RowSteps) {
				// 
				// Alpha-blend the pixels in the current row.
				//
				int x = 0;
				int lpSrc2 = lpSrc;
				int lpDst2 = lpDst;
				while ( ++x<= desc.ColumnSteps ) {
					// Read in the next source pixel.
					Int32 vSrc = *(( Int32*)lpSrc2 );
					// If the source pixel is not black ...
					if ( ( vSrc & pixMask ) != colKey ) {
						// ... read in the next target pixel.
						Int32 vDst = *(( Int32*)lpDst2 );
						// Calculate the destination pixel.
						vSrc &= masks[0];
						Int32 work  = vSrc + (vSrc>>masks[1]);
						work >>=1;
						work &= masks[3];
						work += vSrc>>masks[2];
						work >>=1;
						work &= masks[3];
						//vSrc = 0x010101 * work;
						long v = val * work;
						v >>= masks[5];
						v &= mask;
						vSrc = (int)v + (int)(v>>shift);
						vSrc &= pixMask;
						// Write the destination pixel.
						*(( Int32* )lpDst2) = (vDst&pixNegMask)|vSrc;
					}
					// Proceed to the next pixel.
					lpDst2 += desc.Dest.PixelPitch;
					lpSrc2 += desc.Src.PixelPitch;
				}
				// Proceed to the next line.
				lpDst += desc.Dest.RowPitch;
				lpSrc += desc.Src.RowPitch;
			};
		}

		private static readonly int[] patternArray = { 
														 (1<<1)+(1<<3),//1
														 (1<<2)+(1<<5)+(1<<8),//2
														 (1<<3)+(1<<7)+(1<<11)+(1<<15),//3
														 (1<<4)+(1<<9)+(1<<14)+(1<<19)+(1<<24)//4
													 };
		private static unsafe void BltStretch(ref DrawingDesc desc, ref Int32[] masks, long val, long mask){
			Int32 colKey = desc.Src.ColorKey;
			Int32 pixMask = desc.Src.PixelMask;
			Int32 pixNegMask = ~pixMask;
			int shift = (masks[1]<<1)-masks[5];
			int n = Math.Min(desc.Stretch,4);
			int pattern = patternArray[n-1];
			n++;
			int dstColPitch = desc.Dest.PixelPitch*n;
			int dstRowPitch = desc.Dest.RowPitch*n;
			int dstPadding = desc.Dest.RowPitch-dstColPitch;
			n*=n;
			int lpSrc = desc.Src.lpOffset;
			int lpDst = desc.Dest.lpOffset;
			int y = 0;
			while(++y<= desc.RowSteps) {
				// 
				// Alpha-blend the pixels in the current row.
				//
				int x = 0;
				int lpSrc2 = lpSrc;
				int lpDst2 = lpDst;
				while ( ++x<= desc.ColumnSteps ) {
					// Read in the next source pixel.
					Int32 vSrc = *(( Int32*)lpSrc2 );
					// If the source pixel is not black ...
					if ( ( vSrc & pixMask ) != colKey ) {
						int lpDst3 = lpDst2;
						int c = n;
						int p = pattern;
						// Calculate the destination pixel.
						vSrc &= masks[0];
						Int32 work  = vSrc + (vSrc>>masks[1]);
						work >>=1;
						work &= masks[3];
						work += vSrc>>masks[2];
						work >>=1;
						work &= masks[3];
						//vSrc = 0x010101 * work;
						long v = val * work;
						v >>= masks[5];
						v &= mask;
						vSrc = (int)v + (int)(v>>shift);
						vSrc &= pixMask;
						while(c-->0){
							// ... read in the next target pixel.
							Int32 vDst = *(( Int32*)lpDst2 );
							// Write the destination pixel.
							*(( Int32* )lpDst3) = (vDst&pixNegMask)|vSrc;
							lpDst3+=desc.Dest.PixelPitch;
							lpDst3+=dstPadding*(p&1);
							p>>=1;
						}
					}
					// Proceed to the next pixel.
					lpDst2 += dstColPitch;
					lpSrc2 += desc.Src.PixelPitch;
				}
				// Proceed to the next line.
				lpDst += dstRowPitch;
				lpSrc += desc.Src.RowPitch;
			};		}
	}

	#endregion


	#region PixelFilterWrapper
	class PixelFilterDrawer : ISurfaceDrawer{
		protected IPixelFilter filter;

		public PixelFilterDrawer(IPixelFilter filter){
			this.filter = filter;
		}

		public IPixelFilter PixelFilter { 
			get{ return filter; }
			set{ filter = (value==null)?new NullFilter():filter; }
		}
		
		public void Blt(ref DrawingDesc desc ){
			filter.Begin(desc.Dest.Mode,desc.Dest.ColorKey);
			if(desc.Stretch>0)
				BltStretch(ref desc , filter);
			else
				Blt(ref desc, filter);
			filter.End();
		}

		private static unsafe void Blt(ref DrawingDesc desc, IPixelFilter filter){	
			Int32 colKey = desc.Src.ColorKey;
			Int32 pixMask = desc.Src.PixelMask;
			Int32 pixNegMask = ~pixMask;
			int lpSrc = desc.Src.lpOffset;
			int lpDst = desc.Dest.lpOffset;
			int y = 0;
			while(++y<= desc.RowSteps) {
				// 
				// Alpha-blend the pixels in the current row.
				//
				int x = 0;
				int lpSrc2 = lpSrc;
				int lpDst2 = lpDst;
				while ( ++x<= desc.ColumnSteps ) {
					// Read in the next source pixel.
					Int32 vSrc = *(( Int32*)lpSrc2 );
					// If the source pixel is not black ...
					if ( ( vSrc & pixMask ) != colKey ) {
						// ... read in the next target pixel.
						Int32 vDst = *(( Int32*)lpDst2 );

						// Calculate the destination pixel.
						vSrc = filter.Convert(vDst,vSrc);
						// Write the destination pixel.
						*(( Int32* )lpDst2) = (vDst&pixNegMask)|vSrc;
					}
					// Proceed to the next pixel.
					lpDst2 += desc.Dest.PixelPitch;
					lpSrc2 += desc.Src.PixelPitch;
				}
				// Proceed to the next line.
				lpDst += desc.Dest.RowPitch;
				lpSrc += desc.Src.RowPitch;
			};
		}

		private static readonly int[] patternArray = { 
														 (1<<1)+(1<<3),//1
														 (1<<2)+(1<<5)+(1<<8),//2
														 (1<<3)+(1<<7)+(1<<11)+(1<<15),//3
														 (1<<4)+(1<<9)+(1<<14)+(1<<19)+(1<<24)//4
													 };
		private static unsafe void BltStretch(ref DrawingDesc desc, IPixelFilter filter){
			Int32 colKey = desc.Src.ColorKey;
			Int32 pixMask = desc.Src.PixelMask;
			Int32 pixNegMask = ~pixMask;
			int n = Math.Min(desc.Stretch,4);
			int pattern = patternArray[n-1];
			n++;
			int dstColPitch = desc.Dest.PixelPitch*n;
			int dstRowPitch = desc.Dest.RowPitch*n;
			int dstPadding = desc.Dest.RowPitch-dstColPitch;
			n*=n;
			int lpSrc = desc.Src.lpOffset;
			int lpDst = desc.Dest.lpOffset;
			int y = 0;
			while(++y<= desc.RowSteps) {
				// 
				// Alpha-blend the pixels in the current row.
				//
				int x = 0;
				int lpSrc2 = lpSrc;
				int lpDst2 = lpDst;
				while ( ++x<= desc.ColumnSteps ) {
					// Read in the next source pixel.
					Int32 vSrc = *(( Int32*)lpSrc2 );
					// If the source pixel is not black ...
					if ( ( vSrc & pixMask ) != colKey ) {
						int lpDst3 = lpDst2;
						int c = n;
						int p = pattern;
						// Calculate the destination pixel.
						while(c-->0){
							// ... read in the next target pixel.
							Int32 vDst = *(( Int32*)lpDst2 );
							// Write the destination pixel.
							*(( Int32* )lpDst3) = (vDst&pixNegMask)|filter.Convert(vDst,vSrc);
							lpDst3+=desc.Dest.PixelPitch;
							lpDst3+=dstPadding*(p&1);
							p>>=1;
						}
					}
					// Proceed to the next pixel.
					lpDst2 += dstColPitch;
					lpSrc2 += desc.Src.PixelPitch;
				}
				// Proceed to the next line.
				lpDst += dstRowPitch;
				lpSrc += desc.Src.RowPitch;
			};		
		}

		class NullFilter : IPixelFilter {
			public void Begin(PixelColorMode mode, Int32 colorKey){}

			public Int32 Convert(Int32 dest, Int32 source){ return source; }

			public void End(){}
		}
	}

	#endregion	
	
	/*
		class AlphaPixelFilter : IPixelFilter {
			/// <summary>
			/// フィルター作業開始直前に呼ばれる
			/// </summary>
			/// <param name="mode">カラーモード</param>
			/// <param name="colorKey">透明色</param>
			public void Begin(PixelColorMode mode, Int32 colorKey) {
			}

			/// <summary>
			/// 与えられたカラーを変換する
			/// 16bitモードでは、一度に2pixel分のデータが与えられる
			/// </summary>
			/// <param name="source">変換前の色値</param>
			/// <returns></returns>
			public Int32 Convert(Int32 dest, Int32 source) {
				return ( ( dest & 0xfefefe ) + ( source & 0xfefefe ))>> 1;
			}

			/// <summary>
			/// フィルター作業終了事に呼ばれる
			/// </summary>
			public void End() {
			}
		}
	}
	*/
}
