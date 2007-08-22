﻿using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using System.Collections;
//using DxVBLib;
//using DirectDrawAlphaBlendLib;
//using SdlDotNet.Graphics;
using Tao.Sdl;

namespace org.kohsuke.directdraw
{
	/// <summary>
	/// Color mask.
	/// </summary>
	public enum ColorMask { R,G,B };

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
		//private DirectDrawSurface7 surface;
		//public Tao.Sdl.Sdl.SDL_Surface surface;
        //Surface mask;
		//private static AlphaBlender alpha = new AlphaBlenderClass();

		/// <summary> Bit-width. </summary>
		//private readonly byte widthR,widthB,widthG;

		/// <summary>
		/// Clipping rect. Even if the client doesn't set any clipping,
		/// this is initialized to (0,0)-(size)
		/// </summary>
		//private Rectangle clip;

        private string _filename;

        const int TOTAL_SURFACES = 10;

        public IntPtr[] surfacePtrs = new IntPtr[TOTAL_SURFACES];
        private String[] surfaceSignatures = new String[TOTAL_SURFACES];
        
        public IntPtr mask, dupSurface;
        public Sdl.SDL_Surface surface; //, dupSurface, mask;
        private Sdl.SDL_PixelFormat pixelFormat;

        int flags = (Sdl.SDL_SWSURFACE | Sdl.SDL_HWACCEL | Sdl.SDL_ANYFORMAT | Sdl.SDL_SRCCOLORKEY);
        int bpp = 32;

        private int currentSurfaceCount = 0;



        public Size size { 
            get { return new Size(this.surface.w, this.surface.h); }
        }

        public Surface(int w, int h, int newbpp)
        {
            bpp = newbpp;
            this.surfacePtrs[0] = Sdl.SDL_CreateRGBSurface(flags, w, h, bpp, 0, 0, 0, 0);
            this.surface = (Sdl.SDL_Surface)Marshal.PtrToStructure(this.surfacePtr(), typeof(Sdl.SDL_Surface));
            pixelFormat = (Sdl.SDL_PixelFormat)Marshal.PtrToStructure(this.surface.format, typeof(Sdl.SDL_PixelFormat));
            this.resetClipRect();
            this.currentSurfaceCount++;
        }

        public Surface(int w, int h) : this(w, h, IntPtr.Zero) {}
        public Surface(int w, int h, IntPtr pf){
            if (pf == IntPtr.Zero) {
                this.surfacePtrs[0] = Sdl.SDL_CreateRGBSurface(flags, w, h, bpp, 0, 0, 0, 0);
                this.surface = (Sdl.SDL_Surface)Marshal.PtrToStructure(this.surfacePtr(), typeof(Sdl.SDL_Surface));
                pixelFormat = (Sdl.SDL_PixelFormat)Marshal.PtrToStructure(this.surface.format, typeof(Sdl.SDL_PixelFormat));
            }
            else
            {
                pixelFormat = (Sdl.SDL_PixelFormat)Marshal.PtrToStructure(pf, typeof(Sdl.SDL_PixelFormat));
                this.surfacePtrs[0] = Sdl.SDL_CreateRGBSurface(flags, w, h, bpp, pixelFormat.Rmask, pixelFormat.Gmask, pixelFormat.Bmask, pixelFormat.Amask);
                this.surface = (Sdl.SDL_Surface)Marshal.PtrToStructure(this.surfacePtr(), typeof(Sdl.SDL_Surface));
            }
            
            this.sourceColorKey = Color.Magenta;
            this.resetClipRect();
            this.currentSurfaceCount++;
        }
        public Surface(string filename)
        {
            this.surfacePtrs[0] = Tao.Sdl.SdlImage.IMG_Load(filename);
            this.surface = (Sdl.SDL_Surface)Marshal.PtrToStructure(this.surfacePtr(), typeof(Sdl.SDL_Surface));
            this.pixelFormat = (Sdl.SDL_PixelFormat)Marshal.PtrToStructure(this.surface.format, typeof(Sdl.SDL_PixelFormat));
            this.sourceColorKey = Color.Magenta;
            this.resetClipRect();
            this.currentSurfaceCount++;
            this._filename = filename;
        }
        internal Surface(IntPtr surface)
        {
            this.surfacePtrs[0] = surface;
            this.surface = (Sdl.SDL_Surface)Marshal.PtrToStructure(this.surfacePtr(), typeof(Sdl.SDL_Surface));
            this.pixelFormat = (Sdl.SDL_PixelFormat)Marshal.PtrToStructure(this.surface.format, typeof(Sdl.SDL_PixelFormat));
            this.sourceColorKey = Color.Magenta;
            this.resetClipRect();
            this.currentSurfaceCount++;
        }

