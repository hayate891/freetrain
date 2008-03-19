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
using System.Xml;
using System.Windows.Forms;
using FreeTrain.Framework;
using FreeTrain.World;

namespace FreeTrain.Contributions.Others
{
    /// <summary>
    /// Creates a new empty game by allowing the user to specify the size of the world.
    /// </summary>
    public class EmptyNewGameContributionImpl : NewGameContribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public EmptyNewGameContributionImpl(XmlElement e) : base(e) { }
        /// <summary>
        /// 
        /// </summary>
        public override string author { get { return "-"; } }
        /// <summary>
        /// 
        /// </summary>
        public override string name { get { return "Empty map"; } }
        //! public override string name { get { return "空マップ"; } }
        /// <summary>
        /// 
        /// </summary>
        public override string description { get { return "Create a completely empty map"; } }
        //! public override string description { get { return "何もない空のマップを作成します"; } }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override WorldDefinition createNewGame()
        {
            using (NewWorldDialog dialog = new NewWorldDialog())
            {
                if (dialog.ShowDialog(MainWindow.mainWindow) == DialogResult.OK)
                    return dialog.createWorld();
                else
                    return null;
            }
        }
    }
}
