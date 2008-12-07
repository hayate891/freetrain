using System;
#if windows
using DxVBLibA;
#else
using ERY.AgateLib;
#endif

namespace org.kohsuke.directaudio
{
	/// <summary>
	/// Music clip
	/// </summary>
	public class Segment : IDisposable
	{
#if windows
		internal DirectMusicSegment8 handle;
#else
#warning STUB
#endif


#if windows
		private Segment( DirectMusicSegment8 handle ) {
			this.handle = handle;
		}
#else
		private Segment() {
#warning STUB
		}
#endif
		
		public static Segment fromFile( string fileName ) {
#if windows
			try {
				return new Segment( DirectAudio.loader.LoadSegment(fileName) );
			} catch( Exception e ) {
				throw new Exception("unable to load music file: "+fileName,e);
			}
#else
#warning STUB
			throw new Exception("unable to load music file: "+fileName);
#endif
		}

		public static Segment fromMidiFile( string fileName ) {
#if windows
			Segment seg = fromFile(fileName);
			seg.handle.SetStandardMidiFile();
			return seg;
#else
#warning STUB
			throw new Exception("unable to load music file: "+fileName);
#endif
		}


		public void Dispose() {
#if windows
			if(handle!=null) {
				System.Runtime.InteropServices.Marshal.ReleaseComObject(handle);
			}
			handle = null;
#else
#warning STUB
#endif
		}



		/// <summary>
		/// Prepares this sound object for the play by the performance object.
		/// </summary>
		public void downloadTo( Performance p ) {
#if windows
			handle.Download( p.handle.GetDefaultAudioPath() );
#else
#warning STUB
#endif
		}

		/// <summary>
		/// Reverses the effect of the downloadTo method.
		/// </summary>
		public void unloadFrom( Performance p ) {
#if windows
			handle.Unload( p.handle.GetDefaultAudioPath() );
#else
#warning STUB
#endif
		}

		public Segment clone() {
#if windows
			return new Segment( handle.Clone(0,0) );
#else
#warning STUB
			return new Segment();
#endif
		}

		public int repeats {
			get {
#if windows
				return handle.GetRepeats();
#else
#warning STUB
				return 1;
#endif
			}
			set {
#if windows
				handle.SetRepeats(value);
#else
#warning STUB
#endif
			}
		}

	}
}