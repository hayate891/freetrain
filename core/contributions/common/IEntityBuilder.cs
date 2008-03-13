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
using freetrain.framework.graphics;
using freetrain.controllers;
using freetrain.contributions.population;

namespace freetrain.contributions.common
{
    /// <summary>
    /// IEntityBuilder の概要の説明です。
    /// </summary>
    public interface IEntityBuilder
    {

        /// <summary> 
        /// Population of this structure, or null if this structure is not populated. 
        /// </summary>
        Population population { get; }

        /// <summary>
        /// True if the computer (the development algorithm) is not allowed to
        /// build this structure.
        /// </summary>
        // TODO: make IEntityBuilder responsible for creating a new Plan object.
        bool computerCannotBuild { get; }

        /// <summary>
        /// True if the player is not allowed to build this structure.
        /// </summary>
        bool playerCannotBuild { get; }

        /// <summary>
        /// Name of this entity builder. Primarily used as the display name.
        /// Doesn't need to be unique.
        /// </summary>
        string name { get; }
        /// <summary>
        /// 
        /// </summary>
        int price { get; }

        /// <summary>
        /// price par area (minimum).
        /// </summary>
        double pricePerArea { get; }

        /// <summary>
        /// Creates a preview
        /// </summary>
        /// <param name="pixelSize"></param>
        /// <returns></returns>
        PreviewDrawer createPreview(Size pixelSize);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        ModalController createBuilder(IControllerSite site);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        ModalController createRemover(IControllerSite site);
    }
}
