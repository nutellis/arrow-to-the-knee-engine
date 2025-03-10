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
        public Sprite sprite; //reference to sprite object
        private List<Sprite> animationFrames; //replace sprite with animationFrames as teh default
        private int currentFrame;
        private double frameTimer;
        private float frameDuration = 1.0f;
        public bool hasAnimation = false;

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
            frameTimer += Bootstrap.getDeltaTime();
            //if (frameTimer >= frameDuration)
            //{
            //    frameTimer = 0;
            //    currentFrame = (currentFrame + 1) % animationFrames.Count;
            //    sprite = animationFrames[currentFrame];
            //}

            sprite.X = owner.transform.X;
            sprite.Y = owner.transform.Y;

            Bootstrap.getDisplay().addToDraw(this.sprite);
        }

        public void addSprite(string assetName)
        {
            Sprite frame = Bootstrap.getAssetManager().getSprite(assetName);
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
    }

}

