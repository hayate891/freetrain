using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using Tao.Sdl;
using org.kohsuke.directdraw;

namespace FreeTrainSDL
{
    public enum GUI_DRAWER_Movements
    {
        NO_MOVEMENT,
        DRAWER_OPENING,
        DRAWER_CLOSING
    };

    public enum GUI_DRAWER_Alignment
    {
        TOP_DOWN,
        RIGHT_LEFT,
        BOTTOM_UP,
        LEFT_RIGHT
    };

    public class SDLGUIDrawer
    {
        protected Rectangle DRAWER_TAB_POS;
        protected Surface drawer, container, containerON;

        protected int X_POSITION, Y_POSITION;
        protected int X_POSITION_OFFSET, Y_POSITION_OFFSET;
        protected int CLICK_X_MIN, CLICK_X_MAX, CLICK_Y_MIN, CLICK_Y_MAX;
        protected int DRAWER_HEIGHT = 104;

        protected int SCR_WIDTH, SCR_HEIGHT;

        protected int REAL_Y_HEIGHT, MIN_SPEED = 15, LIST_BUTTONS = 5;

        public int DRAW_OPEN_AMOUNT = 0;

        protected bool drawHighlight = false;

        public event EventHandler MenuItemClicked;

        protected IntPtr freesans;

        protected int currentArea = -1;

        protected string[] LIST_TEXT = { "NEW","SAVE","LOAD","FULLSCREEN","EXIT" };

        public Color drawerColor = Color.FromArgb(236, 236, 184), baseGUIColor = Color.FromArgb(252, 252, 236);

        protected int dColor, bColor, wColor;

        protected GUI_DRAWER_Movements drawer_movement = GUI_DRAWER_Movements.NO_MOVEMENT;
        protected GUI_DRAWER_Alignment drawer_alignment = GUI_DRAWER_Alignment.TOP_DOWN;

        public SDLGUIDrawer(int width, int height, int x_off, int y_off, IntPtr pf)
        {
            X_POSITION_OFFSET = x_off; // FROM RIGHT
            Y_POSITION_OFFSET = y_off; // FROM TOP
            X_POSITION = width - x_off;
            Y_POSITION = y_off;

            SCR_WIDTH = width;
            SCR_HEIGHT = height;
                        
            drawer = new Surface(Application.StartupPath + "\\gui\\drawer.bmp");
            container = new Surface(Application.StartupPath + "\\gui\\system.bmp");
            containerON = new Surface(Application.StartupPath + "\\gui\\systemon.bmp");

            DRAWER_TAB_POS = new Rectangle(X_POSITION, Y_POSITION, drawer.size.Width, drawer.size.Height);

            CLICK_X_MIN = X_POSITION;
            CLICK_X_MAX = X_POSITION + drawer.size.Width;

            CLICK_Y_MIN = 0;
            CLICK_Y_MAX = Y_POSITION + drawer.size.Height;

            REAL_Y_HEIGHT = drawer.size.Height;

            freesans = SdlTtf.TTF_OpenFont(Application.StartupPath + "\\gui\\freesans.ttf", 14);

            dColor = Sdl.SDL_MapRGB(pf, drawerColor.R, drawerColor.G, drawerColor.B);
            bColor = Sdl.SDL_MapRGB(pf, baseGUIColor.R, baseGUIColor.G, baseGUIColor.B);
            wColor = Sdl.SDL_MapRGB(pf, 255,255,255);
        }

