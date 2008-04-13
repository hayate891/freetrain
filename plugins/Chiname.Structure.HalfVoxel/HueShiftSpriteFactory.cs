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
using System.Collections;
using System.Drawing;
using System.Xml;
using System.Runtime.Serialization;
using FreeTrain.Framework.Graphics;
using FreeTrain.World;
using FreeTrain.Framework.Plugin;

namespace FreeTrain.World.Structs.HalfVoxelStructure
{

    /// <summary>
    /// SpriteFactory for ColorMappedSprite.
    /// </summary>
    public class HueShiftSpriteFactory : SpriteFactory
    {
        private int steps;

        /// <summary>
        /// Load a color map from the XML manifest of the format:
        ///		&lt;map from="100,200, 50" to="50,30,20" />
        ///		&lt;map from="  0, 10,100" to="..." />
        ///		...
        /// </summary>
        /// <param name="e">
        /// The parent of &lt;map> elements.
        /// </param>
        public HueShiftSpriteFactory(XmlElement e)
        {
            try
            {
                steps = int.Parse(e.Attributes["counts"].InnerText);
            }
            catch
            {
                steps = 6;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="counts"></param>
        public HueShiftSpriteFactory(int counts)
        {
            steps = counts;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="picture"></param>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public override ISprite CreateSprite(Picture picture, Point offset, Point origin, Size size)
        {
            return new SimpleSprite(picture, offset, origin, size);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bit"></param>
        /// <param name="picture"></param>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ISprite[] CreateSprites(Bitmap bit, Picture picture, Point offset, Point origin, Size size)
        {
            int sprites = steps;
            int shift = 360 / sprites; // hue shift per step
            ISprite[] dest = new ISprite[sprites];
            ArrayList work = new ArrayList();
            for (int y = 0; y < size.Height; y++)
            {
                for (int x = 0; x < size.Width; x++)
                {
                    Color c = bit.GetPixel(x + origin.X, y + origin.Y);
                    if (!work.Contains(c))
                        work.Add(c);
                }
            }
            work.Remove(bit.GetPixel(0, 0));


            dest[0] = new SimpleSprite(picture, offset, origin, size);
            if (work.Count == 0)
            {
                // no replace color
                for (int i = 1; i < sprites; i++)
                    dest[i] = dest[0];
            }
            else
            {
                Color[] srcColors = new Color[work.Count];
                Color[] dstColors = new Color[work.Count];
                for (int j = 0; j < work.Count; j++)
                    srcColors[j] = (Color)work[j];
                for (int i = 1; i < sprites; i++)
                {
                    int s2 = shift * i;
                    for (int j = 0; j < srcColors.GetLength(0); j++)
                    {
                        HSVColor c = new HSVColor(srcColors[j]);
                        c.Hue += s2;
                        dstColors[j] = c.ToRGBColor();
                    }
                    dest[i] = new ColorMappedSprite(picture, offset, origin, size, srcColors, dstColors);
                }
            }
            return dest;
        }
    }
}
