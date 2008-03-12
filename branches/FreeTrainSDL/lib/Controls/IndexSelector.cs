#region LICENSE
/*
 * Copyright (C) 2004 - 2007 David Hudson (jendave@yahoo.com)
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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace freetrain.controls
{
    /// <summary>
    /// Allow the user to select an index
    /// </summary>
    public class IndexSelector : System.Windows.Forms.UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public IndexSelector()
        {
            InitializeComponent();
            _count = 10;
        }

        private System.Windows.Forms.Label indexBox;

        private int _current;
        private int _count;
        private IList _collection;

        /// <summary>
        /// 
        /// </summary>
        public int current
        {
            get { return _current; }
            set
            {
                if (value < 0 || _count <= value)
                    throw new Exception("incorrect parameter:" + value);
                _current = value;
                updateBox();
                if (indexChanged != null)
                    indexChanged(this, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int count
        {
            get { return _count; }
            set
            {
                _count = value;
                if (_current >= _count) _current = _count - 1;
                if (_current < 0 && _count > 0)
                    _current = 0;
                updateBox();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IList dataSource
        {
            get { return _collection; }
            set
            {
                _collection = value;
                if (_collection != null)
                    count = _collection.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public object currentItem
        {
            get
            {
                if (_collection != null) return _collection[_current];
                else return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler indexChanged;


        private void updateBox()
        {
            indexBox.Text = (_current + 1) + "/" + _count;
        }

        private void onLeft(object sender, System.EventArgs e)
        {
            current = (current - 1 + count) % count;
        }

        private void onRight(object sender, System.EventArgs e)
        {
            current = (current + 1 + count) % count;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        private System.Windows.Forms.Button buttonLeft;
        private System.Windows.Forms.Button buttonRight;
        private System.ComponentModel.Container components = null;

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonLeft = new System.Windows.Forms.Button();
            this.buttonRight = new System.Windows.Forms.Button();
            this.indexBox = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonLeft
            // 
            this.buttonLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonLeft.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonLeft.Font = new System.Drawing.Font("Webdings", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(2)));
            this.buttonLeft.Name = "buttonLeft";
            this.buttonLeft.Size = new System.Drawing.Size(24, 24);
            this.buttonLeft.TabIndex = 0;
            this.buttonLeft.Text = "3";
            this.buttonLeft.Click += new System.EventHandler(this.onLeft);
            // 
            // buttonRight
            // 
            this.buttonRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonRight.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonRight.Font = new System.Drawing.Font("Webdings", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(2)));
            this.buttonRight.Location = new System.Drawing.Point(88, 0);
            this.buttonRight.Name = "buttonRight";
            this.buttonRight.Size = new System.Drawing.Size(24, 24);
            this.buttonRight.TabIndex = 1;
            this.buttonRight.Text = "4";
            this.buttonRight.Click += new System.EventHandler(this.onRight);
            // 
            // indexBox
            // 
            this.indexBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.indexBox.Location = new System.Drawing.Point(24, 0);
            this.indexBox.Name = "indexBox";
            this.indexBox.Size = new System.Drawing.Size(64, 24);
            this.indexBox.TabIndex = 2;
            this.indexBox.Text = "1/3";
            this.indexBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // IndexSelector
            // 
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.indexBox,
																		  this.buttonRight,
																		  this.buttonLeft});
            this.Name = "IndexSelector";
            this.Size = new System.Drawing.Size(112, 24);
            this.ResumeLayout(false);

        }
        #endregion

    }
}
