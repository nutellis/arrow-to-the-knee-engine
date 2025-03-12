using System.Collections.Generic;


namespace Shard.Shard.Components
{
    public class SpriteComponent : BaseComponent
    {
        private Sprite currentSprite;
        private Dictionary<string, List<Sprite>> animations; 
        private Dictionary<string, Sprite> sprites; 
        private string currentAnimation = "";
        private int currentFrameIndex;
        private double frameTimer; 
        private float frameDuration = 0.2f; 

        private bool hasAnimation = false;
        private List<Sprite> animationFrames;

        private bool isVisible;
        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }

        public SpriteComponent(GameObject owner) : base(owner)
        {
            animations = new Dictionary<string, List<Sprite>>();
            sprites = new Dictionary<string, Sprite>();

            isVisible = true;
        }

        public override void initialize()
        {
        }

        public override void update()
        {
            if (animations.TryGetValue(currentAnimation, out List<Sprite> animation))
            {
                // Update the frame timer
                frameTimer += Bootstrap.getDeltaTime();
                if (frameTimer >= frameDuration)
                {
                    frameTimer = 0;
                    currentFrameIndex = (currentFrameIndex + 1) % animation.Count;
                    currentSprite = animation[currentFrameIndex];
                }
            }

            // Ensure the correct sprite is being drawn
            if (currentSprite != null)
            {
                recenterIfNeeded(currentSprite.width, currentSprite.height, (int)currentSprite.scaleX, (int)currentSprite.scaleY);
                currentSprite.setWorldPosition(owner.transform.X, owner.transform.Y);
                Bootstrap.getDisplay().addToDraw(currentSprite);
            }
        }

        public void setCurrentAnimation(string animationName)
        {
            if (animations.ContainsKey(animationName))
            {
                currentAnimation = animationName;
                currentFrameIndex = 0;
                frameTimer = 0;
                currentSprite = animations[currentAnimation][0]; 
            } else
            {
                var animation = SpriteManager.getInstance().getAnimation(animationName);
                if(animation != null)
                {
                    animations[animationName] = animation;

                    currentAnimation = animationName;
                    currentFrameIndex = 0;
                    frameTimer = 0;
                    currentSprite = animations[currentAnimation][0];
                }
            }
            recenterIfNeeded(currentSprite.width, currentSprite.height, currentSprite.scaleX, currentSprite.scaleY);
        }

        public void setupAnimation(string animationName, float x, float y, float rotation = 0, int zOrder = 0)
        {
            if (animations.TryGetValue(animationName, out List<Sprite> value)) {
                foreach (var item in value)
                {
                    item.setLocalPosition(x, y);
                    item.zOrder = zOrder;
                    item.rotz = rotation;
                }
            }
        }

        public void addSprite(string spriteName, string filepath, float scale = 1, float x = 0, float y = 0, int zOrder = 0)
        {
            Sprite frame = SpriteManager.getInstance().getSprite(spriteName, filepath);
            frame.setUniformScale(scale);
            frame.setLocalPosition(x, y);
            frame.zOrder = zOrder;

            sprites[spriteName] = frame;
        }

        public void addAnimationFrames(string spriteName, string filepath, string animationName)
        {
            Sprite frame = SpriteManager.getInstance().getSprite(spriteName,filepath);
            if (frame != null)
            {
                if (!animations.ContainsKey(animationName))
                {
                    animations[animationName] = new List<Sprite>();
                }
                animations[animationName].Add(frame);
            }

        }

        public void setSprite(string spriteName)
        {
            currentSprite = sprites[spriteName];
            currentSprite.setWorldPosition(owner.transform.X, owner.transform.Y);

            recenterIfNeeded(currentSprite.width, currentSprite.height, currentSprite.scaleX, currentSprite.scaleY);
        }

        public Sprite getSprite()
        {
            return currentSprite;
        }

        public void addAnimationFrames(string animationName, List<Sprite> frames)
        {
            if (!animations.ContainsKey(animationName))
            {
                animations[animationName] = new List<Sprite>();
            }
            animations[animationName].AddRange(frames);

            // Set the first frame if no current animation is set
            if (currentAnimation == null || currentAnimation == animationName)
            {
                currentAnimation = animationName;
                currentFrameIndex = 0;
                frameTimer = 0;
                currentSprite = animations[currentAnimation][0];
            }
        }

        private void recenterIfNeeded(int width, int height, float scaleX, float scaleY)
        {
            if (owner.transform.Wid < width)
            {
                owner.transform.Wid = width;
            }
            if (owner.transform.Ht < height)
            {
                owner.transform.Ht = height;
            }
            if (owner.transform.Scalex < scaleX || owner.transform.Scaley < scaleY)
            {
                owner.transform.Scalex = scaleX;
                owner.transform.Scaley = scaleY;

            }
            owner.transform.recalculateCentre();
        }
    }
}
