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
using System.Xml.Serialization;
using System.Windows.Forms;

namespace freetrain.framework
{
    /// <summary>
    /// Base implementation for persistent configuration information
    /// via XML serialization.
    /// 
    /// Derived class should add actual data members. See GlobalOptions for example.
    /// It should also override the load method.
    /// 
    /// For some reason, the derived class needs to be public.
    /// 
    /// To load a persistent configuration, do
    /// <code>new DerivedClass().load()</code>.
    /// An instance of the derived class is necessary because it determined where and
    /// how an XML file is loaded.
    /// </summary>
    public abstract class PersistentOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public PersistentOptions() { }

        /// <summary>
        /// Compute the stem of the file name to be used.
        /// Default implementation should be fine for the most cases.
        /// </summary>
        protected virtual string Stem
        {
            get
            {
                string stem = "." + this.GetType().FullName;
                return stem.Replace('+', '.');
            }
        }

        private string FileName
        {
            get
            {
                return Application.ExecutablePath + Stem + ".options";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void save()
        {
            using (Stream s = new FileStream(FileName, FileMode.Create))
                saveTo(s);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        protected void saveTo(Stream stream)
        {
            XmlSerializer s = new XmlSerializer(this.GetType());
            s.Serialize(stream, this);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected PersistentOptions load()
        {
            try
            {
                using (Stream s = new FileStream(FileName, FileMode.Open))
                    return loadFrom(s);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                // unable to load. use default.
                return this;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        protected PersistentOptions loadFrom(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            return (PersistentOptions)serializer.Deserialize(stream);
        }
    }
}
