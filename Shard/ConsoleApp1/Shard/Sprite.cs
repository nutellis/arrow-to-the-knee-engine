using System;

namespace Shard
{
    public class Sprite
    {
        public string path;
        public IntPtr img;
        public float X, Y;
        public float rotz;
        public float scaleX, scaleY;
        public int width, height;
        private bool isAnimating = false;


        public Sprite(string path)
        {
            this.path = path;
        }

        //Loads texture from file path
        // TODO: move DisplaySDL loadTexture here!
        private void loadTexture()
        {
            Console.WriteLine("Loading sprite from: " + path); //placeholder
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
            Console.WriteLine($"Animating sprite {path}"); //placeholder
        }

        //Plays the animation once.
        public void repeatOnce()
        {
            isAnimating = true;
            Console.WriteLine($"Playing sprite animation {path} once"); //placeholder
        }

    }
}

