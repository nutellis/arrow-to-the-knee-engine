using SDL2;
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

        public AssetManager()
        {
            assets = new Dictionary<string,string>();
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
        public override Sprite getSprite(string asset)
        {
            Sprite newSprite = new Sprite(asset);

            IntPtr img;
            uint format;
            int access;
            int w;
            int h;

            img = SDL_image.IMG_Load(asset);

            SDL.SDL_QueryTexture(img, out format, out access, out w, out h);

            newSprite.path = asset;
            newSprite.img = img;
            newSprite.height = h;
            newSprite.width = w;

            //TODO: lets keep that here for now. We will have to do changes on the game object later
            //trans.recalculateCentre();

            Debug.getInstance().log("IMG_Load: " + SDL_image.IMG_GetError());

            return newSprite;
        }
    }
}
