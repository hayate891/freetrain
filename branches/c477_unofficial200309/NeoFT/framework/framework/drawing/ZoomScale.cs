using System;
using System.Drawing;
using System.Diagnostics;

namespace nft.framework.drawing
{
	public enum ZoomScale {xOctet=-7, xQuarter=-3, xTriplet, xHalf, x1, x2, x3, x4, x8=7 };

	public class ZoomUtil
	{
		public static Size Scale(Size orig, ZoomScale scale){
			switch(scale){
				case ZoomScale.x2:
					return new Size(orig.Width<<1,orig.Height<<1);
				case ZoomScale.x1:
					return orig;
				case ZoomScale.xHalf:
					return new Size(orig.Width>>1,orig.Height>>1);
				case ZoomScale.xQuarter:
					return new Size(orig.Width>>2,orig.Height>>2);
			}
			return orig;
		}

		public static int Scale(int val, ZoomScale scale){
			switch(scale){
				case ZoomScale.x2:
					return val<<1;
				case ZoomScale.x1:
					return val;
				case ZoomScale.xHalf:
					return val>>1;
				case ZoomScale.xQuarter:
					return val>>2;
			}
			return val;
		}

		public static Size Scale(Size orig, int zoomFactor){
			if(zoomFactor>=0) {
				zoomFactor++;
				return new Size(orig.Width*zoomFactor,orig.Height*zoomFactor);
			} else {
				zoomFactor = 1-zoomFactor;
				return new Size(orig.Width/zoomFactor,orig.Height/zoomFactor);
			}
		}

		public static int Scale(int val, int zoomFactor){
			if(zoomFactor>=0) {
				zoomFactor++;
				return val*zoomFactor;
			} else {
				zoomFactor = 1-zoomFactor;
				return val/zoomFactor;
			}
		}

		public static int ToZoomFactor(double zoomScale){
			Debug.Assert(zoomScale<=0);
			if(zoomScale>1)
				return (int)zoomScale;
			else
				return -(int)(1/zoomScale);
		}

		public static int ToZoomFactor(ZoomScale zoomScale){
			switch(zoomScale){
				case ZoomScale.x2:
					return 1;
				case ZoomScale.x1:
					return 0;
				case ZoomScale.xHalf:
					return -1;
				case ZoomScale.xQuarter:
					return -3;
			}
			return 0;
		}
	}

}
