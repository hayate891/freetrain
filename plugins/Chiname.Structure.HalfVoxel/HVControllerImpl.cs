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
using System.Windows.Forms;
using FreeTrain.Framework;
using FreeTrain.Framework.Graphics;
using FreeTrain.Views;
using FreeTrain.Views.Map;
using FreeTrain.Controllers;


namespace FreeTrain.World.Structs.HalfVoxelStructure
{
    internal delegate void createCallback();
    /// <summary>
    /// ModalController that selects the half voxel region
    /// and do something with it.
    /// </summary>
    public class HVControllerImpl : IModalController, IMapOverlay
    {

        /// <summary>Constant</summary>
        [CLSCompliant(false)]
        protected static readonly Location UNPLACED = World.Location.Unplaced;
        static private readonly string cur_id = "{HALF-VOXEL-STRUCTURE-CURSOR-IMAGE}";
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        static protected ISprite[] cursors = new ISprite[]{
			createCursorSprite(0,0), 
			createCursorSprite(32, 0), createCursorSprite(64, 0),
			createCursorSprite(96, 0), createCursorSprite(128, 0),
			createCursorSprite(32,16), createCursorSprite(64,16), 
			createCursorSprite(96,16), createCursorSprite(128,16)
		};

        static private ISprite createCursorSprite(int x, int y)
        {
            return new SimpleSprite(PictureManager.get(cur_id), new Point(0, 0), new Point(x, y), new Size(32, 16));
        }

        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        protected Location anchor = UNPLACED;
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        protected Location currentPos = UNPLACED;
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        protected Direction curSide;

        internal createCallback onCreated = null;

        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        protected readonly IControllerSite site;
        private readonly HalfVoxelContribution contrib;
        private readonly bool remover;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_contrib"></param>
        /// <param name="_site"></param>
        /// <param name="_remove"></param>
        [CLSCompliant(false)]
        public HVControllerImpl(HalfVoxelContribution _contrib, IControllerSite _site, bool _remove)
        {
            this.contrib = _contrib;
            this.site = _site;
            this.remover = _remove;

        }

        #region convenience methods
        //		/// <summary>
        //		/// North-west corner of the selected region.
        //		/// </summary>
        //		protected Location location1 
        //		{
        //			get 
        //			{
        //				Debug.Assert( anchor!=UNPLACED );
        //				return new Location(
        //					Math.Min( currentPos.x, anchor.x ),
        //					Math.Min( currentPos.y, anchor.y ),
        //					anchor.z );
        //			}
        //		}
        //
        //		/// <summary>
        //		/// South-east corner of the selected region.
        //		/// </summary>
        //		protected Location location2 
        //		{
        //			get 
        //			{
        //				Debug.Assert( anchor!=UNPLACED );
        //				return new Location(
        //					Math.Max( currentPos.x, anchor.x ),
        //					Math.Max( currentPos.y, anchor.y ),
        //					anchor.z );
        //			}
        //		}

        /// <summary>
        /// 
        /// </summary>
        protected PlaceSide currentSide
        {
            get
            {
                HalfDividedVoxel v = WorldDefinition.World[anchor] as HalfDividedVoxel;
                if (v != null)
                {
                    ContributionReference[] refs = v.getReferences();
                    if (remover)
                    {
                        // On remover mode, if the voxel has only one side occupied, select it.
                        if (refs.Length == 1)
                            return refs[0].placeSide;
                        // Otherwise, select side in order to the cursor (in following code).
                    }
                    else
                    {
                        // On builder mode, there should be only one side remains empty.
                        return (PlaceSide)(1 - (int)refs[0].placeSide);
                    }
                }

                // Select side according to the mouse cursor position.
                PlaceSide side = PlaceSide.Fore;
                if (front == Direction.NORTH || front == Direction.SOUTH)
                {
                    if (curSide == Direction.NORTHEAST || curSide == Direction.SOUTHEAST)
                        side = PlaceSide.Back;
                }
                else
                {
                    if (curSide == Direction.NORTHEAST || curSide == Direction.NORTHWEST)
                        side = PlaceSide.Back;
                }
                return side;
            }
        }

