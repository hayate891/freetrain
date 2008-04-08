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
using System.Windows.Forms;
using FreeTrain.Controllers;
using FreeTrain.Framework;
using FreeTrain.Framework.Graphics;
using FreeTrain.Views;
using FreeTrain.World;
using FreeTrain.World.Structs;

namespace FreeTrain.Contributions.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class FixedSizeStructureRemovalController : CubeSelectorController
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly FixedSizeStructureContribution contrib;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contrib"></param>
        /// <param name="site"></param>
        public FixedSizeStructureRemovalController(FixedSizeStructureContribution contrib, IControllerSite site)
            : base(contrib.Size, site)
        {
            this.contrib = contrib;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cube"></param>
        protected override void OnSelected(Cube cube)
        {
            PThreeDimStructure s = WorldDefinition.World.GetEntityAt(cube.corner) as PThreeDimStructure;
            if (s == null || s.type != contrib)
            {
                MessageBox.Show("Wrong type");
                //! MessageBox.Show("種類が違います");
                return;
            }
            s.remove();
        }
    }
}
