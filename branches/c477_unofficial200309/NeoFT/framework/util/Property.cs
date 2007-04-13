using System;
using System.Xml;
using System.Xml.Serialization;


namespace nft.util
{
	public delegate void PropertyChangedHandler(Property sender,object oldvalue);
	/// <summary>
	/// Properties ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	[Serializable]
	public class Property
	{
		public const char SEPARATOR = '.';
		public readonly bool isReadonly;
		public readonly string name;
		public readonly bool isStatic;
		protected readonly string type;
		protected object _value;
		// parent Property(=Properties)
		internal protected Property parent = null;
		/// <summary>
		/// fired when property value changed.
		/// Ancestors (Properties class) can catch every child PropertyChange event.
		/// </summary>
		[NonSerialized]
		public PropertyChangedHandler PropertyChanged;

		public string fullname
		{
			get
			{
				if(parent!=null)
					return parent.fullname+SEPARATOR+name;
				else
					return name;
			}
		}

		public static bool IsValidName(string name)
		{
			if(name==null || name.Trim().Length==0)
				return false;
			return (name.IndexOf(SEPARATOR)==-1);
		}

		protected Property(string name, string type, object val, bool isStatic, bool Readonly)
		{
			string tmp = name.Trim();
			if(!IsValidName(tmp))
				throw new ArgumentException("property must need valid name.");
			this.name = tmp;
			this.isStatic = isStatic;
			this.isReadonly = Readonly;
			this.type = type;
			this._value = val;
		}

		protected Property(XmlNode node)
		{
			this.name = XmlUtil.getAttribute(node,"name",null);
			this.type = XmlUtil.getAttribute(node,"type","string");
			this.isStatic = bool.Parse(XmlUtil.getAttribute(node,"static","false"));
			this.isReadonly = bool.Parse(XmlUtil.getAttribute(node,"readonly","false"));
			switch(type)
			{
				case "int":
					this._value = int.Parse(node.InnerText);
					break;
				case "double":
					this._value = double.Parse(node.InnerText);
					break;
				case "bool":
					this._value = bool.Parse(node.InnerText);
					break;
				case "string":
					this._value = node.InnerText;
					break;
				default:
					ParseXml(node);
					break;
			}
		}

		protected virtual void ParseXml(XmlNode node)
		{
			throw new FormatException("unknown value type ["+type+"].");
		}
		
		public Property(string name, double defaultValue, bool isStatic)
			:this(name,"double",defaultValue,isStatic,false){}
		public Property(string name, int defaultValue, bool isStatic)
			:this(name,"int",defaultValue,isStatic,false){}
		public Property(string name, string defaultValue, bool isStatic)
			:this(name,"string",defaultValue,isStatic,false){}
		public Property(string name, bool defaultValue, bool isStatic)
			:this(name,"bool",defaultValue,isStatic,false){}
		public Property(string name, double defaultValue, bool isStatic, bool Readonly)
			:this(name,"double",defaultValue,isStatic,Readonly){}
		public Property(string name, int defaultValue, bool isStatic, bool Readonly)
			:this(name,"int",defaultValue,isStatic,Readonly){}
		public Property(string name, string defaultValue, bool isStatic, bool Readonly)
			:this(name,"string",defaultValue,isStatic,Readonly){}
		public Property(string name, bool defaultValue, bool isStatic, bool Readonly)
			:this(name,"bool",defaultValue,isStatic,Readonly){}

		internal protected virtual XmlNode CreateXmlNode(XmlDocument doc)
		{
			XmlElement work = doc.CreateElement("property");
			XmlAttribute at_name = doc.CreateAttribute("name");
			at_name.Value = name;
			work.Attributes.Append(at_name);
			XmlAttribute at_type = doc.CreateAttribute("type");
			at_type.Value = type;
			work.Attributes.Append(at_type);
			if(isStatic)
			{
				XmlAttribute at_static = doc.CreateAttribute("static");
				at_static.Value = isStatic.ToString();
				work.Attributes.Append(at_static);
			}
			XmlText text = doc.CreateTextNode(_value.ToString());
			work.AppendChild(text);
			return work;
		}

		#region type safe value properties
		public int intValue
		{
			get
			{
				try
				{
					return (int)_value;
				}
				catch(Exception e)
				{
					throw new InvalidOperationException("property ["+name+"] is not integer.",e);
				}
			}
			set
			{
				if( type.ToString().Equals("int"))
					SetValue(value);
				else
					throw new InvalidOperationException("property ["+name+"] is not integer.");
			}
		}
		public double doubleValue
		{
			get
			{
				try
				{
					return (double)_value;
				}
				catch(Exception e)
				{
					throw new InvalidOperationException("property ["+name+"] is not double.",e);
				}
			}
			set
			{
				if( type.ToString().Equals("double"))
					SetValue(value);
				else
					throw new InvalidOperationException("property ["+name+"] is not double.");
			}
		}
		public bool boolValue
		{
			get
			{
				try
				{
					return (bool)_value;
				}
				catch(Exception e)
				{
					throw new InvalidOperationException("property ["+name+"] is not boolean.",e);
				}
			}
			set
			{
				if( type.ToString().Equals("bool") )
					SetValue(value);
				else
					throw new InvalidOperationException("property ["+name+"] is not boolean.");
			}
		}
		public string stringValue
		{
			get
			{
				try
				{
					return (string)_value;
				}
				catch(Exception e)
				{
					throw new InvalidOperationException("property ["+name+"] is not string.",e);
				}
			}
			set
			{
				if( type.ToString().Equals("bool") )
					SetValue(value);
				else
					throw new InvalidOperationException("property ["+name+"] is not string.");
			}
		}
		#endregion
		public override string ToString()
		{
			return _value.ToString();
		}

		protected void SetValue(object newObj)
		{
			if( isReadonly )
				throw new InvalidOperationException("This property is readonly");
			object old = _value;
			_value = newObj;
			FireEvent(this,old);
		}

		protected void FireEvent(Property sender, object old)
		{
			if(PropertyChanged!=null)
				PropertyChanged(sender,old);
			if(parent!=null)
				parent.FireEvent(sender,old);
		}
	}
}
