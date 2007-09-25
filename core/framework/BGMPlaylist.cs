using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using freetrain.contributions.sound;


namespace freetrain.framework
{
    public partial class BGMPlaylist : Form
    {
        private bool isPlaying = false;
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
            if (Core.bgmManager != null && lstCurrentPlaylist.Items.Count > 0)
            {
                if (isPlaying)
                {
                    cmdPlay.Text = "Play";
                    isPlaying = false;
                    Core.bgmManager.currentBGM = null;
                }
                else
                {
                    if (lstCurrentPlaylist.SelectedItem == null)
                        Core.bgmManager.currentBGM = (BGMContribution)lstCurrentPlaylist.Items[0];
                    else
                        Core.bgmManager.currentBGM = (BGMContribution)lstCurrentPlaylist.SelectedItem;
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
                Core.bgmManager.removeSong(lstCurrentPlaylist.Text);
                refreshPlaylists();
                lstBGMs.Text = selected;
            }
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            if (lstBGMs.SelectedItem != null)
            {
                string selected = lstBGMs.Text;
                Core.bgmManager.addSong(lstBGMs.Text);
                refreshPlaylists();
                lstCurrentPlaylist.Text = selected;
            }
        }

        private void refreshPlaylists()
        {
            lstBGMs.Items.Clear();
            lstCurrentPlaylist.Items.Clear();
            foreach (BGMContribution contrib in Core.plugins.bgms)
                if (!Core.bgmManager.currentPlaylist.Contains(contrib)) lstBGMs.Items.Add(contrib);
            foreach (BGMContribution contrib in Core.bgmManager.currentPlaylist)
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
                Core.bgmManager.addSong(lstBGMs.Text);
                refreshPlaylists();
            }
        }

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

                if (Core.bgmManager.currentBGM == lstCurrentPlaylist.Items[e.Index])
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
                Core.bgmManager.currentBGM = (BGMContribution)lstCurrentPlaylist.Items[0];
            else
                Core.bgmManager.currentBGM = (BGMContribution)lstCurrentPlaylist.SelectedItem;
            refreshPlaylists();
            isPlaying = true;
            cmdPlay.Text = "Pause";
        }

        private void cmdNext_Click(object sender, EventArgs e)
        {
            Core.bgmManager.nextSong();
            refreshPlaylists();
        }

        private void cmdPrev_Click(object sender, EventArgs e)
        {
            Core.bgmManager.previousSong();
            refreshPlaylists();
        }

        private void cmdDown_Click(object sender, EventArgs e)
        {
            if (lstCurrentPlaylist.SelectedItem != null)
            {
                string selected = lstCurrentPlaylist.Text;
                Core.bgmManager.moveDown(lstCurrentPlaylist.Text);
                refreshPlaylists();
                lstCurrentPlaylist.Text = selected;
            }
        }

        private void cmdUp_Click(object sender, EventArgs e)
        {
            if (lstCurrentPlaylist.SelectedItem != null)
            {
                string selected = lstCurrentPlaylist.Text;
                Core.bgmManager.moveUp(lstCurrentPlaylist.Text);
                refreshPlaylists();
                lstCurrentPlaylist.Text = selected;
            }
        }
    }
}