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

/******************************************************************************
Dictionary
-------------------------------------------------------------------------------
CommandManager          -   Global service that manages a collection of 
                            commands

Command                 -   A conceptual representation for an application
                            operation (ie.  Save, Edit, Load, etc...)
                            
Command Instance        -   A UI element associated with a command (ie, Menu
                            item, Toolbar Item, etc...).  A command can have
                            multiple instances
                            
Command Type            -   A UI class that can house a command Instance (ie
                            "System.Windows.Forms.MenuItem",
                            "System.Windows.Forms.ToolbarItem" )

CommandExecutor         -   An object that can handle all communication between
                            the command manager and a particular command instance
                            for a particular command type.

UpdateHandler           -   Event handler for the Commands Update event.
******************************************************************************/
using System;
using System.Collections;
using System.Timers;
using System.Windows.Forms;

namespace FreeTrain.Util.Command
{
    /// <summary>
    /// 
    /// </summary>
    public class CommandManager : System.ComponentModel.Component
    {
        // Commands Property: Fetches the Command collection
        internal readonly Set commands = new Set();
        private readonly Hashtable hashCommandExecutors = new Hashtable();

        // Constructor
        /// <summary>
        /// 
        /// </summary>
        public CommandManager()
        {
            // Setup idle processing
            Application.Idle += new EventHandler(this.OnIdle);

            // By default, menus and toolbars are known
            RegisterCommandExecutor("System.Windows.Forms.MenuItem", new MenuCommandExecutor());
            RegisterCommandExecutor("System.Windows.Forms.ToolBarButton", new ToolbarCommandExecutor());
            RegisterCommandExecutor("System.Windows.Forms.Label", new LabelCommandExecutor());
            RegisterCommandExecutor("System.Windows.Forms.Button", new ButtonCommandExecutor());
            RegisterCommandExecutor("System.Windows.Forms.StatusBarPanel", new StatusBarPanelCommandExecutor());
        }

        // Command Executor association methods
        internal void RegisterCommandExecutor(string strType, CommandExecutor executor)
        {
            hashCommandExecutors.Add(strType, executor);
        }

        internal CommandExecutor GetCommandExecutor(object instance)
        {
            return (CommandExecutor)hashCommandExecutors[instance.GetType().ToString()];
        }
        /// <summary>
        /// 
        /// </summary>
        public void updateAll()
        {
            foreach (Command cmd in commands)
                cmd.Update();
        }

        //  Handler for the Idle application event.
        private void OnIdle(object sender, EventArgs args)
        {
            updateAll();
        }
    }

}  // end namespace
