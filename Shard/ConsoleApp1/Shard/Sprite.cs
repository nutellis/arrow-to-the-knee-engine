using System;

namespace Shard
{
    public class Sprite: ICloneable
    {
        public string spriteName;
        public IntPtr img;
        public float X, Y;
        public float rotz;
        public float scaleX = 1, scaleY = 1;
        public int width, height;
        private bool isAnimating = false;


        public Sprite(string assetName)
        {
            spriteName = assetName;
        }

        //Updates position of sprite
        public void setPosition(float x, float y)
        {
            X = x;
            Y = y;
        }

        public int getWidth()
        {
            return width;
        }

        public int getHeight()
        {
            return height;
        }

        //Change scale of sprite
        public void setUniformScale(float scale)
        {
            this.scaleX = scale;
            this.scaleY = scale;

            //Minor priority
            //TODO: actually scale the img when the scale is changed
        }

        //Change color of sprite
        public void changeColor(float r, float g, float b, float a)
        {
            Console.WriteLine($"Changing sprite {path} color to RGBA({r}, {g}, {b}, {a})"); //placeholder
        }

        //Start animation for sprite
        public void animate()
        {
            isAnimating = true;
            Console.WriteLine($"Animating sprite {spriteName}"); //placeholder
        }

        //Plays the animation once.
        public void repeatOnce()
        {
            isAnimating = true;
            Console.WriteLine($"Playing sprite animation {spriteName} once"); //placeholder
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}

