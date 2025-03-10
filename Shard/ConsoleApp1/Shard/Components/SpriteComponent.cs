using System.Collections.Generic;


namespace Shard.Shard.Components
{
    internal class SpriteComponent : BaseComponent
    {
        private Sprite currentSprite;
        private Dictionary<string, List<Sprite>> animations; 
        private string currentAnimation = "";
        private int currentFrameIndex;
        private double frameTimer; 
        private float frameDuration = 0.5f; 

        private bool hasAnimation = false;
        private List<Sprite> animationFrames;

        public SpriteComponent(GameObject owner) : base(owner)
        {
            animations = new Dictionary<string, List<Sprite>>();
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
                    owner.transform.Wid = currentSprite.getWidth();
                    owner.transform.Ht = currentSprite.getHeight();
                    owner.transform.recalculateCentre();
                }
            }

            // Ensure the correct sprite is being drawn
            if (currentSprite != null)
            {
                currentSprite.setPosition(owner.transform.X, owner.transform.Y);
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
        }

        public void addSprite(string spriteName, string filepath)
        {
            Sprite frame = SpriteManager.getInstance().getSprite(spriteName, filepath);
            
            currentSprite = frame;
            currentSprite.setPosition(owner.transform.X, owner.transform.Y);
            owner.transform.Wid = currentSprite.getWidth();
            owner.transform.Ht = currentSprite.getHeight();
            owner.transform.recalculateCentre();

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

        // For static sprites (no animation), set the sprite directly
        public void setSprite(Sprite staticSprite)
        {
            currentSprite = staticSprite;
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
    }
}
