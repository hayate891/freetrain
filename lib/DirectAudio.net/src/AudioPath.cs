using System;
#if windows
using DxVBLibA;
#else
using ERY.AgateLib;
#endif

namespace org.kohsuke.directaudio
{
	/// <summary>
	/// AudioPath の概要の説明です。
	/// </summary>
	public class AudioPath : IDisposable
	{
#if windows
		internal DirectMusicAudioPath8 handle;
#endif

#if windows
		internal AudioPath( DirectMusicAudioPath8 handle ) {
			this.handle = handle;
		}
#else
		internal AudioPath() {
#warning STUB
		}
#endif

		public void Dispose() {
#if windows
#warning STUB
			if(handle!=null) {
				System.Runtime.InteropServices.Marshal.ReleaseComObject(handle);
			}
			handle = null;
#endif
		}
	}
}
