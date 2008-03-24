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
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Tao.Sdl;
using FreeTrain.Framework.Graphics;

namespace FreeTrain
{
    class SDLGUIButton
    {
        private int x_pos, y_pos;
        Surface btnon, btnoff;
        private string button_name, tooltip_text;
        bool button_down = false, draw_tooltip = false;

        public EventHandler ButtonClick;

        IntPtr fnt;

        public SDLGUIButton(string name, string tooltip, int x, int y, ref EventHandler click)
        {
            tooltip_text = tooltip;
            x_pos = x;
            y_pos = y;

            ButtonClick += click;

            btnoff = new Surface(Path.Combine(Application.StartupPath, Path.Combine(SDLGUI.GuiDirectory, name + ".png")));
            btnon = new Surface(Path.Combine(Application.StartupPath, Path.Combine(SDLGUI.GuiDirectory, name + "_down.png")));

            fnt = SdlTtf.TTF_OpenFont(Path.Combine(Application.StartupPath, Path.Combine(SDLGUI.GuiDirectory, "FreeSans.ttf")), 12);

            button_name = name;
        }

        public void draw(IntPtr screen)
        {
            Sdl.SDL_Rect dst = new Sdl.SDL_Rect((short)x_pos, (short)y_pos, btnon.clipSDLRect.w, btnon.clipSDLRect.h);
            Sdl.SDL_Rect src = btnon.clipSDLRect;

            if (button_down) Tao.Sdl.Sdl.SDL_BlitSurface(btnon.surfacePtr(), ref src, screen, ref dst);
            else Tao.Sdl.Sdl.SDL_BlitSurface(btnoff.surfacePtr(), ref src, screen, ref dst);

            //TOOL TIPS!?!?!
            if (draw_tooltip)
            {
                SDLGUI.drawText(screen, fnt, tooltip_text, Color.White, new Point(x_pos, y_pos + btnon.clipRect.Height + 6), 0, true, false);
            }
        }

        public void checkMovement(Sdl.SDL_MouseMotionEvent e)
        {
            draw_tooltip = false;
            button_down = false;
            if (e.x >= x_pos && e.x <= (x_pos + btnon.clipSDLRect.w))
                if (e.y >= y_pos && e.y <= (y_pos + btnon.clipSDLRect.h))
                {
                    draw_tooltip = true;
                }

        }

        public bool checkClick(Sdl.SDL_MouseButtonEvent e)
        {
            bool wasGUIClick = false;
            button_down = false;
            int x = e.x, y = e.y;
            if (e.x >= x_pos && e.x <= (x_pos + btnon.clipSDLRect.w))
            {
                if (e.y >= y_pos && e.y <= (y_pos + btnon.clipSDLRect.h))
                {
                    if (e.button == Sdl.SDL_BUTTON_LEFT)
                    {
                        button_down = true;
                        if (ButtonClick != null) ButtonClick(button_name, null);
                    }
                    wasGUIClick = true;
                }
            }
            return wasGUIClick;
        }
    }
}
