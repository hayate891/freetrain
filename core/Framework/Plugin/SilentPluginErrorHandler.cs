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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using FreeTrain.Util;
using FreeTrain.Contributions.Sound;
using FreeTrain.Contributions.Others;
using FreeTrain.Contributions.Rail;
using FreeTrain.Contributions.Land;
using FreeTrain.Contributions.Train;
using FreeTrain.Contributions.Common;
using FreeTrain.Contributions.Structs;
using FreeTrain.Contributions.Road;

namespace FreeTrain.Framework.Plugin
{
    /// <summary>
    /// 
    /// </summary>
    public class SilentPluginErrorHandler : IPluginErrorHandler
    {
        #region PluginErrorHandler o
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_1st"></param>
        /// <param name="p_2nd"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool OnNameDuplicated(PluginDefinition p_1st, PluginDefinition p_2nd, Exception e)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool OnPluginLoadError(PluginDefinition p, Exception e)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool OnContributionInitError(Contribution c, Exception e)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c_1st"></param>
        /// <param name="c_2nd"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool OnContribIDDuplicated(Contribution c_1st, Contribution c_2nd, Exception e)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorPlugins"></param>
        /// <param name="totalErrorCount"></param>
        /// <returns></returns>
        public bool OnFinal(IDictionary errorPlugins, int totalErrorCount)
        {
            return true;
        }
        #endregion
    }
}
