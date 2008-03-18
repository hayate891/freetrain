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
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace FreeTrain.Controls
{
    /// <summary>
    /// the button pick color by opening ColorPicker
    /// </summary>
    public class ColorPickButton : Button
    {
        /// <summary>
        /// 
        /// </summary>
        public ColorPickButton()
        {
            SelectedColor = Color.Black;
            Size = new Size(16, 16);
            ForeColor = Color.Transparent;
        }

        /// <summary>
        /// 
        /// </summary>
        protected IColorLibrary[] lib;
        /// <summary>
        /// 
        /// </summary>
        public IColorLibrary[] colorLibraries
        {
            get { return lib; }
            set
            {
                lib = value;
                if (picker != null)
                {
                    picker.Dock = DockStyle.None;
                    picker.setColors(lib);
                    picker.PalettesInRow = lib.Length < 3 ? 4 : 8;
                    pickerForm.ClientSize = new Size(picker.Width + 2, picker.Height + 2);
                    picker.Dock = DockStyle.Fill;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (lib != null)
                picker = new ColorPicker(lib, lib.Length < 3 ? 4 : 8);
            else
                picker = new ColorPicker();

            pickerForm = new Form();
            pickerForm.ControlBox = false;
            pickerForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            //pickerForm.FormBorderStyle = FormBorderStyle.None;
            pickerForm.MaximizeBox = false;
            pickerForm.MinimizeBox = false;
            pickerForm.Name = "ColorPickerForm";
            pickerForm.ShowInTaskbar = false;
            pickerForm.SuspendLayout();
            picker.CreateControl();
            picker.SelectedColor = selected;
            picker.OnColorSelected += new EventHandler(picker_OnColorSelected);
            pickerForm.ClientSize = new Size(picker.Width + 2, picker.Height + 2);
            picker.Dock = DockStyle.Fill;
            pickerForm.Controls.Add(picker);
            pickerForm.Deactivate += new EventHandler(picker_LostFocus);
            pickerForm.ResumeLayout(false);
            pickerForm.Show();
            pickerForm.Hide();
        }

        /// <summary>
        /// 
        /// </summary>
        protected Color selected;
        /// <summary>
        /// 
        /// </summary>
        public Color SelectedColor
        {
            get { return selected; }
            set
            {
                selected = value;
                BackColor = selected;
                //				if(picker!=null)
                //					picker.SelectedColor = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected Form pickerForm;
        /// <summary>
        /// 
        /// </summary>
        private ColorPicker picker;
        /// <summary>
        /// 
        /// </summary>
        public ColorPicker Picker { get { return picker; } }

        private void picker_OnColorSelected(object sender, EventArgs e)
        {
            SelectedColor = picker.SelectedColor;
            pickerForm.Hide();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            //pickerForm.ClientSize = new Size(picker.Width,picker.Height);
            pickerForm.Location = PointToScreen(new Point(0, Height));
            picker.SelectedColor = selected;
            pickerForm.Focus();
            pickerForm.Show();
        }

        private void picker_LostFocus(object sender, EventArgs e)
        {
            pickerForm.Hide();
        }
    }
}
