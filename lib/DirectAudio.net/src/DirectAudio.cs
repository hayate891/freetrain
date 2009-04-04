using System;
using AgateLib;

namespace org.kohsuke.directaudio
{
	/// <summary>
	/// DirectAudio shared objects pool
	/// </summary>
	internal class DirectAudio
	{
#if windows
		internal static readonly DirectX8Class dx = new DirectX8Class();
		
		internal static readonly DirectMusicLoader8 loader = dx.DirectMusicLoaderCreate();
#else
#warning STUB
#endif
	}
}
