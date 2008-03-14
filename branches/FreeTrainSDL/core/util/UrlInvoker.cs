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
using System.Diagnostics;
using System.IO;
//using Microsoft.Win32;

namespace freetrain.util
{
    /// <summary>
    /// UrlInvoker の概要の説明です。
    /// </summary>
    public class UrlInvoker
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetUrl"></param>
        static public void openUrl(String targetUrl)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            // URLに関連づけられたアプリケーションを探す
            //RegistryKey rkey = Registry.ClassesRoot.OpenSubKey(@"http\shell\open\command");
            String val = "";// rkey.GetValue("").ToString();
            // レジストリ値には、起動パラメータも含まれるので、
            // 実行ファイル名と起動パラメータを分離する
            if (val.StartsWith("\""))
            {
                int n = val.IndexOf("\"", 1);
                info.FileName = val.Substring(1, n - 1);
                info.Arguments = val.Substring(n + 1);
            }
            else
            {
                string[] a = val.Split(new char[] { ' ' });
                info.FileName = a[0];
                info.Arguments = val.Substring(a[0].Length + 1);
            }
            // 作業ディレクトリも指定しないとダメなようだ・・・
            info.WorkingDirectory = Path.GetDirectoryName(info.FileName);
            // 引数の最後にURLを加える
            info.Arguments += targetUrl;
            Process.Start(info);
        }

    }
}
