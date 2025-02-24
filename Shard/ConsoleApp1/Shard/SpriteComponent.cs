using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;


namespace Shard.Shard.Components
{
    public class SpriteComponent : Component
    {
        private Sprite sprite; //reference to sprite object
        public string spritePath { get; set; }

        public SpriteComponent(string spritePath)
        {
            spritePath = spritePath;
        }

        public override void initialize()
        {
            if(!string.IsNullOrEmpty(spritePath))
            {
                loadSprite();
            }
        }

        public override void update()
        {
            if (sprite != null)
            {
                sprite.animate;
            }
        }

        private void loadSprite()
        {
            string spriteFilePath = Bootstrap.getAssetManager().getAssetPath(SpritePath);  //Should change code, should be something like getSprite() I believe?

            if (!string.IsNullOrEmpty(spriteFilePath)
            {
                sprite = new Sprite(spriteFilePath);
            }
            else 
            {
                Console.WriteLine($"Failed to Load Sprite {spritePath}");
            }
            

        }

        public void setSprite(string newAssetName)
        {
            spritePath = newAssetName;
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

