using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

namespace FreeTrain.Util
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Translation
    {
        static readonly Translation instance = new Translation();
        static private ResourceManager resman;

        Translation()
        {
            //uncomment to get a popup right before launch of the current locale.
            //MessageBox.Show(Application.CurrentCulture.DisplayName);
            resman = ResourceManager.CreateFileBasedResourceManager(
                        "freetrain", Path.Combine(Application.StartupPath, "lang"),
                        null);
        }

        private static Translation Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetString(string code)
        {
            return resman.GetString(code, Application.CurrentCulture);
        }
    }
}
