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
using FreeTrain.Contributions.Train;
using FreeTrain.Framework;
using FreeTrain.Framework.Graphics;
using FreeTrain.Framework.Plugin;
using FreeTrain.Views;
using FreeTrain.World.Accounting;

namespace FreeTrain.World.Rail
{
    /// <summary>
    /// Dialog box to buy trains
    /// </summary>
    public partial class TrainTradingDialog : Form
    {
        /// <summary>
        /// 
        /// </summary>
        public TrainTradingDialog()
        {
            InitializeComponent();
            //handler = new OptionChangedHandler(updatePreview);
            WorldDefinition.World.ViewOptions.OnViewOptionChanged += new OptionChangedHandler(updatePreview);
            Bitmap bmp = ResourceUtil.LoadSystemBitmap("DayNight.bmp");
            buttonImages.TransparentColor = bmp.GetPixel(0, 0);
            buttonImages.Images.AddStrip(bmp);

            tbDay.Pushed = (WorldDefinition.World.ViewOptions.nightSpriteMode == NightSpriteMode.AlwaysDay);
            tbNight.Pushed = (WorldDefinition.World.ViewOptions.nightSpriteMode == NightSpriteMode.AlwaysNight);

            // organize trains into a tree
            IDictionary types = new SortedList();
            foreach (TrainContribution tc in Core.Plugins.trains)
            {
                IDictionary company = (IDictionary)types[tc.CompanyName];
                if (company == null)
                    types[tc.CompanyName] = company = new SortedList();

                IDictionary type = (IDictionary)company[tc.TypeName];
                if (type == null)
                    company[tc.TypeName] = type = new SortedList();

                type.Add(tc.NickName, tc);
            }

            // build a tree
            foreach (DictionaryEntry company in types)
            {
                TreeNode cn = new TreeNode((string)company.Key);
                typeTree.Nodes.Add(cn);

                foreach (DictionaryEntry type in (IDictionary)company.Value)
                {
                    IDictionary t = (IDictionary)type.Value;
                    if (t.Count == 1)
                    {
                        addTrains(cn, t);
                    }
                    else
                    {
                        TreeNode tn = new TreeNode((string)type.Key);
                        cn.Nodes.Add(tn);

                        addTrains(tn, t);
                    }
                }
            }

            onTypeChanged(null, null);
        }

        private void addTrains(TreeNode parent, IDictionary list)
        {
            foreach (DictionaryEntry trainEntry in list)
            {
                TrainContribution t = (TrainContribution)trainEntry.Value;

                TreeNode trainNode = new TreeNode(t.Name);
                trainNode.Tag = t;
                parent.Nodes.Add(trainNode);
            }
        }

        /// <summary>
        /// 使用されているリソースに後処理を実行します。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private TrainContribution selectedTrain
        {
            get
            {
                TreeNode n = typeTree.SelectedNode;
                if (n == null) return null;
                return (TrainContribution)n.Tag;
            }
        }

        private long getTotalPrice()
        {
            return (long)(selectedTrain.Price(1) * length.Value * count.Value);
        }

        private void onTypeChanged(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            updatePreview();
        }

        /// <summary>
        /// 
        /// </summary>
        public void updatePreview()
        {
            length.Enabled = count.Enabled = buttonOK.Enabled = (selectedTrain != null);

            Image im = preview.Image;
            if (im != null)
            {
                preview.Image = null;
                im.Dispose();
            }

            if (selectedTrain != null)
            {
                name.Text = selectedTrain.Name;
                author.Text = selectedTrain.Author;
                description.Text = selectedTrain.Description;
                speed.Text = selectedTrain.SpeedDisplayName;
                length.Maximum = selectedTrain.MaxLength;
                length.Minimum = selectedTrain.MinLength;
                //if (length.Value > selectedTrain.maxLength) length.Value = selectedTrain.maxLength;
                using (PreviewDrawer pd = selectedTrain.CreatePreview(preview.ClientSize, (int)length.Value))
                {
                    preview.Image = pd.createBitmap();
                }

                if (count.Value == 0)
                    // if the user changes the type, s/he is going to buy another train.
                    // thus change the value to 1.
                    count.Value = 1;

                onAmountChanged(null, null);
            }
            else
            {
                name.Text = author.Text = description.Text = speed.Text = "";
            }
        }

        private void onAmountChanged(object sender, EventArgs e)
        {
            if (count.Value != 0 && selectedTrain != null)
            {
                TrainCarContribution[] cars = selectedTrain.Create((int)length.Value);
                if (cars != null)
                {
                    buttonOK.Enabled = true;

                    // TODO: non-linear price support
                    totalPrice.Text = getTotalPrice().ToString();

                    int p = 0;
                    foreach (TrainCarContribution car in cars)
                        p += car.Capacity;

                    passenger.Text = p.ToString() + " passengers/set";
                    //! passenger.Text = p.ToString()+" 人/編成";

                    using (PreviewDrawer pd = selectedTrain.CreatePreview(preview.ClientSize, (int)length.Value))
                    {
                        preview.Image = pd.createBitmap();
                    }

                    return;
                }

            }

            buttonOK.Enabled = false;
            totalPrice.Text = "---";
            passenger.Text = "---";

        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            // buy trains
            for (int i = 0; i < (int)count.Value; i++)
                new Train(WorldDefinition.World.RootTrainGroup,
                    (int)length.Value, selectedTrain);

            FreeTrain.Framework.Sound.SoundEffectManager
                .PlaySynchronousSound(ResourceUtil.FindSystemResource("vehiclePurchase.wav"));

            AccountManager.theInstance.spend(getTotalPrice(), AccountGenre.RailService);

            // set count to 0 to avoid accidental purchase
            count.Value = 0;
        }

        private void toolBar1_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            foreach (ToolBarButton tb in toolBarDayNight.Buttons)
            {
                if (e.Button == tb)
                {
                    if (tb.Pushed)
                        WorldDefinition.World.ViewOptions.nightSpriteMode = (NightSpriteMode)tb.Tag;
                    else
                        WorldDefinition.World.ViewOptions.nightSpriteMode = NightSpriteMode.AlignClock;
                }
                else
                {
                    tb.Pushed = false;
                }
            }
        }

        private void TrainTradingDialog_Closed(object sender, System.EventArgs e)
        {
            WorldDefinition.World.ViewOptions.OnViewOptionChanged -= new OptionChangedHandler(updatePreview);
        }

        void TrainTradingDialogLoad(object sender, EventArgs e)
        {

        }
    }
}
