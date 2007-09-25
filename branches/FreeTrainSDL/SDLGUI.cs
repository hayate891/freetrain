using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using Tao.Sdl;
using SDL.net;

namespace FreeTrainSDL
{
    public class SDLGUI
    {
        private Color baseGUIColor, shadeGUIColor, drawerGUIColor;
        private int baseColor, shadeColor;
        private Surface top_bar, splash; //, satmap, bot_right, bot_left;
        private IntPtr freesans;

        //int BOTTOM_GUI_HEIGHT = 25;
        //public int SIDE_GUI_WIDTH = 25;
        public int TOP_GUI_HEIGHT = 24;

        int MAX_SPLASH_LINES = 10;

        public bool SHOW_SPLASH = false;
        private string[] SPLASH_TEXT;
        public string clock_text;
        protected int SPLASH_TEXT_HEIGHT = 10;
        protected int SPLASH_PERCENT = 0;

        public EventHandler ButtonClick;

        const int MAX_BUTTONS = 16;
        
        private SDLGUIButton[] top_toolbar;

        public SDLGUI()
        {
            SdlTtf.TTF_Init();
            baseGUIColor = Color.FromArgb(252, 252, 236);
            shadeGUIColor = Color.FromArgb(152, 152, 116);
            drawerGUIColor = Color.FromArgb(236, 236, 184);
        }

        public void initGUI(int width, int height, IntPtr pf)
        {
            baseColor = Sdl.SDL_MapRGB(pf, baseGUIColor.R, baseGUIColor.G, baseGUIColor.B);
            shadeColor = Sdl.SDL_MapRGB(pf, shadeGUIColor.R, shadeGUIColor.G, shadeGUIColor.B);
            
            top_toolbar = new SDLGUIButton[MAX_BUTTONS];
            
            /*rail = new SDLGUIButton("rail","Railroad Construction", 10, 10);
            if (ButtonClick != null) rail.ButtonClick += ButtonClick;
            
            station = new SDLGUIButton("station","Station Construction", 10, 10);
            if (ButtonClick != null) station.ButtonClick += ButtonClick;
            
            station = new SDLGUIButton("station","Station Construction", 10, 10);
            if (ButtonClick != null) station.ButtonClick += ButtonClick;*/

            top_bar = new Surface(Application.StartupPath + "\\gui\\top.bmp");
            splash = new Surface(Application.StartupPath + "\\gui\\splash.bmp");
            freesans = SdlTtf.TTF_OpenFont(Application.StartupPath + "\\gui\\freesans.ttf", SPLASH_TEXT_HEIGHT);
            SPLASH_TEXT = new string[MAX_SPLASH_LINES];
            for (int cur = 0; cur < MAX_SPLASH_LINES; cur++) SPLASH_TEXT[cur] = string.Empty;
        }

        public void checkMouseMovement(Sdl.SDL_MouseMotionEvent e)
        {
        	for (int i = 0; i < MAX_BUTTONS; i++) if (top_toolbar[i] != null) top_toolbar[i].checkMovement(e);
        }

        public bool checkIfGUIClick(Sdl.SDL_MouseButtonEvent e)
        {
            bool wasGUIClick = false;
            for (int i = 0; i < MAX_BUTTONS; i++) {
            	if (!wasGUIClick && top_toolbar[i] != null) wasGUIClick = top_toolbar[i].checkClick(e);
            }
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

            for (int i = 0; i < MAX_BUTTONS; i++) if (top_toolbar[i] != null) top_toolbar[i].draw(screen);

            /*Sdl.SDL_Rect current = new Sdl.SDL_Rect((short)(width - SIDE_GUI_WIDTH), (short)TOP_GUI_HEIGHT, (short)width, (short)height);
            Tao.Sdl.Sdl.SDL_FillRect(screen, ref current, baseColor);
            current = new Sdl.SDL_Rect(0, (short)TOP_GUI_HEIGHT, (short)SIDE_GUI_WIDTH, (short)height);
            Tao.Sdl.Sdl.SDL_FillRect(screen, ref current, baseColor);
            current = new Sdl.SDL_Rect(0, (short)(height - BOTTOM_GUI_HEIGHT), (short)width, (short)height);
            Tao.Sdl.Sdl.SDL_FillRect(screen, ref current, baseColor);

            
            Tao.Sdl.SdlGfx.vlineColor(screen,(short)(width - (SIDE_GUI_WIDTH + 1)),(short)TOP_GUI_HEIGHT,(short)(height - BOTTOM_GUI_HEIGHT - 17),0);
            Tao.Sdl.SdlGfx.vlineColor(screen, (short)(width - (SIDE_GUI_WIDTH)), (short)(TOP_GUI_HEIGHT + 2), (short)(height - BOTTOM_GUI_HEIGHT - 17), shadeColor);

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
            Tao.Sdl.SdlGfx.pixelRGBA(screen, (short)(width - SIDE_GUI_WIDTH), (short)(TOP_GUI_HEIGHT + 1), 0, 0, 0, 255);*/

            if (clock_text != null && clock_text != string.Empty)
            {
                drawText(screen, freesans, clock_text, Color.Black, (width - 160), 2, 0, false, false);
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
                        drawText(screen,freesans, SPLASH_TEXT[i], Color.Black, xpos + 10, (ypos + splash.size.Height + 10) + (i * (2 + SPLASH_TEXT_HEIGHT)),0,false, false);
                drawText(screen, freesans, "[" + SPLASH_PERCENT.ToString() + "%]", Color.Black, new Point(xpos + (splash.size.Height * 2) - 16, ypos + splash.size.Width - 20), 0, false, false);
            }

        }
        
