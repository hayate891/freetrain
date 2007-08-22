using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

using Tao.Sdl;
using Tao.Platform.Windows;
using org.kohsuke.directdraw;

using freetrain.world;
using freetrain.world.rail;
using freetrain.world.terrain;
using freetrain.controllers.land;
using freetrain.framework.plugin;
using freetrain.framework.sound;
using freetrain.util;
using freetrain.util.command;
using freetrain.controllers;
using freetrain.controllers.rail;
using freetrain.controllers.terrain;
using freetrain.controllers.structs;
using freetrain.framework;
using freetrain.views;
using freetrain.framework.graphics;

namespace FreeTrainSDL
{
    public class FreeTrainSDL : IDisposable
    {
        int flags = (Sdl.SDL_HWSURFACE | Sdl.SDL_DOUBLEBUF | Sdl.SDL_ANYFORMAT | Sdl.SDL_RESIZABLE);

        static int width = 640,height = 480;
        private int MAP_X_OFFSET = 12, MAP_Y_OFFSET = 24;
        private IntPtr screen;
        private SDLGUI gui;
        bool quitFlag = false;
        Sdl.SDL_VideoInfo videoInfo;
        Sdl.SDL_PixelFormat pixelFormat;

        static int CURRENT_BPP = 16;

        Surface tmp, satmap;

        ModalController controller = null;

        private bool dragMode = false;
        private Point dragStartMousePos, dragStartScrollPos, AutoScrollPosition;

        private Point scrollPos
        {
            get
            {
                Point pt = AutoScrollPosition;
                return new Point(pt.X + MAP_X_OFFSET, pt.Y + MAP_Y_OFFSET);
            }
            set
            {
                AutoScrollPosition =new Point(
                    Math.Max(value.X - MAP_X_OFFSET,0),
                    Math.Max(value.Y - MAP_Y_OFFSET,0));
            }
        }

        QuarterViewDrawer qview = null;
        World w = null;
        WeatherOverlay weatherOverlay;

        Clock clock = null;

        Sdl.SDL_Rect source_rect,dst;
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            FreeTrainSDL ftsdl = new FreeTrainSDL();
            ftsdl.Go();
        }

        public void updateMessage(string msg, float progress)
        {
            //Console.WriteLine("LOADING: " + msg + "{" + progress + "}");
            gui.addSplashText(msg.Replace('\n',' '), progress);
        }

