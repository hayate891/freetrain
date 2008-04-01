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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace FreeTrain.Framework
{
    /// <summary>
    /// Splash screen that reports the progress of initialization.
    /// </summary>
    public partial class Splash : Form
    {
        bool exflag = true;
        /// <summary>
        /// 
        /// </summary>
        public Splash()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="progress"></param>
        public void UpdateMessage(string msg, float progress)
        {
            status.Text = msg;
            progressBar.Value = (int)(100 * progress);
            Application.DoEvents();
            if (progress > 0.4 && exflag)
            {
                System.Drawing.Graphics g = pictureBox1.CreateGraphics();
                imageList.Draw(g, 232, 128, 0);
                exflag = false;
            }
        }
    }
}
