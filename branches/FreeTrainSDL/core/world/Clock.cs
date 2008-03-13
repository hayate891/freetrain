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
using System.Diagnostics;
using System.Windows.Forms;
using freetrain.util;
using freetrain.framework;
using freetrain.framework.graphics;

namespace freetrain.world
{
    /// <summary>
    /// Handles a clock event.
    /// </summary>
    public delegate void ClockHandler();
    /// <summary>
    /// 
    /// </summary>
    public enum Season : byte
    {
        /// <summary>
        /// 
        /// </summary>
        Spring = 0,
        /// <summary>
        /// 
        /// </summary>
        Summer = 1,
        /// <summary>
        /// 
        /// </summary>
        Autumn = 2,
        /// <summary>
        /// 
        /// </summary>
        Winter = 3
    }
    /// <summary>
    /// 
    /// </summary>
    public enum DayNight : byte
    {
        /// <summary>
        /// 
        /// 
        /// </summary>
        DayTime = 0,
        /// <summary>
        /// 
        /// </summary>
        Night = 1
    }

    /// <summary>
    /// Clock that governs the time of the world.
    /// 
    /// Because of the way Windows Forms work, this class is not self-sufficient.
    /// The main window needs to run a timr and periodically call the tick method
    /// of this class to make this class work.
    /// </summary>
    [Serializable]
    public class Clock : Time
    {
        // creatable only from the World class.
        // Initialize the value to April, 1st 8:00 AM.
        /// <summary>
        /// 
        /// </summary>
        public Clock() : base(START_TIME) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        public void setCurrentTime(long t)
        {
            this.currentTime = t;
            // notify the time change
            PictureManager.reset();
            World.world.onAllVoxelUpdated();
        }

        /// <summary>
        /// Handlers that are waiting for the clock notification.
        /// </summary>
        private readonly PriorityQueue queue = new PriorityQueue();

        #region handler maintainance
        /// <summary>
        /// Registers an one-shot timer, which will be fired after
        /// the specified time span.
        /// </summary>
        public void registerOneShot(ClockHandler handler, TimeLength time)
        {
            Debug.Assert(time.totalMinutes > 0);
            queue.insert(currentTime + time.totalMinutes, handler);
        }

        /// <summary>
        /// Registers an one-shot timer, which will be fired at
        /// the specified time.
        /// </summary>
        public void registerOneShot(ClockHandler handler, Time time)
        {
            registerOneShot(handler, time - World.world.clock);
        }


        /// <summary>
        /// Registers a repeated-timer, which will be fired
        /// periodically for every specified interval.
        /// 
        /// The first clock notification will be sent also after the
        /// specified minutes.
        /// </summary>
        /// <returns>
        /// The cookie, which shall be then used to unregister the timer.
        /// </returns>
        public ClockHandler registerRepeated(ClockHandler handler, TimeLength time)
        {
            return registerRepeated(handler, time, time);
        }

        /// <summary>
        /// Registers a repeated-timer, which will be fired
        /// periodically for every specified interval.
        /// </summary>
        /// <param name="first">The first event will be sent after this interval.</param>
        /// <param name="interval">Successive events will be fired with this interval.</param>
        /// <returns>
        /// The cookie, which shall be then used to unregister the timer.
        /// </returns>
        /// <param name="handler"></param>
        public ClockHandler registerRepeated(ClockHandler handler, TimeLength first, TimeLength interval)
        {
            Debug.Assert(interval.totalMinutes != 0 && first.totalMinutes != 0);
            RepeatedTimer rt = new RepeatedTimer(this, handler, first, interval);
            return new ClockHandler(rt.onClock);
        }

        /// <summary>
        /// Unregisters a repeated timer.
        /// </summary>
        public void unregister(ClockHandler handler)
        {
            queue.remove(handler);
        }
        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        protected class RepeatedTimer
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="_clock"></param>
            /// <param name="_handler"></param>
            /// <param name="first"></param>
            /// <param name="_interval"></param>
            public RepeatedTimer(Clock _clock, ClockHandler _handler, TimeLength first, TimeLength _interval)
            {
                this.clock = _clock;
                this.handler = _handler;
                this.interval = _interval;
                clock.registerOneShot(new ClockHandler(onClock), first);
            }

            private readonly Clock clock;
            private readonly ClockHandler handler;
            private readonly TimeLength interval;
            /// <summary>
            /// 
            /// </summary>
            public void onClock()
            {
                handler();
                clock.registerOneShot(new ClockHandler(onClock), interval);
            }
        }
        #endregion




        /// <summary>
        /// One-time call back at the end of a turn.
        /// 
        /// To get continuous call back after each end of turn,
        /// keep registering handlers at the end of each callback.
        /// </summary>
        [NonSerialized]
        public EventHandler endOfTurnHandlers;


        /// <summary>
        /// Make the clock tick.
        /// </summary>
        public void tick()
        {
            if (MainWindow.mainWindow.currentController != null)
                return;	// if a controller is active, stop the timer.

            currentTime++;

            long m = (currentTime % DAY);
            if (m == 6 * HOUR || m == 18 * HOUR)
            {
                PictureManager.reset();
                World.world.onAllVoxelUpdated();	// time change
            }

            Debug.Assert((long)queue.minPriority >= currentTime);

            while (queue.count != 0 && (long)queue.minPriority == currentTime)
            {
                ((ClockHandler)queue.minValue)();
                queue.removeMin();
            }

            // call end-of-the-turn handlers
            EventHandler eot = endOfTurnHandlers;
            endOfTurnHandlers = null;	// so that callback methods can re-register themselves
            if (eot != null) eot(this, null);
        }

    }
}
