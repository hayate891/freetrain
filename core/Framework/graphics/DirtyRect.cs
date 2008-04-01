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
using System.Drawing;

namespace FreeTrain.Framework.Graphics
{
    /// <summary>
    /// Maintain a set of dirty rects.
    /// </summary>
    [Serializable]
    public class DirtyRect
    {
        private Rectangle _rect;
        private bool _isEmpty = true;

        /// <summary>
        /// 
        /// </summary>
        public bool isEmpty { get { return _isEmpty; } }
        /// <summary>
        /// 
        /// </summary>
        public Rectangle rect { get { return _rect; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        public void add(Rectangle r)
        {
            Debug.Assert(r.Left >= 0);

            // this assertion is incorrect. a higher voxel on the top border can yield
            // a rectangle with top<0.
            // Debug.Assert( r.Top >=0 );

            if (_isEmpty)
            {
                _rect = r;
                _isEmpty = false;
            }
            else
            {
                _rect = Rectangle.Union(_rect, r);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public void add(int x, int y, int w, int h)
        {
            add(new Rectangle(x, y, w, h));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public void add(Point pt, int w, int h)
        {
            add(pt.X, pt.Y, w, h);
        }
        /// <summary>
        /// 
        /// </summary>
        public void clear()
        {
            _isEmpty = true;
        }
    }

    /*
        /// <summary>
        /// Maintain a set of dirty rects.
        /// 
        /// The algorithm is taken from "Yaneurao-SDK"
        /// </summary>
        public class DirtyRect
        {
            /// <summary> Number of rects to keep. </summary>
            private const int MAX = 4;

            /// <summary> Number of rects in the buffer. </summary>
            private int n;

            /// <summary> Dirty rects. </summary>
            private readonly Rectangle[] rects = new Rectangle[MAX];

            private readonly int[] areas = new int[MAX];

            public DirtyRect() {
                clear();
            }

            public int size { get { return n; } }

            public Rectangle getRect( int idx ) { return rects[idx]; }



            public void clear() {
                n=0;
            }

            /// <summary>
            /// Adds a new dirty rect.
            /// </summary>
            public void add( int x1, int y1, int x2, int y2 ) {
                rects[n] = new Rectangle(x1,y1,x2,y2);
                areas[n] = (x2-x1)*(y2-y1);
                if( (++n)!=MAX )	return;

                // compact the existing rects
                // find a pair of rects that minimizes the increase of area
                Rectangle minRect = new Rectangle(0,0,0,0);
                int minDelta=int.MaxValue,minArea=0,I=0,J=0;

                for( int i=MAX-2; i>=0; i-- ) {
                    for( int j=MAX-1; j>i; j-- ) {
                        // try merging i-th and j-th rects
                        Rectangle newRect = Rectangle.Union( rects[i], rects[j] );
                        int a = newRect.Height * newRect.Width;
                        int delta = a-(areas[i]+areas[j]);	// increase in the area
                        if( delta < minDelta ) {
                            minArea = a;
                            minRect = newRect;
                            I=i; J=j;
                        }
                    }
                }

                // update rects
                areas[I] = minArea;
                rects[I] = minRect;

                // delete the J-th rect
                if(J!=MAX-1) {
                    rects[J] = rects[MAX-1];
                    areas[J] = areas[MAX-1];
                }
                n--;
            }

            /// <summary>
            /// Fully compact the rects.
            /// </summary>
            public void refresh() {
                for( int i=0; i<n-1; i++ ) {
                    for( int j=i+1; j<n;  ) {
                        Rectangle u = Rectangle.Union( rects[i], rects[j] );
                        int a = u.Height*u.Width;
                        int delta = a-(areas[i]+areas[j]);
                        if( delta<0 ) {
                            areas[i] = a;
                            rects[i] = u;
                            if(j!=n-1) {
                                rects[j] = rects[n-1];
                                areas[j] = areas[n-1];
                            }
                            n--;
                            // i-th rect is updated, so try the same j again.
                        } else {
                            j++;
                        }
                    }
                }
            }
        }
    */

}
