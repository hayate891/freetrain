using System;
using System.Drawing;
using org.kohsuke.directdraw;

namespace freetrain.framework.graphics
{
	/// <summary>
	/// Sprite that directly draws from a surface.
	/// </summary>
	public class DirectSprite : Sprite
	{
		public DirectSprite( AgateSurface _surface, Point _offset, Point _origin, Size _size ) {
			this.surface = _surface;
			this._offset = _offset;
			this.origin = _origin;
			this._size = _size;
		}
		public DirectSprite( AgateSurface _surface, Point _offset )
			: this(_surface,_offset,new Point(0,0),_surface.size) {}


		/// <summary>
		/// Surface that contains the image.
		/// </summary>
		protected AgateSurface surface;

		/// <summary>
		/// The point in the image that will be aligned to
		/// the left-top corner of a voxel.
		/// </summary>
		protected readonly Point _offset;

		/// <summary>
		/// The area of the image to be drawn.
		/// </summary>
		protected readonly Point origin;
		protected readonly Size _size;

		public virtual void draw( AgateSurface surface, Point pt ) {
			pt.X -= _offset.X;
			pt.Y -= _offset.Y;
			surface.blt( pt, this.surface, origin, _size );
		}

		/// <summary>
		/// Draws the shape of this sprite in the specified color.
		/// </summary>
		public virtual void drawShape( AgateSurface surface, Point pt, Color color ) {
			pt.X -= _offset.X;
			pt.Y -= _offset.Y;
			surface.bltShape( pt, this.surface, origin, _size, color );
		}

		public virtual void drawAlpha( AgateSurface surface, Point pt ) {
			pt.X -= _offset.X;
			pt.Y -= _offset.Y;
			surface.bltAlpha( pt, this.surface, origin, _size );
		}

		public Size size { get { return _size; } }
		public Point offset { get { return offset; } }
		public bool HitTest(int x, int y)
		{
			return surface.HitTest(x,y);
		}
	}
}
