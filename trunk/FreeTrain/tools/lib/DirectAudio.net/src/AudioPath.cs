using System;
using DxVBLibA;

namespace org.kohsuke.directaudio
{
	/// <summary>
	/// AudioPath ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class AudioPath : IDisposable
	{
		internal DirectMusicAudioPath8 handle;

		internal AudioPath( DirectMusicAudioPath8 handle ) {
			this.handle = handle;
		}

		public void Dispose() {
			if(handle!=null) {
				System.Runtime.InteropServices.Marshal.ReleaseComObject(handle);
			}
			handle = null;
		}
	}
}
