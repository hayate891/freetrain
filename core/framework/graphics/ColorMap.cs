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
using System.Drawing;

namespace freetrain.framework.graphics
{
    /// <summary>
    /// Determines the color mapping between daylight time and night time.
    /// </summary>
    public class ColorMap
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static Color getNightColor(Color src)
        {
            // handle three light colors
            for (int i = 0; i < lightColorMap.Length; i += 2)
                if (lightColorMap[i] == src)
                    return lightColorMap[i + 1];

            // TODO: handle season colors

            return Color.FromArgb(ratio(src.R), ratio(src.G), ratio(src.B));
        }

        private static int ratio(int i) { return i / 4; }

        private static readonly Color[] lightColorMap = new Color[] {
			// original color			light color
			Color.FromArgb(8,0,0),		Color.FromArgb(255,  8,  8),
			Color.FromArgb(0,8,0),		Color.FromArgb(252,243,148),
			Color.FromArgb(0,0,8),		Color.FromArgb(255,227, 99)
		};

    }
}
