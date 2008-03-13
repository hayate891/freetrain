using System;
using System.Drawing;
using SDL.net;

namespace freetrain.world
{
	/// <summary>
	/// Voxels will access GDI DC or DirectDraw surface
	/// via this object.
	/// 
	/// This object minimizes the number of "context switch"
	/// between GDI DC and DirectDraw.
	/// </summary>
	public class DrawContext : IDisposable {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
		public DrawContext( Surface s ) { this._surface=s; }

		private readonly Surface _surface;
		/*private GDIGraphics _graphics=null;*/
		
		/*public Graphics graphics {
			get {
				if( _graphics==null )
					_graphics = new GDIGraphics(_surface);
				return _graphics.graphics;
			}
		}*/
        /// <summary>
        /// 
        /// </summary>
		public Surface surface {
			get {
				/*if( _graphics!=null ) {
					_graphics.Dispose();
					_graphics = null;
				}*/
				return _surface;
			}
		}
		
		/// <summary>
		/// Only the owner of the DrawContext class can
		/// call this method.
		/// </summary>
		public void Dispose() {
			/*if( _graphics!=null )
				_graphics.Dispose();*/
		}
	}
}
