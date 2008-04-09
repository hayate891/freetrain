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
using FreeTrain.Framework;
using FreeTrain.World;
using FreeTrain.Framework.Graphics;

namespace FreeTrain.Framework.Graphics
{
    /// <summary>
    /// Utility class that draws a preview image programatically.
    /// 
    /// This helper class it typically useful to prepare an image
    /// for a controller dialog box.
    /// </summary>
    public class PreviewDrawer : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Surface surface;

        /// <summary>
        /// 
        /// </summary>
        public Surface Surface
        {
            get { return surface; }
        } 

        /// <summary>
        /// pixelSize of the canvas.
        /// </summary>
        private readonly Size pixelSize;

        /// <summary>
        /// The point in the surface of (X,Y)=(0,0).
        /// </summary>
        private readonly Point ptOrigin = new Point();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pixelSize"></param>
        /// <param name="size"></param>
        public PreviewDrawer(Size pixelSize, Distance size)
            : this(pixelSize, new Size(size.x, size.y), size.z) { }

        /// <summary>
        /// Creates an empty canvas with the given pixel size.
        /// </summary>
        /// <param name="pixelSize">Pixel size of the canvas</param>
        /// <param name="objSize">
        ///		Chip size of the object that we'd like to draw.
        ///		All the successive method calls will use this size as offset.
        /// </param>
        /// <param name="height"></param>
        public PreviewDrawer(Size pixelSize, Size objSize, int height)
        {
            surface = new Surface(pixelSize.Width, pixelSize.Height);// ResourceUtil.directDraw.createOffscreenSurface(pixelSize);
            this.pixelSize = pixelSize;

            int P = (objSize.Width + objSize.Height) * 8;

            ptOrigin.X = (pixelSize.Width - P * 2) / 2;
            ptOrigin.Y = (pixelSize.Height - P - height * 16) / 2 /*top*/ + (8 * objSize.Width + height * 16) - 8;

            Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            surface.Dispose();
        }

        /// <summary> Clears the canvas by tiling empty chips. </summary>
        public void Clear()
        {
            ISprite empty = ResourceUtil.GetGroundChip(WorldDefinition.World);
            for (int y = (ptOrigin.Y % 8) - 16; y < pixelSize.Height; y += 8)
            {
                int x = (ptOrigin.X % 32) - 64;
                if ((((y - ptOrigin.Y) / 8) % 2) != 0) x += 16;

                for (; x < pixelSize.Width; x += 32)
                    empty.Draw(surface, new Point(x, y));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <returns></returns>
        public Point GetPoint(int offsetX, int offsetY)
        {
            return GetPoint(offsetX, offsetY, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <param name="offsetZ"></param>
        /// <returns></returns>
        public Point GetPoint(int offsetX, int offsetY, int offsetZ)
        {
            Point o = ptOrigin;
            o.X += (offsetX + offsetY) * 16;
            o.Y += (-offsetX + offsetY) * 8;
            o.Y -= offsetZ * 16;
            return o;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        public void Draw(ISprite sprite, int offsetX, int offsetY)
        {
            sprite.Draw(surface, GetPoint(offsetX, offsetY));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sprites"></param>
        public void DrawCenter(ISprite[,] sprites)
        {
            Draw(sprites, 0, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sprites"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        public void Draw(ISprite[,] sprites, int offsetX, int offsetY)
        {
            Draw(sprites, offsetX, offsetY, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sprites"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <param name="offsetZ"></param>
        public void Draw(ISprite[,] sprites, int offsetX, int offsetY, int offsetZ)
        {
            int X = sprites.GetLength(0);
            int Y = sprites.GetLength(1);

            Point o = GetPoint(offsetX, offsetY, offsetZ);

            for (int y = 0; y < Y; y++)
            {
                for (int x = 0; x < X; x++)
                {
                    Point pt = o;
                    pt.X += (x + y) * 16;
                    pt.Y += (-x + y) * 8;

                    sprites[x, y].Draw(surface, pt);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sprites"></param>
        [CLSCompliant(false)]
        public void DrawCenter(ISprite[, ,] sprites)
        {
            Draw(sprites, 0, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sprites"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        [CLSCompliant(false)]
        public void Draw(ISprite[, ,] sprites, int offsetX, int offsetY)
        {
            Draw(sprites, offsetX, offsetY, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sprites"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <param name="offsetZ"></param>
        [CLSCompliant(false)]
        public void Draw(ISprite[, ,] sprites, int offsetX, int offsetY, int offsetZ)
        {
            int X = sprites.GetLength(0);
            int Y = sprites.GetLength(1);
            int Z = sprites.GetLength(2);

            Point o = GetPoint(offsetX, offsetY, offsetZ);

            for (int z = 0; z < Z; z++)
            {
                for (int y = 0; y < Y; y++)
                {
                    for (int x = 0; x < X; x++)
                    {
                        Point pt = o;
                        pt.X += (x + y) * 16;
                        pt.Y += (-x + y) * 8 - z * 16;

                        sprites[x, y, z].Draw(surface, pt);
                    }
                }
            }
        }

        //		/// <summary>
        //		/// Gets the pixel location from (h,v) coordinate.
        //		/// </summary>
        //		public Point getLocation( int h, int v ) {
        //			Point pt = new Point(h*32-16,v*8-8);
        //			if((v%2)==1)	pt.X += 16;
        //
        //			return pt;
        //		}
        //
        //		public Point getCenterChip( Size sz ) {
        //			// TODO: think about this equation more
        //			return getLocation( (size.Width-sz.x-sz.y)/2, size.Height/2 + (sz.x-sz.y) );
        //		}




        /// <summary>
        /// Makes the bitmap of the current picture.
        /// 
        /// The caller needs to dispose the bitmap.
        /// </summary>
        public Bitmap CreateBitmap()
        {
            return surface.Bitmap;
        }
    }
}
