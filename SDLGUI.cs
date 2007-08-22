using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using Tao.Sdl;
using org.kohsuke.directdraw;

namespace FreeTrainSDL
{
    public class SDLGUI
    {
        private Color baseGUIColor, shadeGUIColor, drawerGUIColor;
        private int baseColor, shadeColor;
        private Surface bot_right, bot_left, top_bar, splash, satmap;
        //private SdlDotNet.Graphics.Font font;
        private IntPtr freesans;

        int BOTTOM_GUI_HEIGHT = 25;
        public int SIDE_GUI_WIDTH = 25;
        public int TOP_GUI_HEIGHT = 24;

        int MAX_SPLASH_LINES = 10;

        public bool SHOW_SPLASH = false;
        private string[] SPLASH_TEXT;
        public string clock_text;
        protected int SPLASH_TEXT_HEIGHT = 10;
        protected int SPLASH_PERCENT = 0;

        SDLGUIDrawer system;

        SDLGUIDrawerSatellite sat;

        public EventHandler SystemMenuClick;

        public SDLGUI()
        {
            SdlTtf.TTF_Init();
            baseGUIColor = Color.FromArgb(252, 252, 236);
            shadeGUIColor = Color.FromArgb(152, 152, 116);
            drawerGUIColor = Color.FromArgb(236, 236, 184);
        }

        public void updateTrainStatus(int index, int status)
        {
            sat.TrainStatus[index] = status;
        }

        public void initGUI(int width, int height, IntPtr pf)
        {
            baseColor = Sdl.SDL_MapRGB(pf, baseGUIColor.R, baseGUIColor.G, baseGUIColor.B);
            shadeColor = Sdl.SDL_MapRGB(pf, shadeGUIColor.R, shadeGUIColor.G, shadeGUIColor.B);

            bot_right = new Surface(Application.StartupPath + "\\gui\\botright.bmp");
            bot_left = new Surface(Application.StartupPath + "\\gui\\botleft.bmp");
            top_bar = new Surface(Application.StartupPath + "\\gui\\top.bmp");
            splash = new Surface(Application.StartupPath + "\\gui\\splash.bmp");
            system = new SDLGUIDrawer(width, height, 400, 15, pf);
            if (SystemMenuClick != null) system.MenuItemClicked += new EventHandler(SystemMenuClick);
            sat = new SDLGUIDrawerSatellite(width, height, 34, 24, pf);
            freesans = SdlTtf.TTF_OpenFont(Application.StartupPath + "\\gui\\freesans.ttf", SPLASH_TEXT_HEIGHT);
            SPLASH_TEXT = new string[MAX_SPLASH_LINES];
            for (int cur = 0; cur < MAX_SPLASH_LINES; cur++) SPLASH_TEXT[cur] = string.Empty;
        }

        public void checkMouseMovement(Sdl.SDL_MouseMotionEvent e)
        {
            system.checkMovement(e);
            sat.checkMovement(e);
        }

        public bool checkIfGUIClick(Sdl.SDL_MouseButtonEvent e)
        {
            bool wasGUIClick = false;
            if (!system.checkClick(e))
            {
                if (!sat.checkClick(e))
                {
                    //yada
                }
                else wasGUIClick = true;
                
            } else wasGUIClick = true;
            return wasGUIClick;
        }

        public void addSplashText(String nstr, float percent)
        {
            SPLASH_PERCENT = (int)(percent);
            if (!SPLASH_TEXT[MAX_SPLASH_LINES - 1].Contains(nstr))
                for (int cur = 0; cur < MAX_SPLASH_LINES; cur++)
                {
                    if (cur < (MAX_SPLASH_LINES-1)) SPLASH_TEXT[cur] = SPLASH_TEXT[cur + 1];
                    else SPLASH_TEXT[cur] = nstr;
                }
        }

        public void updateGUIElements(int width, int height)
        {
            system.updateDrawer(width, height);
            sat.updateDrawer(width, height);
        }

