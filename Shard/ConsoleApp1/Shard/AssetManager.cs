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



    }
}
