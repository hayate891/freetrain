#region LICENSE
/*
 * Copyright (C) 2004 - 2007 David Hudson (jendave@yahoo.com)
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
#endregion LICENSE

using System;
using System.Collections.Generic;
using System.Text;
using Tao.Sdl;

namespace SDL.net
{
    /// <summary>
    /// 
    /// </summary>
    public class Audio : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public Audio()
        {
            //SdlMixer.Mix_OpenAudio(SdlMixer.MIX_DEFAULT_FREQUENCY, unchecked(Sdl.AUDIO_S16LSB), 2, 1024);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        public static void play(string filename)
        {
            IntPtr tmp = SdlMixer.Mix_LoadWAV(filename);
            SdlMixer.Mix_PlayChannel(-1, tmp, 0);
            //SdlMixer.Mix_FreeChunk(tmp);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            //SdlMixer.Mix_CloseAudio();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Segment
    {
        IntPtr sndHandle;

        /// <summary>
        /// 
        /// </summary>
        public Segment()
        {
            sndHandle = IntPtr.Zero;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        public Segment(string p)
        {
            sndHandle = SdlMixer.Mix_LoadWAV(p);
        }

        /// <summary>
        /// 
        /// </summary>
        public void play()
        {
            if (sndHandle != IntPtr.Zero) SdlMixer.Mix_PlayChannel(-1, sndHandle, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        public void play(int ms)
        {
            play();
            //if (sndHandle != IntPtr.Zero) then do something!
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Segment fromFile(string p)
        {
            return new Segment(p);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BGM
    {
        private string _filename;
        /// <summary>
        /// 
        /// </summary>
        public string fileName
        {
            get
            {
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

        /// <summary>
        /// 
        /// </summary>
        public BGM()
        {
            _filename = string.Empty;
            music = IntPtr.Zero;
        }

        /// <summary>
        /// 
        /// </summary>
        public void stop()
        {
            //throw new Exception("The method or operation is not implemented.");
            SdlMixer.Mix_HaltMusic();
        }

        /// <summary>
        /// 
        /// </summary>
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
