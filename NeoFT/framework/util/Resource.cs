using System;

namespace nft.util
{
	/// <summary>
	/// Resource �̊T�v�̐����ł��B
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
