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
using System.Runtime.Serialization;
using System.Collections;
using System.Windows.Forms;
using freetrain.contributions.sound;
using freetrain.framework.plugin;
using SDL.net;

namespace freetrain.framework
{
    /// <summary>
    /// Manages the selection and control of BGM
    /// </summary>
    public class BGMManager
    {
        /// <summary> BGM player. </summary>
        private readonly BGM bgm = new BGM();

        /// <summary> Reference to the "music" menu item. </summary>
        //private readonly MenuItem musicMenu;

        internal BGMManager()
        {

        }


        /*internal BGMManager( MenuItem musicMenu ) {
            this.musicMenu = musicMenu;

            // "silent"
            MusicMenuItem silence = new MusicMenuItem(this,null);
            musicMenu.MenuItems.Add( silence );
            musicMenu.Popup += new EventHandler(silence.update);

            // populate BGM contributions
            foreach( BGMContribution contrib in Core.plugins.bgms ) {
                MusicMenuItem child = new MusicMenuItem(this,contrib);
                musicMenu.MenuItems.Add( child );
                musicMenu.Popup += new EventHandler(child.update);
            }

            musicMenu.MenuItems.Add( new MenuItem("-") );

            // "select from disk"
            SelectMenuItem miSelect = new SelectMenuItem(this);
            musicMenu.MenuItems.Add( miSelect );
            musicMenu.Popup += new EventHandler(miSelect.update);
        }*/
        /// <summary>
        /// 
        /// </summary>
        public ArrayList currentPlaylist = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="songname"></param>
        public void addSong(string songname)
        {
            foreach (BGMContribution contrib in Core.plugins.bgms)
            {
                if (contrib.name == songname) currentPlaylist.Add(contrib);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="songname"></param>
        public void moveUp(string songname)
        {
            foreach (BGMContribution contrib in Core.plugins.bgms)
            {
                if (contrib.name == songname)
                {
                    int index = currentPlaylist.IndexOf(contrib);
                    if (index > 0)
                    {
                        currentPlaylist.Remove(contrib);
                        currentPlaylist.Insert(index - 1, contrib);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="songname"></param>
        public void moveDown(string songname)
        {
            foreach (BGMContribution contrib in Core.plugins.bgms)
            {
                if (contrib.name == songname)
                {
                    int index = currentPlaylist.IndexOf(contrib);
                    if (index < currentPlaylist.Count - 1)
                    {
                        currentPlaylist.Remove(contrib);
                        currentPlaylist.Insert(index + 1, contrib);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="songname"></param>
        public void removeSong(string songname)
        {
            foreach (BGMContribution contrib in Core.plugins.bgms)
            {
                if (contrib.name == songname)
                {
                    if (contrib == currentBGM)
                    {
                        if (currentPlaylist.Count > 1) nextSong();
                        else bgm.stop();
                    }
                    currentPlaylist.Remove(contrib);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void nextSong()
        {
            bool getNext = false, wasChanged = false;
            foreach (BGMContribution contrib in currentPlaylist)
            {
                if (getNext)
                {
                    this.currentBGM = contrib;
                    wasChanged = true;
                    getNext = false;
                }
                else if (contrib == this.currentBGM) getNext = true;
            }
            if (!wasChanged && currentPlaylist.Count > 0) this.currentBGM = (BGMContribution)currentPlaylist[0];
        }
        /// <summary>
        /// 
        /// </summary>
        public void previousSong()
        {
            BGMContribution prev = null;
            bool useLast = false;
            foreach (BGMContribution contrib in currentPlaylist)
            {
                if (contrib == this.currentBGM)
                {
                    if (prev == null) useLast = true;
                    else this.currentBGM = prev;
                }
                prev = contrib;
            }
            if (useLast && currentPlaylist.Count > 0) this.currentBGM = (BGMContribution)currentPlaylist[currentPlaylist.Count - 1];
        }

        private BGMContribution current = null;

        /// <summary>
        /// Sets or gets the current BGM.
        /// Set null for silence.
        /// </summary>
        public BGMContribution currentBGM
        {
            get
            {
                return current;
            }
            set
            {
                current = value;
                //bgm.stop();
                if (current != null)
                {
                    try
                    {
                        bgm.fileName = current.fileName;
                        bgm.run();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(MainWindow.mainWindow,
                            "Can not play back\n" + e.StackTrace, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //! "再生できません¥n"+e.StackTrace, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error );
                    }
                }
            }
        }



        /// <summary>
        /// MenuItem that selects a BGM from BGMContribution.
        /// </summary>
        /*internal class MusicMenuItem : MenuItem
        {
            private readonly BGMContribution contrib;
            private readonly BGMManager owner;

            public MusicMenuItem() : base() {}	// for some reason Windows Forms need this constructor
            internal MusicMenuItem( BGMManager owner, BGMContribution contrib ) {
                this.contrib = contrib;
                this.owner = owner;
                if( contrib==null)
                    this.Text = "Silence";
                    //! this.Text = "なし";
                else
                    this.Text = contrib.name;
            }

            protected override void OnClick(EventArgs e) {
                owner.currentBGM = contrib;
            }
            internal void update( object sender, EventArgs e ) {
                this.Checked = (owner.currentBGM == contrib);
            }
        }

        /// <summary>
        /// MenuItem that selects a BGM from a file.
        /// </summary>
        internal class SelectMenuItem : MenuItem
        {
            private readonly BGMManager owner;

            public SelectMenuItem() : base() {}	// for some reason Windows Forms need this constructor
            internal SelectMenuItem( BGMManager owner ) {
                this.owner = owner;
                this.Text = "&Select From File...";
                //! this.Text = "ファイルから選択(&S)...";
            }

            protected override void OnClick(EventArgs e) {
                using( OpenFileDialog ofd = new OpenFileDialog() ) {
                    if( ofd.ShowDialog(MainWindow.mainWindow)==DialogResult.OK )
                        owner.currentBGM = new TempBGMContribution(ofd.FileName);
                }
            }
            internal void update( object sender, EventArgs e ) {
                this.Checked = (owner.currentBGM is TempBGMContribution);
            }
        }*/

        /// <summary>
        /// Temporary BGM contribution created from a music in a file system.
        /// </summary>
        [Serializable]
        internal class TempBGMContribution : BGMContribution
        {
            internal TempBGMContribution(string fileName)
                : base("temp", fileName, Guid.NewGuid().ToString())
            {
            }
            // serialize this object by reference
            /// <summary>
            /// 
            /// </summary>
            /// <param name="info"></param>
            /// <param name="context"></param>
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.SetType(typeof(ReferenceImpl));
                info.AddValue("fileName", fileName);
            }
            [Serializable]
            internal new sealed class ReferenceImpl : IObjectReference
            {
                private string fileName = null;
                public object GetRealObject(StreamingContext context)
                {
                    return new TempBGMContribution(fileName);
                }
            }
        }

    }
}
