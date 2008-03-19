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
using FreeTrain.Framework;
using FreeTrain.Framework.Graphics;
using FreeTrain.Framework.Plugin;
using FreeTrain.World;
using SDL.net;

namespace FreeTrain.Contributions.Train
{
	/// <summary>
	/// Symmetric train car.
	/// 
	/// This train car contribution uses the same graphics
	/// for N and S. Thus 8 graphics for level and 4 for
	/// slopes are necessary.
	/// </summary>
	[Serializable]
	public class SymTrainCarImpl : TrainCarContribution
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
		public SymTrainCarImpl( XmlElement e ) : base(e) {

			levelSprites = new Sprite[8];
			slopeSprites = new Sprite[4];

			XmlElement sprite = (XmlElement)XmlUtil.selectSingleNode(e,"sprite");
			Picture picture = getPicture(sprite);
			SpriteFactory factory = SpriteFactory.getSpriteFactory(sprite);

			Point origin = XmlUtil.parsePoint( sprite.Attributes["origin"].Value );

			for( int i=0; i<8; i++ ) {
				Point sprOrigin = new Point( i*32 +origin.X, origin.Y );
				levelSprites[i] = factory.createSprite( picture, new Point(0,0), sprOrigin, new Size(32,32) );
			}
			for( int i=0; i<4; i++ ) {
				Point sprOrigin = new Point( i*32 +origin.X, 32+origin.Y );
				slopeSprites[i] = factory.createSprite( picture, new Point(0,0), sprOrigin, new Size(32,32) );
			}
		}

		/// <summary> Sprites used to draw a car on a level ground. 8-way from dir=0 to 7 </summary>
		private readonly Sprite[] levelSprites;

		/// <summary> Sprites used to draw a car on a slope. 4 way from dir=0,2,4, and 6 </summary>
		private readonly Sprite[] slopeSprites;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="display"></param>
        /// <param name="pt"></param>
        /// <param name="angle"></param>
		public override void draw( Surface display, Point pt, int angle ) {
			levelSprites[angle&7].draw( display, pt );
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="display"></param>
        /// <param name="pt"></param>
        /// <param name="angle"></param>
        /// <param name="isClimbing"></param>
		public override void drawSlope( Surface display, Point pt, Direction angle, bool isClimbing ) {
			if(!isClimbing)		angle = angle.opposite;
			slopeSprites[ angle.index/2 ].draw( display, pt );
		}

	}
}
