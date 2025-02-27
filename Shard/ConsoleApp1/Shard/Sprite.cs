using System;

namespace Shard
{
    public class Sprite
    {
        public string path;
        public IntPtr img;
        public float X, Y;
        public float scaleX, scaleY;
        public float width, height;
        private bool isAnimating = false;


        public Sprite()
        {
            
        }

        //Loads texture from file path
        // TODO: move DisplaySDL loadTexture here!
        private void loadTexture()
        {
            Console.WriteLine("Loading sprite from: " + path); //placeholder
        }

        //Draw the sprite on the screen
        //TODO: not needed for now comment it
        public void draw()
        {
            Console.WriteLine($"Drawing sprite {path} at ({X}, {Y}) with scale {scale}"); //placeholder
        }


        //Updates position of sprite
        public void setPosition(float x, float y)
        {
            X = x;
            Y = y;
          
        }

        public int getWidth()
        {
            return width ?? 0.0;
        }

        public int getHeight()
        {
            return height ?? 0.0;
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

