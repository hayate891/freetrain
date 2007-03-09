using System;
using System.Collections;
using System.ComponentModel;

namespace freetrain.contributions.land
{
	/// <summary>
	/// Group of LandContribution.
	/// </summary>
	public class LandBuilderGroup : CollectionBase
	{
		public LandBuilderGroup( string _name ) {
			this.name = _name;
		}

		/// <summary> Name of this group. </summary>
		public string name;


		public void add( LandBuilderContribution sc ) {
			base.List.Add(sc);
		}
		public LandBuilderContribution get( int idx ) {
			return (LandBuilderContribution)base.List[idx];
		}
		public void remove( LandBuilderContribution sc ) {
			base.List.Remove(sc);
		}

		public override string ToString() { return name; }
	}

	/// <summary>
	/// Group of LandGroup.
	/// 
	/// This object implements IListSource to support data-binding.
	/// </summary>
	public class LandBuilderGroupGroup : IListSource
	{
		public LandBuilderGroupGroup() {}

		private readonly Hashtable core = new Hashtable();
		// used for data-binding
		private readonly ArrayList list = new ArrayList();

		public LandBuilderGroup this[ string name ] {
			get {
				LandBuilderGroup g = (LandBuilderGroup)core[name];
				if(g==null) {
					core[name] = g = new LandBuilderGroup(name);
					list.Add(g);
				}

				return g;
			}
		}

		public IEnumerator getEnumerator() {
			return core.Values.GetEnumerator();
		}

		public IList GetList() { return list; }
		public bool ContainsListCollection { get { return true; } }
	}
}
