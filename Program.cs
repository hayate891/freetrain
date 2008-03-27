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
//using SdlDotNet.Graphics;
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
        System.Windows.Forms.Timer timer;
        SdlDotNet.Graphics.Surface screen;
        SDLGUI gui;
        Sdl.SDL_VideoInfo videoInfo;
        Sdl.SDL_PixelFormat pixelFormat;
        int currentBpp = 16;
        ModalController controller = null;
        bool dragMode = false;
        Point dragStartMousePosition;
        Point dragStartScrollPosition;
        Point autoScrollPosition;
        QuarterViewDrawer qView = null;
        WorldDefinition world = null;
        WeatherOverlay weatherOverlay;
        Sdl.SDL_Rect sourceRect;
        Sdl.SDL_Rect dst;
        short oldX;
        short oldY;
        bool lastMouseState;
        MainWindowMDI mainWindowMDI;

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
            //Console.WriteLine("LOADING: " + msg + "{" + progress + "}");
            gui.addSplashText(msg.Replace('\n', ' '), progress);
        }

        private void MusicHasStopped()
        {
            Core.bgmManager.nextSong();
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
                controller = MainWindow.mainWindow.currentController;
                //MainWindow.mainWindow.Show();
                qView.updateScreen();
                if (WorldDefinition.World.Satellite == null ||
                    WorldDefinition.World.Satellite.surface.w != 150 ||
                    WorldDefinition.World.Satellite.surface.h != 150)
                {
                    WorldDefinition.World.Satellite = new Surface(150, 150, 32);
                    WorldDefinition.World.Satellite.sourceColorKey = Color.Magenta;
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
            if (e.Key == Key.B)
            {
                RailRoadController.create();
            }
            
        }

        private void Resize(object sender, SdlDotNet.Graphics.VideoResizeEventArgs e)
        {
            this.width = e.Width;
            this.height = e.Height;
            screen = SdlDotNet.Graphics.Video.SetVideoMode(width, height, currentBpp, true);
            screen.SourceColorKey = Color.Magenta;
            weatherOverlay.setSize(new Size(width, height));
            qView.size = new Size(e.Width, e.Height);
        }

        private void MusicFinished(object sender, MusicFinishedEventArgs e)
        {
            Core.bgmManager.nextSong();
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
                if (!gui.checkIfGUIClick(e.EventStruct.button))
                {
                    if (controller != null)
                    {
                        Point ab = qView.fromClientToAB(e.X + ScrollPosition.X, e.Y + ScrollPosition.Y);
                        Location xyz = qView.fromABToXYZ(ab, controller);

                        if (e.Button == MouseButton.PrimaryButton) controller.OnClick(null, xyz, ab);
                    }
                }
            }
            lastMouseState = e.ButtonPressed;
        }

        private void MouseMotion(object sender, MouseMotionEventArgs e)
        {
            if (dragMode)
            {
                scrollByDrag(new Point(e.X, e.Y));
            }
            else
            {
                gui.checkMouseMovement(e.EventStruct.motion);
                if (controller != null)
                {
                    Point ab = qView.fromClientToAB(e.X + ScrollPosition.X, e.Y + ScrollPosition.Y);
                    controller.OnMouseMove(null, qView.fromABToXYZ(ab, controller), ab);
                }
                oldX = e.X;
                oldY = e.Y;
            }
        }

        private void timerTick(object sender, EventArgs e)
        {
            if (qView != null)
            {
                WorldDefinition.World.clock.tick();
                WorldDefinition.World.clock.tick();

                sourceRect = new Sdl.SDL_Rect((short)ScrollPosition.X, (short)ScrollPosition.Y, (short)width, (short)height);
                dst = new Sdl.SDL_Rect(0, 0, (short)width, (short)height);
                Tao.Sdl.Sdl.SDL_BlitSurface(qView.offscreenBuffer.surfacePtr(), ref sourceRect, screen.Handle, ref dst);
            }

            finalDraw();
        }

        private void finalDraw()
        {
            if (gui != null)
            {
                gui.updateGUIElements(width, height);
                if (WorldDefinition.World != null) gui.clock_text = WorldDefinition.World.clock.displayString;
                gui.drawGUI(width, height, screen.Handle);
            }
            screen.Update();
        }

        private void GUIButtonClick(object sender, EventArgs e)
        {
            switch ((string)sender)
            {
                case "rail":
                    RailRoadController.create();
                    RailRoadController.theInstance.Hide();
                    RailRoadController.theInstance.MdiParent = mainWindowMDI;
                    //mainWindowMDI.AddOwnedForm(RailRoadController.theInstance);
                    RailRoadController.theInstance.Show();
                    break;
                case "station":
                    PlatformController.create();
                    PlatformController.theInstance.Hide();
                    PlatformController.theInstance.MdiParent = mainWindowMDI;
                    PlatformController.theInstance.Show();
                    break;
                case "train":
                    TrainPlacementController.create();
                    TrainPlacementController.theInstance.Hide();
                    TrainPlacementController.theInstance.MdiParent = mainWindowMDI;
                    TrainPlacementController.theInstance.Show();
                    break;
                case "land":
                    MountainController.create();
                    MountainController.theInstance.Hide();
                    MountainController.theInstance.MdiParent = mainWindowMDI;
                    MountainController.theInstance.Show();
                    break;
                case "struct":
                    VarHeightBuildingController.create();
                    VarHeightBuildingController.theInstance.Hide();
                    VarHeightBuildingController.theInstance.MdiParent = mainWindowMDI;
                    VarHeightBuildingController.theInstance.Show();
                    break;
                case "playlist":
                    BGMPlaylist bgmplaylist = new BGMPlaylist();
                    bgmplaylist.MdiParent = mainWindowMDI;
                    bgmplaylist.Show();
                    break;
            }
        }

        private bool scrollByDrag(Point curMousePos)
        {
            int dragAccel = 1;
            Point pt = this.dragStartScrollPosition;
            pt.X += (curMousePos.X - dragStartMousePosition.X) * dragAccel;
            pt.Y += (curMousePos.Y - dragStartMousePosition.Y) * dragAccel;
            ScrollPosition = pt;
            qView.origin = ScrollPosition;
            return true;
        }

        private int distance(Point a, Point b)
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
            FreeTrainSDL ftsdl = new FreeTrainSDL();
            ftsdl.Go();
        }

        private void Go()
        {
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

            gui = new SDLGUI();
            gui.ButtonClick += new EventHandler(GUIButtonClick);
            gui.initGUI(width, height, videoInfo.vfmt);
            gui.addButton("rail", "Railroad Construction");
            gui.addButton("station", "Station Construction");
            gui.addButton("train", "Train Stuff");
            gui.addButton("land", "Modify Terrain");
            gui.addButton("struct", "Structure Construction");
            gui.addButton("playlist", "Music Playlist");

            gui.clock_text = "Initialising...";

            UpdateMessage("FreeTrain SDL Starting...", -1);
            gui.SHOW_SPLASH = true;

            finalDraw();

            MainWindow.mainWindow = new MainWindow(null, true);
            UpdateMessage("Loading plugins...", -1);
            finalDraw();
            Core.init(null, null, null, new ProgressHandler(UpdateMessage), true);
            world = new WorldDefinition(new Distance(150, 150, 7), 3);
            WorldDefinition.World = world;

            weatherOverlay = NullWeatherOverlay.theInstance;

            UpdateMessage("Creating Map...", 0);
            finalDraw();
            qView = new QuarterViewDrawer(world, new Rectangle(0, 0, world.Size.x * 32 - 16, (world.Size.y - 2 * world.Size.z - 1) * 8));
            qView.offscreenBuffer = new Surface(world.Size.x * 32 - 16, (world.Size.y - 2 * world.Size.z - 1) * 8, screen.CreateCompatibleSurface().Pixels);
            qView.offscreenBuffer.sourceColorKey = Color.Magenta;
            qView.recreateDrawBuffer(new Size(width, height), true);
            UpdateMessage("Creating Map...", 100);
            finalDraw();

            qView.draw(new Rectangle(0, 0, world.Size.x * 32 - 16, (world.Size.y - 2 * world.Size.z - 1) * 8), null);

            gui.SHOW_SPLASH = false;

            timer.Tick += new EventHandler(timerTick);
            timer.Interval = 33;
            timer.Enabled = true;
            timer.Start();
            mainWindowMDI = new MainWindowMDI();
            mainWindowMDI.Show();
            RailRoadController.create();
            RailRoadController.theInstance.Hide();
            RailRoadController.theInstance.MdiParent = mainWindowMDI;
            RailRoadController.theInstance.WindowState = FormWindowState.Maximized;
            RailRoadController.theInstance.Show();
            PlatformController.create();
            PlatformController.theInstance.Hide();
            PlatformController.theInstance.MdiParent = mainWindowMDI;
            PlatformController.theInstance.WindowState = FormWindowState.Maximized;
            PlatformController.theInstance.Show();
            TrainPlacementController.create();
            TrainPlacementController.theInstance.Hide();
            TrainPlacementController.theInstance.MdiParent = mainWindowMDI;
            TrainPlacementController.theInstance.WindowState = FormWindowState.Maximized;
            TrainPlacementController.theInstance.Show();
            MountainController.create();
            MountainController.theInstance.Hide();
            MountainController.theInstance.MdiParent = mainWindowMDI;
            MountainController.theInstance.WindowState = FormWindowState.Maximized;
            MountainController.theInstance.Show();
            VarHeightBuildingController.create();
            VarHeightBuildingController.theInstance.Hide();
            VarHeightBuildingController.theInstance.MdiParent = mainWindowMDI;
            VarHeightBuildingController.theInstance.WindowState = FormWindowState.Maximized;
            VarHeightBuildingController.theInstance.Show();
            LandController.create();
            LandController.theInstance.Hide();
            LandController.theInstance.MdiParent = mainWindowMDI;
            LandController.theInstance.WindowState = FormWindowState.Maximized;
            LandController.theInstance.Show();
            LandPropertyController.create();
            LandPropertyController.theInstance.Hide();
            LandPropertyController.theInstance.MdiParent = mainWindowMDI;
            LandPropertyController.theInstance.WindowState = FormWindowState.Maximized;
            LandPropertyController.theInstance.Show();
            StationPassagewayController.create();
            StationPassagewayController.theInstance.Hide();
            StationPassagewayController.theInstance.MdiParent = mainWindowMDI;
            StationPassagewayController.theInstance.WindowState = FormWindowState.Maximized;
            StationPassagewayController.theInstance.Show();
            SlopeRailRoadController.create();
            SlopeRailRoadController.theInstance.Hide();
            SlopeRailRoadController.theInstance.MdiParent = mainWindowMDI;
            SlopeRailRoadController.theInstance.WindowState = FormWindowState.Maximized;
            SlopeRailRoadController.theInstance.Show();
            PluginListDialog pluginListDialog = new PluginListDialog();
            pluginListDialog.MdiParent = mainWindowMDI;
            pluginListDialog.WindowState = FormWindowState.Maximized;
            pluginListDialog.Show();
            //RoadController roadController = new RoadController();
            //roadController.MdiParent = mainWindowMDI;
            //roadController.Show();
            //BulldozeController.create();
            //BulldozeController.theInstance.Hide();
            //BulldozeController.theInstance.MdiParent = mainWindowMDI;
            //BulldozeController.theInstance.Show();
            BGMPlaylist bgmplaylist = new BGMPlaylist();
            bgmplaylist.MdiParent = mainWindowMDI;
            bgmplaylist.Show();


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
