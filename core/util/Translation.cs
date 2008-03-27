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
        static ResourceManager resourceManager;
        static CultureInfo englishCulture = new CultureInfo("en-US");
        static CultureInfo japaneseCulture = new CultureInfo("ja-JP");
        static CultureInfo frenchCulture = new CultureInfo("fr-FR");
        static CultureInfo swedishCulture = new CultureInfo("sv-SE");

        static Translation()
        {
            //Uncomment to test Japanese interface.
            //Thread.CurrentThread.CurrentUICulture = japaneseCulture;
            //Thread.CurrentThread.CurrentUICulture = frenchCulture;
            //Thread.CurrentThread.CurrentUICulture = swedishCulture;
            resourceManager = new ResourceManager("FreeTrain.Properties.Resources", 
                                    System.Reflection.Assembly.GetExecutingAssembly());
            //uncomment to get a popup right before launch of the current locale.
            //MessageBox.Show(Thread.CurrentThread.CurrentUICulture.DisplayName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetString(string code)
        {
            try {
                return resourceManager.GetString(code);
            } catch (Exception e) {
                System.Console.Out.WriteLine("missing translation code: "+code);
                System.Console.Out.WriteLine(e.StackTrace);
                return code;
            }
        }
    }
}
