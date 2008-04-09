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
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

using Tao.Sdl;
using SdlDotNet.Audio;
using SdlDotNet.Core;
using SdlDotNet.Input;

using FreeTrain.Framework.Graphics;
using FreeTrain.World;
using FreeTrain.Controllers;
using FreeTrain.Controllers.Rail;
using FreeTrain.Controllers.Land;
using FreeTrain.Controllers.Road;
using FreeTrain.Controllers.Terrain;
using FreeTrain.Controllers.Structs;
using FreeTrain.Framework.Plugin;
using FreeTrain.Framework;
using FreeTrain.Views;
using FreeTrain.Util;

namespace FreeTrain
{
    class FreeTrainSDL : IDisposable
    {
        #region Private fields

        int width = 640;
        int height = 480;
        int mapXOffset = 12;
        int mapYOffset = 24;
        Splash splashscreen;
        System.Windows.Forms.Timer timer;
        SdlDotNet.Graphics.Surface screen;
        Sdl.SDL_VideoInfo videoInfo;
        Sdl.SDL_PixelFormat pixelFormat;
        int currentBpp = 16;
        IModalController controller = null;
        bool dragMode = false;
        Point dragStartMousePosition;
        Point dragStartScrollPosition;
        Point autoScrollPosition;
        QuarterViewDrawer qView = null;
        WorldDefinition world = null;
        IWeatherOverlay weatherOverlay;
        Sdl.SDL_Rect sourceRect;
        Sdl.SDL_Rect dst;
        short oldX;
        short oldY;
        bool lastMouseState;
        MainWindow mainWindowMDI;

        #endregion

        #region Private Methods

        private Point ScrollPosition
        {
            get
            {
                Point pt = autoScrollPosition;
                return new Point(pt.X + mapXOffset, pt.Y + mapYOffset);
            }
            set
            {
                autoScrollPosition = new Point(
                    Math.Max(value.X - mapXOffset, 0),
                    Math.Max(value.Y - mapYOffset, 0));
            }
        }

        private void UpdateMessage(string msg, float progress)
        {
            splashscreen.status.AppendText(msg);
            splashscreen.status.AppendText("\n");
        }

        private void MusicHasStopped()
        {
            Core.BgmManager.nextSong();
        }

        private void Quit(object sender, QuitEventArgs e)
        {
            SdlMixer.Mix_CloseAudio();

            timer.Stop();
            Events.QuitApplication();
        }

        private void Tick(object sender, TickEventArgs e)
        {
            if (qView != null)
            {
                controller = MainWindow.mainWindow.CurrentController;
                qView.UpdateScreen();
                if (WorldDefinition.World.Satellite == null ||
                    WorldDefinition.World.Satellite.surface.w != 150 ||
                    WorldDefinition.World.Satellite.surface.h != 150)
                {
                    WorldDefinition.World.Satellite = new Surface(150, 150, 32);
                    WorldDefinition.World.Satellite.SourceColorKey = Color.Magenta;
                    WorldDefinition.World.Satellite.Fill(Color.FromArgb(222, 195, 132));
                }
            }
            Application.DoEvents();
        }

        private void KeyDown(object sender, KeyboardEventArgs e)
        {
            if (e.Key == Key.Escape || e.Key == Key.Q)
            {
                Events.QuitApplication();
            }
        }

        private void Resize(object sender, SdlDotNet.Graphics.VideoResizeEventArgs e)
        {
            this.width = e.Width;
            this.height = e.Height;
            screen = SdlDotNet.Graphics.Video.SetVideoMode(width, height, currentBpp, true);
            screen.SourceColorKey = Color.Magenta;
            weatherOverlay.setSize(new Size(width, height));
            qView.Size = new Size(e.Width, e.Height);
        }

        private void MusicFinished(object sender, MusicFinishedEventArgs e)
        {
            Core.BgmManager.nextSong();
        }