        public IntPtr surfacePtr() { return this.surfacePtrs[0]; }

        public void resetClipRect()
        {
            Sdl.SDL_Rect r = new Sdl.SDL_Rect(0, 0, (short)size.Width, (short)size.Height);
            Sdl.SDL_SetClipRect(this.surfacePtr(), ref r);
        }
        public Sdl.SDL_Rect clipSDLRect
        {
            get
            {
                Sdl.SDL_Rect r = new Sdl.SDL_Rect();
                Sdl.SDL_GetClipRect(this.surfacePtr(), ref r);
                return r;
            }
        }
		public Rectangle clipRect {
            get
            {
                Sdl.SDL_Rect r = new Sdl.SDL_Rect();
                Sdl.SDL_GetClipRect(this.surfacePtr(), ref r);
                return new Rectangle(r.x, r.y, r.w, r.h);
            }
			set
            {
				value.Intersect( new Rectangle( 0,0, size.Width, size.Height ) );
                srect.x = (short)value.X;
                srect.y = (short)value.Y;
                srect.w = (short)value.Width;
                srect.h = (short)value.Height;
                Sdl.SDL_SetClipRect(this.surfacePtr(), ref srect);
			}
		}
		

		public void Dispose() {
            //i'm sure something needs to be cleared here
		}

        public int getSDLColor(Color c) { return Sdl.SDL_MapRGB(this.surfacePtrs[0], c.R, c.G, c.B); }

		public void bltFast( int destX, int destY, Surface source, Rectangle srcRect ) {
			//RECT srect = Util.toRECT(srcRect);

            //Sdl.SDL_Rect src = toRect(srcRect), dst = toRect(destX, destY, srcRect.Width, srcRect.Height);
            //Tao.Sdl.Sdl.SDL_BlitSurface(source.surface, ref src, this.surface, ref dst);

            drect.x = (short)destX;
            drect.y = (short)destY;
            drect.w = (short)srcRect.Width;
            drect.h = (short)srcRect.Height;
            srect.x = (short)srcRect.X;
            srect.y = (short)srcRect.Y;
            srect.w = (short)srcRect.Width;
            srect.h = (short)srcRect.Height;

            blt(drect,source,srect);
		}

        //public Sdl.SDL_Rect toRect(Rectangle r) { return new Sdl.SDL_Rect((short)r.Left, (short)r.Top, (short)r.Width, (short)r.Height); }
        //public Sdl.SDL_Rect toRect(int x, int y, int w, int h) { return new Sdl.SDL_Rect((short)x, (short)y, (short)w, (short)h); }

		/// <summary>
		/// Copies an image from another surface.
		/// </summary>
        /// 

        private Sdl.SDL_Rect drect, srect;

        public void blt(Point dstPos, Surface source, Point srcPos, Size sz)
        {
            drect.x = (short)dstPos.X;
            drect.y = (short)dstPos.Y;
            drect.w = (short)sz.Width;
            drect.h = (short)sz.Height;
            srect.x = (short)srcPos.X;
            srect.y = (short)srcPos.Y;
            srect.w = (short)sz.Width;
            srect.h = (short)sz.Height;
            blt(drect, source, srect);
        }

        public void blt(Point dstPos, Surface source)
        {
            drect.x = (short)dstPos.X;
            drect.y = (short)dstPos.Y;
            drect.w = (short)source.size.Width;
            drect.h = (short)source.size.Height;
            srect.x = 0;
            srect.y = 0;
            srect.w = (short)source.size.Width;
            srect.h = (short)source.size.Height;
            blt(drect, source, srect);
        }

		public void blt( int dstX1, int dstY1, int dstX2, int dstY2, Surface source,
						 int srcX1, int srcY1, int srcX2, int srcY2 ) {
                             drect.x = (short)dstX1;
                             drect.y = (short)dstY1;
                             drect.w = (short)(dstX2 - dstX1);
                             drect.h = (short)(dstY2 - dstY1);
                             srect.x = (short)srcX1;
                             srect.y = (short)srcY1;
                             srect.w = (short)(srcX2 - srcX1);
                             srect.h = (short)(srcY2 - srcY1);
			                 blt( drect, source, srect );
		}

