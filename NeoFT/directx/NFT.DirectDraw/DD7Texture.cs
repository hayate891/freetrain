using System;
using System.IO;
using System.Drawing;
using nft.framework.drawing;

namespace nft.drawing.ddraw7
{
	/// <summary>
	/// SimpleTexture の概要の説明です。
	/// </summary>
	public class DD7Texture : ISimpleTexture
	{
		protected DD7Surface source;
		protected IPixelFilter filter;
		protected Rectangle srcRegion;
		protected Point drawOffset;

		public DD7Texture(ISurface surface, Rectangle region, Point offset) {
			if(surface is DD7Surface) {
				this.source = surface as DD7Surface;
				this.srcRegion = new Rectangle(0,0,source.Size.Width, source.Size.Height);
				this.srcRegion.Intersect(region);
			}
			else {
				PixelColorMode mode = surface.PixelColorMode;
				this.source = (DD7Surface)DD7GraphicManager.TheInstance.CreateOffscreenSurface(region.Size,mode,SurfaceAlloc.SystemMem);
				this.source.BitBlt(new Point(0,0), surface, region, 0);
				this.srcRegion = new Rectangle(0,0,source.Size.Width, source.Size.Height);
			}
			this.drawOffset = offset;
		}

		#region ITexture メンバ

		public Rectangle Boundary { get { return new Rectangle(drawOffset,srcRegion.Size); } }

		public bool HitTest(Point pos) {
			if(Boundary.Contains(pos)) {
				int x = pos.X-srcRegion.Left-drawOffset.X;
				int y = pos.Y-srcRegion.Top-drawOffset.Y;					
				return source.IsOpaque(x, y);
			}
			else
				return false;
		}

		#endregion

		#region IDrawable メンバ

		public void Draw(ISurface dest, System.Drawing.Point location, int zoom, int frame) {
			Point pt = new Point(location.X+drawOffset.X,location.Y+drawOffset.Y);
			if(filter!=null)
				dest.BitBlt( pt, source, srcRegion, zoom, filter);
			else
				dest.BitBlt( pt, source, srcRegion, zoom);
		}

		public void DrawEx(ISurface dest, System.Drawing.Point location, int zoom, IPixelFilter f, int frame) {
			Point pt = new Point(location.X+drawOffset.X,location.Y+drawOffset.Y);
			if(filter!=null) {
				CompositFilter cf = new CompositFilter(filter,f);
				dest.BitBlt( pt, source, srcRegion, zoom, cf);
			}
			else
				dest.BitBlt( pt, source, srcRegion, zoom, f);
		}

		public void DrawEx(ISurface dest, System.Drawing.Point location, int zoom, ISurfaceDrawer drawer, int frame) {
			Point pt = new Point(location.X+drawOffset.X,location.Y+drawOffset.Y);
			dest.BitBlt( pt, source, srcRegion, zoom, drawer);
		}

		public IPixelFilter Filter {
			get { return filter; }
			set { filter = value; }
		}

		#endregion

		#region IDisposable メンバ

		public void Dispose() {
			if(source!=null) {
				source.Dispose();
				source = null;
			}
		}

		#endregion

		#region ISimpleTexture メンバ
		// Becare!! If the source surface is refered from other objects,
		// ColorKey setting will affects all those objects.
		public Color ColorKey { 
			get { return source.ColorKey; } 
			set { source.ColorKey = value; }
		}

		public void PickColorKeyFromSource( int x, int y ){
			x += srcRegion.X;
			y += srcRegion.Y;
			if(srcRegion.Contains(x,y)){
				ColorKey = PixelFormatUtil.ToColor(source.GetPixel(x,y),source.PixelColorMode);
			}
			else
				ColorKey = Color.Empty;
		}

		public Point DrawOffset { get { return drawOffset; } }

		public Rectangle SourceRegion { get { return srcRegion; } }

		public ISurface SourceSurface { get { return source; } }
		#endregion
	}
}
