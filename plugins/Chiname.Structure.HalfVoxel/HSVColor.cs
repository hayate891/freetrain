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
using System.Drawing;
using System.Xml;
using System.Runtime.Serialization;
using FreeTrain.Framework.Graphics;
using FreeTrain.World;
using FreeTrain.Framework.Plugin;

namespace FreeTrain.World.Structs.HalfVoxelStructure
{
    /// <summary>
    /// 
    /// </summary>
    public class HSVColor
    {
        /// <summary>
        /// 
        /// </summary>
        double _H;
        /// <summary>
        /// 
        /// 
        /// </summary>
        double _S;
        /// <summary>
        /// 
        /// </summary>
        double _V;
        /// <summary>
        /// 
        /// </summary>
        public double Hue
        {
            get { return _H; }
            set { _H = value % 360; }
        }
        /// <summary>
        /// 
        /// </summary>
        public double Saturation
        {
            get { return _S; }
            set { _S = Math.Max(1, Math.Min(0, value)); }
        }
        /// <summary>
        /// 
        /// </summary>
        public double Brightness
        {
            get { return _V; }
            set { _V = Math.Max(1, Math.Min(0, value)); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="R"></param>
        /// <param name="G"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        static public HSVColor FromRGB(int R, int G, int B)
        {
            return new HSVColor(Color.FromArgb(R, G, B));
        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        public HSVColor()
        {
            _H = _S = _V = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        public HSVColor(Color src)
        {
            int V = Math.Max(src.R, Math.Max(src.G, src.B));
            int Z = Math.Min(src.R, Math.Min(src.G, src.B));
            _V = V / 255.0;
            double d = V - Z;
            if (V != 0)
                _S = d / V;
            else
                _S = 0.0;

            double r;
            double g;
            double b;
            if ((V - Z) != 0)
            {
                r = (V - src.R) / d;
                g = (V - src.G) / d;
                b = (V - src.B) / d;
            }
            else
                r = g = b = 0;

            if (V == src.R)
                _H = 60 * (b - g);
            else if (V == src.G)
                _H = 60 * (2 + r - b);
            else
                _H = 60 * (4 + g - r);
            if (_H < 0.0)
                _H += 360;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Color ToRGBColor()
        {
            int ht = (int)Math.Floor(_H / 60);
            double d = (_H / 60 - ht) / 60;
            if ((ht & 1) == 0) d = 1 - d;
            int V = (int)(255 * _V);
            int m = (int)(255 * _V * (1 - _S));
            int n = (int)(255 * _V * (1 - _S * d));
            switch (ht)
            {
                case 0: return Color.FromArgb(V, n, m);
                case 1: return Color.FromArgb(n, V, m);
                case 2: return Color.FromArgb(m, V, n);
                case 3: return Color.FromArgb(m, n, V);
                case 4: return Color.FromArgb(n, m, V);
                case 5: return Color.FromArgb(V, m, n);
            }
            return Color.Black;
        }
    }
}
