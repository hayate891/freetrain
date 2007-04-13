using System;

namespace nft.util
{
	/// <summary>
	/// Resource ‚ÌŠT—v‚Ìà–¾‚Å‚·B
	/// </summary>
	public class Resource
	{
		protected Properties properties;

		public Resource(Properties source)
		{
			properties = source;
		}

		public Resource(string xmlFilePath)
		{
			properties = Properties.LoadFromFile(xmlFilePath);
		}

	}
}
