using System;
#if windows
using DxVBLibA;
#else
using ERY.AgateLib;
#endif

namespace org.kohsuke.directaudio
{
	/// <summary>
	/// SegmentState の概要の説明です。
	/// </summary>
	public class SegmentState
	{
#if windows
		internal SegmentState( Performance perf, DirectMusicSegmentState8 state, int endTime ) {
			this.performance = perf;
			this.state = state;
			this.estimatedEndTime = endTime;
		}
#else
		internal SegmentState( Performance perf, int state, int endTime ) {
#warning STUB
		}
#endif

#if windows
		private readonly Performance performance;
		private readonly DirectMusicSegmentState8 state;
		private readonly int estimatedEndTime;
#else
#warning STUB
#endif

		/// <summary>
		/// Returns true if this segment is still being played.
		/// </summary>
		public bool isPlaying {
			get {
#if windows
				if( performance.handle.IsPlaying(null,state) )
					return true;

				if( performance.handle.GetMusicTime() < estimatedEndTime )
					return true;

//				// because of the latency, sometimes this method false even if it's not being played yet.
//				// thus make sure that it has the reasonable start time.
//				int currentTime = performance.handle.GetMusicTime();
//				if( currentTime <= state.GetStartTime() )
//					return true;	// this will be played in a future
				
				return false;
#else
#warning STUB
				return false;
#endif
			}
		}
	}
}
