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
using System.IO;
//using Microsoft.Win32;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace freetrain.util
{
    /// <summary>
    /// Persist window state/location across application sessions.
    /// </summary>
    public class WindowStateTracker
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly Form owner;

        /// <summary> This object receives the window size/position information. </summary>
        public readonly PersistentWindowState state;
        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="_owner"></param>
        /// <param name="_state"></param>
        public WindowStateTracker(Form _owner, PersistentWindowState _state)
        {
            this.owner = _owner;
            this.state = _state;

            // subscribe to parent form's events
            owner.Closing += new System.ComponentModel.CancelEventHandler(OnClosing);
            owner.Resize += new System.EventHandler(OnResize);
            owner.Move += new System.EventHandler(OnMove);
            owner.Load += new System.EventHandler(OnLoad);

            // get initial width and height in case form is never resized
            state.size = owner.Size;
        }

        private void OnResize(object sender, EventArgs e)
        {
            // save width and height
            if (owner.WindowState == FormWindowState.Normal)
                state.size = owner.Size;
        }

        private void OnMove(object sender, EventArgs e)
        {
            // save position
            if (owner.WindowState == FormWindowState.Normal)
            {
                state.left = owner.Left;
                state.top = owner.Top;
            }
            // save state
            state.windowState = owner.WindowState;
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            state.save();
        }

        private void OnLoad(object sender, System.EventArgs e)
        {
            try
            {
                state.load();
                // if successfully loaded, set it up
                owner.Location = new Point(state.left, state.top);
                owner.Size = state.size;
                owner.WindowState = state.windowState;
            }
            catch (Exception)
            {
                // ignore this error
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public abstract class PersistentWindowState
    {
        /// <summary>
        /// 
        /// </summary>
        public int left, top, height, width;
        /// <summary>
        /// 
        /// </summary>
        public FormWindowState windowState;

        internal Size size
        {
            get { return new Size(width, height); }
            set { width = value.Width; height = value.Height; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rhs"></param>
        public void set(PersistentWindowState rhs)
        {
            this.left = rhs.left;
            this.top = rhs.top;
            this.height = rhs.height;
            this.width = rhs.width;
            this.windowState = rhs.windowState;
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected abstract void save();
        /// <summary>
        /// 
        /// </summary>
        internal protected abstract void load();
    }

    /// <summary>
    /// Store window state in an XML file.
    /// </summary>
    public class XmlPersistentWindowState : PersistentWindowState
    {
        private string fileName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_fileName"></param>
        public XmlPersistentWindowState(string _fileName)
        {
            this.fileName = _fileName;
        }

        // default constructor required by XML serialization
        /// <summary>
        /// 
        /// </summary>
        public XmlPersistentWindowState() { }
        /// <summary>
        /// 
        /// </summary>
        internal protected override void save()
        {
            XmlSerializer xs = new XmlSerializer(this.GetType());
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                xs.Serialize(fs, this);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected override void load()
        {
            XmlSerializer xs = new XmlSerializer(this.GetType());
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                set((PersistentWindowState)xs.Deserialize(fs));
            }
        }
    }

    /// <summary>
    /// PersistentWindowState that saves the state to a registry.
    /// </summary>
    //public class RegistryPersistentWindowState : PersistentWindowState
    //{
    //    private readonly RegistryKey key;
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="_key"></param>
    //    public RegistryPersistentWindowState(RegistryKey _key)
    //    {
    //        this.key = _key;
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    internal protected override void load()
    //    {
    //        left = (int)key.GetValue("Left");
    //        top = (int)key.GetValue("Top");
    //        width = (int)key.GetValue("Width");
    //        height = (int)key.GetValue("Height");
    //        windowState = (FormWindowState)key.GetValue("WindowState");
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    internal protected override void save()
    //    {
    //        // save position, size and state
    //        key.SetValue("Left", left);
    //        key.SetValue("Top", top);
    //        key.SetValue("Width", width);
    //        key.SetValue("Height", height);

    //        if (windowState == FormWindowState.Minimized)
    //            windowState = FormWindowState.Normal;

    //        key.SetValue("WindowState", (int)windowState);
    //    }
    //}

}
