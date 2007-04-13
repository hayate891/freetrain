using System;

namespace nft.util
{
	/// <summary>
	/// BitFieldUtil ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class BitFieldUtil {
		static readonly byte[] highBit;

		static BitFieldUtil(){
			highBit = new byte[256];
			byte b = 0;
			for(int i=0; i<256; i++){
				if(i>>b != 0)
					b++;
				highBit[i] = b;
			}
		}

		/// <summary>
		/// returns the position of left side bit
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		static public int HighestBit( byte b ){
			return highBit[b];
		}

		/// <summary>
		/// returns the position of left side bit
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		static public int HighestBit( ushort b ){
			if((b&0xff00) != 0)
				return highBit[b>>4];
			else
				return highBit[b&0xf];
		}

		/// <summary>
		/// returns the position of left side bit
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		static public int HighestBit( ulong b ){
			if((b&0xffff0000) != 0)
				return HighestBit((ushort)(b>>16));
			else
				return HighestBit((ushort)(b&0xffff));
		}
	}
}
