using System;
using System.Collections;
using System.Diagnostics;
using nft.core;
using nft.core.geometry;
using nft.ui.command;

namespace nft.debug
{
	/// <summary>
	/// TestDirection の概要の説明です。
	/// </summary>
	public class TestLocation : ICommandEntity
	{
		public TestLocation(){}
		public void CommandExecuted( CommandUI cmdUI,object sender )
		{
			Show(new Location(1,2,3),new Location(3,2,1));
			Show(new Location(2,3,1),new Location(3,2,1));
			Show(new LocationF(1,1,3),new LocationF(2,4,3));
			Show(new LocationF(4,2,1),new LocationF(1,1,3));
		}		

		private void Show(Location l1, Location l2)
		{
			Debug.WriteLine(string.Format("from {0} to {1}",l1,l2));
			Debug.WriteLine(string.Format("dir={0}, distance={1}", l1.GetDirectionTo(l2), l1.GetDistanceTo(l2)));		
		}

		private void Show(LocationF l1, LocationF l2)
		{
			Debug.WriteLine(string.Format("from {0} to {1}",l1,l2));
			Debug.WriteLine(string.Format("dir={0}, distance={1}", l1.GetDirectionTo(l2), l1.GetDistanceTo(l2)));		
		}

	}
}