        private void MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            lastMouseState = e.ButtonPressed;
            if (e.Button == MouseButton.SecondaryButton && !dragMode)
            {
                dragMode = true;
                dragStartMousePosition = new Point(e.X, e.Y);
                dragStartScrollPosition = ScrollPosition;
                Mouse.ShowCursor = true;
            }
        }

        private void MouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (dragMode)
            {
                dragMode = false;
                Mouse.MousePosition = new Point(oldX, oldY);
                Mouse.ShowCursor = true;
            }

            if (lastMouseState)
            {
                if (controller != null)
                {
                    Point ab = qView.fromClientToAB(e.X + ScrollPosition.X, e.Y + ScrollPosition.Y);
                    Location xyz = qView.fromABToXYZ(ab, controller);

                    if (e.Button == MouseButton.PrimaryButton)
                    {
                        controller.OnClick(null, xyz, ab);
                    }
                }
            }
            lastMouseState = e.ButtonPressed;
        }

        private void MouseMotion(object sender, MouseMotionEventArgs e)
        {
            if (dragMode)
            {
                ScrollByDrag(new Point(e.X, e.Y));
            }
            else
            {
                if (controller != null)
                {
                    Point ab = qView.fromClientToAB(e.X + ScrollPosition.X, e.Y + ScrollPosition.Y);
                    controller.OnMouseMove(null, qView.fromABToXYZ(ab, controller), ab);
                }
                oldX = e.X;
                oldY = e.Y;
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (qView != null)
            {
                WorldDefinition.World.Clock.tick();
                WorldDefinition.World.Clock.tick();

                sourceRect = new Sdl.SDL_Rect((short)ScrollPosition.X, (short)ScrollPosition.Y, (short)width, (short)height);
                dst = new Sdl.SDL_Rect(0, 0, (short)width, (short)height);
                Tao.Sdl.Sdl.SDL_BlitSurface(qView.OffscreenBuffer.SurfacePtr(), ref sourceRect, screen.Handle, ref dst);
            }

            FinalDraw();
        }

        private void FinalDraw()
        {
            if (mainWindowMDI != null && WorldDefinition.World != null)
            {
                mainWindowMDI.toolStripStatusLabel.Text = WorldDefinition.World.Clock.displayString;
            }
            screen.Update();
        }

        private bool ScrollByDrag(Point curMousePos)
        {
            int dragAccel = 1;
            Point pt = this.dragStartScrollPosition;
            pt.X += (curMousePos.X - dragStartMousePosition.X) * dragAccel;
            pt.Y += (curMousePos.Y - dragStartMousePosition.Y) * dragAccel;
            ScrollPosition = pt;
            qView.Origin = ScrollPosition;
            return true;
        }

        private int Distance(Point a, Point b)
        {
            return (int)Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }
        #endregion

        #region Main

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            FreeTrainSDL freeTrainSDL = new FreeTrainSDL();
            freeTrainSDL.Go();
        }

        private void Go()
        {
            splashscreen = new Splash();
            splashscreen.Show();
            splashscreen.BringToFront();
            splashscreen.Refresh();
            Core.Init(null, null, null, new ProgressHandler(UpdateMessage), true);
            world = new WorldDefinition(new Distance(150, 150, 7), 3);
            WorldDefinition.World = world;
            mainWindowMDI = new MainWindow();
            timer = new System.Windows.Forms.Timer();
            Events.KeyboardDown += new EventHandler<KeyboardEventArgs>(this.KeyDown);
            Events.MouseButtonDown +=
                new EventHandler<MouseButtonEventArgs>(this.MouseButtonDown);
            Events.MouseButtonUp +=
                new EventHandler<MouseButtonEventArgs>(this.MouseButtonUp);
            Events.Quit += new EventHandler<QuitEventArgs>(this.Quit);
            Events.Tick += new EventHandler<TickEventArgs>(this.Tick);
            Events.VideoResize += new EventHandler<SdlDotNet.Graphics.VideoResizeEventArgs>(this.Resize);
            Events.MusicFinished +=
                new EventHandler<MusicFinishedEventArgs>(this.MusicFinished);
            Events.MouseMotion += new EventHandler<MouseMotionEventArgs>(this.MouseMotion);
            dragStartMousePosition = new Point(0, 0);
            dragStartScrollPosition = new Point(0, 0);
            ScrollPosition = new Point(0, 0);
            this.width = Properties.Settings.Default.RenderWindowWidth;
            this.height = Properties.Settings.Default.RenderWindowHeight;

            try
            {
                SdlDotNet.Graphics.Video.WindowIcon();
                SdlDotNet.Graphics.Video.WindowCaption = Translation.GetString("MAIN_WINDOW_TITLE");
                SdlDotNet.Graphics.Video.Initialize();
                splashscreen.BringToFront();
            }
            catch
            {
                MessageBox.Show(Translation.GetString("SDL_NOT_FOUND"));
                return;
            }

            Mixer.Initialize();

            SdlMixer.MusicFinishedDelegate musicStopped = new SdlMixer.MusicFinishedDelegate(MusicHasStopped);
            SdlMixer.Mix_HookMusicFinished(musicStopped);


            screen = SdlDotNet.Graphics.Video.SetVideoMode(width, height, currentBpp, true);
            screen.SourceColorKey = Color.Magenta;
            IntPtr videoInfoPointer = Sdl.SDL_GetVideoInfo();

            if (videoInfoPointer != IntPtr.Zero)
            {
                videoInfo = (Sdl.SDL_VideoInfo)Marshal.PtrToStructure(videoInfoPointer, typeof(Sdl.SDL_VideoInfo));
                pixelFormat = (Sdl.SDL_PixelFormat)Marshal.PtrToStructure(videoInfo.vfmt, typeof(Sdl.SDL_PixelFormat));
            }
            splashscreen.status.AppendText("FreeTrain SDL Starting...");
            splashscreen.status.AppendText("\n");
            splashscreen.Refresh();

            FinalDraw();

            splashscreen.status.AppendText("Loading plugins...");
            splashscreen.status.AppendText("\n");
            splashscreen.Refresh();
            FinalDraw();
            

            weatherOverlay = NullWeatherOverlay.theInstance;
            FinalDraw();
            qView = new QuarterViewDrawer(world, new Rectangle(0, 0, world.Size.x * 32 - 16, (world.Size.y - 2 * world.Size.z - 1) * 8));
            qView.OffscreenBuffer = new Surface(world.Size.x * 32 - 16, (world.Size.y - 2 * world.Size.z - 1) * 8, screen.CreateCompatibleSurface().Pixels);
            qView.OffscreenBuffer.SourceColorKey = Color.Magenta;
            qView.RecreateDrawBuffer(new Size(width, height), true);
            splashscreen.status.AppendText("Creating Map...");
            splashscreen.status.AppendText("\n");
            splashscreen.Refresh();
            FinalDraw();

            qView.Draw(new Rectangle(0, 0, world.Size.x * 32 - 16, (world.Size.y - 2 * world.Size.z - 1) * 8), null);
            timer.Tick += new EventHandler(TimerTick);
            timer.Interval = 33;
            timer.Enabled = true;
            timer.Start();

            mainWindowMDI.Show();
            splashscreen.BringToFront();
            splashscreen.Close();

            Events.Run();
        }

        #endregion

        #region IDisposable Members

        private bool disposed;

        /// <summary>
        /// Destroy object
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destroy object
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>
        /// Destroy object
        /// </summary>
        ~FreeTrainSDL()
        {
            Dispose(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.weatherOverlay != null)
                    {
                        this.weatherOverlay.Dispose();
                        this.weatherOverlay = null;
                    }
                }
                this.disposed = true;
            }
        }

        #endregion
    }
}