        public void updateDrawer(int width, int height)
        {
            SCR_WIDTH = width;
            SCR_HEIGHT = height;

            switch (drawer_movement)
            {
                case GUI_DRAWER_Movements.DRAWER_CLOSING:
                    DRAW_OPEN_AMOUNT /= 2;
                    if (DRAW_OPEN_AMOUNT <= 0)
                    {
                        DRAW_OPEN_AMOUNT = 0;
                        drawer_movement = GUI_DRAWER_Movements.NO_MOVEMENT;
                    }
                    break;
                case GUI_DRAWER_Movements.DRAWER_OPENING:
                    if (DRAW_OPEN_AMOUNT == 0) DRAW_OPEN_AMOUNT = 1;
                    DRAW_OPEN_AMOUNT *= 2;
                    if (DRAW_OPEN_AMOUNT > 100)
                    {
                        DRAW_OPEN_AMOUNT = 100;
                        drawer_movement = GUI_DRAWER_Movements.NO_MOVEMENT;
                    }
                    break;
                case GUI_DRAWER_Movements.NO_MOVEMENT:
                    //nothing as yet.
                    break;
            }

            if (drawer_alignment == GUI_DRAWER_Alignment.LEFT_RIGHT || drawer_alignment == GUI_DRAWER_Alignment.RIGHT_LEFT)
            {
                if (DRAW_OPEN_AMOUNT > 0) DRAWER_TAB_POS.X = (SCR_WIDTH - X_POSITION_OFFSET) - (DRAWER_HEIGHT / (100/DRAW_OPEN_AMOUNT));
                else DRAWER_TAB_POS.X = (SCR_WIDTH - X_POSITION_OFFSET);
                DRAWER_TAB_POS.Y = (Y_POSITION_OFFSET);

                if (drawer_alignment == GUI_DRAWER_Alignment.RIGHT_LEFT)
                {
                    CLICK_X_MIN = DRAWER_TAB_POS.X;
                    CLICK_X_MAX = SCR_WIDTH;
                    CLICK_Y_MIN = DRAWER_TAB_POS.Y;
                    CLICK_Y_MAX = DRAWER_TAB_POS.Y + REAL_Y_HEIGHT;
                }
            }
            else
            {
                DRAWER_TAB_POS.X = (SCR_WIDTH - X_POSITION_OFFSET);
                if (DRAW_OPEN_AMOUNT > 0) DRAWER_TAB_POS.Y = Y_POSITION_OFFSET + (DRAWER_HEIGHT / (100/DRAW_OPEN_AMOUNT));
                else DRAWER_TAB_POS.Y = (Y_POSITION_OFFSET);

                if (drawer_alignment == GUI_DRAWER_Alignment.TOP_DOWN)
                {
                    CLICK_X_MIN = DRAWER_TAB_POS.X;
                    CLICK_X_MAX = DRAWER_TAB_POS.X + drawer.size.Width;
                    CLICK_Y_MIN = 0;
                    CLICK_Y_MAX = DRAWER_TAB_POS.Y + drawer.size.Height;
                }
            }
        }

        public bool checkClick(Sdl.SDL_MouseButtonEvent e)
        {
            bool wasGUIClick = false;
            int x = e.x, y = e.y;
            if (x >= CLICK_X_MIN && x <= CLICK_X_MAX)
                if (y >= CLICK_Y_MIN && y <= CLICK_Y_MAX)
                {
                    if (e.button == Sdl.SDL_BUTTON_MIDDLE)
                    {
                        DRAWER_TAB_POS.Y = y;
                    }
                    else
                    {
                        if (currentArea >= 0 && MenuItemClicked != null && DRAW_OPEN_AMOUNT == 100) MenuItemClicked(currentArea, null);

                        if (drawer_movement == GUI_DRAWER_Movements.NO_MOVEMENT && DRAW_OPEN_AMOUNT == 0)
                            drawer_movement = GUI_DRAWER_Movements.DRAWER_OPENING;
                        else
                            drawer_movement = GUI_DRAWER_Movements.DRAWER_CLOSING;
                    }
                    wasGUIClick = true;
                }
            Console.WriteLine("CLICK!");
            return wasGUIClick;
        }