        public void drawGUI(int width, int height, IntPtr screen)
        {         
            int curXPos = -(top_bar.size.Width);

            Sdl.SDL_Rect src, dst;

            while (curXPos < width)
            {
                curXPos += top_bar.size.Width;
                src = new Sdl.SDL_Rect((short)curXPos, 0, (short)top_bar.size.Width, (short)top_bar.size.Height);
                dst = top_bar.clipSDLRect;
                Tao.Sdl.Sdl.SDL_BlitSurface(top_bar.surfacePtr(), ref dst, screen, ref src);
            }

            system.drawDrawer(screen,width,height);

            Sdl.SDL_Rect current = new Sdl.SDL_Rect((short)(width - SIDE_GUI_WIDTH), (short)TOP_GUI_HEIGHT, (short)width, (short)height);
            Tao.Sdl.Sdl.SDL_FillRect(screen, ref current, baseColor);
            current = new Sdl.SDL_Rect(0, (short)TOP_GUI_HEIGHT, (short)SIDE_GUI_WIDTH, (short)height);
            Tao.Sdl.Sdl.SDL_FillRect(screen, ref current, baseColor);
            current = new Sdl.SDL_Rect(0, (short)(height - BOTTOM_GUI_HEIGHT), (short)width, (short)height);
            Tao.Sdl.Sdl.SDL_FillRect(screen, ref current, baseColor);

            
            Tao.Sdl.SdlGfx.vlineColor(screen,(short)(width - (SIDE_GUI_WIDTH + 1)),(short)TOP_GUI_HEIGHT,(short)(height - BOTTOM_GUI_HEIGHT - 17),0);
            Tao.Sdl.SdlGfx.vlineColor(screen, (short)(width - (SIDE_GUI_WIDTH)), (short)(TOP_GUI_HEIGHT + 2), (short)(height - BOTTOM_GUI_HEIGHT - 17), shadeColor);

            sat.drawDrawer(screen, width, height, satmap);

            src = bot_right.clipSDLRect;
            dst = new Sdl.SDL_Rect((short)(width - (SIDE_GUI_WIDTH) - bot_right.size.Width), (short)(height - BOTTOM_GUI_HEIGHT - bot_right.size.Height + 1), (short)bot_right.size.Width, (short)bot_right.size.Height);
            Tao.Sdl.Sdl.SDL_BlitSurface(bot_right.surfacePtr(), ref src, screen, ref dst);


            Tao.Sdl.SdlGfx.hlineColor(screen, 
                            (short)(width - (SIDE_GUI_WIDTH) - bot_right.size.Width - 1), 
                            (short)(SIDE_GUI_WIDTH + bot_left.size.Width - 2),
                            (short)(height - BOTTOM_GUI_HEIGHT - 2),
                            0);
            Tao.Sdl.SdlGfx.hlineColor(screen,
                            (short)(width - (SIDE_GUI_WIDTH) - bot_right.size.Width - 1),
                            (short)(SIDE_GUI_WIDTH + bot_left.size.Width - 1),
                            (short)(height - BOTTOM_GUI_HEIGHT - 1),
                            shadeColor);

            Tao.Sdl.SdlGfx.vlineColor(screen,(short)(SIDE_GUI_WIDTH),(short)(TOP_GUI_HEIGHT),(short)(height - BOTTOM_GUI_HEIGHT - 18),0);
            Tao.Sdl.SdlGfx.vlineColor(screen,(short)(SIDE_GUI_WIDTH - 1),(short)(TOP_GUI_HEIGHT + 2),(short)(height - BOTTOM_GUI_HEIGHT - 16), shadeColor);

            src = bot_left.clipSDLRect;
            dst = new Sdl.SDL_Rect((short)(SIDE_GUI_WIDTH - 1), (short)(height - BOTTOM_GUI_HEIGHT - bot_left.size.Height + 1), (short)bot_left.size.Width, (short)bot_left.size.Height);
            Tao.Sdl.Sdl.SDL_BlitSurface(bot_left.surfacePtr(), ref src, screen, ref dst);


            Tao.Sdl.SdlGfx.hlineColor(screen,(short)(width - (SIDE_GUI_WIDTH + 1)),(short)(width),(short)(TOP_GUI_HEIGHT),0);
            Tao.Sdl.SdlGfx.hlineColor(screen,0,(short)(SIDE_GUI_WIDTH),(short)(TOP_GUI_HEIGHT),0);

            Tao.Sdl.SdlGfx.pixelRGBA(screen, (short)(SIDE_GUI_WIDTH - 2), (short)(TOP_GUI_HEIGHT), 0, 0, 0, 255);
            Tao.Sdl.SdlGfx.pixelRGBA(screen, (short)(SIDE_GUI_WIDTH - 1), (short)(TOP_GUI_HEIGHT), 0, 0, 0, 255);
            Tao.Sdl.SdlGfx.pixelRGBA(screen, (short)(SIDE_GUI_WIDTH - 1), (short)(TOP_GUI_HEIGHT + 1), 0, 0, 0, 255);

            Tao.Sdl.SdlGfx.pixelRGBA(screen, (short)(width - SIDE_GUI_WIDTH + 1), (short)(TOP_GUI_HEIGHT), 0, 0, 0, 255);
            Tao.Sdl.SdlGfx.pixelRGBA(screen, (short)(width - SIDE_GUI_WIDTH), (short)(TOP_GUI_HEIGHT), 0, 0, 0, 255);
            Tao.Sdl.SdlGfx.pixelRGBA(screen, (short)(width - SIDE_GUI_WIDTH), (short)(TOP_GUI_HEIGHT + 1), 0, 0, 0, 255);
           

            if (clock_text != null && clock_text != string.Empty)
            {
                drawText(screen,freesans,clock_text,Color.Black,(width - (src.w) - 140),2);
            }

            if (SHOW_SPLASH)
            {
                int xpos = (width / 2) - (splash.size.Width / 2);
                int ypos = (height / 2) - (splash.size.Height / 2);
                src = new Sdl.SDL_Rect((short)xpos, (short)ypos, (short)splash.size.Width, (short)(splash.size.Height * 2));
                Tao.Sdl.Sdl.SDL_FillRect(screen,ref src,baseColor);
                src = splash.clipSDLRect;
                dst = new Sdl.SDL_Rect((short)xpos, (short)ypos, (short)splash.size.Width, (short)(splash.size.Height));
                Tao.Sdl.Sdl.SDL_BlitSurface(splash.surfacePtr(), ref src, screen, ref dst);
                for (int i = 0; i < MAX_SPLASH_LINES; i++)
                    if (SPLASH_TEXT[i] != string.Empty)
                        drawText(screen,freesans, SPLASH_TEXT[i], Color.Black, xpos + 10, (ypos + splash.size.Height + 10) + (i * (2 + SPLASH_TEXT_HEIGHT)));
                drawText(screen,freesans, "[" + SPLASH_PERCENT.ToString() + "%]", Color.Black, new Point(xpos + (splash.size.Height * 2) - 16, ypos + splash.size.Width - 20));
            }

        }