        public  void blt(Point dst, Surface source, Rectangle src) {blt(new Rectangle(dst.X, dst.Y, src.Width, src.Height), source, src);}
		private void blt(Rectangle dst, Surface source, Rectangle src ) {
            drect.x = (short)dst.X;
            drect.y = (short)dst.Y;
            drect.w = (short)dst.Width;
            drect.h = (short)dst.Height;
            srect.x = (short)src.X;
            srect.y = (short)src.Y;
            srect.w = (short)src.Width;
            srect.h = (short)src.Height;
            Tao.Sdl.Sdl.SDL_BlitSurface(source.surfacePtr(), ref srect, this.surfacePtr(), ref drect);
		}

        private void blt(Sdl.SDL_Rect dst, Surface source, Sdl.SDL_Rect src)
        {
            Tao.Sdl.Sdl.SDL_BlitSurface(source.surfacePtr(), ref src, this.surfacePtr(), ref dst);
        }
		
		public void setAlpha(byte val) {
			//surface.AlphaBlending = true;
			//surface.Alpha = 128;
            Sdl.SDL_SetAlpha(this.surfacePtrs[0],  Sdl.SDL_SRCALPHA | Sdl.SDL_SRCCOLORKEY, val);
		}
/*
        public void clipRectangle(ref Rectangle dst, ref Rectangle src)
        {
            int t;

            // compute new dst.Left
            t = Math.Max(dst.Left, clip.Left);
            src.X += (t - dst.Left);
            dst.X = t;

            t = Math.Max(dst.Top, clip.Top);
            src.Y += (t - dst.Top);
            dst.Y = t;

            t = Math.Min(dst.Right, clip.Right);
            //src.Width += (t - dst.Right);
            //dst.Width = t - dst.Left;

            t = Math.Min(dst.Bottom, clip.Bottom);
            //src.Height += (t - dst.Bottom);
            //dst.Height = t - dst.Top;
        }

        public void clipVflip(ref Rectangle dst, ref Rectangle src)
        {
            //this.resetClipRect();
            int t;

            // compute new dst.Left
            t = Math.Max(dst.Left, clip.Left);
            //src.X += (t - dst.Left);
            dst.X = t;		// dst.Left += (t-dst.Left)

            t = Math.Max(dst.Top, clip.Top);
            //src.Height -= (t - dst.Top);		// different than the clip method
            dst.Y = t;

            t = Math.Min(dst.Right, clip.Right);
            //src.Width += (t - dst.Right);
            dst.Width = t - dst.Left;

            t = Math.Min(dst.Bottom, clip.Bottom);
            //src.Y -= (t - dst.Bottom);	// different than the clip method
            dst.Height = t - dst.Top;
        }
        */
        public void bltAlpha(Point dstPos,Surface source) {bltAlpha(dstPos, source, new Point(0, 0), source.size);}
		public void bltAlpha(Point dstPos,Surface source, Point srcPos, Size sz ) {
			//Rectangle dst = new Rectangle( dstPos.X,dstPos.Y,sz.Width,sz.Height );
			//Rectangle src = new Rectangle( srcPos.X,srcPos.Y,sz.Width,sz.Height );
			//Util.clip( ref dst, ref src, clip );
			/*alpha.bltAlphaFast( surface, source.surface,
				dst.Left, dst.Top,
				src.Left, src.Top, src.Right, src.Bottom,
				source.colorKey );*/
            ///source.handle.Transparent = false;
            source.setAlpha(128);

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
            blt(drect, source, srect);
            source.setAlpha(255);
		}

		public void bltShape( Point dstPos, Surface source, Point srcPos, Size sz, Color fill ) {
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
            Tao.Sdl.Sdl.SDL_BlitSurface(this.surfacePtrs[index], ref srect, this.surfacePtr(), ref drect);
		}

