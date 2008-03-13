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
using System.Reflection;
using System.Runtime.Serialization;
using freetrain.framework;

namespace freetrain.framework.plugin
{
    /// <summary>
    /// Allows objects from plug-ins to be de-serialized.
    /// </summary>
    public class PluginSerializationBinder : SerializationBinder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public override System.Type BindToType(string assemblyName, string typeName)
        {
            Type t;

            t = Type.GetType(typeName);
            if (t != null) return t;

            Trace.WriteLine("binding " + typeName);

            // try assemblies of plug-ins
            foreach (Contribution cont in Core.plugins.publicContributions)
            {
                Assembly asm = cont.assembly;
                if (asm != null)
                {
                    t = asm.GetType(typeName);
                    if (t != null) return t;
                }
            }
            Trace.WriteLine("not found");
            return null;
        }
    }
}
