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
using System.Collections;
using FreeTrain.World;
using FreeTrain.Views.Map;
using SdlDotNet.Audio;
using SdlDotNet;
//using FreeTrain.Framework.Graphics;

namespace FreeTrain.Framework.Sound
{
    /// <summary>
    /// SoundEffect object that handles multiple
    /// simultaneous requests in a smart way.
    /// 
    /// A sound that a train moves for example doesn't simply
    /// play five sounds simultaneously when five trains are moving.
    /// Instead, it plays just two sounds but with a short interval.
    /// 
    /// This implementation handles this kind of behavior.
    /// </summary>
    public class RepeatableSoundEffectImpl : ISoundEffect
    {
        /// <summary>
        /// </summary>
        /// <param name="seg">Sound-effect object</param>
        /// <param name="concurrentPlaybackMax">Number of maximum concurrent playback.</param>
        /// <param name="intervalTime">Interval between two successive playbacks</param>
        public RepeatableSoundEffectImpl(SdlDotNet.Audio.Sound seg, int concurrentPlaybackMax, int intervalTime)
        {
            this.segment = seg;
            this.concurrentPlaybackMax = concurrentPlaybackMax;
            this.intervalTime = intervalTime;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="seg"></param>
        public RepeatableSoundEffectImpl(SdlDotNet.Audio.Sound seg) : this(seg, 3, 200) { }

        private readonly SdlDotNet.Audio.Sound segment;

        ///// <summary>
        ///// SegmentState objects that represent the state
        ///// of segments being played.
        ///// </summary>
        //private readonly ArrayList states = new ArrayList();

        /// <summary>
        /// Don't schedule more than this number of concurrent playback.
        /// </summary>
        private readonly int concurrentPlaybackMax;

        private readonly int intervalTime;


        /// <summary>
        /// Number of scheduled playbacks.
        /// </summary>
        private int queue = 0;

        /// <summary> Count the number of simltaneously played sound. </summary>
        /*private int countOverlap() {
            while( states.Count!=0 && !((SegmentState)states[0]).isPlaying )
                states.RemoveAt(0);
            return states.Count;
        }*/

        public void play(Location loc)
        {
            if (!MapView.isVisibleInAny(loc))
                return;

            /*if( countOverlap()+queue < concurrentPlaybackMax )*/
            if (queue++ == 0)
                WorldDefinition.World.clock.endOfTurnHandlers += new EventHandler(onTurnEnd);
        }

        // called at the end of turn
        private void onTurnEnd(object sender, EventArgs a)
        {
            int ms = 0;
            for (; queue > 0; queue--, ms += intervalTime)
            {
                //SegmentState st = Core.soundEffectManager.play(segment,ms);
                //if(st!=null)	states.Add(st);
                try
                {
                    segment.Play();
                }
                catch (SdlDotNet.Core.SdlException e)
                {
                    e.Message.ToString();
                }
            }
        }
    }
}
