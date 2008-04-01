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
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Text;
using System.Xml;
using FreeTrain.Util;
using FreeTrain.Contributions.Train;
using FreeTrain.Contributions.Land;
using FreeTrain.Framework.Graphics;

namespace FreeTrain.Framework.Plugin
{
    /// <summary>
    /// Represents a loaded plug-in
    /// </summary>
    public class PluginDefinition
    {
        private readonly string _title;
        /// <summary>
        /// 
        /// </summary>
        public string title { get { return _title; } }

        private readonly string _author;
        /// <summary>
        /// 
        /// </summary>
        public string author { get { return _author; } }

        private readonly string _homepage;
        /// <summary>
        /// 
        /// </summary>
        public string homepage { get { return _homepage; } }

        /// <summary>
        /// Base directory of this plug-in
        /// </summary>
        public readonly string dirName;

        /// <summary>
        /// Name of the plug-in
        /// </summary>
        public string name { get { return Path.GetFileName(dirName); } }

        /// <summary>
        /// All the contributions in this plug-in
        /// </summary>
        public readonly IList contributions = new ArrayList();

        /// <summary>
        /// Contents of plugin.xml
        /// Available only during the initialization phase.
        /// </summary>
        private XmlDocument doc;
        /// <summary>
        /// 
        /// </summary>
        public XmlDocument Document
        {
            get { return doc; }
        }

        /// <summary>
        /// Stub instance to show information on parse error.
        /// </summary>
        /// <param name="dirname"></param>
        /// <param name="title"></param>
        /// <param name="homepage"></param>
        /// <param name="author"></param>
        protected PluginDefinition(string dirname, string title, string homepage, string author)
        {
            this.dirName = dirname;
            this._title = title;
            this._homepage = homepage;
            this._author = author;
            Debug.WriteLine(title + "," + author);
        }

        /// <summary>
        /// Loads a plug-in from manifest XML "plugin.xml".
        /// </summary>
        public PluginDefinition(string dirName)
        {
            this.dirName = dirName;
            doc = loadManifest(dirName);

            XmlElement root = doc.DocumentElement;
            _title = XmlUtil.selectSingleNode(root, "title").InnerText;
            _homepage = XmlUtil.selectSingleNode(root, "homepage").InnerText;

            if (root.SelectSingleNode("author") == null)
                _author = "";
            else
                _author = XmlUtil.selectSingleNode(root, "author").InnerText;

            Debug.WriteLine("loading plug-in: " + title);
            Debug.WriteLine("  base dir: " + dirName);

        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual void loadContributionFactories()
        {
            XmlElement root = doc.DocumentElement;
            // locate contribution factories first,
            // because we'll need them to load contributions.
            foreach (XmlElement contrib in root.SelectNodes("contribution"))
            {
                string type = contrib.Attributes["type"].Value;
                if (type == "contribution")
                {
                    // load a contribution factory

                    string contributionName = XmlUtil.selectSingleNode(contrib, "name").InnerText;

                    IContributionFactory factory =
                        (IContributionFactory)PluginUtil.loadObjectFromManifest(contrib);

                    // register it
                    PluginManager.theInstance.addContributionFactory(
                        contributionName,
                        factory);
                }
            }
        }

        /// <summary>
        /// Get all the dependent plug-ins.
        /// </summary>
        public PluginDefinition[] getDependencies()
        {
            ArrayList a = new ArrayList();
            if (!this.name.Equals("system"))
                a.Add(PluginManager.theInstance.getPlugin("system"));

            foreach (XmlElement depend in doc.DocumentElement.SelectNodes("depend"))
            {
                string name = depend.Attributes["on"].Value;
                PluginDefinition p = PluginManager.theInstance.getPlugin(name);
                if (p == null)
                    throw new Exception(String.Format(
                        "Plugin {1} that is needed for plugin {0} could not be found", this.name, name));
                //! "プラグイン{0}に必要なプラグイン{1}がみつかりません",this.name,name));

                a.Add(p);
            }
            return (PluginDefinition[])a.ToArray(typeof(PluginDefinition));
        }

        /// <summary>
        /// Loads plugin.xml file from the directory.
        /// </summary>
        private static XmlDocument loadManifest(string dirName)
        {
            string path = Path.Combine(dirName, "plugin.xml");
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                XmlDocument doc = new XmlDocument();
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.XmlResolver = null;
                settings.ValidationType = ValidationType.None;
                settings.ProhibitDtd = false;
                XmlReader reader = XmlReader.Create(new XmlTextReader(path, file), settings);
                doc.Load(reader);
                return doc;
            }
        }

        /// <summary>
        /// Loads plugin.xml file as plain text.
        /// </summary>
        public static PluginDefinition loadFailSafe(string dirName)
        {
            string path = Path.Combine(dirName, "plugin.xml");
            string title = "s";
            string homepage = "";
            string author = "s";
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(path, Encoding.GetEncoding("utf-8"));
                string s = sr.ReadToEnd();
                title = extractNodeText(s, "title");
                homepage = extractNodeText(s, "homepage");
                author = extractNodeText(s, "author");
            }
            finally
            {
                if (sr != null)
                    sr.Close();
            }
            return new PluginDefinition(dirName, title, homepage, author);
        }

        private static string extractNodeText(string buffer, string tagname)
        {
            if (buffer == null || tagname == null)
                return "";
            int begin = buffer.IndexOf(string.Format("<{0}>", tagname));
            if (begin == -1)
                return "";
            begin += tagname.Length + 2;
            int end = buffer.IndexOf(string.Format("</{0}>", tagname), begin);
            if (end == -1)
                end = buffer.Length;
            return buffer.Substring(begin, end - begin);
        }

        /// <summary>
        /// Loads contributions from this plug-in
        /// </summary>
        internal protected virtual void loadContributions()
        {
            Debug.WriteLine("loading contributions from " + name);
            XmlElement root = doc.DocumentElement;

            Uri baseUri = new Uri(root.BaseURI);

            // load contributions
            foreach (XmlElement contrib in root.SelectNodes("contribution"))
            {
                try
                {
                    string type = contrib.Attributes["type"].Value;
                    if (type == "contribution") continue;	// ignore

                    IContributionFactory factory = PluginManager.theInstance.getContributionFactory(type);
                    Contribution c = factory.Load(this, contrib);
                    contributions.Add(c);
                    PluginManager.theInstance.addContribution(c);
                    c.init(this, baseUri);
                }
                catch (Exception e)
                {
                    throw new Exception("failed to load contribution " + contrib.Attributes["id"].Value, e);
                }
            }
        }


        /// <summary>
        /// Loads a stream from the plug-in directory.
        /// </summary>
        public Stream loadStream(string name)
        {
            return new FileStream(Path.Combine(dirName, name), FileMode.Open, FileAccess.Read, FileShare.Read);
        }
    }
}
