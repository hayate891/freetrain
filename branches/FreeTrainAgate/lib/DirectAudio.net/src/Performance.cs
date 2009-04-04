using System;
using System.Diagnostics;
using System.Windows.Forms;
using AgateLib;

namespace org.kohsuke.directaudio
{
	/// <summary>
	/// Performance の概要の説明です。
	/// </summary>
	public class Performance : IDisposable
	{
#if windows
		internal DirectMusicPerformance8 handle;
#else
#warning STUB
#endif

		/// <summary>
		/// 
		/// </summary>
		/// <param name="owner">owner window object</param>
		public Performance( IWin32Window owner ) {
#if windows
			handle = DirectAudio.dx.DirectMusicPerformanceCreate();
			this.handle = handle;

			DMUS_AUDIOPARAMS param = new DMUS_AUDIOPARAMS();
			DirectSound8 nullDs8 = null;

			// TODO: learn more about this initialization process
			handle.InitAudio( owner.Handle.ToInt32(),
				CONST_DMUS_AUDIO.DMUS_AUDIOF_ALL,  
				ref param,
				ref nullDs8,
				CONST_DMUSIC_STANDARD_AUDIO_PATH.DMUS_APATH_DYNAMIC_STEREO, 16 );
#else
#warning STUB
#endif
		}

		public void Dispose() {
#if windows
			if(handle!=null) {
				handle.CloseDown();
				System.Runtime.InteropServices.Marshal.ReleaseComObject(handle);
			}
			handle = null;
#else
#warning STUB
#endif
		}

		/// <summary>
		/// Plays the given segment exclusively.
		/// </summary>
		public SegmentState playExclusive( Segment seg ) {
#if windows
			return new SegmentState( this, 
				handle.PlaySegmentEx( seg.handle, 0, 0, null, null ),
				handle.GetMusicTime()+seg.handle.GetLength() );
#else
#warning STUB
			return new SegmentState( this, 0, 0);
#endif
		}

		/// <summary>
		/// Plays the given segment immediately.
		/// </summary>
		public SegmentState play( Segment seg ) {
			return play(seg,0);
		}

		/// <summary>
		/// Plays the given segment after the specified lead time (in milliseconds)
		/// </summary>
		public SegmentState play( Segment seg, int leadTime ) {
#if windows
			if( leadTime!=0 )
				leadTime = handle.ClockToMusicTime(leadTime*10*1000 + handle.GetClockTime());
			if( leadTime<0 )
				leadTime = 0;

			return new SegmentState( this, handle.PlaySegmentEx( seg.handle,
				CONST_DMUS_SEGF_FLAGS.DMUS_SEGF_SECONDARY, leadTime, null, null ),
				handle.GetMusicTime()+seg.handle.GetLength()+leadTime );
#else
#warning STUB
			return new SegmentState( this, 0, 0);
#endif
		}

		/// <summary>
		/// Creates an audio path from the properties of the given segment.
		/// </summary>
		public AudioPath createAudioPath( Segment seg ) {
#if windows
			return new AudioPath( handle.CreateAudioPath( seg.handle.GetAudioPathConfig(), true ) );
#else
#warning STUB
			return new AudioPath ();
#endif
		}
	}
}