        public bool addButton(string name, string tooltip) {
        	bool added = false;
        	for (int i = 0; i < MAX_BUTTONS; i++)
        	{
        		if (!added && top_toolbar[i] == null) {
        			top_toolbar[i] = new SDLGUIButton(name,tooltip, (10 + (i * 26)), 1,ref ButtonClick);
        			added = true;
        		}
        	}
        	return added;
        }

        public static void drawText(IntPtr screen, IntPtr in_font, string in_text, Color c, Point p, int rotate, bool fill, bool centered)
        {
            drawText(screen, in_font, in_text, c.R, c.G, c.B, 233, p.X, p.Y, rotate, fill, centered);
        }
        public static void drawText(IntPtr screen, IntPtr in_font, string in_text, Color c, Point p, int alpha, int rotate, bool fill, bool centered)
        {
            drawText(screen, in_font, in_text, c.R, c.G, c.B, (byte)alpha, p.X, p.Y, rotate, fill, centered);
        }

        public static void drawText(IntPtr screen, IntPtr in_font, string in_text, Color c, int x, int y, int rotate, bool fill, bool centered)
        {
            drawText(screen, in_font, in_text, c.R, c.G, c.B, 255, x, y, rotate, fill, centered);
        }

        public static void drawText(IntPtr screen, IntPtr in_font, string in_text, byte r, byte g, byte b, int x, int y, int rotate, bool fill, bool centered)
        {
            drawText(screen, in_font, in_text, r, g, b, 255, x, y, rotate, fill, centered);
        }

        public static void drawText(IntPtr screen, IntPtr in_font, string in_text, byte r, byte g, byte b, byte a, int x, int y, int rotate, bool fill, bool centered)
        {
            IntPtr text;
            if (in_text != string.Empty)
            {
                if (fill) text = SdlTtf.TTF_RenderUTF8_Blended(in_font, in_text, new Sdl.SDL_Color(255, 255, 255, a));
                else text = SdlTtf.TTF_RenderUTF8_Blended(in_font, in_text, new Sdl.SDL_Color(r, g, b, a));
                Sdl.SDL_Surface txt = (Sdl.SDL_Surface)Marshal.PtrToStructure(text, typeof(Sdl.SDL_Surface));
                Sdl.SDL_Rect src = new Sdl.SDL_Rect(0, 0, (short)txt.w, (short)txt.h);

                Sdl.SDL_Rect dst, back;// = new Sdl.SDL_Rect((short)(x - (txt.w / 2)), (short)(y - (txt.h / 2)), src.w, src.h);
                if (centered) dst = new Sdl.SDL_Rect((short)(x - (txt.w / 2)), (short)(y - (txt.h / 2)), (short)(src.w), (short)(src.h));
                else dst = new Sdl.SDL_Rect((short)(x), (short)(y), (short)(src.w), (short)(src.h));
                back = new Sdl.SDL_Rect((short)(dst.x - 2), (short)(dst.y - 1), (short)(dst.w + 4), (short)(dst.h + 2));
                if (rotate != 0)
                {
                    IntPtr newSrc = SdlGfx.rotozoomSurface(text, rotate, 1, 0);
                    src = new Sdl.SDL_Rect(0, 0, (short)(txt.h + 2), (short)(txt.w + 2));
                    if (centered) dst = new Sdl.SDL_Rect((short)(x - (txt.h / 2)), (short)(y - (txt.w / 2)), (short)(src.w), (short)(src.h));
                    else dst = new Sdl.SDL_Rect((short)(x), (short)(y), (short)(src.w), (short)(src.h));
                    if (fill)
                    {
                        back = new Sdl.SDL_Rect((short)(dst.x - 2), (short)(dst.y - 1), (short)(dst.w + 4), (short)(dst.h + 2));
                        Tao.Sdl.Sdl.SDL_FillRect(screen, ref back, 1);
                    }
                    Tao.Sdl.Sdl.SDL_BlitSurface(newSrc, ref src, screen, ref dst);
                    Sdl.SDL_FreeSurface(newSrc);
                }
                else
                {
                    if (fill) Tao.Sdl.Sdl.SDL_FillRect(screen, ref back, 1);
                    Tao.Sdl.Sdl.SDL_BlitSurface(text, ref src, screen, ref dst);
                }
                Sdl.SDL_FreeSurface(text);
            }
        }

        public void updateSatellite(ref Surface s)
        {
        /*    if (sat.DRAW_OPEN_AMOUNT > 0)
            {
                //if (satmap == null || satmap.surface.w != w || satmap.surface.h != h) satmap = new Surface(w, h, 16);

                satmap = s;
            }*/
        }
    }
}
