/*
*
*   This is the implementation of the Simple Directmedia Layer through C#.   This isn't a course on 
*       graphics, so we're not going to roll our own implementation.   If you wanted to replace it with 
*       something using OpenGL, that'd be a pretty good extension to the base Shard engine.
*       
*   Note that it extends from DisplayText, which also uses SDL.  
*   
*   @author Michael Heron
*   @version 1.0
*   
*/

using SDL2;
using Shard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Shard
{

    class Line
    {
        private int sx, sy;
        private int ex, ey;
        private int r, g, b, a;

        public int Sx { get => sx; set => sx = value; }
        public int Sy { get => sy; set => sy = value; }
        public int Ex { get => ex; set => ex = value; }
        public int Ey { get => ey; set => ey = value; }
        public int R { get => r; set => r = value; }
        public int G { get => g; set => g = value; }
        public int B { get => b; set => b = value; }
        public int A { get => a; set => a = value; }
    }

    class Circle
    {
        int x, y, rad;
        private int r, g, b, a;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int Radius { get => rad; set => rad = value; }
        public int R { get => r; set => r = value; }
        public int G { get => g; set => g = value; }
        public int B { get => b; set => b = value; }
        public int A { get => a; set => a = value; }
    }


    class DisplaySDL : DisplayText
    {
        private List<Sprite> _toDraw;

        private List<Line> _linesToDraw;
        private List<Circle> _circlesToDraw;
        private Dictionary<string, IntPtr> spriteBuffer;
        public override void initialize()
        {
            spriteBuffer = new Dictionary<string, IntPtr>();

            base.initialize();

            _toDraw = new List<Sprite>();
            _linesToDraw = new List<Line>();
            _circlesToDraw = new List<Circle>();


        }

        public override IntPtr loadTexture(IntPtr loadedImage)
        {
            IntPtr result;

            result = SDL.SDL_CreateTextureFromSurface(_rend, loadedImage);

            SDL.SDL_SetTextureBlendMode(result, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);

            return result;

        }

        public override void addToDraw(Sprite sprite)
        {
            //TODO: gob.SpriteComponent.Sprite
            _toDraw.Add(sprite);

            if (sprite.path == null)
            {
                return;
            }
            //if the sprite exists and it is valid, try and see if it is contained on the draw buffer
            // if its already there return the buffer
            if (spriteBuffer.ContainsKey(sprite.path))
            {
                return;
            }

            spriteBuffer[sprite.path] = sprite.img;

            SDL.SDL_SetTextureBlendMode(spriteBuffer[sprite.path], SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);

            //return spriteBuffer[sprite.path];

        }

        public override void removeToDraw(Sprite sprite)
        {
            _toDraw.Remove(sprite);
        }

        //public override void removeToDraw(GameObject gob)
        //{
        //    _toDraw.Remove(gob.transform);
        //}


        void renderCircle(int centreX, int centreY, int rad)
        {
            int dia = (rad * 2);
            byte r, g, b, a;
            int x = (rad - 1);
            int y = 0;
            int tx = 1;
            int ty = 1;
            int error = (tx - dia);

            SDL.SDL_GetRenderDrawColor(_rend, out r, out g, out b, out a);

            // We draw an octagon around the point, and then turn it a bit.  Do 
            // that until we have an outline circle.  If you want a filled one, 
            // do the same thing with an ever decreasing radius.
            while (x >= y)
            {

                SDL.SDL_RenderDrawPoint(_rend, centreX + x, centreY - y);
                SDL.SDL_RenderDrawPoint(_rend, centreX + x, centreY + y);
                SDL.SDL_RenderDrawPoint(_rend, centreX - x, centreY - y);
                SDL.SDL_RenderDrawPoint(_rend, centreX - x, centreY + y);
                SDL.SDL_RenderDrawPoint(_rend, centreX + y, centreY - x);
                SDL.SDL_RenderDrawPoint(_rend, centreX + y, centreY + x);
                SDL.SDL_RenderDrawPoint(_rend, centreX - y, centreY - x);
                SDL.SDL_RenderDrawPoint(_rend, centreX - y, centreY + x);

                if (error <= 0)
                {
                    y += 1;
                    error += ty;
                    ty += 2;
                }

                if (error > 0)
                {
                    x -= 1;
                    tx += 2;
                    error += (tx - dia);
                }

            }
        }

        public override void drawCircle(int x, int y, int rad, int r, int g, int b, int a)
        {
            Circle c = new Circle();

            c.X = x;
            c.Y = y;
            c.Radius = rad;

            c.R = r;
            c.G = g;
            c.B = b;
            c.A = a;

            _circlesToDraw.Add(c);
        }
        public override void drawLine(int x, int y, int x2, int y2, int r, int g, int b, int a)
        {
            Line l = new Line();
            l.Sx = x;
            l.Sy = y;
            l.Ex = x2;
            l.Ey = y2;

            l.R = r;
            l.G = g;
            l.B = b;
            l.A = a;

            _linesToDraw.Add(l);
        }

        public override void display()
        {

            SDL.SDL_Rect sRect;
            SDL.SDL_Rect tRect;

            foreach (Sprite sprite in _toDraw)
            {

                if (sprite.path == null)
                {
                    continue;
                }

                sRect.x = 0;
                sRect.y = 0;
                sRect.w = (int)(sprite.width * sprite.scaleX);
                sRect.h = (int)(sprite.height * sprite.scaleY);

                tRect.x = (int)sprite.X;
                tRect.y = (int)sprite.Y;
                tRect.w = sRect.w;
                tRect.h = sRect.h;

                SDL.SDL_RenderCopyEx(_rend, sprite.img, ref sRect, ref tRect, (int)sprite.rotz, IntPtr.Zero, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
            }

            foreach (Circle c in _circlesToDraw)
            {
                SDL.SDL_SetRenderDrawColor(_rend, (byte)c.R, (byte)c.G, (byte)c.B, (byte)c.A);
                renderCircle(c.X, c.Y, c.Radius);
            }

            foreach (Line l in _linesToDraw)
            {
                SDL.SDL_SetRenderDrawColor(_rend, (byte)l.R, (byte)l.G, (byte)l.B, (byte)l.A);
                SDL.SDL_RenderDrawLine(_rend, l.Sx, l.Sy, l.Ex, l.Ey);
            }

            // Show it off.
            base.display();

        }

        public override void clearDisplay()
        {

            _toDraw.Clear();
            _circlesToDraw.Clear();
            _linesToDraw.Clear();

            base.clearDisplay();
        }

    }


}
