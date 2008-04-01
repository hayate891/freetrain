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
using System.Timers;
using System.Windows.Forms;


namespace FreeTrain.Util.Command
{
    // Command Executor base class
    /// <summary>
    /// 
    /// </summary>
    public abstract class CommandExecutor
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly Hashtable hashInstances = new Hashtable();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="cmd"></param>
        public virtual void InstanceAdded(object item, Command cmd)
        {
            hashInstances.Add(item, cmd);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected Command GetCommandForInstance(object item)
        {
            return (Command)hashInstances[item];
        }

        // Interface for derived classed to implement
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="bEnable"></param>
        public abstract void Enable(object item, bool bEnable);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="bCheck"></param>
        public abstract void Check(object item, bool bCheck);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="text"></param>
        public abstract void SetText(object item, string text);
    }

    // Menu command executor
    /// <summary>
    /// 
    /// </summary>
    public class MenuCommandExecutor : CommandExecutor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="cmd"></param>
        public override void InstanceAdded(object item, Command cmd)
        {
            MenuItem mi = (MenuItem)item;
            mi.Click += new System.EventHandler(menuItem_Click);

            base.InstanceAdded(item, cmd);
        }

        // State setters
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="bEnable"></param>
        public override void Enable(object item, bool bEnable)
        {
            MenuItem mi = (MenuItem)item;
            mi.Enabled = bEnable;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="bCheck"></param>
        public override void Check(object item, bool bCheck)
        {
            MenuItem mi = (MenuItem)item;
            mi.Checked = bCheck;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="text"></param>
        public override void SetText(object item, string text)
        {
            MenuItem mi = (MenuItem)item;
            mi.Text = text;
        }


        // Execution event handler
        private void menuItem_Click(object sender, System.EventArgs e)
        {
            Command cmd = GetCommandForInstance(sender);
            cmd.Execute();
        }
    }

    // Toolbar command executor
    /// <summary>
    /// 
    /// </summary>
    public class ToolbarCommandExecutor : CommandExecutor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="cmd"></param>
        public override void InstanceAdded(object item, Command cmd)
        {
            ToolBarButton button = (ToolBarButton)item;
            ToolBarButtonClickEventHandler handler =
                new ToolBarButtonClickEventHandler(toolbar_ButtonClick);

            // Attempt to remove the handler first, in case we have already 
            // signed up for the event in this toolbar
            button.Parent.ButtonClick -= handler;
            button.Parent.ButtonClick += handler;

            base.InstanceAdded(item, cmd);
        }


        // State setters
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="bEnable"></param>
        public override void Enable(object item, bool bEnable)
        {
            ToolBarButton button = (ToolBarButton)item;
            button.Enabled = bEnable;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="bCheck"></param>
        public override void Check(object item, bool bCheck)
        {
            ToolBarButton button = (ToolBarButton)item;
            button.Style = ToolBarButtonStyle.ToggleButton;
            button.Pushed = bCheck;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="text"></param>
        public override void SetText(object item, string text)
        {
            ToolBarButton button = (ToolBarButton)item;
            button.Text = text;
        }

        // Execution event handler
        private void toolbar_ButtonClick(object sender,
                                            ToolBarButtonClickEventArgs args)
        {
            Command cmd = GetCommandForInstance(args.Button);
            if (cmd != null)
                cmd.Execute();
        }
    }

    // Label command executor
    /// <summary>
    /// 
    /// </summary>
    public class LabelCommandExecutor : CommandExecutor
    {
        // State setters
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="bEnable"></param>
        public override void Enable(object item, bool bEnable) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="bCheck"></param>
        public override void Check(object item, bool bCheck) { }	// no support for them
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="text"></param>
        public override void SetText(object item, string text)
        {
            ((Label)item).Text = text;
        }
    }

    // Status bar panel command executor
    /// <summary>
    /// 
    /// </summary>
    public class StatusBarPanelCommandExecutor : CommandExecutor
    {
        // State setters
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="bEnable"></param>
        public override void Enable(object item, bool bEnable) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="bCheck"></param>
        public override void Check(object item, bool bCheck) { }	// no support for them
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="text"></param>
        public override void SetText(object item, string text)
        {
            ((StatusBarPanel)item).Text = text;
        }
    }

    // Button command executor
    /// <summary>
    /// 
    /// </summary>
    public class ButtonCommandExecutor : CommandExecutor
    {
        // State setters
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="bEnable"></param>
        public override void Enable(object item, bool bEnable)
        {
            ((Button)item).Enabled = bEnable;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="bCheck"></param>
        public override void Check(object item, bool bCheck) { }	// no support for them
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="text"></param>
        public override void SetText(object item, string text)
        {
            ((Button)item).Text = text;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="cmd"></param>
        public override void InstanceAdded(object item, Command cmd)
        {
            ((Button)item).Click += new EventHandler(onClick);
            base.InstanceAdded(item, cmd);
        }

        private void onClick(object sender, EventArgs args)
        {
            Command cmd = GetCommandForInstance(sender);
            if (cmd != null)
                cmd.Execute();
        }
    }
}