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
using FreeTrain.Framework.plugin;
using FreeTrain.Framework.graphics;
using FreeTrain.world;
using FreeTrain.world.Rail;

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
        /// <param name="_id"></param>
        protected TrainContribution(string _id) : base("train", _id) { }

        /// <summary>Display name of this train type, such as "series 01500 Blue Line"</summary>
        public abstract string name { get; }

        /// <summary>nick name of train, such as "Blue Line"</summary>
        public abstract string nickName { get; }

        /// <summary>Author who created this contribution.</summary>
        public abstract string author { get; }

        /// <summary>Company name that operates this train, such as "MBTA".</summary>
        public abstract string companyName { get; }

        /// <summary>Type name of train, such as "series 01500"</summary>
        public abstract string typeName { get; }

        /// <summary>
        /// 
        /// </summary>
        public abstract string description { get; }

        /// <summary>
        /// 
        /// </summary>
        public abstract int maxLength { get; }
        /// <summary>
        /// 
        /// 
        /// </summary>
        public abstract int minLength { get; }

        /// <summary>Price of the train .</summary>
        public abstract int price(int length);

        /// <summary>Inverse of speed. # of minutes to go for one pixel.</summary>
        public abstract int minutesPerVoxel { get; }

        /// <summary> Fare of this train. </summary>
        public abstract int fare { get; }

        /// <summary>
        /// Creates a new train by designating TrainCarContributions for each car.
        /// </summary>
        public abstract TrainCarContribution[] create(int length);

        /// <summary>
        /// 
        /// </summary>
        public string speedDisplayName
        {
            get
            {
                switch (minutesPerVoxel)
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
        public override string ToString() { return name; }

        /// <summary>
        /// Builds a nice preview of a train.
        /// </summary>
        public PreviewDrawer createPreview(Size pixelSize, int trainlength)
        {
            PreviewDrawer pd = new PreviewDrawer(pixelSize, new Size(1, 3), 0);

            // build up rail like
            //
            //     /~~~~~
            //    /
            for (int x = -10; x < 0; x++)
                pd.draw(RailPattern.get(Direction.WEST, Direction.EAST), x, 0);
            pd.draw(RailPattern.get(Direction.WEST, Direction.SOUTHEAST), 0, 0);
            for (int x = 1; x <= 10; x++)
                pd.draw(RailPattern.get(Direction.NORTHWEST, Direction.SOUTHEAST), x, x);

            TrainCarContribution[] cars = create(trainlength);
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

            using (DrawContext dc = new DrawContext(pd.surface))
            {
                for (int i = 7; i >= 0; i--)
                {
                    if (cars.Length <= i)
                        continue;		// no car

                    Point pt = pd.getPoint(pos[i * 2], pos[i * 2 + 1]);
                    cars[i].draw(pd.surface,
                        new Point(pt.X + offset[i * 2], pt.Y + offset[i * 2 + 1] - 9), angle[i]);
                }
            }

            return pd;
        }
    }
}
