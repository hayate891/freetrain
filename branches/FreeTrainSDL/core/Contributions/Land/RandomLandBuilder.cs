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
using FreeTrain.Framework.Graphics;
using FreeTrain.Controllers;
using FreeTrain.Framework.Plugin;
using FreeTrain.Views;
using FreeTrain.World;
using FreeTrain.World.Land;

namespace FreeTrain.Contributions.Land
{
    /// <summary>
    /// RandomLandBuilder の概要の説明です。
    /// </summary>
    [Serializable]
    public class RandomLandBuilder : LandBuilderContribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public RandomLandBuilder(XmlElement e)
            : base(e)
        {
            ArrayList array = new ArrayList();
            string[] guids = XmlUtil.SelectSingleNode(e, "lands").InnerText.Split(' ', '\t', '\r', '\n');
            for (int i = 0; i < guids.Length; i++)
            {
                if (guids[i].Length != 0)
                    array.Add(guids[i]);
            }
            lands = (string[])array.ToArray(typeof(string));
        }

        private static readonly Random random = new Random();

        private StaticLandBuilder getLand()
        {
            return (StaticLandBuilder)
                PluginManager.theInstance.getContribution(lands[random.Next(lands.Length)]);
        }

        /// <summary>
        /// Lands in this array will be placed randomly.
        /// </summary>
        private readonly string[] lands;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="z"></param>
        /// <param name="owned"></param>
        public override void Create(int x1, int y1, int x2, int y2, int z, bool owned)
        {
            for (int x = x1; x <= x2; x++)
            {
                for (int y = y1; y <= y2; y++)
                {
                    Location loc = new Location(x, y, z);
                    if (LandVoxel.canBeBuilt(loc))
                        new StaticLandVoxel(loc, getLand()).isOwned = owned;
                }
            }
        }

        /// <summary>
        /// Creates the preview image of the land builder.
        /// </summary>
        public override PreviewDrawer CreatePreview(Size pixelSize)
        {
            PreviewDrawer drawer = new PreviewDrawer(pixelSize, new Size(10, 10), 0);

            for (int y = 0; y < 10; y++)
                for (int x = 0; x < 10; x++)
                    drawer.draw(getLand().sprite, x, y);

            return drawer;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public override IModalController CreateBuilder(IControllerSite site)
        {
            return new DefaultControllerImpl(this, site, new DefaultControllerImpl.SpriteBuilder(getLandSprite));
        }

        private ISprite getLandSprite()
        {
            return getLand().sprite;
        }
    }
}
