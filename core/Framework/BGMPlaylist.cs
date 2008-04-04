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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using FreeTrain.Contributions.Sound;
using FreeTrain.Controllers;


namespace FreeTrain.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public partial class BGMPlaylist : AbstractControllerImpl
    {
        private bool isPlaying = false;
        /// <summary>
        /// 
        /// </summary>
        public BGMPlaylist()
        {
            InitializeComponent();
        }

        private void BGMPlaylist_Load(object sender, EventArgs e)
        {
            refreshPlaylists();
        }

        private void cmdPlayPause_Click(object sender, EventArgs e)
        {
            if (Core.BgmManager != null && lstCurrentPlaylist.Items.Count > 0)
            {
                if (isPlaying)
                {
                    cmdPlay.Text = "Play";
                    isPlaying = false;
                    Core.BgmManager.currentBGM = null;
                }
                else
                {
                    if (lstCurrentPlaylist.SelectedItem == null)
                        Core.BgmManager.currentBGM = (BGMContribution)lstCurrentPlaylist.Items[0];
                    else
                        Core.BgmManager.currentBGM = (BGMContribution)lstCurrentPlaylist.SelectedItem;
                    isPlaying = true;
                    cmdPlay.Text = "Pause";
                }
            }
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            if (lstCurrentPlaylist.SelectedItem != null)
            {
                string selected = lstCurrentPlaylist.Text;
                Core.BgmManager.removeSong(lstCurrentPlaylist.Text);
                refreshPlaylists();
                lstBGMs.Text = selected;
            }
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            if (lstBGMs.SelectedItem != null)
            {
                string selected = lstBGMs.Text;
                Core.BgmManager.addSong(lstBGMs.Text);
                refreshPlaylists();
                lstCurrentPlaylist.Text = selected;
            }
        }

        private void refreshPlaylists()
        {
            lstBGMs.Items.Clear();
            lstCurrentPlaylist.Items.Clear();
            foreach (BGMContribution contrib in Core.Plugins.bgms)
                if (!Core.BgmManager.currentPlaylist.Contains(contrib)) lstBGMs.Items.Add(contrib);
            foreach (BGMContribution contrib in Core.BgmManager.currentPlaylist)
            {
                lstCurrentPlaylist.Items.Add(contrib);
                /*if (Core.bgmManager.currentBGM == contrib)
                {
                    lstCurrentPlaylist.SelectedIndex = lstCurrentPlaylist.Items.Count - 1;
                }*/
            }
        }

        void lstBGMs_DoubleClick(object sender, System.EventArgs e)
        {
            if (lstBGMs.SelectedItem != null)
            {
                Core.BgmManager.addSong(lstBGMs.Text);
                refreshPlaylists();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void playNextTrack()
        {
            isPlaying = false;
            cmdPlay.Text = "Play";
        }

        private void lstCurrentPlaylist_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        void lstCurrentPlaylist_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                e.DrawBackground();
                Brush textBrush = SystemBrushes.ControlText;
                Font drawFont = e.Font;

                if (Core.BgmManager.currentBGM == lstCurrentPlaylist.Items[e.Index])
                {
                    drawFont = new Font(drawFont.FontFamily, drawFont.Size, FontStyle.Bold);
                    cmdPlay.Text = "Pause";
                    isPlaying = true;
                }
                else if ((e.State & DrawItemState.Selected) > 0)
                {
                    textBrush = SystemBrushes.HighlightText;
                }

                e.Graphics.DrawString(lstCurrentPlaylist.Items[e.Index].ToString(), drawFont, textBrush, e.Bounds);
            }
        }

        void lstCurrentPlaylist_DoubleClick(object sender, System.EventArgs e)
        {
            if (lstCurrentPlaylist.SelectedItem == null)
                Core.BgmManager.currentBGM = (BGMContribution)lstCurrentPlaylist.Items[0];
            else
                Core.BgmManager.currentBGM = (BGMContribution)lstCurrentPlaylist.SelectedItem;
            refreshPlaylists();
            isPlaying = true;
            cmdPlay.Text = "Pause";
        }

        private void cmdNext_Click(object sender, EventArgs e)
        {
            Core.BgmManager.nextSong();
            refreshPlaylists();
        }

        private void cmdPrev_Click(object sender, EventArgs e)
        {
            Core.BgmManager.previousSong();
            refreshPlaylists();
        }

        private void cmdDown_Click(object sender, EventArgs e)
        {
            if (lstCurrentPlaylist.SelectedItem != null)
            {
                string selected = lstCurrentPlaylist.Text;
                Core.BgmManager.moveDown(lstCurrentPlaylist.Text);
                refreshPlaylists();
                lstCurrentPlaylist.Text = selected;
            }
        }

        private void cmdUp_Click(object sender, EventArgs e)
        {
            if (lstCurrentPlaylist.SelectedItem != null)
            {
                string selected = lstCurrentPlaylist.Text;
                Core.BgmManager.moveUp(lstCurrentPlaylist.Text);
                refreshPlaylists();
                lstCurrentPlaylist.Text = selected;
            }
        }
    }
}