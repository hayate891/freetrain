using System;
using System.Diagnostics;
using DxVBLib;

namespace nft.drawing.ddraw7
{
	/// <summary>
	/// DDraw7 ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	/// <remarks>this class is originaly implemented by K.Kawaguchi</remarks>
	public class DDraw7 : IDisposable
	{
		protected DirectDraw7 handle;

		public DDraw7() {
			// initialize DirectDraw
			DirectX7 dxc = new DirectX7Class();
			handle = dxc.DirectDrawCreate("");

			handle.SetCooperativeLevel( 0, CONST_DDSCLFLAGS.DDSCL_NORMAL );	// window mode
		}

		public virtual void Dispose() {
			handle=null;
		}
		
		internal DirectDraw7 Handle { get { return handle; } }
	}


}