        private Direction front
        {
            get
            {
                // Check if the front direction can be determined.
                if (currentPos == UNPLACED || currentPos.Equals(anchor))
                    throw new Exception("invalid call");

                HalfDividedVoxel v = WorldDefinition.World[anchor] as HalfDividedVoxel;
                // There is no restriction for empty voxel.
                if (v == null)
                    return anchor.getDirectionTo(currentPos);

                //The X/Y alignment should be parallel to another side.
                ContributionReference[] refs = v.getReferences();
                if (refs[0].frontface.isParallelToY)
                {
                    int dy = currentPos.y - anchor.y;
                    if (dy < 0)
                        return Direction.NORTH;
                    else
                        return Direction.SOUTH;
                }
                else
                {
                    int dx = currentPos.x - anchor.x;
                    if (dx < 0)
                        return Direction.WEST;
                    else
                        return Direction.EAST;
                }
            }
        }

        #endregion

        #region internal logic

        /// <summary>
        /// Called when the selection is completed.
        /// </summary>
        [CLSCompliant(false)]
        protected void onVoxelSelected(Location loc, Direction front, PlaceSide side)
        {
            if (remover)
            {
                contrib.destroy(loc, front, side);
            }
            else
            {
                contrib.create(loc, front, side);
                onCreated();
            }
        }

        /// <summary>
        /// Called when the selection is changed.
        /// </summary>
        [CLSCompliant(false)]
        protected void onVoxelUpdated(Location loc, Direction front, PlaceSide side)
        {
        }

        /// <summary>
        /// Called when the user wants to cancel the modal controller.
        /// </summary>
        protected void onCanceled()
        {
            site.Close();
        }

        /// <summary>
        /// Aligns the given location to the anchor so that
        /// the location will become straight.
        /// </summary>
        private Location align(Location loc)
        {
            loc.z = anchor.z;
            return loc.align4To(anchor);
        }

        /// <summary>
        /// Get direction from anchor to which mouse coursor pointed.
        /// Used as calcuration of front face direction.
        /// </summary>
        /// <param name="ab">Cursor position</param>
        /// <returns></returns>
        private Direction getSide(Point ab)
        {
            Point p = WorldDefinition.World.fromXYZToAB(currentPos);
            int x = ab.X - p.X - 16;
            int y = (ab.Y - p.Y - 8) * 2;

            Direction d;
            if (y < 0)
            {
                if (x > 0 && x > (-y))
                    d = Direction.SOUTHEAST;
                else if (x < 0 && (-x) > (-y))
                    d = Direction.NORTHWEST;
                else
                    d = Direction.NORTHEAST;
            }
            else
            {
                if (x > 0 && x > y)
                    d = Direction.SOUTHEAST;
                else if (x < 0 && (-x) > y)
                    d = Direction.NORTHWEST;
                else
                    d = Direction.SOUTHWEST;
            }
            return d;
        }

        //		private void modeChanged( object sender, EventArgs e ) 
        //		{
        //			anchor = UNPLACED;
        //		}

        //		private bool inBetween( Location loc, Location lhs, Location rhs ) 
        //		{
        //			if( !loc.inBetween(lhs,rhs) )	return false;
        //
        //			if(( lhs.x==rhs.x && rhs.x==loc.x )
        //				|| ( lhs.y==rhs.y && rhs.y==loc.y ) )	return true;
        //
        //			if( Math.Abs(loc.x-lhs.x)==Math.Abs(loc.y-lhs.y) )	return true;
        //
        //			return false;
        //		}

        #endregion

        #region public methods