        public void checkMovement(Sdl.SDL_MouseMotionEvent e)
        {
            drawHighlight = false;
            switch (drawer_alignment) 
            {
                case GUI_DRAWER_Alignment.LEFT_RIGHT:
                    if (e.x >= 0 && e.x <= (DRAWER_TAB_POS.X + DRAWER_TAB_POS.Width))
                        if (e.y >= DRAWER_TAB_POS.Y && e.y <= (DRAWER_TAB_POS.Y + REAL_Y_HEIGHT)) drawHighlight = true;
                    break;
                case GUI_DRAWER_Alignment.TOP_DOWN:
                    currentArea = -1;
                    if (e.x >= DRAWER_TAB_POS.X && e.x <= (DRAWER_TAB_POS.X + DRAWER_TAB_POS.Width))
                        if (e.y >= 0 && e.y <= (DRAWER_TAB_POS.Y + REAL_Y_HEIGHT))
                        {
                            drawHighlight = true;
                            currentArea = (e.y - Y_POSITION_OFFSET - 4) / 18;
                        }
                    break;
                case GUI_DRAWER_Alignment.RIGHT_LEFT:
                    if (e.x >= DRAWER_TAB_POS.X && e.x <= SCR_WIDTH)
                        if (e.y >= DRAWER_TAB_POS.Y && e.y <= (DRAWER_TAB_POS.Y + REAL_Y_HEIGHT)) drawHighlight = true;
                    break;
                case GUI_DRAWER_Alignment.BOTTOM_UP:
                    break;
            }
        }

        public void drawDrawer(IntPtr screen, int width, int height)
        {
            Sdl.SDL_Rect src, dst;
            if (DRAW_OPEN_AMOUNT > 0)
            {
                src = new Sdl.SDL_Rect((short)(DRAWER_TAB_POS.X + 4), (short)(Y_POSITION), (short)(DRAWER_TAB_POS.Width - 9), (short)(DRAWER_TAB_POS.Y - 5));
                Tao.Sdl.Sdl.SDL_FillRect(screen, ref src, dColor);
                Tao.Sdl.SdlGfx.vlineColor(screen, (short)(DRAWER_TAB_POS.X + 3), (short)(DRAWER_TAB_POS.Y + 4), (short)(Y_POSITION), 0);
                Tao.Sdl.SdlGfx.vlineColor(screen, (short)((DRAWER_TAB_POS.X + 3) + (DRAWER_TAB_POS.Width - 8)), (short)(DRAWER_TAB_POS.Y + 4), (short)(Y_POSITION), 0);
            }

            src = drawer.clipSDLRect;
            dst = new Sdl.SDL_Rect((short)DRAWER_TAB_POS.X, (short)DRAWER_TAB_POS.Y, (short)drawer.size.Width, (short)drawer.size.Height);
            Tao.Sdl.Sdl.SDL_BlitSurface(drawer.surfacePtr(), ref src, screen, ref dst);

            int fColor = 0;

            if (DRAW_OPEN_AMOUNT > 0)
            {
                for (int cur = 0; cur < LIST_BUTTONS; cur++)
                {
                    fColor = 0;
                    Tao.Sdl.SdlGfx.rectangleRGBA(screen,(short)(DRAWER_TAB_POS.X + 7),
                                                        (short)(DRAWER_TAB_POS.Y - ((LIST_BUTTONS - (cur)) * 20)),
                                                        (short)(DRAWER_TAB_POS.X + DRAWER_TAB_POS.Width - 9),
                                                        (short)((DRAWER_TAB_POS.Y - ((LIST_BUTTONS - (cur)) * 20)) + 16),0,0,0,255);

                    if (currentArea == cur)
                    {
                        src=new Sdl.SDL_Rect((short)(DRAWER_TAB_POS.X + 8),
                                                        (short)(DRAWER_TAB_POS.Y - ((LIST_BUTTONS - (cur)) * 20) + 1),
                                                        (short)(DRAWER_TAB_POS.Width - 17),
                                                        (short)15);
                        Tao.Sdl.Sdl.SDL_FillRect(screen, ref src,0);
                        fColor = 255;
                    }
                    drawText(screen, freesans, LIST_TEXT[cur], (byte)fColor, (byte)fColor, (byte)fColor, 255, (DRAWER_TAB_POS.X + (DRAWER_TAB_POS.Width / 2)), DRAWER_TAB_POS.Y - (((LIST_BUTTONS - cur) * 20) + 2));
                }
            }

            if (drawHighlight)
            {
                src = containerON.clipSDLRect;
                dst = new Sdl.SDL_Rect((short)DRAWER_TAB_POS.X, 0, (short)containerON.size.Width, (short)containerON.size.Height);
                Tao.Sdl.Sdl.SDL_BlitSurface(containerON.surfacePtr(), ref src, screen, ref dst);
            }
            else
            {
                src = container.clipSDLRect;
                dst = new Sdl.SDL_Rect((short)DRAWER_TAB_POS.X, 0, (short)container.size.Width, (short)container.size.Height);
                Tao.Sdl.Sdl.SDL_BlitSurface(container.surfacePtr(), ref src, screen, ref dst);
            }
        }

