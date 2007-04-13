using System;
using System.Collections;

namespace nft.framework.drawing
{
	/// <summary>
	/// CompositFilter ‚Í•¡”‚ÌIPixelFilter‚ğˆê‚Â‚É‚Ü‚Æ‚ß‚é
	/// </summary>
	public class CompositFilter : IPixelFilter
	{
		protected ArrayList list;

		public CompositFilter(){
			list = new ArrayList();
		}

		public CompositFilter(IPixelFilter f1st, IPixelFilter f2nd)
		{
			list = new ArrayList(2);
		}

		public void Add(IPixelFilter f){
			list.Add(f);
		}

		public void Remove(IPixelFilter f){
			list.Remove(f);
		}

		#region IPixelFilter ƒƒ“ƒo

		public void Begin(nft.framework.drawing.PixelColorMode mode, Int32 colorKey) {
			foreach(IPixelFilter f in list)
				f.Begin(mode,colorKey);
		}

		public Int32 Convert(Int32 dest, Int32 source) {			
			foreach(IPixelFilter f in list)
				source = f.Convert(dest, source);
			return source;
		}

		public void End() {
			foreach(IPixelFilter f in list)
				f.End();
		}

		#endregion
	}
}
