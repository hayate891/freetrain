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
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FreeTrain.Framework;
using FreeTrain.Views;
using FreeTrain.world;

namespace FreeTrain.Controllers
{
    /// <summary>
    /// Pop-up tool window to host modal controllers.
    /// </summary>
    public class AbstractControllerForm : Form
    {
        private Point mouse_offset;
        /// <summary>
        /// 
        /// </summary>
        public AbstractControllerForm()
        {
            InitializeComponent();
            try
            {
                World.world.viewOptions.OnViewOptionChanged += new OptionChangedHandler(updatePreview);
            }
            catch (NullReferenceException nre)
            {
                Debug.WriteLine(nre);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            World.world.viewOptions.OnViewOptionChanged -= new OptionChangedHandler(updatePreview);
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(System.EventArgs e)
        {
            try
            {
                // attach this window.
                MainWindow.mainWindow.AddOwnedForm(this);
                // move this window to the left-top position of the parent window
                this.Left = MainWindow.mainWindow.Left;
                this.Top = MainWindow.mainWindow.Top;
            }
            catch
            {
                //Debug.WriteLine(nre);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void updatePreview() { }
        /// <summary>
        /// 
        /// </summary>
        protected Label lblTitle;
        /// <summary>
        /// 
        /// </summary>
        protected Label lblExit;

        #region Windows Form Designer generated code
        private System.ComponentModel.Container components = null;

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblExit = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(220)))), ((int)(((byte)(84)))));
            this.lblTitle.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(275, 15);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "AbstractController";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AbstractControllerForm_MouseDown);
            this.lblTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.AbstractControllerForm_MouseMove);
            this.lblTitle.Paint += new System.Windows.Forms.PaintEventHandler(this.lblTitle_Paint);
            // 
            // lblExit
            // 
            this.lblExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(220)))), ((int)(((byte)(84)))));
            this.lblExit.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExit.Location = new System.Drawing.Point(241, 5);
            this.lblExit.Margin = new System.Windows.Forms.Padding(0);
            this.lblExit.Name = "lblExit";
            this.lblExit.Size = new System.Drawing.Size(39, 15);
            this.lblExit.TabIndex = 1;
            this.lblExit.Text = "EXIT";
            this.lblExit.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblExit.MouseLeave += new System.EventHandler(this.lblExit_MouseLeave);
            this.lblExit.Click += new System.EventHandler(this.lblExit_Click);
            this.lblExit.MouseEnter += new System.EventHandler(this.lblExit_MouseEnter);
            // 
            // AbstractControllerForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(184)))));
            this.ClientSize = new System.Drawing.Size(292, 271);
            this.Controls.Add(this.lblExit);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AbstractControllerForm";
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.AbstractControllerForm_Paint);
            this.Resize += new System.EventHandler(this.AbstractControllerForm_Resize);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.AbstractControllerForm_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AbstractControllerForm_MouseDown);
            this.Load += new System.EventHandler(this.AbstractControllerForm_Load);
            this.ResumeLayout(false);

        }

        void lblTitle_Paint(object sender, PaintEventArgs e)
        {
            System.Drawing.Graphics gr = e.Graphics;
            gr.DrawLine(new Pen(Brushes.Black, 1), new Point(this.lblExit.Left - this.lblTitle.Left - 1, -1), new Point(this.lblExit.Left - this.lblTitle.Left - 1, this.lblTitle.Height));
        }

        void lblExit_MouseLeave(object sender, EventArgs e)
        {
            this.lblExit.BackColor = Color.FromArgb(236, 220, 84);
            this.lblExit.ForeColor = Color.Black;
        }

        void lblExit_MouseEnter(object sender, EventArgs e)
        {
            this.lblExit.BackColor = Color.Black;
            this.lblExit.ForeColor = Color.White;
        }
        #endregion

        void AbstractControllerForm_Resize(object sender, EventArgs e)
        {
            this.lblTitle.Width = this.Width - (this.lblTitle.Left * 2);
            this.lblExit.Left = this.lblTitle.Left + this.lblTitle.Width - this.lblExit.Width;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AbstractControllerForm_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_offset = new Point(-e.X, -e.Y);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AbstractControllerForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouse_offset.X, mouse_offset.Y);
                this.Location = mousePos;
            }
        }

        void AbstractControllerForm_Paint(object sender, PaintEventArgs e)
        {
            System.Drawing.Graphics gr = e.Graphics;
            Point[] pts = new Point[4];
            pts[0] = new Point(0, 0);
            pts[1] = new Point(this.Width - 1, 0);
            pts[2] = new Point(this.Width - 1, this.Height - 1);
            pts[3] = new Point(0, this.Height - 1);
            gr.DrawPolygon(new Pen(Brushes.Black, 1), pts);

            pts[0].X += 2; pts[0].Y += 2;
            pts[1].X -= 2; pts[1].Y += 2;
            pts[2].X -= 2; pts[2].Y -= 2;
            pts[3].X += 2; pts[3].Y -= 2;
            gr.DrawPolygon(new Pen(Color.FromArgb(206, 204, 170), 3), pts);

            pts[0].X += 1; pts[0].Y += 1;
            pts[1].X -= 1; pts[1].Y += 1;
            pts[2].X -= 1; pts[2].Y -= 1;
            pts[3].X += 1; pts[3].Y -= 1;
            gr.DrawPolygon(new Pen(Brushes.Black, 1), pts);

            pts[0].X -= 1; pts[0].Y -= 1;
            pts[1].Y -= 1;
            gr.DrawLine(new Pen(Color.FromArgb(120, 119, 102), 1), pts[0], pts[1]);

            pts[3].X -= 1;// pts[3].Y += 1;
            gr.DrawLine(new Pen(Color.FromArgb(120, 119, 102), 1), pts[0], pts[3]);

            pts[1].X += 2; pts[1].Y -= 1;
            pts[2].X += 2; pts[2].Y += 2;
            gr.DrawLine(new Pen(Color.FromArgb(120, 119, 102), 1), pts[1], pts[2]);

            pts[3].X -= 1; pts[3].Y += 2;
            gr.DrawLine(new Pen(Color.FromArgb(120, 119, 102), 1), pts[3], pts[2]);

            gr.DrawLine(new Pen(Brushes.White, 1), new Point(this.lblTitle.Left - 1, this.lblTitle.Top - 1), new Point(this.lblTitle.Left + this.lblTitle.Width, this.lblTitle.Top - 1));
            gr.DrawLine(new Pen(Brushes.White, 1), new Point(this.lblTitle.Left - 1, this.lblTitle.Top - 1), new Point(this.lblTitle.Left - 1, this.lblTitle.Top + this.lblTitle.Height + 1));
            gr.DrawLine(new Pen(Brushes.Black, 1), new Point(this.lblTitle.Left - 1, this.lblTitle.Top + this.lblTitle.Height), new Point(this.lblTitle.Left + this.lblTitle.Width, this.lblTitle.Top + this.lblTitle.Height));
            gr.DrawLine(new Pen(Brushes.Black, 1), new Point(this.lblTitle.Left + this.lblTitle.Width, this.lblTitle.Top - 1), new Point(this.lblTitle.Left + this.lblTitle.Width, this.lblTitle.Top + this.lblTitle.Height));
        }

        private void AbstractControllerForm_Load(object sender, EventArgs e)
        {

        }

        private void lblExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
