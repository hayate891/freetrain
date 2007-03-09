using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Xml;
using freetrain.util;
using freetrain.contributions.population;
using freetrain.framework.plugin;
using freetrain.framework.graphics;
using freetrain.world;
using freetrain.world.structs;
using org.kohsuke.directdraw;

namespace freetrain.contributions.common
{
	/// <summary>
	/// Generic structure contribution.
	/// 
	/// Structure is an object that occupies a cubic area in the World,
	/// has sprites to draw it.
	/// </summary>
	[Serializable]
	public abstract class StructureContribution : EntityBuilderContribution
	{
		/// <summary>
		/// Parses a structure contribution from a DOM node.
		/// </summary>
		/// <exception cref="XmlException">If the parsing fails</exception>
		protected StructureContribution( XmlElement e ) : base(e) {
			XmlNode nameNode = e.SelectSingleNode("name");
			XmlNode groupNode = e.SelectSingleNode("group");

			_name = (nameNode!=null)? nameNode.InnerText : (groupNode!=null ? groupNode.InnerText : null );
			if(name==null)
				throw new FormatException("<name> and <group> are both missing");

			string groupName = (groupNode!=null)? groupNode.InnerText : name;
			group = getGroup(groupName);
			group.add(this);

			XmlElement pop = (XmlElement)e.SelectSingleNode("population");
			if(pop!=null)
				population = new PersistentPopulation(
					Population.load(pop),
					new PopulationReferenceImpl(this.id));			
		}

		public readonly StructureGroup group;

		/// <summary>
		/// Implemented by the derived class and
		/// used to determine which group this structure should go.
		/// </summary>
		protected abstract StructureGroup getGroup( string name );

		/// <summary> Population of this structure, or null if this structure is not populated. </summary>
		public readonly Population population;

		/// <summary>Name of this structure.</summary>
		private readonly string _name;		// TODO: should be moved up



		public override string name { get { return _name; } }
		



	
		/// <summary>
		/// Used to resolve references to the population object.
		/// </summary>
		[Serializable]
		internal sealed class PopulationReferenceImpl : IObjectReference {
			internal PopulationReferenceImpl( string id ) { this.id=id; }
			private string id;
			public object GetRealObject(StreamingContext context) {
				return ((StructureContribution)PluginManager.theInstance.getContribution(id)).population;
			}
		}
	}
}
