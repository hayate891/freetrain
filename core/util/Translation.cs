using System;
using System.Threading;
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
    public static class Translation
    {
        //static readonly Translation instance = new Translation();
        static ResourceManager resman;
        static CultureInfo englishCulture = new CultureInfo("en-US");
        static CultureInfo japaneseCulture = new CultureInfo("ja-JP");

        static Translation()
        {
            Thread.CurrentThread.CurrentUICulture = japaneseCulture;
            resman = new ResourceManager("FreeTrain.Properties.Resources", 
                                    System.Reflection.Assembly.GetExecutingAssembly());
            //uncomment to get a popup right before launch of the current locale.
            //MessageBox.Show(Application.CurrentCulture.DisplayName);
        }

        //private static Translation Instance
        //{
        //    get
        //    {
        //        return instance;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetString(string code)
        {
            return resman.GetString(code);
        }
    }
}