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
using FreeTrain.Framework.Plugin;
using FreeTrain.Framework.Graphics;
using FreeTrain.World;

namespace FreeTrain.Contributions.Train
{
    /// <summary>
    /// Assymetric train car.
    /// 
    /// This train car is usually used for the head car,
    /// where a picture to go N and that to go S are different.
    /// This type requires 16 graphics on the level ground
    /// and 8 graphics for slopes.
    /// </summary>
    [Serializable]
    public class AsymTrainCarImpl : TrainCarContribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public AsymTrainCarImpl(XmlElement e)
            : base(e)
        {

            levelSprites = new ISprite[16];
            slopeSprites = new ISprite[8];

            XmlElement sprite = (XmlElement)XmlUtil.SelectSingleNode(e, "sprite");
            Picture picture = GetPicture(sprite);
            SpriteFactory factory = SpriteFactory.GetSpriteFactory(sprite);

            Point origin = XmlUtil.ParsePoint(sprite.Attributes["origin"].Value);

            for (int i = 0; i < 16; i++)
            {
                Point sprOrigin = new Point((i % 8) * 32 + origin.X, (i / 8) * 32 + origin.Y);
                levelSprites[i] = factory.CreateSprite(picture, new Point(0, 0), sprOrigin, new Size(32, 32));
            }
            for (int i = 0; i < 8; i++)
            {
                Point sprOrigin = new Point(i * 32 + origin.X, 64 + origin.Y);
                slopeSprites[i] = factory.CreateSprite(picture, new Point(0, 0), sprOrigin, new Size(32, 32));
            }
        }

        /// <summary> Sprites used to draw a car on a level ground. 8-way from dir=0 to 7 </summary>
        private readonly ISprite[] levelSprites;

        /// <summary> Sprites used to draw a car on a slope. 4 way from dir=0,2,4, and 6 </summary>
        private readonly ISprite[] slopeSprites;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="display"></param>
        /// <param name="pt"></param>
        /// <param name="angle"></param>
        public override void Draw(Surface display, Point pt, int angle)
        {
            levelSprites[angle].Draw(display, pt);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="display"></param>
        /// <param name="pt"></param>
        /// <param name="angle"></param>
        /// <param name="isClimbing"></param>
        public override void DrawSlope(Surface display, Point pt, Direction angle, bool isClimbing)
        {
            slopeSprites[angle.index + (isClimbing ? 0 : 1)].Draw(display, pt);
        }

    }
}
