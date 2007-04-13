using System;

namespace nft.util
{
	/// <summary>
	/// Resource の概要の説明です。
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
