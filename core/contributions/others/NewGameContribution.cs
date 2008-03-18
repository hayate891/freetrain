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
using FreeTrain.Framework.plugin;
using FreeTrain.world;

namespace FreeTrain.Contributions.Others
{
    /// <summary>
    /// Plug-in that creates a new game.
    /// </summary>
    public abstract class NewGameContribution : Contribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected NewGameContribution(XmlElement e) : base("newGame", e.Attributes["id"].Value) { }

        /// <summary>
        /// Name of the new game.
        /// </summary>
        public abstract string name { get; }

        /// <summary>
        /// Author of the new game.
        /// </summary>
        public abstract string author { get; }

        /// <summary>
        /// Human-readable description of the new game.
        /// </summary>
        public abstract string description { get; }

        /// <summary>
        /// Creates a new game by creating a new instance of the World object.
        /// </summary>
        /// <returns>null to indicate that the operation was cancelled.</returns>
        public abstract World createNewGame();
    }
}
