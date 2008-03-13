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
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using freetrain.contributions.rail;
using freetrain.framework.plugin;
using freetrain.views.map;
using freetrain.world;
using freetrain.world.rail;

namespace freetrain.controllers.rail
{
    /// <summary>
    /// Controller to place/remove BridgeRails
    /// </summary>
    public class SpecialPurposeRailController : AbstractLineController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public SpecialPurposeRailController(SpecialRailContribution type) : base(type) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="canvas"></param>
        /// <param name="pt"></param>
        protected override void draw(Direction d, DrawContextEx canvas, Point pt)
        {
            RailPattern.get(d, d.opposite).drawAlpha(canvas.surface, pt);
        }
    }
}
