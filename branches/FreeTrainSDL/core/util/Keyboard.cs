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

namespace freetrain.util
{
    /// <summary>
    /// Keyboard-related utility functions
    /// </summary>
    public class Keyboard
    {
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        // virtual key code (see Platform SDK)
        private const int VK_LEFT = 0x25;
        private const int VK_UP = 0x26;
        private const int VK_RIGHT = 0x27;
        private const int VK_DOWN = 0x28;
        private const int VK_SHIFT = 0x10;
        private const int VK_CONTROL = 0x11;
        /// <summary>
        /// 
        /// </summary>
        public static bool isShiftKeyPressed
        {
            get
            {
                return GetAsyncKeyState(VK_SHIFT) < 0;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static bool isControlKeyPressed
        {
            get
            {
                return GetAsyncKeyState(VK_CONTROL) < 0;
            }
        }
    }
}
