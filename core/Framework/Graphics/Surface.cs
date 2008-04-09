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
using System.Drawing;
using System.Diagnostics;
using System.Collections;
using Tao.Sdl;
using System.IO;

namespace FreeTrain.Framework.Graphics
{
    /// <summary>
    /// Color mask.
    /// </summary>
    public enum ColorMask { 
        /// <summary>
        /// 
        /// </summary>
        R, 
        /// <summary>
        /// 
        /// </summary>
        G, 
        /// <summary>
        /// 
        /// </summary>
        B 
    };

    /// <summary>
    /// Wraps DirectDraw surface object.
    /// 
    /// This is the core object of DirectDraw.
    /// The code is a wrapper around Visual BASIC binding of DirectDraw.
    /// 
    /// Since I couldn't figure out how to create a CLR binding for
    /// clipper, this class implements a clipping support by itself.
    /// </summary>
    public class Surface
    {
        private string _filename;

        const int TOTAL_SURFACES = 10;

        private readonly int BmpHeader = 54;

        /// <summary>
        /// 
        /// </summary>
        private IntPtr[] surfacePtrs = new IntPtr[TOTAL_SURFACES];

        /// <summary>
        /// 
        /// </summary>
        public IntPtr[] SurfacePtrs
        {
            get { return surfacePtrs; }
            set { surfacePtrs = value; }
        }

        private String[] surfaceSignatures = new String[TOTAL_SURFACES];

        /// <summary>
        /// 
        /// </summary>
        private IntPtr mask;//, dupSurface;

