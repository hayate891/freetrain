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
	public class TestDirection : ICommandEntity
	{
		public TestDirection(){}
		public void CommandExecuted( CommandUI cmdUI,object sender )
		{
			Array da = Enum.GetValues(Direction16.NORTH.GetType());
			Debug.WriteLine("--test1--");
			foreach(Direction16 d in da)
			{
				Direction dir = Direction.Get(d);
				Debug.Write(string.Format("{0}:c={1},ic={2},mj={3},",dir.Name,dir.IsCardinal,dir.IsInterCardinal,dir.IsMajor));
				Debug.WriteLine(string.Format("opposite={0},left={1},left_q={2},right={3},right_q={4}",dir.Opposite.Name,dir.Left.Name,dir.LeftQuater.Name,dir.Right.Name,dir.RightQuater.Name));
			}
			Debug.WriteLine("--test2--");
			for(int i=0; i<da.Length; i++)
			{
				for(int j=0; j<da.Length; j++)
				{
					Direction d1 = (Direction16)da.GetValue(i);
					Direction d2 = (Direction16)da.GetValue(j);
					Debug.Write(string.Format("{0}<->{1}:",d1.Name,d2.Name));
					Debug.WriteIf(d1==d2,"==,");
					Debug.WriteIf(d1.Equals(d2),"Equals,");
					Debug.WriteIf(d1.IsParallel(d2),"Parallel,");
					Debug.WriteIf(d1.IsOpposite(d2),"Opposite,");
					Debug.WriteIf(d1.IsRightAngle(d2),"RightAngle,");
					Debug.WriteLine(string.Format("Angle={0}", Direction.AngleStepCount(d1,d2)));
				}
			}
		
			// cast test
			Direction direction;
			direction = Direction4.EAST;
			direction = Direction8.NORTHEAST;
			direction = Direction16.NORTHNORTHWEST;
			//Direction4 d4 = direction; // compile error
			//Direction8 d8 = direction; // compile error
			Direction16 d16 = direction;
		
		}		
	}
}
