using System;
using System.Drawing;
using System.Windows.Forms;

namespace nft.framework.drawing
{
	public enum SurfaceAlloc { Auto, VideoMem, SystemMem };

	/// <summary>
	/// IGraphicManager ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public interface IGraphicManager : IGlobalModule{

		/// <summary>
		/// Creates a primitive ITexture instance which reffers specified image file.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="region"></param>
		/// <param name="offset"></param>
		/// <returns></returns>
		ISimpleTexture CreateSimpleTexture(string filepath, Rectangle region, Point offset);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="source"></param>
		/// <param name="region"></param>
		/// <param name="offset"></param>
		/// <returns></returns>
		ISimpleTexture CreateSimpleTexture(ISurface source, Rectangle region, Point offset);

		ISurface CreateOffscreenSurface(Size size, PixelColorMode mode, SurfaceAlloc alloc);

		/// <summary>
		/// Creates an off-screen surface from an image.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="alloc"></param>
		/// <returns></returns>
		ISurface CreateSurfaceFromBitmap(Bitmap source, PixelColorMode mode, SurfaceAlloc alloc);
		ISurface CreateSurfaceFromBitmap(Bitmap source, Rectangle region, PixelColorMode mode, SurfaceAlloc alloc);

		/// <summary>
		/// Creates an instance of sprite.
		/// </summary>
		/// <param name="texture"></param>
		/// <returns></returns>
		ISprite CreateSprite( ITexture texture );

		/// <summary>
		/// Creates a DrawableControl which contains primary surface
		/// </summary>
		/// <returns></returns>
		DrawableControl CreateDrawableControl();

		#region ISurfaceDrawer factories
		/// <summary>
		/// create intersect drawer
		/// draws only when both source and destination pixel is not transpalent
		/// this drawer must be necessary for drawing perspective scenes
		/// </summary>
		/// <returns></returns>
		ISurfaceDrawer GetIntersectDrawer();
		/// <summary>
		/// create 50% alpha blend drawer
		/// </summary>
		/// <returns></returns>
		ISurfaceDrawer GetHalfAlphaDrawer();
		/// <summary>
		/// create color burn drawer
		/// this drawer is used for red sunset and morning haze
		/// </summary>
		/// <param name="c"></param>
		/// <param name="apply"></param>
		/// <returns></returns>
		ISurfaceDrawer GetColorBurnDrawer(Color c, float apply);
		/// <summary>
		/// create mono color drawer
		/// draws gray-scaled image with specified color
		/// this drawer is used for setting highlihgt some sprites.
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		ISurfaceDrawer GetMonoColorDrawer(Color c);
		/// <summary>
		/// create brighter drawer
		/// draws only when source pixel is brighter than destination
		/// this drawer is used for red sunset and morning haze
		/// </summary>
		/// <returns></returns>
		ISurfaceDrawer GetBrighterDrawer();
		#endregion

		/// <summary>
		/// Returns current display color mode.
		/// </summary>
		PixelColorMode CurrentColorMode { get; }

		/// <summary>
		/// Retruns total amount of video memory.
		/// </summary>
		int TotalVideoMemory { get; }

		/// <summary>
		/// Retruns current available amount of video memory.
		/// </summary>
		int AvailableVideoMemory { get; }
	}
}
