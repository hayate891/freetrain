using System;
using System.Drawing;
using org.kohsuke.directdraw;

namespace freetrain.framework.graphics
{
	/// <summary>
	/// A sprite object that can draw itself to other surfaces.
	/// 
	/// Note that this structure doesn't handle resource management
	/// of the surface object it uses.
	/// 
	/// Sprites are serializable
	/// </summary>
	public interface Sprite {
		// draw a sprite to the given point.
		void draw( AgateSurface surface, Point pt );
		void drawShape( AgateSurface surface, Point pt, Color color );
		void drawAlpha( AgateSurface surface, Point pt );

		Size size { get; }
		Point offset { get; }
		bool HitTest( int x, int y);
	}
}
