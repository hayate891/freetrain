#region LICENSE
/*
 * Copyright (C) 2007 - 2008 FreeTrain Team (http://freetrain.sourceforge.net)
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
#endregion LICENSE

using System;
using System.Drawing;
using System.Xml;
using FreeTrain.Framework.Plugin;
using FreeTrain.Framework.Graphics;
using FreeTrain.World;
using FreeTrain.World.Rail;

namespace FreeTrain.Contributions.Train
{
    /// <summary>
    /// Train type
    /// </summary>
    [Serializable]
    public abstract class TrainContribution : Contribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        protected TrainContribution(string id) : base("train", id) { }

        /// <summary>Display name of this train type, such as "series 01500 Blue Line"</summary>
        public abstract string Name { get; }

        /// <summary>nick name of train, such as "Blue Line"</summary>
        public abstract string NickName { get; }

        /// <summary>Author who created this contribution.</summary>
        public abstract string Author { get; }

        /// <summary>Company name that operates this train, such as "MBTA".</summary>
        public abstract string CompanyName { get; }

        /// <summary>Type name of train, such as "series 01500"</summary>
        public abstract string TypeName { get; }

        /// <summary>
        /// 
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// 
        /// </summary>
        public abstract int MaxLength { get; }

        /// <summary>
        /// 
        /// 
        /// </summary>
        public abstract int MinLength { get; }

        /// <summary>Price of the train .</summary>
        public abstract int Price(int length);

        /// <summary>Inverse of speed. # of minutes to go for one pixel.</summary>
        public abstract int MinutesPerVoxel { get; }

        /// <summary> Fare of this train. </summary>
        public abstract int Fare { get; }

        /// <summary>
        /// Creates a new train by designating TrainCarContributions for each car.
        /// </summary>
        public abstract TrainCarContribution[] Create(int length);

        /// <summary>
        /// 
        /// </summary>
        public string SpeedDisplayName
        {
            get
            {
                switch (MinutesPerVoxel)
                {
                    case 1: return "Highest";
                    case 2: return "High";
                    case 3: return "Medium";
                    case 4: return "Low";
                    default: return "Lowest";
                    //! case 1:	return "超高速";
                    //! case 2:	return "高速";
                    //! case 3: return "中速";
                    //! case 4: return "低速";
                    //! default:	return "超低速";
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() { return Name; }

        /// <summary>
        /// Builds a nice preview of a train.
        /// </summary>
        public PreviewDrawer CreatePreview(Size pixelSize, int trainlength)
        {
            PreviewDrawer pd = new PreviewDrawer(pixelSize, new Size(1, 3), 0);

            // build up rail like
            //
            //     /~~~~~
            //    /
            for (int x = -10; x < 0; x++)
            {
                pd.Draw(RailPattern.get(Direction.WEST, Direction.EAST), x, 0);
            }
            pd.Draw(RailPattern.get(Direction.WEST, Direction.SOUTHEAST), 0, 0);
            for (int x = 1; x <= 10; x++)
            {
                pd.Draw(RailPattern.get(Direction.NORTHWEST, Direction.SOUTHEAST), x, x);
            }

            TrainCarContribution[] cars = Create(trainlength);
            /*if( cars==null ) {
                for( int i=6; cars==null && i<15; i++ )
                    cars = create(i);
                for( int i=4; cars==null && i>0; i-- )
                    cars = create(i);
                if( cars==null )
                    return pd;	// no preview
            }*/

            if (cars == null) return pd;	// no preview

            int[] pos = new int[] { -2, 0, -1, 0, 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5 };
            int[] angle = new int[] { 12, 12, 13, 14, 14, 14, 14, 14 };
            int[] offset = new int[] { 0, 0, 0, 0, 4, +2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            using (DrawContext dc = new DrawContext(pd.Surface))
            {
                for (int i = 7; i >= 0; i--)
                {
                    if (cars.Length <= i)
                    {
                        continue;		// no car
                    }

                    Point pt = pd.GetPoint(pos[i * 2], pos[i * 2 + 1]);
                    cars[i].Draw(pd.Surface,
                        new Point(pt.X + offset[i * 2], pt.Y + offset[i * 2 + 1] - 9), angle[i]);
                }
            }

            return pd;
        }
    }
}
