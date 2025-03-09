using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    abstract class AssetManagerBase
    {
        private String assetPath;

        public string AssetPath { get; set; }

        public abstract void registerAssets();
        public abstract string getAssetPath(string asset);
        public abstract Sprite getSprite(string asset);
        public abstract Sprite extractSprite(IntPtr spriteSheet, int startX, int startY, int width, int height, string spriteName);
    }

}