        /// <summary> LocationDisambiguator implementation </summary>
        [CLSCompliant(false)]
        public bool isSelectable(Location loc)
        {
            if (anchor != UNPLACED)
                return loc.z == anchor.z;
            else
                // lands can be placed only on the ground
                return GroundDisambiguator.theInstance.IsSelectable(loc);
        }
        /// <summary>
        /// 
        /// </summary>
        public void close()
        {
            onCanceled();
        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        public string Name { get { return site.Name; } }

        // can be overrided by a derived class to return another object.
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public IMapOverlay Overlay
        {
            get
            {
                // return this object if it implements MapOverlay by itself.
                return this as IMapOverlay;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public ILocationDisambiguator Disambiguator
        {
            get
            {
                // the 2nd selection must go to the same height as the anchor.
                if (anchor == UNPLACED) return GroundDisambiguator.theInstance;
                else return sameLevelDisambiguator;
            }
        }
        private ILocationDisambiguator sameLevelDisambiguator;

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnAttached()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void OnDetached()
        {
            // clear the remaining image
            if (currentPos != Location.Unplaced)
                WorldDefinition.World.OnVoxelUpdated(currentPos);
        }


        #endregion

        #region mouse handlers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        [CLSCompliant(false)]
        public void OnMouseMove(MapViewWindow view, Location loc, Point ab)
        {
            if (anchor != UNPLACED)
            {
                currentPos = align(loc);
                curSide = getSide(ab);
                //if( !currentPos.Equals(anchor) )
                //onVoxelUpdated(anchor,front,currentSide);

                WorldDefinition.World.OnVoxelUpdated(anchor);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        [CLSCompliant(false)]
        public void OnClick(MapViewWindow source, Location loc, Point ab)
        {

            if (anchor == UNPLACED)
            {
                if (remover)
                {
                    if (null == WorldDefinition.World[loc] as HalfDividedVoxel)
                        return;
                }
                else
                {
                    if (!HalfVoxelContribution.canBeBuilt(loc))
                        return;
                }
                anchor = loc;
                currentPos = loc;
                curSide = getSide(ab);
                sameLevelDisambiguator = new SameLevelDisambiguator(anchor.z);
            }
            else
            {
                if (!currentPos.Equals(anchor))
                    onVoxelSelected(anchor, front, currentSide);
                WorldDefinition.World.OnVoxelUpdated(anchor);
                anchor = UNPLACED;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="loc"></param>
        /// <param name="ab"></param>
        [CLSCompliant(false)]
        public void OnRightClick(MapViewWindow source, Location loc, Point ab)
        {
            if (anchor == UNPLACED)
                close();	// cancel
            else
            {
                WorldDefinition.World.OnVoxelUpdated(anchor);
                anchor = UNPLACED;
            }
        }

        #endregion

        #region drawing methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="surface"></param>
        [CLSCompliant(false)]
        public void DrawBefore(QuarterViewDrawer view, DrawContext surface) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="canvas"></param>
        /// <param name="loc"></param>
        /// <param name="pt"></param>
        [CLSCompliant(false)]
        public void DrawVoxel(QuarterViewDrawer view, DrawContext canvas, Location loc, Point pt)
        {
            if (loc != anchor) return;
            if (anchor.Equals(currentPos))
                cursors[0].Draw(canvas.Surface, pt);
            else
            {
                //HalfDividedVoxel v = World.world[loc] as HalfDividedVoxel;
                int n, m, l;
                n = remover ? 5 : 1;
                m = front.isParallelToX ? 0 : 1;
                l = (currentSide == PlaceSide.Back) ? 0 : 2;
                cursors[n + m + l].Draw(canvas.Surface, pt);
                if (!remover)
                {
                    contrib.getSprite(front, currentSide, contrib.currentColor).DrawAlpha(canvas.Surface, pt);
                    ISprite hls = contrib.getHighLightSprite(front, currentSide, contrib.currentHighlight);
                    if (hls != null) hls.DrawAlpha(canvas.Surface, pt);
                }
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="surface"></param>
        [CLSCompliant(false)]
        public void DrawAfter(QuarterViewDrawer view, DrawContext surface) { }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public void UpdatePreview()
        { }
    }
}