        public void drawText(IntPtr screen, IntPtr in_font, string in_text, byte r, byte g, byte b, byte a, int x, int y)
        {
            IntPtr text = SdlTtf.TTF_RenderUTF8_Blended(in_font, in_text, new Sdl.SDL_Color(r, g, b, a));
            Sdl.SDL_Surface txt = (Sdl.SDL_Surface)Marshal.PtrToStructure(text, typeof(Sdl.SDL_Surface));
            Sdl.SDL_Rect src = new Sdl.SDL_Rect(0, 0, (short)txt.w, (short)txt.h);
            Sdl.SDL_Rect dst = new Sdl.SDL_Rect((short)(x - (txt.w / 2)), (short)y, src.w, src.h);
            Tao.Sdl.Sdl.SDL_BlitSurface(text, ref src, screen, ref dst);
            Sdl.SDL_FreeSurface(text);
        }
    }

    public class SDLGUIDrawerSatellite : SDLGUIDrawer
    {
        public SDLGUIDrawerSatellite(int w, int h, int x, int y, IntPtr pf) : base(w,h,x,y, pf)
        {
            drawer = new Surface(Application.StartupPath + "\\gui\\satdrawertop.bmp");
            container = new Surface(Application.StartupPath + "\\gui\\sattext.bmp");
            containerON = new Surface(Application.StartupPath + "\\gui\\sattexton.bmp");

            DRAWER_TAB_POS = new Rectangle(X_POSITION, Y_POSITION, drawer.size.Width, drawer.size.Height);

            drawer_alignment = GUI_DRAWER_Alignment.RIGHT_LEFT;

            DRAWER_HEIGHT = 180;
            REAL_Y_HEIGHT = SCR_HEIGHT - 58;

            CLICK_X_MIN = X_POSITION;
            CLICK_X_MAX = SCR_WIDTH;

            CLICK_Y_MIN = Y_POSITION;
            CLICK_Y_MAX = Y_POSITION + REAL_Y_HEIGHT;

            for (int i = 0; i < MAX_TRAINS; i++) TrainStatus[i] = 0;
        }

        int TrainRegistryLeft, TrainRegistryTop, TrainRegistryButtonWidth = 25, TrainRegistryButtonHeight = 16, TrainRegistryButtonSpacing = 4;
        const int MAX_TRAINS = 30;
        public int[] TrainStatus = new int[MAX_TRAINS];

