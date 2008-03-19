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
using System.Collections;
using System.Drawing;
using System.IO;
using System.Net;
//using org.kohsuke.directaudio;
using SDL.net;
using SdlDotNet.Audio;
using FreeTrain.Util;
using FreeTrain.Framework.Graphics;
using FreeTrain.world;

namespace FreeTrain.Framework
{
    /// <summary>
    /// Simplified resource manager.
    /// </summary>
    public abstract class ResourceUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string findSystemResource(string name)
        {
            string path;

            path = Path.Combine(Core.installationDirectory, Path.Combine("res", name));
            if (File.Exists(path)) return path;

            path = Path.Combine(Core.installationDirectory, Path.Combine("..", Path.Combine("..", Path.Combine("core", Path.Combine("res", name)))));
            if (File.Exists(path)) return path;

            throw new FileNotFoundException("system resource: " + name);
        }

        //		private static WebResponse getStream( Uri uri ) {
        //			return WebRequest.Create(uri).GetResponse();
        //		}

        //		public static Bitmap loadBitmap( string location ) {
        //			using(WebResponse res = getStream(uri)) {
        //				return new Bitmap(res.GetResponseStream());
        //			}
        //		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Bitmap loadSystemBitmap(string name)
        {
            return new Bitmap(findSystemResource(name));
        }

        //		public static Icon loadIcon( Uri uri) {
        //			using(WebResponse res = getStream(uri)) {
        //				return new Icon(res.GetResponseStream());
        //			}
        //		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static SdlDotNet.Audio.Sound loadSystemSound(String name)
        {
            // can't read from stream
            return new SdlDotNet.Audio.Sound(findSystemResource(name));
        }

        // using URI is essentially dangerous as Segment only support file names.
        // I should limit it to file names only.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static SdlDotNet.Audio.Sound loadSound(Uri uri)
        {
            return new SdlDotNet.Audio.Sound(uri.LocalPath);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Picture loadSystemPicture(string name)
        {
            string id = "{8AD4EF28-CBEF-4C73-A8FF-5772B87EF005}:" + name;

            // check if it has already been loaded
            if (PictureManager.contains(id))
                return PictureManager.get(id);

            // otherwise load a new picture
            return new Picture(id, findSystemResource(name));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dayname"></param>
        /// <param name="nightname"></param>
        /// <returns></returns>
        public static Picture loadSystemPicture(string dayname, string nightname)
        {
            string id = "{8AD4EF28-CBEF-4C73-A8FF-5772B87EF005}:" + dayname;

            // check if it has already been loaded
            if (PictureManager.contains(id))
                return PictureManager.get(id);

            // otherwise load a new picture
            return new Picture(id, findSystemResource(dayname), findSystemResource(nightname));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Surface loadTimeIndependentSystemSurface(string name)
        {
            //using(Bitmap bmp=loadSystemBitmap(name))
            //	return directDraw.createSprite(bmp);
            return new Surface(findSystemResource(name));
        }



        /// <summary>
        /// DirectDraw instance for loading surface objects.
        /// </summary>
        //public static readonly DirectDraw directDraw = new DirectDraw();

        private static Picture emptyChips = loadSystemPicture("EmptyChip.bmp", "EmptyChip_n.bmp");
        private static Picture cursorChips = loadSystemPicture("cursorChip.bmp", "cursorChip.bmp");

        /// <summary>
        /// 
        /// </summary>
        public static Sprite emptyChip
        {
            get
            {
                return groundChips[0];
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        public static Sprite getGroundChip(World w)
        {
            if (w.clock.season != Season.Winter)
                return groundChips[0];
            else
                return groundChips[1];
        }

        private static Sprite[] groundChips = new Sprite[]{
			new SimpleSprite(emptyChips,new Point(0,0),new Point( 0,0),new Size(32,16)),
			new SimpleSprite(emptyChips,new Point(0,0),new Point(32,0),new Size(32,16))
		};
        /// <summary>
        /// 
        /// </summary>
        public static Sprite removerChip =
            new SimpleSprite(cursorChips, new Point(0, 0), new Point(0, 0), new Size(32, 16));
        /// <summary>
        /// 
        /// </summary>
        public static Sprite underWaterChip =
            new SimpleSprite(emptyChips, new Point(0, 0), new Point(64, 0), new Size(32, 16));
        /// <summary>
        /// 
        /// </summary>
        public static Sprite underGroundChip =
            new SimpleSprite(emptyChips, new Point(0, 0), new Point(96, 0), new Size(32, 16));
    }
}
