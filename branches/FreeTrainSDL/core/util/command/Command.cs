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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cmd"></param>
    public delegate void CommandHandler(Command cmd);
    /// <summary>
    /// 
    /// </summary>
    public delegate void CommandHandlerNoArg();

    /// <summary>
    /// 
    /// </summary>
    public class Command
    {
        // Members
        /// <summary>
        /// 
        /// </summary>
        public readonly CommandInstanceList commandInstances;
        /// <summary>
        /// 
        /// </summary>
        public readonly CommandManager manager;

        // state of this command
        private bool enabled = true;
        private bool check = false;

        /// <summary> Application specified tag value. </summary>
        public object tag;

        // Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="manager"></param>
        public Command(CommandManager manager)
        {
            this.manager = manager;
            this.commandInstances = new CommandInstanceList(this);

            manager.commands.add(this);
        }

        // Methods to trigger events
        /// <summary>
        /// 
        /// </summary>
        public virtual void Execute()
        {
            if (OnExecute == null) return;	// nothing to do

            Update();

            if (Enabled)
                OnExecute(this);			// make sure that the command is enabled before execution
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual void Update()
        {
            if (OnUpdate != null)
                OnUpdate(this);
        }

        /// <summary> Enables or disables this command. </summary>
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
                foreach (object instance in commandInstances)
                {
                    manager.GetCommandExecutor(instance).Enable(
                        instance, enabled);
                }
            }
        }

        /// <summary> Adds or removes a check from this command. </summary>
        public bool Checked
        {
            get
            {
                return check;
            }
            set
            {
                check = value;
                foreach (object instance in commandInstances)
                {
                    manager.GetCommandExecutor(instance).Check(
                        instance, check);
                }
            }
        }

        /// <summary> Sets the text of this command. </summary>
        public string Text
        {
            set
            {
                foreach (object instance in commandInstances)
                {
                    manager.GetCommandExecutor(instance).SetText(
                        instance, value);
                }
            }
        }

        // Events
        /// <summary>
        /// 
        /// </summary>
        public event CommandHandler OnUpdate;
        /// <summary>
        /// 
        /// </summary>
        public event CommandHandler OnExecute;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        public Command addExecuteHandler(CommandHandler h)
        {
            OnExecute += h;
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        public Command addExecuteHandler(CommandHandlerNoArg h)
        {
            return addExecuteHandler(new CommandHandler(new NoArgAdaptor(h).handle));
        }
        /// <summary>
        /// Registers an execute handler that invokes a new dialog.
        /// </summary>
        public Command addDialogExecuteHandler(Type dialogClass, IWin32Window owner)
        {
            return addExecuteHandler(new CommandHandler(new DialogExecutor(dialogClass, owner).handle));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        public Command addUpdateHandler(CommandHandler h)
        {
            OnUpdate += h;
            return this;
        }

        // adaptor classes
        private class NoArgAdaptor
        {
            internal NoArgAdaptor(CommandHandlerNoArg h)
            {
                this.handler = h;
            }
            private readonly CommandHandlerNoArg handler;
            public void handle(Command cmd) { handler(); }
        }
        private class DialogExecutor
        {
            internal DialogExecutor(Type dialogClass, IWin32Window owner)
            {
                this.dialogClass = dialogClass;
                this.owner = owner;
            }
            private readonly Type dialogClass;
            private readonly IWin32Window owner;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cmd"></param>
            public void handle(Command cmd)
            {
                Form dlg = (Form)Activator.CreateInstance(dialogClass);
                dlg.ShowDialog(owner);
            }
        }





        //
        // Nested collection class
        //
        /// <summary>
        /// 
        /// </summary>
        public class CommandInstanceList : System.Collections.CollectionBase
        {
            internal CommandInstanceList(Command acmd)
                : base()
            {
                command = acmd;
            }

            private Command command;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="instance"></param>
            public void Add(object instance)
            {
                this.List.Add(instance);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="items"></param>
            public void AddAll(params object[] items)
            {
                foreach (object item in items)
                    this.Add(item);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="instance"></param>
            public void Remove(object instance)
            {
                this.List.Remove(instance);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public object this[int index]
            {
                get
                {
                    return this.List[index];
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsertComplete(System.Int32 index,
                                                        System.Object value)
            {
                command.manager.GetCommandExecutor(value).InstanceAdded(
                    value, command);
            }
        }
    }
}