        public void drawDrawer(IntPtr screen, int width, int height, Surface satmap)
        {                        
            Sdl.SDL_Rect src, dst;
            src = new Sdl.SDL_Rect((short)(DRAWER_TAB_POS.X + 3), (short)(DRAWER_TAB_POS.Y), (short)(SCR_WIDTH - 28 - DRAWER_TAB_POS.X), (short)(SCR_HEIGHT - 37 - 24));
            if (DRAW_OPEN_AMOUNT > 0)
            {
                Sdl.SDL_FillRect(screen,ref src,dColor);

                Tao.Sdl.SdlGfx.hlineRGBA(screen, (short)(DRAWER_TAB_POS.X + 3), (short)(SCR_WIDTH - 24), (short)(Y_POSITION),0,0,0,255);
                Tao.Sdl.SdlGfx.hlineRGBA(screen, (short)(DRAWER_TAB_POS.X + 3), (short)(SCR_WIDTH - 26), (short)(SCR_HEIGHT - 37), 0, 0, 0, 255);
                Tao.Sdl.SdlGfx.hlineRGBA(screen, (short)((DRAWER_TAB_POS.X + 3) + (DRAWER_TAB_POS.Width - 8)), (short)(SCR_WIDTH - 26), (short)(SCR_HEIGHT - 37), 0, 0, 0, 255);

                if (satmap != null)
                {
                    src = new Sdl.SDL_Rect(0, 0, (short)satmap.surface.w, (short)satmap.surface.h);
                    dst = new Sdl.SDL_Rect((short)(DRAWER_TAB_POS.X + 25), (short)(DRAWER_TAB_POS.Y + 10), (short)((SCR_WIDTH-35) - DRAWER_TAB_POS.X), (short)satmap.surface.h);
                    Tao.Sdl.Sdl.SDL_BlitSurface(satmap.surfacePtr(), ref src, screen, ref dst);
                }

                TrainRegistryLeft = DRAWER_TAB_POS.X + 25;
                TrainRegistryTop = DRAWER_TAB_POS.Y + 200;

                SdlGfx.rectangleRGBA(screen, (short)TrainRegistryLeft, (short)TrainRegistryTop, (short)(TrainRegistryLeft + (5 * (TrainRegistryButtonWidth + TrainRegistryButtonSpacing)) + 3), (short)(TrainRegistryTop + (6 * (TrainRegistryButtonHeight + TrainRegistryButtonSpacing)) + 2), 204, 204, 168, 255);
                for (int curLine = 1; curLine < 6; curLine++)
                {
                    if (curLine < 5) SdlGfx.vlineRGBA(screen, (short)(TrainRegistryLeft + 1 + (curLine * (TrainRegistryButtonWidth + TrainRegistryButtonSpacing))), (short)(TrainRegistryTop), (short)(short)(TrainRegistryTop + (6 * (TrainRegistryButtonHeight + TrainRegistryButtonSpacing)) + 3), 204, 204, 168, 255);
                    SdlGfx.hlineRGBA(screen, (short)TrainRegistryLeft, (short)(TrainRegistryLeft + (5 * (TrainRegistryButtonWidth + TrainRegistryButtonSpacing)) + 1), (short)(short)(TrainRegistryTop + (curLine * (TrainRegistryButtonHeight + TrainRegistryButtonSpacing)) + 1), 204, 204, 168, 255);
                }

                for (int curTrain = 0; curTrain < MAX_TRAINS; curTrain++)
                {
                    short x1 = (short)((TrainRegistryLeft + 3) + ((curTrain % 5) * (TrainRegistryButtonWidth + TrainRegistryButtonSpacing)));
                    short y1 = (short)(TrainRegistryTop + 3 + ((curTrain / 5) * (TrainRegistryButtonHeight + TrainRegistryButtonSpacing)));
                    if (TrainStatus[curTrain] >= 1)
                    {
                        SdlGfx.rectangleRGBA(screen, x1, y1, (short)(x1 + TrainRegistryButtonWidth), (short)(y1 + TrainRegistryButtonHeight), 84, 84, 152, 255);
                        SdlGfx.hlineRGBA(screen, x1, (short)(x1 + TrainRegistryButtonWidth), (short)(y1 + TrainRegistryButtonHeight - 1), 84, 84, 152, 255);
                        SdlGfx.hlineRGBA(screen, x1, (short)(x1 + TrainRegistryButtonWidth), (short)(y1 + TrainRegistryButtonHeight - 2), 84, 84, 152, 255);
                        dst = new Sdl.SDL_Rect((short)(x1 + 1), (short)(y1 + 1), (short)(TrainRegistryButtonWidth - 1), (short)(TrainRegistryButtonHeight - 3));
                        if (TrainStatus[curTrain] == 2) 
                            Sdl.SDL_FillRect(screen, ref dst,0);
                        else if (TrainStatus[curTrain] == 3) 
                            Sdl.SDL_FillRect(screen, ref dst, 2000);
                        drawText(screen, freesans, curTrain.ToString(), 0, 0, 0, 255, x1 + 13, y1 - 3);
                    }
                    else drawText(screen, freesans, curTrain.ToString(), 204, 204, 168, 255, x1 + 13, y1 - 3);
                    
                }
            }

            src = new Sdl.SDL_Rect((short)DRAWER_TAB_POS.X,(short)DRAWER_TAB_POS.Y,(short)(DRAWER_TAB_POS.Width-1),(short)(height - 62));
            Tao.Sdl.Sdl.SDL_FillRect(screen,ref src,dColor);

            Tao.Sdl.SdlGfx.rectangleRGBA(screen, (short)DRAWER_TAB_POS.X,
                                                (short)DRAWER_TAB_POS.Y,
                                                (short)(DRAWER_TAB_POS.X + DRAWER_TAB_POS.Width - 1),
                                                (short)(((SCR_HEIGHT - 38))), 0, 0, 0, 255);
            


            DRAWER_TAB_POS.Y = Y_POSITION;

            src = drawer.clipSDLRect;
            dst = new Sdl.SDL_Rect((short)DRAWER_TAB_POS.X,(short)(DRAWER_TAB_POS.Y-1),src.w,src.h);
            Tao.Sdl.Sdl.SDL_BlitSurface(drawer.surfacePtr(), ref src, screen, ref dst);


            

            if (drawHighlight)
            {
                src = containerON.clipSDLRect;
                dst = new Sdl.SDL_Rect((short)(SCR_WIDTH - 25), (short)(DRAWER_TAB_POS.Y), src.w, src.h);
                Tao.Sdl.Sdl.SDL_BlitSurface(containerON.surfacePtr(), ref src, screen, ref dst);
            }
            else
            {
                src = container.clipSDLRect;
                dst = new Sdl.SDL_Rect((short)(SCR_WIDTH - 25), (short)(DRAWER_TAB_POS.Y), src.w, src.h);
                Tao.Sdl.Sdl.SDL_BlitSurface(container.surfacePtr(), ref src, screen, ref dst);
            }

            Tao.Sdl.SdlGfx.hlineRGBA(screen, (short)(SCR_WIDTH - 25), (short)(SCR_WIDTH), (short)(Y_POSITION),0,0,0,255);
            Tao.Sdl.SdlGfx.vlineRGBA(screen, (short)(SCR_WIDTH - 25), (short)(Y_POSITION + 100), (short)(SCR_HEIGHT - 42), 152, 152, 116, 255);


            Tao.Sdl.SdlGfx.pixelRGBA(screen, (short)(DRAWER_TAB_POS.X + 1), (short)(SCR_HEIGHT - 40),0,0, 0, 255);
            Tao.Sdl.SdlGfx.pixelRGBA(screen, (short)(DRAWER_TAB_POS.X + 1), (short)(SCR_HEIGHT - 39), 0, 0, 0, 255);
            Tao.Sdl.SdlGfx.pixelRGBA(screen, (short)(DRAWER_TAB_POS.X + 2), (short)(SCR_HEIGHT - 39), 0,0, 0, 255);
            /*

            Color[,] cols = new Color[2, 2];
            cols[0, 0] = Color.Black;
            cols[0, 1] = Color.Black;
            cols[1, 0] = drawerColor;
            cols[1, 1] = Color.Black;
            screen.SetPixels(new Point(DRAWER_TAB_POS.X + 1, SCR_HEIGHT - 39), cols);*/
        }
    }
}
