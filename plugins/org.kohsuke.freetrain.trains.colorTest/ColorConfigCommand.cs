using System;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using freetrain.contributions.others;
using freetrain.framework;
using freetrain.framework.plugin;

namespace freetrain.world.rail.cttrain
{
    /// <summary>
    /// Menu item contribution that allows an user to
    /// open a color config dialog.
    /// </summary>
    [CLSCompliant(false)]
    public class ColorConfigCommand : MenuContribution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public ColorConfigCommand(XmlElement e) : base(e) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerMenu"></param>
        public override void mergeMenu(MainMenu containerMenu)
        {
            MenuItem item = new MenuItem();
            item.Text = "Color Test Train Settings";
            //! item.Text = "試験列車の色設定";
            item.Click += new System.EventHandler(onClick);

            containerMenu.MenuItems[1].MenuItems.Add(item);
        }

        private void onClick(object sender, EventArgs args)
        {
            Form form = new ColorConfigDialog(ColorTestTrainCar.theInstance);
            MainWindow.mainWindow.AddOwnedForm(form);
            form.Show();
        }
    }
}
