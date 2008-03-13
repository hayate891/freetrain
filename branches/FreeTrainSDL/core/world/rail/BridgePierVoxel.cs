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
using System.Drawing;
using SDL.net;
using freetrain.framework;
using freetrain.framework.graphics;
using freetrain.util;

namespace freetrain.world.rail
{
    /// <summary>
    /// Voxel that is used to support raised railroad.
    /// 
    /// A derived class needs to provide the surface object.
    /// </summary>
    [Serializable]
    public abstract class BridgePierVoxel : AbstractVoxelImpl//, Entity
    {
        // TODO: not sure if this class should implement Entity
        /// <summary>
        /// 
        /// </summary>
        public override bool transparent { get { return true; } }

        //		public static void electBridgeSupport( Location loc ) {
        //			electBridgeSupport( loc, typeof(DefaultImpl) );
        //		}
        //
        //		public static void electBridgeSupport( Location loc, Type bridgeType ) {
        //			electBridgeSupport( loc, bridgeType, bridgeType );
        //		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="owner"></param>
        public static void electBridgeSupport(Location loc, Entity owner)
        {
            electBridgeSupport(loc, typeof(DefaultImpl), owner);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="bridgeType"></param>
        /// <param name="owner"></param>
        public static void electBridgeSupport(Location loc, Type bridgeType, Entity owner)
        {
            electBridgeSupport(loc, bridgeType, bridgeType, owner);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="topBridgeType"></param>
        /// <param name="otherBridgeType"></param>
        /// <param name="owner"></param>
        public static void electBridgeSupport(Location loc, Type topBridgeType, Type otherBridgeType, Entity owner)
        {
            // check if a support is buildable
            // TODO: start from the surface level
            for (int z = 0; z < loc.z; z++)
                if (World.world[loc.x, loc.y, z] != null)
                    return;

            // if we can, do it
            for (int z = World.world.getGroundLevel(loc); z < loc.z; z++)
            {
                Activator.CreateInstance(
                    (z == loc.z - 1) ? topBridgeType : otherBridgeType,
                    new object[] { loc.x, loc.y, z, owner });
            }
        }

        /// <summary>
        /// Elects a bridge support from the surface level to the given location,
        /// if it can be done.
        /// </summary>
        /// <param name="loc">The location of the elevated RR.</param>
        //		public static void electBridgeSupport( Location loc, Type topBridgeType, Type otherBridgeType ) {
        //			
        //			// check if a support is buildable
        //			// TODO: start from the surface level
        //			for( int z=0; z<loc.z; z++ )
        //				if(World.world[loc.x,loc.y,z]!=null)
        //					return;
        //			
        //			// if we can, do it
        //			for( int z=World.world.getGroundLevel(loc); z<loc.z; z++ ) {
        //				Activator.CreateInstance(
        //					(z==loc.z-1)?topBridgeType:otherBridgeType,
        //					new object[]{ loc.x, loc.y, z });
        //			}
        //		}

        /// <summary>
        /// Tears down a bridge support if any.
        /// </summary>
        public static void teardownBridgeSupport(Location loc, Entity owner)
        {
            for (int z = 0; z < loc.z; z++)
            {
                BridgePierVoxel v = World.world[loc.x, loc.y, z] as BridgePierVoxel;
                if (v != null)
                    World.world.remove(loc.x, loc.y, z);
            }
        }

        //		protected BridgePierVoxel( int x, int y, int z ) : this(x,y,z,null) {
        //		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="owner"></param>
        protected BridgePierVoxel(int x, int y, int z, Entity owner)
            : base(x, y, z)
        {
            this.owner = owner;
        }
        /// <summary>
        /// 
        /// </summary>
        protected Entity owner;
        /// <summary>
        /// 
        /// </summary>
        public override Entity entity { get { return owner; } }
        #region Entity implementation
        /*
		public bool isSilentlyReclaimable { get { return false; } }
		public bool isOwned { get { return true; } }

		public void remove() {
			Location loc = this.location;
			World.world.remove(this);
			if(onEntityRemoved!=null)	onEntityRemoved(this,null);
		}

		// TODO: value?
		public int entityValue { get { return 0; } }

		public event EventHandler onEntityRemoved;
		*/
        #endregion


        /// <summary>
        /// 
        /// </summary>
        protected abstract Sprite sprite { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="display"></param>
        /// <param name="pt"></param>
        /// <param name="heightCutDiff"></param>
        public override void draw(DrawContext display, Point pt, int heightCutDiff)
        {
            // draw the pier in alpha if in the height cut mode
            if (heightCutDiff == 0) sprite.drawAlpha(display.surface, pt);
            else sprite.draw(display.surface, pt);
        }



        private static readonly Picture theImage = ResourceUtil.loadSystemPicture("BridgePier.bmp");
        /// <summary>
        /// 
        /// </summary>
        public static readonly Sprite defaultSprite;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Sprite slopeNESprite;
        static BridgePierVoxel()
        {
            defaultSprite = new SimpleSprite(theImage, new Point(0, 16), new Point(0, 0), new Size(32, 32));
            slopeNESprite = new SimpleSprite(theImage, new Point(0, 16), new Point(32, 0), new Size(32, 32));
        }



        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        public class DefaultImpl : BridgePierVoxel
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="z"></param>
            /// <param name="owner"></param>
            public DefaultImpl(int x, int y, int z, Entity owner) : base(x, y, z, owner) { }
            /// <summary>
            /// 
            /// </summary>
            protected override Sprite sprite { get { return defaultSprite; } }
        }
        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        public class SlopeNEImpl : BridgePierVoxel
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="z"></param>
            /// <param name="owner"></param>
            public SlopeNEImpl(int x, int y, int z, Entity owner) : base(x, y, z, owner) { }
            /// <summary>
            /// 
            /// </summary>
            protected override Sprite sprite { get { return slopeNESprite; } }
        }
    }
}
