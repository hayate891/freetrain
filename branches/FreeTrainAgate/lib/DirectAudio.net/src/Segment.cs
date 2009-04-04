using System;
using AgateLib.AudioLib;

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
		internal SoundBuffer handle;
#warning STUB
#endif


#if windows
		private Segment( DirectMusicSegment8 handle ) {
			this.handle = handle;
		}
#else
		private Segment(SoundBuffer handle) {
			this.handle = handle;
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
			Console.WriteLine("LINUXDEBUG: fromfile segment {0}", fileName);
			try {
				return new Segment( new SoundBuffer(fileName) );
			} catch( Exception e ) {
				throw new Exception("unable to load music file: "+fileName,e);
			}
#endif
		}

		public static Segment fromMidiFile( string fileName ) {
#if windows
			Segment seg = fromFile(fileName);
			seg.handle.SetStandardMidiFile();
			return seg;
#else
#warning STUB
			Console.WriteLine("LINUXDEBUG: frommidifile segment {0}", fileName);
			Segment seg = fromFile(fileName);
			return seg;
#endif
		}


		public void Dispose() {
#if windows
			if(handle!=null) {
				System.Runtime.InteropServices.Marshal.ReleaseComObject(handle);
			}
			handle = null;
#else
			if(handle!=null)
			{
				Console.WriteLine("LINUXDEBUG: dispose segment {0}", handle.Filename);
				handle.Dispose();
				handle = null;
			}
			
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
			Console.WriteLine("LINUXDEBUG: clone segment {0}", handle.Filename);
			return new Segment(new SoundBuffer(handle.Filename));
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