        private void drawText(IntPtr screen, IntPtr in_font, string in_text, Color c, Point p)
        {
            drawText(screen, in_font, in_text, c.R, c.G, c.B, 255, p.X, p.Y);
        }
        private void drawText(IntPtr screen, IntPtr in_font, string in_text, Color c, Point p, int alpha)
        {
            drawText(screen, in_font, in_text, c.R, c.G, c.B, (byte)alpha, p.X, p.Y);
        }

        private void drawText(IntPtr screen, IntPtr in_font, string in_text, Color c, int x, int y)
        {
            drawText(screen, in_font, in_text, c.R, c.G, c.B, 255, x, y);
        }

        private void drawText(IntPtr screen, IntPtr in_font, string in_text, byte r, byte g, byte b, int x, int y)
        {
            drawText(screen, in_font, in_text, r, g, b, 255, x, y);
        }

        public void drawText(IntPtr screen, IntPtr in_font, string in_text, byte r, byte g, byte b, byte a, int x, int y)
        {
            IntPtr text = SdlTtf.TTF_RenderUTF8_Blended(in_font, in_text, new Sdl.SDL_Color(r, g, b, a));
            Sdl.SDL_Surface txt = (Sdl.SDL_Surface)Marshal.PtrToStructure(text, typeof(Sdl.SDL_Surface));
            Sdl.SDL_Rect src = new Sdl.SDL_Rect(0, 0, (short)txt.w, (short)txt.h);
            Sdl.SDL_Rect dst = new Sdl.SDL_Rect((short)x, (short)y, src.w, src.h);
            Tao.Sdl.Sdl.SDL_BlitSurface(text, ref src, screen, ref dst);
            Sdl.SDL_FreeSurface(text);
        }

        public void updateSatellite(ref Surface s)
        {
            if (sat.DRAW_OPEN_AMOUNT > 0)
            {
                //if (satmap == null || satmap.surface.w != w || satmap.surface.h != h) satmap = new Surface(w, h, 16);

                satmap = s;
            }
        }
    }
}
