using System;
using System.Windows.Forms;
using nft.framework;
using nft.framework.plugin;

namespace nft.util
{
	/// <summary>
	/// ContribListControlHelper ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class ContribListControlHelper
	{
		private ContribListControlHelper(){}

		public static int Set( ComboBox combo, string ctbType, bool hideDisabled, IListContribFilter filter )
		{
			Array arr = GetArray( ctbType, hideDisabled );
			if( arr == null ) return -1;
			int n = 0;
			foreach( Contribution c in arr )
				if(filter.IsValid(c))
				{
					combo.Items.Add( new AbstractItem(c) );
					n++;
				}
			return n;
		}

		public static int Set( ComboBox combo, string ctbType, bool hideDisabled )
		{
			return Set( combo, ctbType, hideDisabled, defaultFilter );
		}


		public static Contribution GetContribution( ComboBox combo, int index )
		{
			if( index<0 || combo.Items.Count<index ) return null;
			AbstractItem item = combo.Items[index] as AbstractItem;
			if( item != null )
				return PluginManager.theInstance.GetContribution(item.ID);
			else
				return null;
		}


		private static Array GetArray( string ctbType, bool hideDisabled )
		{
			PluginManager pm = PluginManager.theInstance;
			Type t = pm.GetDefinedType(ctbType);
			if( t == null ) return null;
			return pm.ListContributions(t,hideDisabled);
		}

		#region Default Filter
		static private IListContribFilter defaultFilter = new DefaultFilter();

		class DefaultFilter : IListContribFilter
		{
			#region IListContribFilter ÉÅÉìÉo
			public bool IsValid(Contribution contrib)
			{
				return true;
			}
			#endregion
		}

		#endregion
	}

	public interface IListContribFilter
	{
		bool IsValid( Contribution contrib );									 
	}

		sealed class AbstractItem
		{
			public readonly string Name;
			public readonly string ID;
			public AbstractItem( IHasNameAndID entity )
			{
				this.Name = entity.Name;
				this.ID = entity.ID;
			}
			public override string ToString()
			{
				return Name;
			}
		}
	}
