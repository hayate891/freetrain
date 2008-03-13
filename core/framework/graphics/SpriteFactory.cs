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
using freetrain.framework.plugin;

namespace freetrain.framework.graphics
{
    /// <summary>
    /// Create a sprite from a picture.
    /// 
    /// SpriteFactory encapsulates the logic to instanciate sprite implementations.
    /// </summary>
    public abstract class SpriteFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="picture"></param>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public abstract Sprite createSprite(Picture picture, Point offset, Point origin, Size size);


        /// <summary>
        /// Locate the SpriteFactory that should be used to load sprites.
        /// </summary>
        /// <param name="sprite">&lt;sprite> element in the manifest.</param>
        /// <returns>non-null valid object.</returns>
        public static SpriteFactory getSpriteFactory(XmlNode sprite)
        {
            XmlElement type = (XmlElement)sprite.SelectSingleNode("spriteType");
            if (type == null)
                type = (XmlElement)sprite.SelectSingleNode("colorVariation");
            if (type == null)
                // if none is specified, use the default sprite factory
                return new SimpleSpriteFactory();
            else
            {
                string name = type.Attributes["name"].Value;
                // otherwise load from the spriteType element.
                SpriteFactoryContribution contrib = (SpriteFactoryContribution)
                    PluginManager.theInstance.getContribution("spriteFactory:" + name);
                if (contrib == null)
                    throw new FormatException("unable to locate spriteFactory:" + name);
                return contrib.createSpriteFactory(type);
            }
        }
    }
}
