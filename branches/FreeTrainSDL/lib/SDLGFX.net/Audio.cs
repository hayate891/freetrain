using System;
using System.Collections.Generic;
using System.Text;
using Tao.Sdl;

namespace SDL.net
{
    public class Audio : IDisposable
    {
        public Audio()
        {
            //SdlMixer.Mix_OpenAudio(SdlMixer.MIX_DEFAULT_FREQUENCY, unchecked(Sdl.AUDIO_S16LSB), 2, 1024);
        }

        public static void play(string filename)
        {
            IntPtr tmp = SdlMixer.Mix_LoadWAV(filename);
            SdlMixer.Mix_PlayChannel(-1, tmp, 0);
            //SdlMixer.Mix_FreeChunk(tmp);
        }

        public void Dispose()
        {
            //SdlMixer.Mix_CloseAudio();
        }
    }

    public class Segment
    {
        IntPtr sndHandle;

        public Segment()
        {
            sndHandle = IntPtr.Zero;
        }

        public Segment(string p)
        {
            sndHandle = SdlMixer.Mix_LoadWAV(p);
        }

        public void play()
        {
            if (sndHandle != IntPtr.Zero) SdlMixer.Mix_PlayChannel(-1, sndHandle, 0);
        }

        public void play(int ms)
        {
            play();
            //if (sndHandle != IntPtr.Zero) then do something!
        }

        public static Segment fromFile(string p)
        {
            return new Segment(p);
        }
    }

    public class BGM
    {
        private string _filename;
        public string fileName {
            get {
                return _filename;
            }
            set
            {
                if (music != IntPtr.Zero) SdlMixer.Mix_FreeMusic(music);
                _filename = value;
                music = SdlMixer.Mix_LoadMUS(_filename);
            }
        }

        private IntPtr music;

        public BGM()
        {
            _filename = string.Empty;
            music = IntPtr.Zero;
        }

        public void stop()
        {
            //throw new Exception("The method or operation is not implemented.");
            SdlMixer.Mix_HaltMusic();
        }

        public void run()
        {
            //throw new Exception("The method or operation is not implemented.");
            if (_filename != string.Empty)
            {
                SdlMixer.Mix_LoadMUS(fileName);
                SdlMixer.Mix_PlayMusic(music, 0);
            }
        }
    }
}
