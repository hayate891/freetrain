using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace nft.framework.drawing
{
	public enum PixelColorMode :int{ Default, MonoBinary, RGB16Bit555, RGB16Bit565, RGB24Bit, RGB32Bit, Unknown};

	/// <summary>
	/// PixelUtil ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class PixelFormatUtil {
		public static readonly int[,] bitTable = {{0,0,0},{1,1,1},{5,5,5},{5,6,5},{8,8,8},{8,8,8},{0,0,0}};

		static public UInt32 ValueOf( Color c, PixelColorMode mode){
			if(mode == PixelColorMode.Default)
				throw new ArgumentException("PixelColorMode.Default is not allowed.","mode");
			uint ret = 0;
			int n;
			n = bitTable[(int)mode,0];
			ret |= ((uint)c.R)>>(8-n);
			n = bitTable[(int)mode,1];
			ret <<= n;
			ret |= ((uint)c.G)>>(8-n);
			n = bitTable[(int)mode,2];
			ret <<= n;
			ret |= ((uint)c.B)>>(8-n);
			if(mode == PixelColorMode.MonoBinary)
				return ret==0?0u:1u;
			else
				return ret;
		}

		static public Color ToColor( Int32 val, PixelColorMode mode){
			if(mode == PixelColorMode.Default)
				throw new ArgumentException("PixelColorMode.Default is not allowed.","mode");
			int v = (int)val;
			int r,g,b;
			int n, n2;
			n = bitTable[(int)mode,2];
			n2 = 8-n;
			b = v&(0xff>>n2);
			b <<= n2;
			v >>= n;
			n = bitTable[(int)mode,1];
			n2 = 8-n;
			g = v&(0xff>>n2);
			g <<= n2;
			v >>= n;
			n = bitTable[(int)mode,0];
			n2 = 8-n;
			r = v&(0xff>>n2);
			r <<= n2;
			return Color.FromArgb(r,g,b);
		}

		static public PixelColorMode ToPixelColorMode( System.Drawing.Imaging.PixelFormat format){
			switch(format){
				case PixelFormat.Format24bppRgb:
					return PixelColorMode.RGB24Bit;
				case PixelFormat.Format32bppRgb:
				case PixelFormat.Format32bppArgb:
				case PixelFormat.Format32bppPArgb:
					return PixelColorMode.RGB32Bit;
				case PixelFormat.Format16bppRgb555:
					return PixelColorMode.RGB16Bit555;
				case PixelFormat.Format16bppRgb565:
					return PixelColorMode.RGB16Bit565;
				case PixelFormat.Format1bppIndexed:
					return PixelColorMode.MonoBinary;
				default:
					return PixelColorMode.Unknown;
			}
		}

		static public PixelFormat ToPixelFormat( PixelColorMode mode){
			switch(mode){
				case PixelColorMode.RGB24Bit:
					return PixelFormat.Format24bppRgb;
				case PixelColorMode.RGB32Bit:
					return PixelFormat.Format32bppRgb;
				case PixelColorMode.RGB16Bit555:
					return PixelFormat.Format16bppRgb555;
				case PixelColorMode.RGB16Bit565:
					return PixelFormat.Format16bppRgb565;
				case PixelColorMode.MonoBinary:
					return PixelFormat.Format1bppIndexed;
				default:
					return PixelFormat.Undefined;
			}
		}

	}
}
