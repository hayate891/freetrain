using System;
using System.Diagnostics;
using System.Drawing;
using org.kohsuke.directdraw;

namespace freetrain.world
{
	/// <summary>
	/// A block in the game world.
	/// 
	/// The voxel is the unit of the game world. The game world consists of a cube of
	/// voxels, and this is the base class of such voxels.
	/// </summary>
	[Serializable]
	public abstract class Voxel
	{
		public abstract Location location { get; }

		/// <summary>
		/// Draws this voxel
		/// </summary>
		/// <param name="heightCutDiff">
		/// heightCut - Z.
		///	0 if this voxel is located to the "edge" of the height cut.
		///	negative value if the view is not in the height cut mode.
		///	positive values if this voxel is located below the cut height
		///	(the value will be the difference between the height of
		///	this voxel and the cut height.)
		/// </param>
		public abstract void draw( DrawContext display, Point pt, int heightCutDiff );

		/// <summary>
		/// Processes a mouse click.
		/// </summary>
		/// <returns>true if a mouse click event is "consumed"</returns>
		public virtual bool onClick() { return false; }

		/// <summary>
		/// Query this voxel to return some "aspect" of it.
		/// 
		/// Aspect is usually a tear-off interface that allows
		/// voxels to be extended through compositions.
		/// 
		/// The queryInterface method of voxels should delegate to
		/// the queryInterface method of entities.
		/// </summary>
		/// <returns>null if the given aspect is not supported.</returns>
		public virtual object queryInterface( Type aspect ) { return null; }

		/// <summary>
		/// Calls immediately after the voxel is removed from the world.
		/// </summary>
		public virtual void onRemoved() {}
		// TODO: is this method necessary


		/// <summary>
		/// Short-cut to call the getLandPrice method of the World class.
		/// </summary>
		public int landPrice {
			get {
				return (int)World.world.landValue[location];
			}
		}

		/// <summary>
		/// Obtains a reference to the entity that includes this voxel.
		/// </summary>
		public abstract Entity entity { get; }

	}

	/// <summary>
	/// Partial implementation for most of the voxel.
	/// </summary>
	[Serializable]
	public abstract class AbstractVoxelImpl : Voxel
	{
		protected AbstractVoxelImpl( int x, int y, int z ) : this(new Location(x,y,z)) {
		}

		protected AbstractVoxelImpl( Location _loc) {
			this.loc=_loc;
			World.world[loc] = this;
		}

		private readonly Location loc;

		public override Location location { get { return loc; } }
	}


	/// <summary>
	/// Voxel can additionally implement this interface to
	/// control the painting of the ground surface.
	/// 
	/// The drawing routine queries this interface for voxels
	/// that are directly above and below the surface.
	/// </summary>
	public interface HoleVoxel
	{
		/// <summary>
		/// Returns false to prevent the ground surface to be drawn.
		/// </summary>
		/// <param name="above">
		/// True if the callee is located directly above the ground,
		/// false if directly below the ground.
		/// </param>
		bool drawGround( bool above );
	}
}
