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
using FreeTrain.world;

namespace FreeTrain.Framework.Sound
{
    /// <summary>
    /// Sound Effect
    /// </summary>
    public interface SoundEffect
    {
        /// <summary>
        /// Requests to play this sound effect, which is conceptually
        /// generated from the specified location.
        /// 
        /// The location may or may not be used to determine whether
        /// a sound should be actually played.
        /// 
        /// A sound effect may or may not be played immediately.
        /// For example, a sound effect can accumulate requests
        /// and play it later.
        /// </summary>
        void play(Location src);
    }
}