        /// <summary>
        /// 
        /// </summary>
        public IntPtr Mask
        {
            get { return mask; }
            set { mask = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Sdl.SDL_Surface surface; //, dupSurface, mask;
        private Sdl.SDL_PixelFormat pixelFormat;

        int flags = (Sdl.SDL_SWSURFACE | Sdl.SDL_HWACCEL | Sdl.SDL_ANYFORMAT | Sdl.SDL_SRCCOLORKEY);
        int bpp = 32;

        private int currentSurfaceCount = 0;


        /// <summary>
        /// 
        /// </summary>
        public Size Size
        {
            get { return new Size(this.surface.w, this.surface.h); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="newbpp"></param>
        public Surface(int w, int h, int newbpp)
        {
            bpp = newbpp;
            this.surfacePtrs[0] = Sdl.SDL_CreateRGBSurface(flags, w, h, bpp, 0, 0, 0, 0);
            this.surface = (Sdl.SDL_Surface)Marshal.PtrToStructure(this.SurfacePtr(), typeof(Sdl.SDL_Surface));
            pixelFormat = (Sdl.SDL_PixelFormat)Marshal.PtrToStructure(this.surface.format, typeof(Sdl.SDL_PixelFormat));
            this.ResetClipRect();
            this.currentSurfaceCount++;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public Surface(int w, int h) : this(w, h, IntPtr.Zero) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="pf"></param>
        public Surface(int w, int h, IntPtr pf)
        {
            if (pf == IntPtr.Zero)
            {
                this.surfacePtrs[0] = Sdl.SDL_CreateRGBSurface(flags, w, h, bpp, 0, 0, 0, 0);
                this.surface = (Sdl.SDL_Surface)Marshal.PtrToStructure(this.SurfacePtr(), typeof(Sdl.SDL_Surface));
                pixelFormat = (Sdl.SDL_PixelFormat)Marshal.PtrToStructure(this.surface.format, typeof(Sdl.SDL_PixelFormat));
            }
            else
            {
                pixelFormat = (Sdl.SDL_PixelFormat)Marshal.PtrToStructure(pf, typeof(Sdl.SDL_PixelFormat));
                this.surfacePtrs[0] = Sdl.SDL_CreateRGBSurface(flags, w, h, bpp, pixelFormat.Rmask, pixelFormat.Gmask, pixelFormat.Bmask, pixelFormat.Amask);
                this.surface = (Sdl.SDL_Surface)Marshal.PtrToStructure(this.SurfacePtr(), typeof(Sdl.SDL_Surface));
            }

            this.SourceColorKey = Color.Magenta;
            this.ResetClipRect();
            this.currentSurfaceCount++;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        public Surface(string filename)
        {
            this.surfacePtrs[0] = Tao.Sdl.SdlImage.IMG_Load(filename);
            this.surface = (Sdl.SDL_Surface)Marshal.PtrToStructure(this.SurfacePtr(), typeof(Sdl.SDL_Surface));
            this.pixelFormat = (Sdl.SDL_PixelFormat)Marshal.PtrToStructure(this.surface.format, typeof(Sdl.SDL_PixelFormat));
            this.SourceColorKey = Color.Magenta;
            this.ResetClipRect();
            this.currentSurfaceCount++;
            this._filename = filename;
        }

        internal Surface(IntPtr surface)
        {
            this.surfacePtrs[0] = surface;
            this.surface = (Sdl.SDL_Surface)Marshal.PtrToStructure(this.SurfacePtr(), typeof(Sdl.SDL_Surface));
            this.pixelFormat = (Sdl.SDL_PixelFormat)Marshal.PtrToStructure(this.surface.format, typeof(Sdl.SDL_PixelFormat));
            this.SourceColorKey = Color.Magenta;
            this.ResetClipRect();
            this.currentSurfaceCount++;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IntPtr SurfacePtr() { return this.surfacePtrs[0]; }

        /// <summary>
        /// 
        /// </summary>
        public void ResetClipRect()
        {
            Sdl.SDL_Rect r = new Sdl.SDL_Rect(0, 0, (short)Size.Width, (short)Size.Height);
            Sdl.SDL_SetClipRect(this.SurfacePtr(), ref r);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //public Sdl.SDL_Rect clipSDLRect
        //{
        //    get
        //    {
        //        Sdl.SDL_Rect r = new Sdl.SDL_Rect();
        //        Sdl.SDL_GetClipRect(this.surfacePtr(), ref r);
        //        return r;
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        public Rectangle ClipRect
        {
            get
            {
                Sdl.SDL_Rect r = new Sdl.SDL_Rect();
                Sdl.SDL_GetClipRect(this.SurfacePtr(), ref r);
                return new Rectangle(r.x, r.y, r.w, r.h);
            }
            set
            {
                value.Intersect(new Rectangle(0, 0, Size.Width, Size.Height));
                srect.x = (short)value.X;
                srect.y = (short)value.Y;
                srect.w = (short)value.Width;
                srect.h = (short)value.Height;
                Sdl.SDL_SetClipRect(this.SurfacePtr(), ref srect);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            //i'm sure something needs to be cleared here
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="c"></param>
        ///// <returns></returns>
        //public int getSDLColor(Color c) { return Sdl.SDL_MapRGB(this.surfacePtrs[0], c.R, c.G, c.B); }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="destX"></param>
        ///// <param name="destY"></param>
        ///// <param name="source"></param>
        ///// <param name="srcRect"></param>
        //public void bltFast(int destX, int destY, Surface source, Rectangle srcRect)
        //{
        //    //RECT srect = Util.toRECT(srcRect);

        //    //Sdl.SDL_Rect src = toRect(srcRect), dst = toRect(destX, destY, srcRect.Width, srcRect.Height);
        //    //Tao.Sdl.Sdl.SDL_BlitSurface(source.surface, ref src, this.surface, ref dst);

        //    drect.x = (short)destX;
        //    drect.y = (short)destY;
        //    drect.w = (short)srcRect.Width;
        //    drect.h = (short)srcRect.Height;
        //    srect.x = (short)srcRect.X;
        //    srect.y = (short)srcRect.Y;
        //    srect.w = (short)srcRect.Width;
        //    srect.h = (short)srcRect.Height;

        //    blt(drect, source, srect);
        //}

        private Sdl.SDL_Rect drect, srect;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstPos"></param>
        /// <param name="source"></param>
        /// <param name="srcPos"></param>
        /// <param name="sz"></param>
        public void Blit(Point dstPos, Surface source, Point srcPos, Size sz)
        {
            drect.x = (short)dstPos.X;
            drect.y = (short)dstPos.Y;
            drect.w = (short)sz.Width;
            drect.h = (short)sz.Height;
            srect.x = (short)srcPos.X;
            srect.y = (short)srcPos.Y;
            srect.w = (short)sz.Width;
            srect.h = (short)sz.Height;
            Blit(drect, source, srect);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstPos"></param>
        /// <param name="source"></param>
        public void Blit(Point dstPos, Surface source)
        {
            drect.x = (short)dstPos.X;
            drect.y = (short)dstPos.Y;
            drect.w = (short)source.Size.Width;
            drect.h = (short)source.Size.Height;
            srect.x = 0;
            srect.y = 0;
            srect.w = (short)source.Size.Width;
            srect.h = (short)source.Size.Height;
            Blit(drect, source, srect);
        }

        private void Blit(Sdl.SDL_Rect dst, Surface source, Sdl.SDL_Rect src)
        {
            Tao.Sdl.Sdl.SDL_BlitSurface(source.SurfacePtr(), ref src, this.SurfacePtr(), ref dst);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        public void SetAlpha(byte val)
        {
            //surface.AlphaBlending = true;
            //surface.Alpha = 128;
            Sdl.SDL_SetAlpha(this.surfacePtrs[0], Sdl.SDL_SRCALPHA | Sdl.SDL_SRCCOLORKEY, val);
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstPos"></param>
        /// <param name="source"></param>
        /// <param name="srcPos"></param>
        /// <param name="sz"></param>
        public void BlitAlpha(Point dstPos, Surface source, Point srcPos, Size sz)
        {
            //Rectangle dst = new Rectangle( dstPos.X,dstPos.Y,sz.Width,sz.Height );
            //Rectangle src = new Rectangle( srcPos.X,srcPos.Y,sz.Width,sz.Height );
            //Util.clip( ref dst, ref src, clip );
            /*alpha.bltAlphaFast( surface, source.surface,
                dst.Left, dst.Top,
                src.Left, src.Top, src.Right, src.Bottom,
                source.colorKey );*/
            //source.handle.Transparent = false;
            source.SetAlpha(128);

            //this.sourceColorKey = Color.Magenta;

            drect.x = (short)dstPos.X;
            drect.y = (short)dstPos.Y;
            drect.w = (short)sz.Width;
            drect.h = (short)sz.Height;
            srect.x = (short)srcPos.X;
            srect.y = (short)srcPos.Y;
            srect.w = (short)sz.Width;
            srect.h = (short)sz.Height;
            //Tao.Sdl.Sdl.SDL_BlitSurface(source.surfacePtr, ref nsrc, this.surfacePtr, ref ndst);
            Blit(drect, source, srect);
            source.SetAlpha(255);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstPos"></param>
        /// <param name="source"></param>
        /// <param name="srcPos"></param>
        /// <param name="sz"></param>
        /// <param name="fill"></param>
        public void BlitShape(Point dstPos, Surface source, Point srcPos, Size sz, Color fill)
        {
            drect.x = (short)dstPos.X;
            drect.y = (short)dstPos.Y;
            drect.w = (short)sz.Width;
            drect.h = (short)sz.Height;
            srect.x = (short)srcPos.X;
            srect.y = (short)srcPos.Y;
            srect.w = (short)sz.Width;
            srect.h = (short)sz.Height;
            //this.clipRectangle( ref dst, ref src );

            /*alpha.bltShape( surface, source.surface,
                dst.Left, dst.Top,
                src.Left, src.Top, src.Right, src.Bottom,
                (int)colorToFill(fill),
                source.colorKey );*/
            //source.setAlpha(128);
            //source.fill(src,fill);

            //surface.Blit(source.fillMask(src,fill), dst,src);
            //Tao.Sdl.Sdl.SDL_BlitSurface(source.surfacePtr(), ref src, this.surfacePtr, ref dst);
            //lastMaskColor = fill;
            int index = checkMask(source, srect, fill);

            //srect.x = 0;
            //srect.y = 0;
            Tao.Sdl.Sdl.SDL_BlitSurface(this.surfacePtrs[index], ref srect, this.SurfacePtr(), ref drect);
        }

        private int checkMask(Surface source, Sdl.SDL_Rect r, Color fill)
        {
            int col, pink1, pink2;

            String curSig = "mask" + fill.ToArgb();
            int curIndex = -1;

            for (int i = 1; i <= currentSurfaceCount; i++) if (surfaceSignatures[i] == curSig) curIndex = i;

            if (curIndex == -1)
            {

                curIndex = currentSurfaceCount;
                surfacePtrs[curIndex] = Sdl.SDL_CreateRGBSurface(flags, r.w, r.h, bpp, 0, 0, 0, 0);

                Sdl.SDL_Surface maskSurf = (Sdl.SDL_Surface)Marshal.PtrToStructure(surfacePtrs[curIndex], typeof(Sdl.SDL_Surface));
                col = Sdl.SDL_MapRGB(maskSurf.format, fill.R, fill.G, fill.B);

                pink1 = Sdl.SDL_MapRGB(source.surface.format, 255, 0, 255);
                pink2 = Sdl.SDL_MapRGB(maskSurf.format, 255, 0, 255);

                drect = new Sdl.SDL_Rect(0, 0, 1, 1);
                for (int xx = 0; xx < maskSurf.w; xx++)
                {
                    for (int yy = 0; yy < maskSurf.h; yy++)
                    {
                        drect.x = (short)xx;
                        drect.y = (short)yy;
                        if (this.GetIntPixel(source.SurfacePtr(), xx, yy) != pink1) Sdl.SDL_FillRect(surfacePtrs[curIndex], ref drect, col);
                        else Sdl.SDL_FillRect(surfacePtrs[curIndex], ref drect, pink2);
                    }
                }
                Tao.Sdl.Sdl.SDL_SetColorKey(surfacePtrs[curIndex], Sdl.SDL_SRCCOLORKEY | Sdl.SDL_RLEACCEL, pink1);
                surfaceSignatures[curIndex] = curSig;
                currentSurfaceCount++;
                Console.WriteLine("Created a new one at: " + curIndex + ", sig: " + curSig);
            }
            return curIndex;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Color GetPixel(int x, int y) { return GetPixel(this.SurfacePtr(), x, y); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="surf"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Color GetPixel(IntPtr surf, int x, int y) { return this.GetColor(surf, this.GetIntPixel(surf, x, y)); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surf"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int GetIntPixel(IntPtr surf, int x, int y)
        {
            Sdl.SDL_Surface s;
            Sdl.SDL_PixelFormat pf;
            if (surf != this.SurfacePtr())
            {
                s = (Sdl.SDL_Surface)Marshal.PtrToStructure(surf, typeof(Sdl.SDL_Surface));
                pf = (Sdl.SDL_PixelFormat)Marshal.PtrToStructure(s.format, typeof(Sdl.SDL_PixelFormat));
            }
            else
            {
                s = this.surface;
                pf = this.pixelFormat;
            }

            return GetIntPixel(s, pf, surf, x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="pf"></param>
        /// <param name="surf"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int GetIntPixel(Sdl.SDL_Surface s, Sdl.SDL_PixelFormat pf, IntPtr surf, int x, int y)
        {
            IntPtr ptr = new IntPtr(s.pixels.ToInt32() + (y * s.pitch) + (x * pf.BytesPerPixel)); //* bytesPerPixel);
            int value = 0;
            switch (pf.BytesPerPixel)
            {
                case 1:
                    value = Marshal.ReadByte(ptr);
                    break;
                case 2:
                    value = Marshal.ReadInt16(ptr);
                    break;
                case 3:
                    value = MarshalHelper.ReadInt24(ptr);
                    break;
                case 4:
                    //value = Marshal.ReadInt32(ptr);
                    break;
                //default:
                //throw new SdlException(Events.StringManager.GetString("UnknownBytesPerPixel", CultureInfo.CurrentUICulture));
            }
            return value;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="dstPos"></param>
        ///// <param name="source"></param>
        ///// <param name="fill"></param>
        //public void bltShape(Point dstPos, Surface source, Color fill)
        //{
        //    bltShape(dstPos, source, new Point(0, 0), source.Size, fill);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Surface CreateFlippedVerticalSurface()
        {
            return new Surface(SdlGfx.rotozoomSurfaceXY(this.SurfacePtr(), 0, 1, -1, SdlGfx.SMOOTHING_OFF));
        }

        //public Tao.Sdl.Sdl.SDL_Surface createFlippedHorizontalSurface() { return null; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstPos"></param>
        /// <param name="source"></param>
        /// <param name="srcPos"></param>
        /// <param name="sz"></param>
        /// <param name="_srcColors"></param>
        /// <param name="_dstColors"></param>
        /// <param name="vflip"></param>
        public void BlitColorTransform(Point dstPos, Surface source, Point srcPos, Size sz, Color[] _srcColors, Color[] _dstColors, bool vflip)
        {
            if (vflip) Console.WriteLine("VFLIP ! VFLIP ! VFLIP ! VFLIP ! VFLIP ! VFLIP ! VFLIP ! ");
            int index = source.ReColor(_srcColors, _dstColors);
            //blt(dstPos, source, srcPos,sz);
            drect.x = (short)dstPos.X;
            drect.y = (short)dstPos.Y;
            drect.w = (short)sz.Width;
            drect.h = (short)sz.Height;
            srect.x = (short)srcPos.X;
            srect.y = (short)srcPos.Y;
            srect.w = (short)sz.Width;
            srect.h = (short)sz.Height;
            Tao.Sdl.Sdl.SDL_BlitSurface(source.surfacePtrs[index], ref srect, this.SurfacePtr(), ref drect);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_srcColors"></param>
        /// <param name="_dstColors"></param>
        /// <returns></returns>
        public int ReColor(Color[] _srcColors, Color[] _dstColors)
        {
            String curSig = "recolor";
            for (int curCol = 0; curCol < 4; curCol++)
                if (_dstColors.GetLength(0) > curCol) curSig += _dstColors[0].ToArgb();

            int curIndex = -1;

            for (int i = 1; i <= currentSurfaceCount; i++) if (surfaceSignatures[i] == curSig) curIndex = i;

            int colTo, colFrom;
            drect = new Sdl.SDL_Rect(0, 0, (short)this.surface.w, (short)this.surface.h);

            if (curIndex == -1)
            {
                curIndex = currentSurfaceCount;
                this.surfacePtrs[curIndex] = Sdl.SDL_CreateRGBSurface(flags, drect.w, drect.h, bpp, 0, 0, 0, 0);
                Tao.Sdl.Sdl.SDL_FillRect(this.surfacePtrs[curIndex], ref drect, Sdl.SDL_MapRGB(this.surface.format, 255, 0, 255));
                Tao.Sdl.Sdl.SDL_BlitSurface(this.surfacePtrs[0], ref drect, this.surfacePtrs[curIndex], ref drect);
                IntPtr mask = Sdl.SDL_CreateRGBSurface(flags, drect.w, drect.h, bpp, 0, 0, 0, 0);
                int i;
                for (i = 0; i < _dstColors.GetLength(0); i++)
                {
                    colTo = Sdl.SDL_MapRGB(this.surface.format, _dstColors[i].R, _dstColors[i].G, _dstColors[i].B);
                    colFrom = Sdl.SDL_MapRGB(this.surface.format, _srcColors[i].R, _srcColors[i].G, _srcColors[i].B);

                    if (i % 2 == 0)
                    {
                        Tao.Sdl.Sdl.SDL_FillRect(mask, ref drect, colTo);
                        Tao.Sdl.Sdl.SDL_SetColorKey(this.surfacePtrs[curIndex], Sdl.SDL_SRCCOLORKEY, colFrom);
                        Tao.Sdl.Sdl.SDL_BlitSurface(this.surfacePtrs[curIndex], ref drect, mask, ref drect);
                    }
                    else
                    {
                        Tao.Sdl.Sdl.SDL_FillRect(this.surfacePtrs[curIndex], ref drect, colTo);
                        Tao.Sdl.Sdl.SDL_SetColorKey(mask, Sdl.SDL_SRCCOLORKEY, colFrom);
                        Tao.Sdl.Sdl.SDL_BlitSurface(mask, ref drect, this.surfacePtrs[curIndex], ref drect);
                    }
                }

                if (i % 2 == 0)
                {
                    Tao.Sdl.Sdl.SDL_FillRect(this.surfacePtrs[curIndex], ref drect, Sdl.SDL_MapRGB(this.surface.format, 255, 0, 255));
                    Tao.Sdl.Sdl.SDL_BlitSurface(mask, ref drect, this.surfacePtrs[curIndex], ref drect);
                    Sdl.SDL_FreeSurface(mask);
                }

                this.SourceColorKey = Color.Magenta;

                surfaceSignatures[curIndex] = curSig;
                currentSurfaceCount++;
                Console.WriteLine("Created a new one at: " + curIndex + ", sig: " + curSig);
            }
            return curIndex;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dstPos"></param>
        /// <param name="source"></param>
        /// <param name="srcPos"></param>
        /// <param name="sz"></param>
        /// <param name="R_dest"></param>
        /// <param name="G_dest"></param>
        /// <param name="B_dest"></param>
        public void BlitHueTransform(Point dstPos, Surface source, Point srcPos, Size sz, Color R_dest, Color G_dest, Color B_dest)
        {
            /*RECT dst = Util.toRECT( dstPos, sz );
            RECT src = Util.toRECT( srcPos, sz );
            Util.clip( ref dst, ref src, clip );

            //Debug.WriteLine(""+R_dest.ToArgb()+","+G_dest.ToArgb()+","+B_dest.ToArgb());
            bltHueTransform( surface, source.surface,
                dst.Left, dst.Top,
                src.Left, src.Top, src.Right, src.Bottom,				
                R_dest.ToArgb(), G_dest.ToArgb(), B_dest.ToArgb(),
                source.colorKey );*/
            //surface.RedMask = R_dest.ToArgb();
            //surface.BlueMask = B_dest.ToArgb();
            //surface.Green = G_dest.ToArgb();
            //surface.Blit(source.handle, new Rectangle(dstPos.X, dstPos.Y, sz.Width, sz.Height), new Rectangle(srcPos.X, srcPos.Y, sz.Width, sz.Height));
            //throw new Exception("NOT IMPLEMENTED YET!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="col"></param>
        public void SetPixel(Point p, Color col) { SetPixel(p.X, p.Y, col); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="col"></param>
        public void SetPixel(int x, int y, Color col)
        {
            SdlGfx.pixelRGBA(this.surfacePtrs[0], (short)x, (short)y, col.R, col.G, col.B, 255);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="buf"></param>
        //public void SetPixels(Int32[] buf)
        //{
        //    Sdl.SDL_LockSurface(this.surfacePtrs[0]);
        //    Marshal.Copy(buf, 0, new IntPtr(this.surface.pixels.ToInt32()), buf.Length);
        //    Sdl.SDL_UnlockSurface(this.surfacePtrs[0]);
        //    /*
        //    try
        //    {
        //        int bytesPerPixel = this.pixelFormat.BytesPerPixel;
        //        //int pixels = this.surface.pixels.ToInt32() + point.X * bytesPerPixel;
        //        int pitch = this.surface.pitch;
        //        if (bytesPerPixel == 4)
        //        {
        //            //the buffer for a row of pixels.
        //            //Int32[] buffer = new Int32[colors.GetLength(0)];
        //            DateTime d = DateTime.Now;
        //            /*for (short x = 0; x < colors.GetLength(0); x++)
        //            {
        //                //gets only the pixels in the row that are required.
        //                for (short y = 0; y < colors.GetLength(1); y++)
        //                {
        //                    //converts the pixel to a color value.
        //                    //buffer[x] = Sdl.SDL_MapRGB(this.surface.format, colors[x, y].R, colors[x, y].G, colors[x, y].B);
        //                    SdlGfx.pixelRGBA(this.surfacePtrs[0], x, y, colors[x, y].R, colors[x, y].G, colors[x, y].B, 255);
        //                }
        //                //then copies them to the image.
        //                //Marshal.Copy(buffer, 0, new IntPtr(pixels + (y + point.Y) * pitch), buffer.Length);
                     
        //            }

        //            Marshal.Copy(buf, 0, new IntPtr(pixels), buf.Length);
        //            Console.WriteLine((DateTime.Now - d).TotalMilliseconds + "ms");
        //        }
        //        else if (bytesPerPixel == 3)
        //        {
        //            //the buffer for a row of pixels.
        //            Int32[] buffer = new Int32[colors.GetLength(0)];
        //            for (int y = 0; y < colors.GetLength(1); ++y)
        //            {
        //                //gets only the pixels in the row that are required.
        //                for (int x = 0; x < buffer.Length; ++x)
        //                {
        //                    //converts the pixel to a color value.
        //                    buffer[x] = Sdl.SDL_MapRGB(this.surface.format, colors[x, y].R, colors[x, y].G, colors[x, y].B);
        //                }
        //                //then copies them to the image.
        //                MarshalHelper.CopyInt24(buffer, 0, new IntPtr(pixels + (y + point.Y) * pitch), buffer.Length);
        //            }
        //        }
        //        else if (bytesPerPixel == 2)
        //        {
        //            //the buffer for a row of pixels.
        //            Int16[] buffer = new Int16[colors.GetLength(0)];
        //            for (int y = 0; y < colors.GetLength(1); ++y)
        //            {
        //                //gets only the pixels in the row that are required.
        //                for (int x = 0; x < buffer.Length; ++x)
        //                {
        //                    //converts the pixel to a color value.
        //                    buffer[x] = (short)Sdl.SDL_MapRGB(this.surface.format, colors[x, y].R, colors[x, y].G, colors[x, y].B);
        //                }
        //                //then copies them to the image.
        //                Marshal.Copy(buffer, 0, new IntPtr(pixels + (y + point.Y) * pitch), buffer.Length);
        //            }
        //        }
        //        else if (bytesPerPixel == 1)
        //        {
        //            //the buffer for a row of pixels.
        //            Byte[] buffer = new Byte[colors.GetLength(0)];
        //            for (int y = 0; y < colors.GetLength(1); ++y)
        //            {
        //                //gets only the pixels in the row that are required.
        //                for (int x = 0; x < buffer.Length; ++x)
        //                {
        //                    //converts the pixel to a color value.
        //                    buffer[x] = (byte)Sdl.SDL_MapRGB(this.surface.format, colors[x, y].R, colors[x, y].G, colors[x, y].B);
        //                }
        //                //then copies them to the image.
        //                Marshal.Copy(buffer, 0, new IntPtr(pixels + (y + point.Y) * pitch), buffer.Length);
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        Sdl.SDL_UnlockSurface(this.surfacePtrs[0]);
        //    }*/
        //}

        /// <summary>
        /// 
        /// </summary>
        public void BuildNightImage()
        {
            /*byte[] src = new byte[16],src2 = new byte[16], dst = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                src[i] = (byte)i;
                src2[i] = 4;
            }
            SdlGfx.SDL_imageFilterDiv(src, src2, dst, 16);*/


            if (this.pixelFormat.BytesPerPixel < 3)
            {
                this.mask = Sdl.SDL_CreateRGBSurface(flags, this.surface.w, this.surface.h, bpp, 0, 0, 0, 0);
                surface = (Sdl.SDL_Surface)Marshal.PtrToStructure(this.mask, typeof(Sdl.SDL_Surface));
                this.surfacePtrs[0] = Sdl.SDL_ConvertSurface(this.surfacePtrs[0], surface.format, flags);
                surface = (Sdl.SDL_Surface)Marshal.PtrToStructure(this.surfacePtrs[0], typeof(Sdl.SDL_Surface));
                pixelFormat = (Sdl.SDL_PixelFormat)Marshal.PtrToStructure(surface.format, typeof(Sdl.SDL_PixelFormat));
                Sdl.SDL_FreeSurface(this.mask);
            }

            Sdl.SDL_LockSurface(this.surfacePtrs[0]);
            try
            {
                int bytesPerPixel = this.pixelFormat.BytesPerPixel;
                int pixels = this.surface.pixels.ToInt32();// +point.X * bytesPerPixel;
                int pitch = this.surface.pitch;
                //These 3 �if� blocks are a perfect candidate for generics unfortunately C#
                //does not have the required constrains for this case. So I�m reduced to copying code.
                /*if (bytesPerPixel == 4)
                {
                    //the buffer for a row of pixels.
                    Int32[] buffer = new Int32[colors.GetLength(0)];
                    for (int y = 0; y < colors.GetLength(1); ++y)
                    {
                        //gets only the pixels in the row that are required.
                        for (int x = 0; x < buffer.Length; ++x)
                        {
                            //converts the pixel to a color value.
                            buffer[x] = GetColorValue(colors[x, y]);
                        }
                        //then copies them to the image.
                        Marshal.Copy(buffer, 0, new IntPtr(pixels + (y + point.Y) * pitch), buffer.Length);
                    }
                }
                else*/
                if (bytesPerPixel >= 3)
                {
                    //the buffer for a row of pixels.
                    Int32[] buffer = new Int32[this.surface.w], original = new Int32[this.surface.w];
                    for (int y = 0; y < this.surface.h; ++y)
                    {
                        if (bytesPerPixel == 3) MarshalHelper.CopyInt24(new IntPtr(pixels + y * pitch), original, 0, buffer.Length);
                        else Marshal.Copy(new IntPtr(pixels + y * pitch), original, 0, buffer.Length);
                        //gets only the pixels in the row that are required.
                        for (int x = 0; x < buffer.Length; ++x)
                        {
                            //converts the pixel to a color value.
                            Int32 lower24 = original[x] & 0x00FFFFFF;

                            if (lower24 == Sdl.SDL_MapRGB(this.surface.format, 8, 0, 0)) buffer[x] = (Int32)(original[x] & 0xFF000000) | (Int32)Sdl.SDL_MapRGB(this.surface.format, 255, 8, 8);
                            else
                                if (lower24 == Sdl.SDL_MapRGB(this.surface.format, 0, 8, 0)) buffer[x] = (Int32)(original[x] & 0xFF000000) | (Int32)Sdl.SDL_MapRGB(this.surface.format, 252, 243, 148);
                                else
                                    if (lower24 == Sdl.SDL_MapRGB(this.surface.format, 0, 0, 8)) buffer[x] = (Int32)(original[x] & 0xFF000000) | (Int32)Sdl.SDL_MapRGB(this.surface.format, 255, 227, 99);
                                    else
                                        buffer[x] = (Int32)(original[x] & 0xFF000000) | (Int32)((original[x] & 0x00FCFCFC) >> 2);// pix /= 4

                            if (x == 0 && y == 0) this.colorKey = buffer[x];
                        }
                        //then copies them to the image.
                        if (bytesPerPixel == 3) MarshalHelper.CopyInt24(buffer, 0, new IntPtr(pixels + y * pitch), buffer.Length);
                        else Marshal.Copy(buffer, 0, new IntPtr(pixels + y * pitch), buffer.Length);
                    }
                }
                else if (bytesPerPixel == 2)
                {
                    /*//the buffer for a row of pixels.
                    Int16[] buffer = new Int16[colors.GetLength(0)];
                    for (int y = 0; y < colors.GetLength(1); ++y)
                    {
                        //gets only the pixels in the row that are required.
                        for (int x = 0; x < buffer.Length; ++x)
                        {
                            //converts the pixel to a color value.
                            buffer[x] = (Int16)GetColorValue(colors[x, y]);
                        }
                        //then copies them to the image.
                        Marshal.Copy(buffer, 0, new IntPtr(pixels + (y + point.Y) * pitch), buffer.Length);
                    }*/
                }
                else if (bytesPerPixel == 1)
                {
                    //the buffer for a row of pixels.
                    byte[] buffer = new byte[this.surface.w];
                    for (int y = 0; y < this.surface.h; ++y)
                    {
                        Marshal.Copy(new IntPtr(pixels + y * pitch), buffer, 0, buffer.Length);
                        //gets only the pixels in the row that are required.
                        for (int x = 0; x < buffer.Length; ++x)
                        {
                            //converts the pixel to a color value.
                            darken16((byte)Sdl.SDL_MapRGB(this.surface.format, 8, 0, 0),
                                (byte)Sdl.SDL_MapRGB(this.surface.format, 0, 8, 0),
                                (byte)Sdl.SDL_MapRGB(this.surface.format, 0, 0, 8),
                                (byte)Sdl.SDL_MapRGB(this.surface.format, 255, 8, 8),
                                (byte)Sdl.SDL_MapRGB(this.surface.format, 252, 243, 148),
                                (byte)Sdl.SDL_MapRGB(this.surface.format, 255, 227, 99),
                                0x33, ref buffer[x]);

                            if (x == 0 && y == 0) this.colorKey = buffer[x];
                        }
                        //then copies them to the image.
                        Marshal.Copy(buffer, 0, new IntPtr(pixels + y * pitch), buffer.Length);
                    }
                }
                else
                {
                    //throw new SdlException(Events.StringManager.GetString("UnknownBytesPerPixel", CultureInfo.CurrentUICulture));
                }
            }
            finally
            {
                Sdl.SDL_UnlockSurface(this.surfacePtrs[0]);
            }

        }

        private void darken16(byte color1, byte color2, byte color3,
                                byte light1, byte light2, byte light3, byte colormask, ref byte pix)
        {
            if (pix == color1)
                pix = light1;
            else
                if (pix == color2)
                    pix = light2;
                else
                    if (pix == color3)
                        pix = light3;
                    else
                        pix = (byte)((pix & colormask) >> 1);		// pix /= 4
        }

        /// <summary>
        /// Fills the surface.
        /// </summary>
        public void Fill(Color c)
        {
            Fill(ClipRect, c);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="c"></param>
        public void Fill(Rectangle rect, Color c)
        {
            //rect.Intersect(clip);
            //surface.Fill(rect, c);
            srect.x = (short)rect.X;
            srect.y = (short)rect.Y;
            srect.w = (short)rect.Width;
            srect.h = (short)rect.Height;
            Tao.Sdl.Sdl.SDL_FillRect(this.SurfacePtr(), ref srect, Sdl.SDL_MapRGB(this.surface.format, c.R, c.G, c.B));
        }

        private int _colKey;
        private int colorKey
        {
            get { return _colKey; }
            set { _colKey = value; Tao.Sdl.Sdl.SDL_SetColorKey(this.SurfacePtr(), Sdl.SDL_SRCCOLORKEY | Sdl.SDL_RLEACCEL, _colKey); }
        }

        /// <summary>
        /// Source color key. A mask color that will not be copied to other plains.
        /// </summary>
        public Color SourceColorKey
        {
            get { return Color.FromArgb(_colKey); }
            set
            {
                colorKey = Sdl.SDL_MapRGB(this.surface.format, value.R, value.G, value.B);
            }
        }

        // retruns true if the color at the specified pixel is valid (opaque).
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool HitTest(int x, int y)
        {
            if (x < 0 || x > Size.Width || y < 0 || y > Size.Height)
                return false;
            return ((getColorAt(x, y) & 0xffffff) == colorKey);
        }

        // returns color at specified point.
        // the return value suited for current pixel format.
        // outrange point will raise an error.
        int getColorAt(int x, int y)
        {
            Color c = this.GetPixel(x, y);
            return c.R & c.G & c.B;

        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        ///// <returns></returns>
        //public Color getColor(int x, int y)
        //{
        //    return this.GetPixel(x, y);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="surf"></param>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        ///// <returns></returns>
        //public Color getColor(IntPtr surf, int x, int y)
        //{
        //    return this.GetPixel(surf, x, y);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="colorValue"></param>
        ///// <returns></returns>
        //public System.Drawing.Color GetColor(int colorValue)
        //{
        //    byte r, g, b, a;
        //    Sdl.SDL_GetRGBA(colorValue, this.surface.format, out r, out g, out b, out a);
        //    return System.Drawing.Color.FromArgb(a, r, g, b);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surf"></param>
        /// <param name="colorValue"></param>
        /// <returns></returns>
        public System.Drawing.Color GetColor(IntPtr surf, int colorValue)
        {
            byte r, g, b, a;
            Sdl.SDL_GetRGBA(colorValue, surf, out r, out g, out b, out a);
            return System.Drawing.Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        public void DrawBox(Rectangle r) { DrawBox(r, Color.CornflowerBlue); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="c"></param>
        public void DrawBox(Rectangle r, Color c)
        {
            /*surface.Draw(new SdlDotNet.Graphics.Primitives.Box((short)r.Left,
                                                            (short)r.Top,
                                                            (short)r.Right,
                                                            (short)r.Bottom), c);*/
            //SdlGfx.boxColor(this.surfacePtr(),(short)r.X,(short)r.Y,(short)(r.X+r.Width),(short)(r.Y+r.Height),Sdl.SDL_MapRGB(this.surface.format,c.R,c.G,c.B));
            SdlGfx.rectangleRGBA(this.SurfacePtr(), (short)r.X, (short)r.Y, (short)(r.X + r.Width), (short)(r.Y + r.Height), c.R, c.G, c.B, 255);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="p1"></param>
        ///// <param name="p2"></param>
        ///// <param name="p3"></param>
        ///// <param name="p4"></param>
        //public void drawPolygon(Point p1, Point p2, Point p3, Point p4) { drawBox(new Rectangle(p1, new Size(p2.X - p1.X, p4.Y - p1.Y))); }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="col"></param>
        ///// <param name="pts"></param>
        //public void drawPolygon(Color col, Point[] pts)
        //{
        //    //surface.Draw(new SdlDotNet.Graphics.Primitives.Polygon(new ArrayList(pts)), col);
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="col"></param>
        /// <param name="pts"></param>
        public void FillPolygon(Color col, Point[] pts)
        {
            //Surface tx = new Surface(Video.CreateRgbSurface(20,20));
            //tx.fill(col);
            //surface.Draw(new SdlDotNet.Graphics.Primitives.TexturedPolygon(tx.handle,new ArrayList(pts), new Point(0,0)), col,false,true);
            short[] ptX = new short[pts.GetLength(0)];
            short[] ptY = new short[pts.GetLength(0)];
            for (int i = 0; i < pts.GetLength(0); i++)
            {
                ptX[i] = (short)pts[i].X;
                ptY[i] = (short)pts[i].Y;
            }
            Tao.Sdl.SdlGfx.filledPolygonRGBA(this.SurfacePtr(), ptX, ptY, pts.GetLength(0), col.R, col.G, col.B, 255);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="col"></param>
        /// <param name="pts"></param>
        public void DrawLines(Color col, Point[] pts)
        {
            for (int curPoint = 0; curPoint <= (pts.GetLength(0) - 2); curPoint++)
                Tao.Sdl.SdlGfx.lineRGBA(this.SurfacePtr(), (short)pts[curPoint].X, (short)pts[curPoint].Y, (short)pts[curPoint + 1].X, (short)pts[curPoint + 1].Y, col.R, col.G, col.B, 255);
            //    surface.Draw(new SdlDotNet.Graphics.Primitives.Line((Point)pts[curPoint], (Point)pts[curPoint + 1]), col,false,false);
        }

        ///// <summary>
        ///// Tries to recover a lost surface.
        ///// </summary>
        //public void restore()
        //{
        //    //handle.restore();
        //}

        /// <summary>
        /// Makes the bitmap of this surface.
        /// The caller needs to dispose the bitmap.
        /// </summary>

        public Bitmap Bitmap
        {
            get
            {
                byte[] arr = new byte[(this.Size.Width * this.Size.Height * this.bpp) + this.BmpHeader];
                IntPtr i = Marshal.AllocHGlobal(arr.Length);

                try
                {

                    try
                    {
                        Marshal.Copy(arr, 0, i, arr.Length);
                        Sdl.SDL_SaveBMP_RW(this.SurfacePtr(), Sdl.SDL_RWFromMem(i, arr.Length), 1);
                        Marshal.Copy(i, arr, 0, arr.Length);
                    }
                    catch (AccessViolationException e)
                    {
                        Console.WriteLine(e.StackTrace);
                        e.ToString();
                    }

                    Bitmap bitmap;
                    try
                    {
                        if (arr != null)
                        {
                            bitmap = (Bitmap)Bitmap.FromStream(new MemoryStream(arr, 0, arr.Length));
                            return bitmap;
                        }
                        else
                        {
                            return new Bitmap(1, 1);
                        }
                    }
                    catch (OutOfMemoryException e)
                    {
                        e.ToString();
                        bitmap = new Bitmap(1, 1);
                        return bitmap;
                    }
                    catch (ArgumentException e)
                    {
                        e.ToString();
                        bitmap = new Bitmap(1, 1);
                        return bitmap;
                    }
                    catch (ExternalException e)
                    {
                        e.ToString();
                        bitmap = new Bitmap(1, 1);
                        return bitmap;
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(i);
                }
            }
        }
    }

    static class MarshalHelper
    {
        private const int sizeofInt24 = 3;
        private static readonly int offset = ((BitConverter.IsLittleEndian) ? (0) : (1));

        public static Int32 ReadInt24(IntPtr ptr)
        {
            //creates a buffer to put the data read for the pointer
            Byte[] buffer = new Byte[sizeofInt24];
            //then reads it from the pointer.
            Marshal.Copy(ptr, buffer, 0, sizeofInt24);

            //creates the actaul return value
            Int32[] result = new Int32[1] { 0 };
            //then copies the data in the buffer into it.
            Buffer.BlockCopy(buffer, 0, result, offset, sizeofInt24);
            return result[0];
        }
        //public static void WriteInt24(IntPtr ptr, Int32 value)
        //{
        //    Int32[] valueArray = new Int32[1] { value };
        //    Byte[] buffer = new Byte[sizeofInt24];
        //    Buffer.BlockCopy(valueArray, offset, buffer, 0, sizeofInt24);
        //    Marshal.Copy(buffer, 0, ptr, sizeofInt24);
        //}
        public static void CopyInt24(Int32[] array, int startIndex, IntPtr destination, int length)
        {
            if (array == null) { throw new ArgumentNullException("array"); }
            if (startIndex < 0 || length + startIndex > array.Length) { throw new ArgumentOutOfRangeException("startIndex"); }
            Byte[] buffer = new byte[length * sizeofInt24];
            for (int index = startIndex * sizeof(Int32), index2 = 0;
                index2 < buffer.Length;
                index2 += sizeofInt24, index += sizeof(Int32))
            {
                Buffer.BlockCopy(array, index + offset, buffer, index2, sizeofInt24);
            }
            Marshal.Copy(buffer, 0, destination, buffer.Length);
        }
        public static void CopyInt24(IntPtr source, Int32[] array, int startIndex, int length)
        {
            if (array == null) { throw new ArgumentNullException("array"); }
            if (startIndex < 0 || length + startIndex > array.Length) { throw new ArgumentOutOfRangeException("startIndex"); }
            Byte[] buffer = new byte[length * sizeofInt24];
            Marshal.Copy(source, buffer, 0, buffer.Length);
            Array.Clear(array, startIndex, length);
            for (int index = startIndex * sizeof(Int32), index2 = 0;
                index2 < buffer.Length;
                index2 += sizeofInt24, index += sizeof(Int32))
            {
                Buffer.BlockCopy(buffer, index2, array, index + offset, sizeofInt24);
            }
        }
    }

}
