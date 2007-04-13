using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace nft.util
{
	/// <summary>
	/// Global Configuration.
	/// 
	/// This is an application-wide configuration, which will be used across
	/// all the games.
	/// 
	/// Use freetrain.framework.Core.options to access the instance.
	/// </summary>
	[XmlTypeAttribute(Namespace="http://www.rocket.ne.jp/~nao/NeoFT/ApplicationConfig")]
	[XmlRootAttribute(Namespace="http://www.rocket.ne.jp/~nao/NeoFT/ApplicationConfig", IsNullable=false)]
	public class GlobalOptions : PersistentOptions
	{
		public GlobalOptions() {}

		/// <summary>
		/// If true, show a message box for errors.
		/// If false, show a message into the status bar.
		/// </summary>
		public bool ShowErrorMessageBox = false;

		/// <summary>
		/// Length of the time (in seconds) 
		/// while a message is displayed.
		/// </summary>
		public int MessageDisplayTime = 3;

		public bool EnableSoundEffect = true;
		
		public new GlobalOptions Load() 
		{
			return (GlobalOptions)base.Load();
		}

		// Maintain backward-compatibility
		protected override string Stem { get { return ""; } }
	}
}
