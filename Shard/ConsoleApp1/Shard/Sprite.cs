using SDL2;
using System;

namespace Shard
{
    public class Sprite: ICloneable
    {
        public string spriteName;
        public IntPtr texture, surface;
        public float X, Y;
        public float local_x, local_y;
        public float rotz;
        public float scaleX = 1, scaleY = 1;
        public int width, height;
        public bool canCollide = false;

        public Sprite(string assetName)
        {
            spriteName = assetName;
        }

        //Updates position of sprite
        public void setWorldPosition(float x, float y)
        {
            X = x + local_x;
            Y = y + local_y;
        }

        public void setLocalPosition(float x, float y)
        {
           local_x = x;
           local_y = y;
        }

        public int getWidth()
        {
            return width;
        }

        public int getHeight()
        {
            return height;
        }

        public IntPtr getTexture()
        {
            return texture;
        }

        //Change scale of sprite
        public void setUniformScale(float scale)
        {
            this.scaleX = scale;
            this.scaleY = scale;

            this.width = (int)(this.width * scaleX);
            this.height = (int)(this.height * scaleY);

        }

        //Change color of sprite
        public void changeColor(float r, float g, float b, float a)
        {
            Console.WriteLine($"Changing sprite {spriteName} color to RGBA({r}, {g}, {b}, {a})"); //placeholder
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void Dispose()
        {
            if (texture != IntPtr.Zero)
            {
                SDL.SDL_DestroyTexture(texture);
                texture = IntPtr.Zero;
            }
        }
    }
}

