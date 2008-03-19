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
using SDL.net;
using FreeTrain.Framework.plugin;
using FreeTrain.world;

namespace FreeTrain.Framework.Graphics
{
    /// <summary>
    /// Draw an image in the picture with transforming colors by keying a hue.
    /// </summary>
    [Serializable]
    public class HueTransformSprite : SimpleSprite
    {
        /// <summary>
        /// Source colors are transformed into a color series of this.
        /// </summary>
        private readonly Color[] targetColors = new Color[3];
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_pic"></param>
        /// <param name="_offset"></param>
        /// <param name="_origin"></param>
        /// <param name="_size"></param>
        /// <param name="R_target"></param>
        /// <param name="G_target"></param>
        /// <param name="B_target"></param>
        public HueTransformSprite(Picture _pic, Point _offset, Point _origin, Size _size,
            Color R_target, Color G_target, Color B_target)

            : base(_pic, _offset, _origin, _size)
        {

            SetColorMap(ColorMask.R, R_target);
            SetColorMap(ColorMask.G, G_target);
            SetColorMap(ColorMask.B, B_target);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="pt"></param>
        public override void draw(Surface surface, Point pt)
        {
            pt.X -= offset.X;
            pt.Y -= offset.Y;

            int idx = (World.world.viewOptions.useNightView) ? 1 : 0;

            surface.bltHueTransform(pt, picture.surface, origin, size,
                RedTarget, GreenTarget, BlueTarget);
        }

        /// <summary>
        /// 
        /// </summary>
        public Color RedTarget { get { return targetColors[(int)ColorMask.R]; } }
        /// <summary>
        /// 
        /// </summary>
        public Color GreenTarget { get { return targetColors[(int)ColorMask.G]; } }
        /// <summary>
        /// 
        /// 
        /// </summary>
        public Color BlueTarget { get { return targetColors[(int)ColorMask.B]; } }

        private void SetColorMap(ColorMask channel, Color dest) { targetColors[(int)channel] = dest; }
    }


    /// <summary>
    /// SpriteFactory for HueTransformSprite.
    /// </summary>
    public class HueTransformSpriteFactory : SpriteFactory
    {
        //private readonly Color keyColor;
        //private readonly ColorMask mask;
        /// <summary>
        /// 
        /// </summary>
        protected readonly Color[] targetColors = new Color[3];

        /// <summary>
        /// Load the setting from a XML manifest of the format:
        ///		&lt;map from="100,200,*" to="50,30,20" />
        /// </summary>
        /// <param name="e">
        /// The parent of a &lt;map> element.
        /// </param>
        public HueTransformSpriteFactory(XmlElement e)
        {
            XmlNodeList lst = e.SelectNodes("map");
            for (int i = 0; i < 3; i++)
                targetColors[i] = Color.Transparent;
            foreach (XmlElement map in lst)
            {
                string[] from = map.Attributes["from"].Value.Split(',');
                ColorMask mask;
                if (from.Length == 3)
                {
                    if (from[0].Equals("*")) mask = ColorMask.R;
                    else if (from[1].Equals("*")) mask = ColorMask.G;
                    else if (from[2].Equals("*")) mask = ColorMask.B;
                    else throw new FormatException("no mask is specified:" + map.Attributes["from"].Value);
                    SetColorMap(mask, PluginUtil.parseColor(map.Attributes["to"].Value));
                }
                else
                {
                    string v = from[0].ToLower();
                    if (v.Equals("r") || v.Equals("red")) mask = ColorMask.R;
                    else if (v.Equals("g") || v.Equals("green")) mask = ColorMask.G;
                    else if (v.Equals("b") || v.Equals("blue")) mask = ColorMask.B;
                    else throw new FormatException("no mask is specified:" + v);
                    SetColorMap(mask, PluginUtil.parseColor(map.Attributes["to"].Value));
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_key"></param>
        /// <param name="_mask"></param>
        /// <param name="_target"></param>
        public HueTransformSpriteFactory(Color _key, ColorMask _mask, Color _target)
        {
            SetColorMap(_mask, _target);
            for (int i = 0; i < 3; i++)
            {
                if (i != (int)_mask)
                    targetColors[i] = Color.Transparent;
            }
        }

        private static int safeParse(string value)
        {
            if (value.Equals("*")) return 0;
            else return int.Parse(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="picture"></param>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public override Sprite createSprite(Picture picture, Point offset, Point origin, Size size)
        {
            return new HueTransformSprite(picture, offset, origin, size, RedTarget, GreenTarget, BlueTarget);
        }
        /// <summary>
        /// 
        /// </summary>
        public Color RedTarget { get { return targetColors[(int)ColorMask.R]; } }
        /// <summary>
        /// 
        /// </summary>
        public Color GreenTarget { get { return targetColors[(int)ColorMask.G]; } }
        /// <summary>
        /// 
        /// </summary>
        public Color BlueTarget { get { return targetColors[(int)ColorMask.B]; } }

        private void SetColorMap(ColorMask channel, Color dest) { targetColors[(int)channel] = dest; }
    }
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class HueTransformSpriteFactoryContributionImpl : SpriteFactoryContribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public HueTransformSpriteFactoryContributionImpl(XmlElement e) : base(e) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override SpriteFactory createSpriteFactory(XmlElement e)
        {
            return new HueTransformSpriteFactory(e);
        }
    }
}
