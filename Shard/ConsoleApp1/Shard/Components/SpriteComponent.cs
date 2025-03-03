using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;


namespace Shard.Shard.Components
{
    internal class SpriteComponent : BaseComponent
    {
        private Sprite sprite; //reference to sprite object
        private List<Sprite> animationFrames; //replace sprite with animationFrames as teh default
        private int currentFrame;
        private double frameTimer;
        private float frameDuration = 0.1f;
        public bool hasAnimation = false;

        public SpriteComponent(GameObject owner,bool hasAnimation = false) : base(owner)
        {
            this.hasAnimation = hasAnimation;
        }

        public override void initialize()
        {
            base.initialize();
        }

        public override void update()
        {
            frameTimer += Bootstrap.getDeltaTime();
            if (frameTimer >= frameDuration)
            {
                frameTimer = 0;
                currentFrame = (currentFrame + 1) % animationFrames.Count;
                sprite = animationFrames[currentFrame];
            }
            Bootstrap.getDisplay().addToDraw(this.sprite);
        }

        private void loadSprite(string spritePath)
        {

            string assetPath = Bootstrap.getAssetManager().getAssetPath(spritePath);

            if(assetPath == null)
            {
                Console.WriteLine($"Failed to Load Sprite {spritePath}");
            }

            sprite = Bootstrap.getAssetManager().getSprite(assetPath); 

            if (sprite == null) 
            {
                Console.WriteLine($"Failed to Load Sprite {spritePath}");
            }
        }

        public void setSprite(string newAssetName)
        {
            loadSprite(newAssetName);
        }

        public void changeColor(float r, float g, float b, float a)
        {
            if (sprite != null)
            {
                sprite.changeColor(r, g, b, a);
            }

        }

    }

}

