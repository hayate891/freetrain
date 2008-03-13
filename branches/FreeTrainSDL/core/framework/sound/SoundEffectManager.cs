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
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections;
using SDL.net;

namespace freetrain.framework.sound
{
    /// <summary>
    /// Coordinates sound effects.
    /// </summary>
    public sealed class SoundEffectManager
    {

        private bool available = false;
        /// <summary>
        /// 
        /// </summary>
        public bool IsAvailable { get { return available; } }

        private SDL.net.Audio _audio;

        /// <summary>
        /// A new instance should be created only by the MainWindow class.
        /// </summary>
        public SoundEffectManager(IWin32Window owner)
        {
            try
            {
                this._audio = new Audio();
                available = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(owner, e.StackTrace, "DirectAudio can not be initialized. Sound is disabled.",
                    //! MessageBox.Show( owner, e.StackTrace, "DirectAudioが初期化できません。サウンドは無効です。",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                available = false;
                Core.options.enableSoundEffect = false;
            }
        }

        //private readonly Performance performance;

        ///// <summary>
        ///// Internal method for the SoundEffect class.
        ///// 
        ///// Plays a sound segment, if the sound effect is turned on.
        ///// </summary>
        ///// <returns>non-null if the sound is actually played.</returns>
        /*internal SegmentState play( Segment seg, int leadTime ) {
            if( !Core.options.enableSoundEffect|| !available )	return null;

            seg.downloadTo(performance);
            return performance.play( seg, leadTime );
        }*/


        //[DllImport("winmm.dll")] 
        //private static extern long sndPlaySound(String lpszName, long dwFlags);

        /// <summary>
        /// Plays a wav file in a synchronous way.
        /// 
        /// This method is slow compared to SoundEffect object, but 
        /// it is less resource intensive. Useful for one time sound effect
        /// that can take time.
        /// </summary>
        /// 

        private static Hashtable sounds = new Hashtable(36);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public static void PlaySynchronousSound(string fileName)
        {
            //if( Core.options.enableSoundEffect && Core.soundEffectManager.IsAvailable )
            //	sndPlaySound(fileName,0);
            if (sounds.Contains(fileName)) { ((Segment)sounds[fileName]).play(); }
            else
            {
                Segment newSound = new Segment(fileName);
                sounds.Add(fileName, newSound);
                newSound.play();
            }
            //SDL.net.Audio.play(fileName);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public static void PlayAsynchronousSound(string fileName)
        {
            //if( Core.options.enableSoundEffect && Core.soundEffectManager.IsAvailable )
            //	sndPlaySound(fileName,1);
            //SDL.net.Audio.play(fileName);
            if (sounds.Contains(fileName)) { ((Segment)sounds[fileName]).play(); }
            else
            {
                Segment newSound = new Segment(fileName);
                sounds.Add(fileName, newSound);
                newSound.play();
            }
        }

        /*internal SegmentState play(Segment segment, int ms)
        {
            //throw new Exception("The method or operation is not implemented.");
            segment.play(ms);
        }*/

        internal void play(Segment segment, int ms)
        {
            //throw new Exception("The method or operation is not implemented.");
            segment.play(ms);
        }
    }
}
