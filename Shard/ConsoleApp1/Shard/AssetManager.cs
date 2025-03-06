using SDL2;
using Shard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    class AssetManager : AssetManagerBase
    {

        Dictionary<string,string> assets;
        Dictionary<string,Sprite> sprites;

        public AssetManager()
        {
            assets = new Dictionary<string,string>();
            sprites = new Dictionary<string,Sprite>();

            AssetPath = Bootstrap.getEnvironmentalVariable ("assetpath");
        }

        public override void registerAssets() {
            assets.Clear();
            walkDirectory(AssetPath);
        }

        public string getName (string path) {
            string[] bits = path.Split ("\\");
            
            return bits[bits.Length - 1];
        }

        public override string getAssetPath (string asset) {
            if (assets.ContainsKey (asset)) {
                return assets[asset];
            }

            Debug.Log ("No entry for " + asset);

            return null;
        }

        public void walkDirectory (string dir) {
            string[] files = Directory.GetFiles(dir);
            string[] dirs = Directory.GetDirectories (dir);

            foreach (string d in dirs) {
                walkDirectory (d);
            }

            foreach (string f in files) {
                string filename_raw = getName(f);
                string filename = filename_raw;
                int counter = 0;

                Console.WriteLine ("Filename is " + filename);

                while (assets.ContainsKey (filename)) {
                    counter += 1;
                    filename = filename_raw + counter;
                }

                assets.Add (filename, f);
                Console.WriteLine ("Adding " + filename + " : " + f);
            }

        }

        //TODO: create an IntPtr (img) from the path and store it for future use.
        public override Sprite getSprite(string assetName)
        {
            if (sprites.TryGetValue(assetName, out Sprite value)) {
                return (Sprite)value.Clone();
            } else {
                IntPtr loadedImage, img;
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

                sprites[assetName] = newSprite;

                return (Sprite)newSprite.Clone();
            }
        }
    }
}
