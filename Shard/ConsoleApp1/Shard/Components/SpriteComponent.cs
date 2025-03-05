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
        private bool isMoving = false;

        public SpriteComponent(GameObject owner,bool hasAnimation = false) : base(owner)
        {
            this.hasAnimation = hasAnimation;
            animationFrames = new List<Sprite>();
        }

        public override void initialize()
        {
        }

        public override void update()
        {
            if (isMoving && hasAnimation)
            {
                frameTimer += Bootstrap.getDeltaTime();
                if (frameTimer >= frameDuration)
                {
                    frameTimer = 0;
                    currentFrame = (currentFrame + 1) % animationFrames.Count;
                    sprite = animationFrames[currentFrame];
                }
            }

            sprite.setPosition(owner.transform.X, owner.transform.Y);
            Bootstrap.getDisplay().addToDraw(this.sprite);
        }

        private Sprite loadSprite(string spritePath)
        {

            string assetPath = Bootstrap.getAssetManager().getAssetPath(spritePath);

            if(assetPath == null)
            {
                Console.WriteLine($"Failed to Load Sprite {spritePath}");
                return null;
            }

            return Bootstrap.getAssetManager().getSprite(assetPath);
        }

        public void addSprite(string newAssetName)
        {
            Sprite frame = loadSprite(newAssetName);
            if (frame != null)
            {
                animationFrames.Add(frame);
                if (animationFrames.Count == 1)
                {
                    sprite = frame;
                    sprite.setPosition(owner.transform.X, owner.transform.Y);
                    owner.transform.Wid = sprite.getWidth();
                    owner.transform.Ht = sprite.getHeight();
                    owner.transform.recalculateCentre();
                }
            }
        }

        public void changeColor(float r, float g, float b, float a)
        {
            if (sprite != null)
            {
                sprite.changeColor(r, g, b, a);
            }

        }

        internal void setSprite(int spriteToUse)
        {
            sprite = animationFrames[spriteToUse];
        }

        public void startAnimation()
        {
            if(!sprite.isAnimationPlaying())
            {
                sprite.animate();
                isMoving = true;

            }

        }

        public void stopAnimation()
        {
            if(sprite.isAnimationPlaying())
            {
                sprite.stopAnimate();
                isMoving = false;
                setSprite(0);
            }
        }
    }

}

