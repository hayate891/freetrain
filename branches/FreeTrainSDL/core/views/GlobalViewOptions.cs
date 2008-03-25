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
using FreeTrain.World;
using FreeTrain.Framework.Graphics;


namespace FreeTrain.Views
{
    /// <summary>
    /// 
    /// </summary>
    public delegate void OptionChangedHandler();
    /// <summary>
    /// 
    /// </summary>
    public enum NightSpriteMode 
    { 
        /// <summary>
        /// 
        /// </summary>
        AlignClock, 
        /// <summary>
        /// 
        /// </summary>
        AlwaysDay, 
        /// <summary>
        /// 
        /// </summary>
        AlwaysNight 
    };
    /// <summary>
    /// GlobalViewOptions の概要の説明です。
    /// </summary>
    [Serializable]
    public class GlobalViewOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public GlobalViewOptions()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        [NonSerialized]
        public OptionChangedHandler OnViewOptionChanged;
        /// <summary>
        /// 
        /// 
        /// </summary>
        [CLSCompliant(false)]
        protected NightSpriteMode _nightSpriteMode = NightSpriteMode.AlignClock;
        /// <summary>
        /// 
        /// </summary>
        public NightSpriteMode nightSpriteMode
        {
            get { return _nightSpriteMode; }
            set
            {
                _nightSpriteMode = value;
                PictureManager.reset();
                WorldDefinition.World.onAllVoxelUpdated();
                if (OnViewOptionChanged != null)
                {
                    Debug.WriteLine("###" + OnViewOptionChanged.GetInvocationList().Length);
                    OnViewOptionChanged();
                }
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public bool useNightView
        {
            get
            {
                if (nightSpriteMode == NightSpriteMode.AlignClock)
                    return WorldDefinition.World.clock.dayOrNight == DayNight.Night;
                else
                    return nightSpriteMode == NightSpriteMode.AlwaysNight;
            }
        }
    }
}
