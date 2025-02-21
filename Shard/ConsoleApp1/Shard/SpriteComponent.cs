using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;


namespace Shard.Shard.Components
{
    public class SpriteComponent
    {
        private Sprite sprite;
        private string assetName;

        public SpriteComponent(string assetName_, )
        {
            assetName = assetName_;
            loadSprite(assetName);
        }

        private void loadSprite()
        {
            sprite = Bootstrap.getAssetManager(). //More code, should be something like getSprite(assetName)

            if (sprite == null)
            {
                Console.WriteLine($"Failed to Load Sprite {assetName}");
            }

        }

        public void setSprite(string newAssetName)
        {
            assetName = newAssetName;
            loadSprite(assetName);

        }

        public void changeColor(float r, float g, float b, float a)
        {
            if (sprite != null)
            {
                sprite.changeColor(r, g, b, a);
            }

        }

        public void draw()
        {
            if (sprite != null)
            {
                sprite.setPosition();
                sprite.draw();
            }

        }
    }


}

