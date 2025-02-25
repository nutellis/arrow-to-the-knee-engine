using System;

namespace Shard
{
    public class Sprite
    {
        public string path;
        public float X, Y;
        public float scale;
        private bool isAnimating = false;


        public Sprite(string path)
        {
            this.path = path; //save file path
            loadTexture(); //load in texture
        }

        //Loads texture from file path
        private void loadTexture()
        {
            Console.WriteLine("Loading sprite from: " + path); //placeholder
        }

        //Draw the sprite on the screen
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
            return texture?.Width ?? 0;
        }

        public int getHeight()
        {
            return texture?.Height ?? 0;
        }

        //Change scale of sprite
        public void setScale(float scale)
        {
            this.scale = scale;
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

