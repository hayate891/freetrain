using System;
using System.Collections;

namespace nft.util
{
	/// <summary>
	/// 簡便なキーと値のセット
	/// </summary>
	public class ParamSet
	{
		protected Hashtable table = new Hashtable();

		public ParamSet()
		{
		}

		public string this[ string key ]
		{
			get { 
				object o = table[key];
				if(o!=null) return o.ToString();
				else return null;
			}
			set { table.Add(key,value); }
		}

		public string this[ string key, string defaultVal ]
		{
			get 
			{ 
				object o = table[key];
				if(o!=null) return o.ToString();
				else return defaultVal;
			}
		}

		public int this[ string key, int defaultVal ]
		{
			get 
			{ 
				object o = table[key];
				if(o!=null) return int.Parse(o.ToString());
				else return defaultVal;
			}
		}

		public bool this[ string key, bool defaultVal ]
		{
			get 
			{ 
				object o = table[key];
				if(o!=null) return bool.Parse(o.ToString());
				else return defaultVal;
			}
		}
		
		public void Set(string key, object val)	
		{
			if(val==null)
				Remove(key);
			else
				table.Add(key,val.ToString()); 
		}

		public void Remove(string key)
		{
			table.Remove(key);
		}
	}
}
