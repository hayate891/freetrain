using System;
using System.IO;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using nft.framework;

namespace nft.util
{
	[Serializable]
	public class Properties : Property, IEnumerable
	{
		protected Hashtable table
		{
			get{ return (Hashtable)_value;}
		}

		protected Properties(string name)
			:base(name,"properties",new Hashtable(),true,false)
		{
		}			

		protected Properties(string name, bool Readonly)
			:base(name,"properties",new Hashtable(),true,Readonly)
		{			 
		}

		protected Properties(XmlNode node, bool Readonly)
			:base(node.Attributes["name"].Value,"properties",new Hashtable(),true,Readonly)
		{
			ParseXml(node);
		}

		protected Properties(XmlNode node)
			:this(node,false)
		{
		}

		protected override void ParseXml(XmlNode node)
		{
			
			IEnumerator ie = node.ChildNodes.GetEnumerator();
			while( ie.MoveNext() )
			{
				XmlNode cn = ie.Current as XmlNode;
				Property p;
				if(cn.Name.Equals("properties"))
					p = new Properties(cn);
				else
					p = new Property(cn);
				AddProperty(p);
			}
		}

		public Property this[string name]
		{
			get
			{
				return PickNode(name);
			}
		}
		#region get value methods.
		/// <summary>
		/// get string property. if not exist, defaultVal is used.
		/// in this case, a new property is registerd with the name and the defaultVal.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultVal"></param>
		/// <returns></returns>
		public string getString(string name,string defaultVal)
		{
			if(!ContainsKey(name))
				if(defaultVal!=null)
					AddProperty(new Property(name,defaultVal,false));
			return this[name].stringValue;
		}
		/// <summary>
		/// get integral property. if not exist, defaultVal is used.
		/// in this case, a new property is registerd with the name and the defaultVal.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultVal"></param>
		/// <returns></returns>
		public int getInt(string name,int defaultVal)
		{
			if(!ContainsKey(name))
				AddProperty(new Property(name,defaultVal,false));
			return this[name].intValue;
		}
		/// <summary>
		/// get double type property. if not exist, defaultVal is used.
		/// in this case, a new property is registerd with the name and the defaultVal.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultVal"></param>
		/// <returns></returns>
		public double getDouble(string name,double defaultVal)
		{
			if(!ContainsKey(name))
				AddProperty(new Property(name,defaultVal,false));
			return this[name].doubleValue;
		}
		/// <summary>
		/// get boolean property. if not exist, defaultVal is used.
		/// in this case, a new property is registerd with the name and the defaultVal.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultVal"></param>
		/// <returns></returns>
		public bool getBool(string name,bool defaultVal)
		{
			if(!ContainsKey(name))
					AddProperty(new Property(name,defaultVal,false));
			return this[name].boolValue;
		}
		#endregion

		/// <summary>
		/// solve nested Properties and returns deepest Properties instance.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		protected Property PickNode(string fullpath )
		{
			int i = fullpath.IndexOf(SEPARATOR);
			if(i==0 || i==fullpath.Length)
				throw new ArgumentException("blank node is not allowed in :"+fullpath);
			if(i==-1)
				return table[fullpath] as Property;
			else
			{
				Properties p = table[fullpath.Substring(0,i)] as Properties;
				if(p!=null)
					return p.PickNode(fullpath.Substring(i+1));
				else
					throw new ArgumentException("property node ["+fullpath+"] is not found.");
			}
		}

		/// <summary>
		/// search over every node and returns whether the Properties name is registerd.
		/// </summary>
		/// <param name="name">the name to be checked</param>
		/// <returns></returns>
		public bool ContainsKey(string name )
		{
			int i = name.IndexOf(SEPARATOR);
			if(i==0 || i==name.Length)
				throw new ArgumentException("blank node is not allowed in :"+name);
			if(i==-1)
				return table.ContainsKey(name);
			else
			{
				Properties p = table[name.Substring(0,i)] as Properties;
				if(p!=null)
					return p.ContainsKey(name.Substring(i));
				else
					throw new ArgumentException("property node ["+name+"] is not found.");
			}
		}

		public IEnumerator GetEnumerator()
		{
			return table.Values.GetEnumerator();
		}


		/// <summary>
		/// add nested Properties.
		/// </summary>
		/// <param name="nodename">registration name of the node.</param>
		/// <param name="node"></param>
		public void AddProperty(Property prop)
		{
			string nodename = prop.name;
			int i = nodename.IndexOf(SEPARATOR);
			if(i>=0)			
				throw new ArgumentException("sub node name must not contains '"+SEPARATOR+"'.");
			table.Add(nodename,prop);
			prop.parent = this;
		}

		/// <summary>
		/// remove nested Properties.
		/// </summary>
		/// <param name="nodename">name of the node which should be removed.</param>
		/// <returns></returns>
		public Property RemoveProperty(string name)
		{
			int i = name.IndexOf(SEPARATOR);
			if(i==0 || i==(name.Length-1))			
				throw new ArgumentException("blank node name is found in ["+name+"].");
			Properties p = table[name.Substring(0,i)] as Properties;
			if( p==null )
				throw new ArgumentException("property node ["+name+"] is not found.");
			string key = name.Substring(i+1);
			if(i>0)
				return p.RemoveProperty(name.Substring(i+1));
			Property ret = table[name] as Property;
			table.Remove(name);
			ret.parent = null;
			return ret;
		}

		static public Properties LoadFromFile(string filename, bool Readonly)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(filename);
			XmlNode root = doc.SelectSingleNode("properties");
			return new Properties(root,Readonly);
		}

		public void WriteFile(string filename)
		{
			WriteFile(filename,System.Text.Encoding.Default.BodyName);
		}

		public void WriteFile(string filename, string encode)
		{
			XmlDocument doc = new XmlDocument();
			// Create an XML declaration. 
			XmlDeclaration xmldecl;
			xmldecl = doc.CreateXmlDeclaration("1.0",encode,null);      
			// Add the new node to the document.
			XmlElement root = doc.DocumentElement;
			doc.InsertBefore(xmldecl, root);

			doc.AppendChild(CreateXmlNode(doc));
			doc.Save(filename);
		}

		internal protected override XmlNode CreateXmlNode(XmlDocument doc)
		{
			XmlElement work = doc.CreateElement("properties");
			XmlAttribute at_name = doc.CreateAttribute("name");
			at_name.Value = name;
			work.Attributes.Append(at_name);
			IEnumerator ie = table.Values.GetEnumerator();
			while(ie.MoveNext())
			{
				XmlNode n = ((Property)ie.Current).CreateXmlNode(doc);
				work.AppendChild(n);
			}
			return work;
		}

	}
}
