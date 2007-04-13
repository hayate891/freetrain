using System;
using System.Diagnostics;
using System.Drawing;
using System.Xml;
using nft.framework.drawing;
//using DxVBLib;

namespace nft.drawing.ddraw7
{
	/// <summary>
	/// Draw an image in the picture as-is.
	/// </summary>
	[Serializable]
	public class DD7Sprite : ISprite {
		protected int zoom;
		protected ITexture texture;
		protected IPixelFilter filter;
		protected Point location;

		internal protected DD7Sprite(ITexture texture){
			this.texture = texture;
			zoom = 0;
			location = new Point(0,0);
		}

		#region ISprite ÉÅÉìÉo

		public int Zoom {
			get { return zoom; }
			set { zoom = value; }
		}

		public bool HitTest(Point loc) {
			Point pt = new Point(loc.X - location.X, loc.Y - location.Y);
			return texture.HitTest(pt);
		}

		public ITexture Texture {
			get { return texture; }
			set { 
				Debug.Assert(value!=null);
				texture = value; 
			}
		}

		public Point Location {
			get { return location; }
			set { location = value;	}
		}

		public IPixelFilter Filter {
			get { return filter; }
			set { filter = value; }
		}

		#endregion

		#region IDrawable ÉÅÉìÉo

		public void DrawEx(ISurface dest, Point pos, int zoom, ISurfaceDrawer drawer, int frame) {
			Point pt = new Point(pos.X+location.X,pos.Y+location.Y);
			texture.DrawEx(dest, pt, zoom, drawer, frame);
		}

		public void Draw(ISurface dest, Point pos, int zoom, int frame) {
			Point pt = new Point(pos.X+location.X,pos.Y+location.Y);
			if(filter!=null)
				texture.DrawEx(dest, pt, zoom, filter ,frame);
			else
				texture.Draw(dest, pt, zoom, frame);
		}

		public void DrawEx(ISurface dest, Point pos, int zoom, IPixelFilter f, int frame) {
			Point pt = new Point(pos.X+location.X,pos.Y+location.Y);
			if(filter!=null){
				CompositFilter cf = new CompositFilter(filter, f);
				texture.DrawEx(dest, pt, zoom, cf ,frame);
			}
			else
				texture.DrawEx(dest, pt, zoom, f ,frame);
		}
		#endregion

		#region IDisposable ÉÅÉìÉo

		public void Dispose() {
			if(texture!=null){
				texture.Dispose();
				texture=null;
			}
		}

		#endregion
	}
}
