using System;
using System.Drawing;
using org.kohsuke.directdraw;

namespace freetrain.views
{
	/// <summary>
	/// NullWeatherOverlay ‚ÌŠT—v‚Ìà–¾‚Å‚·B
	/// </summary>
	public sealed class NullWeatherOverlay : WeatherOverlay
	{
		private NullWeatherOverlay() {}

		public static readonly WeatherOverlay theInstance = new NullWeatherOverlay();

		public void setSize( Size sz ) {}

		public void draw( QuarterViewDrawer drawer, Surface target, Point pt ) {
			drawer.draw( target, pt );
		}

		public bool onTimerFired() {
			return false;
		}

		public void Dispose() {}
	}
}