        public void Go()
        {
            dragStartMousePos = new Point(0, 0);
            dragStartScrollPos = new Point(0, 0);
            scrollPos = new Point(0, 0);

            Sdl.SDL_Init(Sdl.SDL_INIT_EVERYTHING);
            Sdl.SDL_WM_SetCaption("FreeTrain SDL", "");
            screen = Sdl.SDL_SetVideoMode(
                    width,
                    height,
                    CURRENT_BPP,
                    flags);

            IntPtr videoInfoPointer = Sdl.SDL_GetVideoInfo();

            if (videoInfoPointer != IntPtr.Zero) {
                videoInfo = (Sdl.SDL_VideoInfo)Marshal.PtrToStructure(videoInfoPointer, typeof(Sdl.SDL_VideoInfo));
                pixelFormat = (Sdl.SDL_PixelFormat)Marshal.PtrToStructure(videoInfo.vfmt,typeof(Sdl.SDL_PixelFormat));
            }

            gui = new SDLGUI();
            gui.SystemMenuClick += new EventHandler(sysMenuClick);
            gui.initGUI(width, height, videoInfo.vfmt);
            gui.clock_text = "Initialising...";

            updateMessage("FreeTrain SDL Starting...", -1);
            gui.SHOW_SPLASH = true;

            finalDraw();

            MainWindow.mainWindow = new MainWindow(null, true);
            updateMessage("Loading plugins...",-1);
            finalDraw();
            Core.init(null, null, null, new ProgressHandler(updateMessage), true);
            w = new World(new Distance(150,150, 7), 3);
            World.world = w;

            weatherOverlay = NullWeatherOverlay.theInstance;
            tmp = new Surface(width, height);

            updateMessage("Creating Map...",0);
            finalDraw();
            qview = new QuarterViewDrawer(w, new Rectangle(0, 0, w.size.x * 32 - 16, (w.size.y - 2 * w.size.z - 1) * 8));
            qview.offscreenBuffer = new Surface(w.size.x * 32 - 16, (w.size.y - 2 * w.size.z - 1) * 8, videoInfo.vfmt);
            qview.recreateDrawBuffer(new Size(width, height), true);
            updateMessage("Creating Map...",100);
            finalDraw();

            qview.draw(new Rectangle(0, 0, w.size.x * 32 - 16, (w.size.y - 2 * w.size.z - 1) * 8), null);
            qview.OnUpdated += new EventHandler(qview_OnUpdated);

            gui.SHOW_SPLASH = false;

            Sdl.SDL_Event evt;

            int result;
            while (quitFlag == false)
            {
                result = Sdl.SDL_PollEvent(out evt);
                switch (evt.type)
                {
                    case Sdl.SDL_QUIT:
                        quitFlag = true;
                        break;
                    case Sdl.SDL_KEYDOWN:
                        if ((evt.key.keysym.sym == (int)Sdl.SDLK_ESCAPE) || (evt.key.keysym.sym == (int)Sdl.SDLK_q)) quitFlag = true;
                        break;
                    case Sdl.SDL_VIDEORESIZE:
                        width = evt.resize.w;
                        height = evt.resize.h;
                        screen = Sdl.SDL_SetVideoMode(width, height, CURRENT_BPP, flags);
                        weatherOverlay.setSize(new Size(width,height));
                        qview.size = new Size(evt.resize.w, evt.resize.h);
                        break;
                    case Sdl.SDL_MOUSEBUTTONDOWN:
                        Events_MouseButtonDown(evt.button);
                        break;
                    case Sdl.SDL_MOUSEBUTTONUP:
                        Events_MouseButtonUp(evt.button);
                        break;
                    case Sdl.SDL_MOUSEMOTION:
                        Events_MouseMotion(evt.motion);
                        break;
                }                

                //Sdl.SDL_Rect current_view = new Sdl.SDL_Rect(0, 0, (short)width, (short)height);
                //Tao.Sdl.Sdl.SDL_FillRect(screen, ref current_view, Sdl.SDL_MapRGB(videoInfo.vfmt, 0, 100, 0));

                if (qview != null)
                {
                    controller = MainWindow.mainWindow.currentController;
                    clock = World.world.clock;
                    clock.tick();
                    //clock.tick();

                    qview.updateScreen();
                    if (World.world.satellite == null || World.world.satellite.surface.w != 150 || World.world.satellite.surface.h != 150)
                    {
                        World.world.satellite = new Surface(150, 150, 32);
                        World.world.satellite.fill(Color.FromArgb(222,195,132));
                    }

                    for (int i = 0; i < World.world.rootTrainGroup.items.Count; i++)
                    {
                        Train t = (Train)World.world.rootTrainGroup.items.get(i);
                        if (t.state == Train.State.Moving) 
                            gui.updateTrainStatus(i, 3);
                        else if (t.isPlaced)   
                            gui.updateTrainStatus(i, 2);
                        else 
                            gui.updateTrainStatus(i, 1);
                    }

                    gui.updateSatellite(ref World.world.satellite);



                    source_rect = new Sdl.SDL_Rect((short)scrollPos.X, (short)scrollPos.Y, (short)width, (short)height);
                    dst = new Sdl.SDL_Rect(0, 0, (short)width, (short)height);
                    Tao.Sdl.Sdl.SDL_BlitSurface(qview.offscreenBuffer.surfacePtr(), ref source_rect, screen, ref dst);
                    //Tao.Sdl.Sdl.SDL_BlitSurface(tmp.surfacePtr(), ref dst, screen, ref dst);
                }


                finalDraw();
           }
        }

        void finalDraw()
        {
            if (gui != null)
            {
                gui.updateGUIElements(width, height);
                if (clock != null) gui.clock_text = clock.displayString;
                gui.drawGUI(width, height, screen);
            }
            int result = Sdl.SDL_Flip(screen);
        }

        private void weatherTimer_Tick(object sender, System.EventArgs e)
        {
        }

        void sysMenuClick(object sender, EventArgs e)
        {
            switch ((int)sender)
            {
                case 0: 
                    RailRoadController.create();
                    break;
                case 1:
                    TrainTradingDialog t = new TrainTradingDialog();
                    t.ShowDialog();
                    t.Dispose();
                    TrainPlacementController.create();
                    break;
                case 2:

                    MountainController.create();
                    break;
                case 3:
                    /*if (screen.FullScreen)
                    {
                        width = old_width;
                        height = old_height;
                    }
                    else
                    {
                        old_width = width;
                        old_height = height;
                        width = 640;
                        height = 480;
                    }
                    bool new_FS = !screen.FullScreen;
                    screen = Video.SetVideoMode(width, height, CURRENT_BPP, true, false, new_FS);
                    //qview.size = new Size(width, height);
                    break;*/

                    StationaryStructPlacementController.create();
                    VarHeightBuildingController.create();
                    CommercialStructPlacementController.create();
                    break;

                case 4:
                    quitFlag = true;
                    break;
            }
        }

