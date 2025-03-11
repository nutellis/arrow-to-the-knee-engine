using SDL2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using Shard.Shard.Components;
using Shard.Shard;
using System.Reflection;

namespace Shard
{
    class SpriteManager
    {
        private static SpriteManager me;
        Dictionary<string, Sprite> sprites;
        Dictionary<string, List<Sprite>> animations;

        private SpriteManager()
        {
            sprites = new Dictionary<string, Sprite>();
            animations = new Dictionary<string, List<Sprite>>();
        }

        public static SpriteManager getInstance()
        {
            if (me == null)
            {
                me = new SpriteManager();
            }

            return me;
        }

        public Sprite getSprite(string spriteName, string filePath)
        {
            if (sprites.TryGetValue(spriteName, out Sprite value))
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

                string absolutePath = Bootstrap.getAssetManager().getAssetPath(filePath);

                if (absolutePath == null)
                {
                    Console.WriteLine($"Failed to Load Sprite {filePath}");
                    return null;
                }

                loadedImage = SDL_image.IMG_Load(absolutePath);

                Debug.getInstance().log("IMG_Load: " + SDL_image.IMG_GetError());

                img = Bootstrap.getDisplay().loadTexture(loadedImage);

                SDL.SDL_QueryTexture(img, out format, out access, out w, out h);

                Sprite newSprite = new Sprite(spriteName);

                newSprite.height = h;
                newSprite.width = w;
                newSprite.texture = img;
                newSprite.surface = loadedImage;

                // i am not checking if the sprite or the img is null ( •̀ᴗ•́)و ☜╗(• ᴥ • ) good luck with the crash.

                sprites[spriteName] = newSprite;

                return (Sprite)newSprite.Clone();
            }
        }

        public void loadSpriteSheet(string spriteSheetName, string spriteSheetPath, string jsonFileName, float scale = 1.0f)
        {
            string jsonAssetPath = Bootstrap.getAssetManager().getAssetPath(jsonFileName);

            Sprite spriteSheet = getSprite(spriteSheetName, spriteSheetPath);
            if (spriteSheet == null)
                return;

            IntPtr spriteSheetPtr = spriteSheet.surface;

            string json = File.ReadAllText(jsonAssetPath);
            var animationData = JsonSerializer.Deserialize<AnimationData>(json);

            if (animationData != null && animationData.animations != null)
            {
                foreach (var animation in animationData.animations)
                {
                    var index = 0;
                    List<Sprite> frames = new List<Sprite>();
                    foreach (var frame in animation.animationFrames)
                    { 
                        Sprite sprite = extractSprite(
                                spriteSheetPtr,
                                frame.x,
                                frame.y,
                                frame.width,
                                frame.height,
                                animation.animationName + "_" + index
                        );
                        if (sprite != null)
                        {
                            sprite.setUniformScale(scale);

                            frames.Add(sprite);
                        }
                        index += 1;
                    }
                    animations[animation.animationName] = frames;
                }
            }
        }

        public Sprite extractSprite(IntPtr spriteSheet, int startX, int startY, int width, int height, string spriteName)
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

                var result = Bootstrap.getDisplay().loadTextureFromPixels(spritePixels, width, height);

                SDL.SDL_QueryTexture(result.Item1, out format, out access, out w, out h);

                Sprite newSprite = new Sprite(spriteName);

                newSprite.height = h;
                newSprite.width = w;
                newSprite.texture = result.Item1;
                newSprite.surface = result.Item2;

                // same. No null checks for us here 〵(″⚈╭╮⚈)ノ

                return newSprite;
            }

        public List<Sprite> getAnimation(string animationName)
        {
            if(animations.TryGetValue(animationName, out List<Sprite> animation))
            {
                return animation;
            }
            return null;
        }
    }
}
