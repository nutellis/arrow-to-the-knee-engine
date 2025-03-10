using SDL2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Shard
{
    class AssetManager : AssetManagerBase
    {

        Dictionary<string, string> assets;
        Dictionary<string, Sprite> sprites;

        public AssetManager()
        {
            assets = new Dictionary<string, string>();
            sprites = new Dictionary<string, Sprite>();

            AssetPath = Bootstrap.getEnvironmentalVariable("assetpath");
        }

        public override void registerAssets()
        {
            assets.Clear();
            walkDirectory(AssetPath);
        }

        public string getName(string path)
        {
            string[] bits = path.Split("\\");

            return bits[bits.Length - 1];
        }

        public override string getAssetPath(string asset)
        {
            if (assets.ContainsKey(asset))
            {
                return assets[asset];
            }

            Debug.Log("No entry for " + asset);

            return null;
        }

        public void walkDirectory(string dir)
        {
            string[] files = Directory.GetFiles(dir);
            string[] dirs = Directory.GetDirectories(dir);

            foreach (string d in dirs)
            {
                walkDirectory(d);
            }

            foreach (string f in files)
            {
                string filename_raw = getName(f);
                string filename = filename_raw;
                int counter = 0;

                Console.WriteLine("Filename is " + filename);

                while (assets.ContainsKey(filename))
                {
                    counter += 1;
                    filename = filename_raw + counter;
                }

                assets.Add(filename, f);
                Console.WriteLine("Adding " + filename + " : " + f);
            }

        }

        public override Sprite getSprite(string assetName)
        {
            if (sprites.TryGetValue(assetName, out Sprite value))
            {
                return (Sprite)value.Clone();
            }
            else
            {
                IntPtr img, loadedImage;
                uint format;
                int access;
                int w;
                int h;

                string absolutePath = Bootstrap.getAssetManager().getAssetPath(assetName);

                if (absolutePath == null)
                {
                    Console.WriteLine($"Failed to Load Sprite {assetName}");
                    return null;
                }

                loadedImage = SDL_image.IMG_Load(absolutePath);

                Debug.getInstance().log("IMG_Load: " + SDL_image.IMG_GetError());

                img = Bootstrap.getDisplay().loadTexture(loadedImage);

                SDL.SDL_QueryTexture(img, out format, out access, out w, out h);

                Sprite newSprite = new Sprite(assetName);

                newSprite.height = h;
                newSprite.width = w;
                newSprite.img = img;
                newSprite.surface = loadedImage;

                // i am not checking if the sprite or the img is null ( •̀ᴗ•́)و ☜╗(• ᴥ • ) good luck with the crash.

                sprites[assetName] = newSprite;

                return (Sprite)newSprite.Clone();
            }
        }
        public override Sprite extractSprite(IntPtr spriteSheet, int startX, int startY, int width, int height, string spriteName)
        {
            if (sprites.TryGetValue(spriteName, out Sprite value))
            {
                return (Sprite)value.Clone();
            }
            else
            {

                SDL.SDL_LockSurface(spriteSheet); // Lock the surface before accessing pixels

                // get surface properties
                SDL.SDL_Surface surface = Marshal.PtrToStructure<SDL.SDL_Surface>(spriteSheet);

                // do some checks before allocating more memory
                if (startX + width > surface.w || startY + height > surface.h)
                {
                    Debug.getInstance().log("Sprite extraction out of bounds");
                    return null;
                }

                SDL.SDL_PixelFormat pixelFormat = Marshal.PtrToStructure<SDL.SDL_PixelFormat>(surface.format);

                int bytesPerPixel = pixelFormat.BytesPerPixel;
                int pitch = surface.pitch;

                byte[] spritePixels = new byte[width * height * bytesPerPixel];

                unsafe
                {
                    byte* pixels = (byte*)surface.pixels;

                    for (int y = 0; y < height; y++)
                    {
                        int srcIndex = ((startY + y) * pitch) + (startX * bytesPerPixel);
                        int destIndex = y * width * bytesPerPixel;

                        for (int x = 0; x < width * bytesPerPixel; x++)
                        {
                            spritePixels[destIndex + x] = pixels[srcIndex + x];
                        }
                    }
                }

                SDL.SDL_UnlockSurface(spriteSheet); // Unlock the surface

                IntPtr img = IntPtr.Zero;
                uint format;
                int access;
                int w;
                int h;

                img = Bootstrap.getDisplay().loadTextureFromPixels(spritePixels, width, height);

                SDL.SDL_QueryTexture(img, out format, out access, out w, out h);

                Sprite newSprite = new Sprite(spriteName);

                newSprite.height = h;
                newSprite.width = w;
                newSprite.img = img;

                // same. No null checks for us here 〵(″⚈╭╮⚈)ノ

                sprites[spriteName] = newSprite;

                return (Sprite)newSprite.Clone();
            }
        }
    }
}