        void qview_OnUpdated(object sender, EventArgs e)
        {
            //if (qview.offscreenBuffer != null)
            //{
               //maybe do something? I don't know.
            //}
            //
        }

        short old_x, old_y;
        void Events_MouseMotion(Sdl.SDL_MouseMotionEvent e)
        {
            if (dragMode)
            {
                scrollByDrag(new Point(e.x, e.y));
               
            }
            else
            {
                gui.checkMouseMovement(e);
                if (controller != null)
                {
                    Point ab = qview.fromClientToAB(e.x + scrollPos.X, e.y + scrollPos.Y);
                    controller.onMouseMove(null, qview.fromABToXYZ(ab, controller), ab);
                }
                old_x = e.x;
                old_y = e.y;
            }

            
        }
        byte lastMouseState;
        void Events_MouseButtonDown(Sdl.SDL_MouseButtonEvent e)
        {
            lastMouseState = e.state;
            if (e.button == Sdl.SDL_BUTTON_RIGHT && !dragMode)
            {
                dragMode = true;
                dragStartMousePos = new Point(e.x, e.y);
                dragStartScrollPos = scrollPos;
                Sdl.SDL_ShowCursor(0);
            }
        }

        void Events_MouseButtonUp(Sdl.SDL_MouseButtonEvent e)
        {
            if (dragMode)
            {
                dragMode = false;
                //if (scrollByDrag(e.Position)) return;
                Sdl.SDL_WarpMouse(old_x, old_y);
                Sdl.SDL_ShowCursor(1);
            }

            if (lastMouseState == Sdl.SDL_PRESSED)
            {
                if (!gui.checkIfGUIClick(e))
                {
                    if (controller != null)
                    {
                        Point ab = qview.fromClientToAB(e.x + scrollPos.X, e.y + scrollPos.Y);
                        Location xyz = qview.fromABToXYZ(ab, controller);

                        if (e.button == Sdl.SDL_BUTTON_LEFT) controller.onClick(null, xyz, ab);
                    }
                }
            }
            lastMouseState = e.state;
        }

        private bool scrollByDrag(Point curMousePos)
        {
            int dragAccel = 1;
            //if (controller == null)
            //{
                Point pt = this.dragStartScrollPos;
                pt.X +=(curMousePos.X - dragStartMousePos.X) * dragAccel;
                pt.Y += (curMousePos.Y - dragStartMousePos.Y) * dragAccel;
                //pt.X *= -1;
                //pt.Y *= -1;
                //Console.WriteLine(pt);
                scrollPos = pt;
                qview.origin = scrollPos;
                //if (qview.origin != scrollPos)
                //{
                //    dragStartMousePos = curMousePos;
                //    dragStartScrollPos = curMousePos;
                //    scrollPos = qview.origin;
                //}
                //Console.Write(scrollPos);
                return true;
            //}
            //return false;
        }

        private int distance(Point a, Point b)
        {
            return (int)Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }

        //private void Tick(object sender, TickEventArgs args)
        //{
            //screen.Fill(Color.Black);
            /*if (qview != null) {
                controller = MainWindow.mainWindow.currentController;
                clock = World.world.clock;
                clock.tick();
                clock.tick();
                clock.tick();
                clock.tick();

                qview.updateScreen();
             
                //if (scrollPos.X == source_rect.X && scrollPos.Y == source_rect.Y)
                //{
                    //tst = screen.CreateScaledSurface(0.5);
                 //   screen.Blit(qview.offscreenBuffer.handle, new Point(0,0), source_rect);
              //screen.Draw(new SdlDotNet.Graphics.Primitives.Box(qview.offscreenBuffer.clipRect.Location, qview.offscreenBuffer.clipRect.Size),Color.Black);
                  //Rectangle r = new Rectangle(0, 0, width / 2, height / 2);
                    
                    //screen.Blit(tst,new Point(0,0));
                    //tst.Dispose();
                    
                //}
                //else
                //{
                    source_rect = new Rectangle(scrollPos, new Size(width, height));
                    screen.Blit(qview.offscreenBuffer.handle, new Point(0, 0), source_rect);
                //}
                
            }*/

        //    if (gui != null) {
        //        gui.updateGUIElements(width,height);
        //        if (clock != null) gui.clock_text = clock.displayString;
        //        gui.drawGUI(width,height,screen);
        //    }

        //    screen.Update();
        //}

        public void Dispose()
        {
            
            weatherOverlay.Dispose();
            Dispose();
        }
    }
}