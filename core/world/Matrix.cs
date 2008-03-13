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

namespace freetrain.world
{
    /// <summary>
    /// Matrix that applies to directions. Immutable.
    /// </summary>
    [Serializable]
    public class Matrix
    {
        /// <summary>
        /// All the numbers are doubled to allow the matrix to perform 45-degree
        /// rotational transformation.
        /// </summary>
        private readonly int a, b, c, d;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        public Matrix(int a, int b, int c, int d)
        {
            this.a = a * 2; this.b = b * 2; this.c = c * 2; this.d = d * 2;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        public Matrix(float a, float b, float c, float d)
        {
            this.a = (int)(a * 2); this.b = (int)(b * 2); this.c = (int)(c * 2); this.d = (int)(d * 2);
        }

        private Matrix(int a, int b, int c, int d, bool dummy)
        {
            this.a = a; this.b = b; this.c = c; this.d = d;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Matrix operator +(Matrix x, Matrix y)
        {
            return new Matrix(x.a + y.a, x.b + y.b, x.c + y.c, x.d + y.d, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Matrix operator -(Matrix x, Matrix y)
        {
            return new Matrix(x.a - y.a, x.b - y.b, x.c - y.c, x.d - y.d, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Matrix operator *(int s, Matrix x)
        {
            return new Matrix(s * x.a, s * x.b, s * x.c, s * x.d, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Distance operator *(Matrix x, Distance d)
        {
            return new Distance(
                    (x.a * d.x + x.b * d.y) / 2,
                    (x.c * d.x + x.d * d.y) / 2,
                    d.z);
        }

        #region constants
        /// <summary>
        /// Identity transformation
        /// </summary>
        public static readonly Matrix E = new Matrix(1, 0, 0, 1);

        /// <summary>
        /// 90-degree left rotational transformation
        /// </summary>
        public static readonly Matrix L90 = new Matrix(0, 1, -1, 0);

        /// <summary>
        /// 90-degree right rotational transformation
        /// </summary>
        public static readonly Matrix R90 = new Matrix(0, -1, 1, 0);

        /// <summary>
        /// 180-degree rotational transformation
        /// </summary>
        public static readonly Matrix REVERSE = new Matrix(-1, 0, 0, -1);
        #endregion
    }
}