        private int checkMask(Surface source, Sdl.SDL_Rect r, Color fill)
        {
            int col, pink1, pink2;

            String curSig = "mask" + fill.ToArgb();
            int curIndex = -1;

            for (int i = 1; i <= currentSurfaceCount; i++) if (surfaceSignatures[i] == curSig) curIndex = i;

            if (curIndex == -1) {

                curIndex = currentSurfaceCount;
                surfacePtrs[curIndex] = Sdl.SDL_CreateRGBSurface(flags, r.w, r.h, bpp, 0, 0, 0, 0);

                Sdl.SDL_Surface maskSurf = (Sdl.SDL_Surface)Marshal.PtrToStructure(surfacePtrs[curIndex], typeof(Sdl.SDL_Surface));
                col = Sdl.SDL_MapRGB(maskSurf.format, fill.R, fill.G, fill.B);

                pink1 = Sdl.SDL_MapRGB(source.surface.format, 255, 0, 255);
                pink2 = Sdl.SDL_MapRGB(maskSurf.format, 255, 0, 255);

                drect = new Sdl.SDL_Rect(0,0, 1, 1);
                for (int xx = 0; xx < maskSurf.w; xx++)
                {
                    for (int yy = 0; yy < maskSurf.h; yy++)
                    {
                        drect.x = (short)xx;
                        drect.y = (short)yy;
                        if (this.GetIntPixel(source.surfacePtr(), xx, yy) != pink1) Sdl.SDL_FillRect(surfacePtrs[curIndex], ref drect, col);
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

        public Color GetPixel(int x, int y) { return GetPixel(this.surfacePtr(), x, y); }
        public Color GetPixel(IntPtr surf, int x, int y) { return this.GetColor(surf, this.GetIntPixel(surf, x, y)); }

        public int GetIntPixel(IntPtr surf, int x, int y)
        {
            Sdl.SDL_Surface s;
            Sdl.SDL_PixelFormat pf;
            if (surf != this.surfacePtr())
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

        public void bltShape(Point dstPos, Surface source, Color fill)
        {
			bltShape( dstPos, source, new Point(0,0), source.size, fill );
		}

        public Surface createFlippedVerticalSurface()
        { 
            return new Surface(SdlGfx.rotozoomSurfaceXY(this.surfacePtr(),0,1,-1,SdlGfx.SMOOTHING_OFF));
        }

        //public Tao.Sdl.Sdl.SDL_Surface createFlippedHorizontalSurface() { return null; }

		public void bltColorTransform( Point dstPos, Surface source,Point srcPos, Size sz,Color[] _srcColors, Color[] _dstColors, bool vflip ) {
            if (vflip) Console.WriteLine("VFLIP ! VFLIP ! VFLIP ! VFLIP ! VFLIP ! VFLIP ! VFLIP ! ");
            int index = source.reColor(_srcColors,_dstColors);
            //blt(dstPos, source, srcPos,sz);
            drect.x = (short)dstPos.X;
            drect.y = (short)dstPos.Y;
            drect.w = (short)sz.Width;
            drect.h = (short)sz.Height;
            srect.x = (short)srcPos.X;
            srect.y = (short)srcPos.Y;
            srect.w = (short)sz.Width;
            srect.h = (short)sz.Height;
            Tao.Sdl.Sdl.SDL_BlitSurface(source.surfacePtrs[index], ref srect, this.surfacePtr(), ref drect);
		}

        public int reColor(Color[] _srcColors, Color[] _dstColors) {
            String curSig = "recolor" + _dstColors[0].ToArgb() + _dstColors[1].ToArgb() + _dstColors[2].ToArgb() + _dstColors[3].ToArgb();
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

                this.sourceColorKey = Color.Magenta;

                surfaceSignatures[curIndex] = curSig;
                currentSurfaceCount++;
                Console.WriteLine("Created a new one at: " + curIndex + ", sig: " + curSig);
            }
            return curIndex;
        }

		public void bltHueTransform(Point dstPos,Surface source,Point srcPos,Size sz,Color R_dest, Color G_dest, Color B_dest ) {
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

        public void SetPixel(Point p, Color col) { SetPixel(p.X, p.Y, col); }
        public void SetPixel(int x, int y, Color col)
        {
            SdlGfx.pixelRGBA(this.surfacePtrs[0], (short)x, (short)y, col.R, col.G, col.B, 255);
        }

        public void SetPixels(Int32[] buf)
        {
            Sdl.SDL_LockSurface(this.surfacePtrs[0]);
            Marshal.Copy(buf, 0, new IntPtr(this.surface.pixels.ToInt32()), buf.Length);
            Sdl.SDL_UnlockSurface(this.surfacePtrs[0]);
            /*
            try
            {
                int bytesPerPixel = this.pixelFormat.BytesPerPixel;
                //int pixels = this.surface.pixels.ToInt32() + point.X * bytesPerPixel;
                int pitch = this.surface.pitch;
                if (bytesPerPixel == 4)
                {
                    //the buffer for a row of pixels.
                    //Int32[] buffer = new Int32[colors.GetLength(0)];
                    DateTime d = DateTime.Now;
                    /*for (short x = 0; x < colors.GetLength(0); x++)
                    {
                        //gets only the pixels in the row that are required.
                        for (short y = 0; y < colors.GetLength(1); y++)
                        {
                            //converts the pixel to a color value.
                            //buffer[x] = Sdl.SDL_MapRGB(this.surface.format, colors[x, y].R, colors[x, y].G, colors[x, y].B);
                            SdlGfx.pixelRGBA(this.surfacePtrs[0], x, y, colors[x, y].R, colors[x, y].G, colors[x, y].B, 255);
                        }
                        //then copies them to the image.
                        //Marshal.Copy(buffer, 0, new IntPtr(pixels + (y + point.Y) * pitch), buffer.Length);
                     
                    }

                    Marshal.Copy(buf, 0, new IntPtr(pixels), buf.Length);
                    Console.WriteLine((DateTime.Now - d).TotalMilliseconds + "ms");
                }
                else if (bytesPerPixel == 3)
                {
                    //the buffer for a row of pixels.
                    Int32[] buffer = new Int32[colors.GetLength(0)];
                    for (int y = 0; y < colors.GetLength(1); ++y)
                    {
                        //gets only the pixels in the row that are required.
                        for (int x = 0; x < buffer.Length; ++x)
                        {
                            //converts the pixel to a color value.
                            buffer[x] = Sdl.SDL_MapRGB(this.surface.format, colors[x, y].R, colors[x, y].G, colors[x, y].B);
                        }
                        //then copies them to the image.
                        MarshalHelper.CopyInt24(buffer, 0, new IntPtr(pixels + (y + point.Y) * pitch), buffer.Length);
                    }
                }
                else if (bytesPerPixel == 2)
                {
                    //the buffer for a row of pixels.
                    Int16[] buffer = new Int16[colors.GetLength(0)];
                    for (int y = 0; y < colors.GetLength(1); ++y)
                    {
                        //gets only the pixels in the row that are required.
                        for (int x = 0; x < buffer.Length; ++x)
                        {
                            //converts the pixel to a color value.
                            buffer[x] = (short)Sdl.SDL_MapRGB(this.surface.format, colors[x, y].R, colors[x, y].G, colors[x, y].B);
                        }
                        //then copies them to the image.
                        Marshal.Copy(buffer, 0, new IntPtr(pixels + (y + point.Y) * pitch), buffer.Length);
                    }
                }
                else if (bytesPerPixel == 1)
                {
                    //the buffer for a row of pixels.
                    Byte[] buffer = new Byte[colors.GetLength(0)];
                    for (int y = 0; y < colors.GetLength(1); ++y)
                    {
                        //gets only the pixels in the row that are required.
                        for (int x = 0; x < buffer.Length; ++x)
                        {
                            //converts the pixel to a color value.
                            buffer[x] = (byte)Sdl.SDL_MapRGB(this.surface.format, colors[x, y].R, colors[x, y].G, colors[x, y].B);
                        }
                        //then copies them to the image.
                        Marshal.Copy(buffer, 0, new IntPtr(pixels + (y + point.Y) * pitch), buffer.Length);
                    }
                }
            }
            finally
            {
                Sdl.SDL_UnlockSurface(this.surfacePtrs[0]);
            }*/
        }

        public void buildNightImage()
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
                else*/ if (bytesPerPixel >= 3)
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
                        Marshal.Copy(new IntPtr(pixels + y * pitch), buffer,0, buffer.Length);
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
        public void fill(Color c)
        {
            fill(clipRect, c);
		}

		public void fill( Rectangle rect, Color c ) {
			//rect.Intersect(clip);
           //surface.Fill(rect, c);
            srect.x = (short)rect.X;
            srect.y = (short)rect.Y;
            srect.w = (short)rect.Width;
            srect.h = (short)rect.Height;
            Tao.Sdl.Sdl.SDL_FillRect(this.surfacePtr(),ref srect,Sdl.SDL_MapRGB(this.surface.format,c.R,c.G,c.B));
		}

        private int _colKey;
		private int colorKey {
            get { return _colKey; }
            set { _colKey = value; Tao.Sdl.Sdl.SDL_SetColorKey(this.surfacePtr(), Sdl.SDL_SRCCOLORKEY | Sdl.SDL_RLEACCEL, _colKey); }
        }

		/// <summary>
		/// Source color key. A mask color that will not be copied to other plains.
		/// </summary>
		public Color sourceColorKey {
            get { return Color.FromArgb(_colKey); }
			set {
                colorKey = Sdl.SDL_MapRGB(this.surface.format, value.R , value.G , value.B );
			}
		}

		// retruns true if the color at the specified pixel is valid (opaque).
		public bool HitTest( Point p ) { return HitTest(p.X, p.Y); }

		// retruns true if the color at the specified pixel is valid (opaque).
		public bool HitTest( int x, int y )
		{
			if(x<0 || x>size.Width || y<0 || y>size.Height )
				return false;
			return ((getColorAt(x,y)&0xffffff) == colorKey);
		}

		// returns color at specified point.
		// the return value suited for current pixel format.
		// outrange point will raise an error.
		int getColorAt( int x, int y )
		{
            Color c = this.GetPixel(x, y);
            return c.R & c.G & c.B;
			
		}
		
		public Color getColor(int x, int y) {
			return this.GetPixel(x,y);
		}

        public Color getColor(IntPtr surf, int x, int y)
        {
            return this.GetPixel(surf, x, y);
        }

        public System.Drawing.Color GetColor(int colorValue)
        {
            byte r, g, b, a;
            Sdl.SDL_GetRGBA(colorValue, this.surface.format, out r, out g, out b, out a);
            return System.Drawing.Color.FromArgb(a, r, g, b);
        }

        public System.Drawing.Color GetColor(IntPtr surf, int colorValue)
        {
            byte r, g, b, a;
            Sdl.SDL_GetRGBA(colorValue, surf, out r, out g, out b, out a);
            return System.Drawing.Color.FromArgb(a, r, g, b);
        }

		public void drawBox( Rectangle r ) { drawBox(r, Color.CornflowerBlue); }
        public void drawBox( Rectangle r , Color c)
        {
            /*surface.Draw(new SdlDotNet.Graphics.Primitives.Box((short)r.Left,
                                                            (short)r.Top,
                                                            (short)r.Right,
                                                            (short)r.Bottom), c);*/
            //SdlGfx.boxColor(this.surfacePtr(),(short)r.X,(short)r.Y,(short)(r.X+r.Width),(short)(r.Y+r.Height),Sdl.SDL_MapRGB(this.surface.format,c.R,c.G,c.B));
            SdlGfx.rectangleRGBA(this.surfacePtr(),(short)r.X,(short)r.Y,(short)(r.X+r.Width),(short)(r.Y+r.Height),c.R,c.G,c.B,255);
        }

        public void drawPolygon(Point p1, Point p2, Point p3, Point p4) {drawBox(new Rectangle(p1, new Size(p2.X - p1.X, p4.Y - p1.Y)));}
        public void drawPolygon(Color col, Point[] pts)
        {
            //surface.Draw(new SdlDotNet.Graphics.Primitives.Polygon(new ArrayList(pts)), col);
        }

        public void fillPolygon(Color col, Point[] pts)
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
            Tao.Sdl.SdlGfx.filledPolygonRGBA(this.surfacePtr(), ptX, ptY, pts.GetLength(0), col.R,col.G,col.B, 255);
        }

        public void drawLines(Color col, Point[] pts)
        {
            for (int curPoint = 0; curPoint <= (pts.GetLength(0) - 2); curPoint++)
                Tao.Sdl.SdlGfx.lineRGBA(this.surfacePtr(), (short)pts[curPoint].X, (short)pts[curPoint].Y, (short)pts[curPoint + 1].X, (short)pts[curPoint + 1].Y, col.R, col.G, col.B, 255);
            //    surface.Draw(new SdlDotNet.Graphics.Primitives.Line((Point)pts[curPoint], (Point)pts[curPoint + 1]), col,false,false);
        }

		/// <summary>
		/// Tries to recover a lost surface.
		/// </summary>
		public void restore() {
			//handle.restore();
		}

		/// <summary>
		/// Makes the bitmap of this surface.
		/// The caller needs to dispose the bitmap.
		/// </summary>
		public Bitmap createBitmap() {
            //return surface.Bitmap;
            return null;
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
        public static void WriteInt24(IntPtr ptr, Int32 value)
        {
            Int32[] valueArray = new Int32[1] { value };
            Byte[] buffer = new Byte[sizeofInt24];
            Buffer.BlockCopy(valueArray, offset, buffer, 0, sizeofInt24);
            Marshal.Copy(buffer, 0, ptr, sizeofInt24);
        }
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
