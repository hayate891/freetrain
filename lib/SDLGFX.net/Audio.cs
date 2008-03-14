#region LICENSE
/*
 * Copyright (C) 2007 - 2008 FreeTrain Team (http://freetrain.sourceforge.net)
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
}
