using SDL2;
using System;

namespace Shard
{
    public class Sprite: ICloneable
    {
        public string path;
        public IntPtr img;
        public float X, Y;
        public float rotz;
        public float scaleX = 1, scaleY = 1;
        public int width, height;
        private bool isAnimating = false;


        public Sprite(string path)
        {
            this.path = path;

            // Load image from asset manager
            string assetPath = Bootstrap.getAssetManager().getAssetPath(path);
            if (assetPath == null)
            {
                Console.WriteLine($"Failed to load sprite from path: {path}");
                return;
            }

            Sprite loadedSprite = Bootstrap.getAssetManager().getSprite(assetPath);
            if (loadedSprite != null)
            {
                this.img = loadedSprite.img;
                this.width = loadedSprite.getWidth();
                this.height = loadedSprite.getHeight();
            }
            else
            {
                Console.WriteLine($"Sprite not found at {path}");
            }
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

        public IntPtr getTexture()
        {
            return img;
        }

        //Change scale of sprite
        public void setUniformScale(float scale)
        {
            this.scaleX = scale;
            this.scaleY = scale;

            //Minor priority
            //TODO: actually scale the img when the scale is changed
            this.width = (int)(this.width * scale);
            this.height = (int)(this.height * scale);
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

        public void stopAnimate()
        {
            isAnimating = false;
            Console.WriteLine($"Stopped animating sprite {path}"); //placeholder
        }

        public bool isAnimationPlaying()
        {
            return isAnimating;
        }

        //Plays the animation once.
        public void repeatOnce()
        {
            isAnimating = true;
            Console.WriteLine($"Playing sprite animation {path} once"); //placeholder
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        //public Sprite getFrame(int x, int y, int frameWidth, int frameHeight)
        //{
        //    // Get the renderer from DisplaySDL
        //    DisplaySDL display = (DisplaySDL)Bootstrap.getDisplay();
        //    IntPtr renderer = display.getRenderer();  // You may need to create this method

        //    if (renderer == IntPtr.Zero)
        //    {
        //        Console.WriteLine("Renderer not available.");
        //        return null;
        //    }

        //    // Create a new sprite for the cropped frame
        //    Sprite frame = new Sprite(this.path)
        //    {
        //        width = frameWidth,
        //        height = frameHeight
        //    };

        //    // Create a new texture for the cropped frame
        //    IntPtr newTexture = SDL.SDL_CreateTexture(
        //        renderer,
        //        SDL.SDL_PIXELFORMAT_RGBA8888,
        //        (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET,
        //        frameWidth, frameHeight
        //    );

        //    SDL.SDL_SetRenderTarget(renderer, newTexture);

        //    SDL.SDL_Rect srcRect = new SDL.SDL_Rect { x = x, y = y, w = frameWidth, h = frameHeight };
        //    SDL.SDL_Rect dstRect = new SDL.SDL_Rect { x = 0, y = 0, w = frameWidth, h = frameHeight };

        //    SDL.SDL_RenderCopy(renderer, this.img, ref srcRect, ref dstRect);

        //    SDL.SDL_SetRenderTarget(renderer, IntPtr.Zero);

        //    frame.img = newTexture;
        //    return frame;
        //}
    }
}

