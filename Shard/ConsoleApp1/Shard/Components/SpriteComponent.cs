//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection.Metadata;
//using System.Text;
//using System.Threading.Tasks;


//namespace Shard.Shard.Components
//{
//    internal class SpriteComponent : BaseComponent
//    {
//        private Sprite sprite; //reference to sprite object
//        private List<Sprite> animationFrames; //replace sprite with animationFrames as teh default
//        private int currentFrame;
//        private double frameTimer;
//        private float frameDuration = 1.0f;
//        public bool hasAnimation = false;

//        public SpriteComponent(GameObject owner, bool hasAnimation = false) : base(owner)
//        {
//            this.hasAnimation = hasAnimation;
//            animationFrames = new List<Sprite>();
//        }

//        public override void initialize()
//        {
//        }

//        public override void update()
//        {
//            if (sprite == null) return; // Prevent null reference error

//            frameTimer += Bootstrap.getDeltaTime();

//            if (hasAnimation && animationFrames.Count > 0)
//            {
//                if (frameTimer >= frameDuration)
//                {
//                    frameTimer = 0;
//                    currentFrame = (currentFrame + 1) % animationFrames.Count;
//                    sprite = animationFrames[currentFrame];
//                }
//            }

//            sprite.setPosition(owner.transform.X, owner.transform.Y);
//            Bootstrap.getDisplay().addToDraw(this.sprite);
//        }

//        public Sprite loadSprite(string spritePath)
//        {

//            string assetPath = Bootstrap.getAssetManager().getAssetPath(spritePath);

//            if (assetPath == null)
//            {
//                Console.WriteLine($"Failed to Load Sprite {spritePath}");
//                return null;
//            }

//            return Bootstrap.getAssetManager().getSprite(assetPath);
//        }

//        public void addSprite(string assetName)
//        {
//            Sprite frame = Bootstrap.getAssetManager().getSprite(assetName);
//            if (frame != null)
//            {
//                animationFrames.Add(frame);
//                if (animationFrames.Count == 1)
//                {
//                    sprite = frame;
//                    sprite.setPosition(owner.transform.X, owner.transform.Y);
//                    owner.transform.Wid = sprite.getWidth();
//                    owner.transform.Ht = sprite.getHeight();
//                    owner.transform.recalculateCentre();
//                }
//            }
//        }

//        public void changeColor(float r, float g, float b, float a)
//        {
//            if (sprite != null)
//            {
//                sprite.changeColor(r, g, b, a);
//            }

//        }

//        public Sprite getSprite()
//        {
//            if (sprite == null && animationFrames.Count > 0)
//            {
//                sprite = animationFrames[0]; // Fallback to first frame if available
//            }
//            return sprite;
//        }

//        internal void setSprite(int spriteToUse)
//        {
//            if (spriteToUse >= 0 && spriteToUse < animationFrames.Count)
//            {
//                sprite = animationFrames[spriteToUse];
//            }
//        }

//        public void startAnimation()
//        {
//            if (!sprite.isAnimationPlaying())
//            {
//                sprite.animate();

//            }

//        }

//        public void stopAnimation()
//        {
//            if (sprite.isAnimationPlaying())
//            {
//                sprite.stopAnimate();
//                setSprite(0);
//            }
//        }
//    }

//}


////////////// JSON and Sprite Sheet Method //////////////
///


using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Shard.Shard.Components
{
    internal class SpriteComponent : BaseComponent
    {
        private Sprite currentSprite; // Currently displayed sprite (could be from animation)
        private Dictionary<string, List<Sprite>> animations; // Store animations with animation name as key
        private string currentAnimation; // Name of the current animation being played
        private int currentFrameIndex; // Index of the current frame in the animation
        private double frameTimer; // Timer to manage frame change based on time
        private float frameDuration = 0.1f; // Time between frame changes (default to 10 fps)

        private bool hasAnimation = false; // Flag to check if the sprite has animation
        private List<Sprite> animationFrames; // List of frames for the animation

        public SpriteComponent(GameObject owner) : base(owner)
        {
            animations = new Dictionary<string, List<Sprite>>();
            currentAnimation = "Idle"; // Default animation
        }

        public Dictionary<string, List<Sprite>> Animations
        {
            get { return animations; }
        }

        public override void initialize()
        {
            // Initialize any necessary components here (e.g., load initial sprite, etc.)
        }

        //public override void update()
        //{
        //    if (animations.Count == 0 || !animations.ContainsKey(currentAnimation))
        //        return;

        //    List<Sprite> frames = animations[currentAnimation];
        //    if (frames.Count == 0)
        //        return;

        //    // Update frame timer and switch frames if needed
        //    frameTimer += Bootstrap.getDeltaTime();
        //    if (frameTimer >= frameDuration)
        //    {
        //        frameTimer = 0;
        //        currentFrameIndex = (currentFrameIndex + 1) % frames.Count; // Loop through frames
        //        currentSprite = frames[currentFrameIndex];
        //        owner.transform.Wid = currentSprite.getWidth();
        //        owner.transform.Ht = currentSprite.getHeight();
        //        owner.transform.recalculateCentre();
        //    }

        //    // Update position and draw the current sprite
        //    currentSprite.setPosition(owner.transform.X, owner.transform.Y);
        //    Bootstrap.getDisplay().addToDraw(currentSprite);
        //}

        public override void update()
        {
            if (!animations.ContainsKey(currentAnimation) || animations[currentAnimation].Count == 0)
                return;

            List<Sprite> frames = animations[currentAnimation];

            // Update the frame timer
            frameTimer += Bootstrap.getDeltaTime();
            if (frameTimer >= frameDuration)
            {
                frameTimer = 0;
                currentFrameIndex = (currentFrameIndex + 1) % frames.Count;
                currentSprite = frames[currentFrameIndex];
                owner.transform.Wid = currentSprite.getWidth();
                owner.transform.Ht = currentSprite.getHeight();
                owner.transform.recalculateCentre();
            }

            // Ensure the correct sprite is being drawn
            if (currentSprite != null)
            {
                currentSprite.setPosition(owner.transform.X, owner.transform.Y);
                Bootstrap.getDisplay().addToDraw(currentSprite);
            }
        }

        // Set the current animation to be played
        //public void setAnimation(string animationName)
        //{
        //    if (animations.ContainsKey(animationName))
        //    {
        //        currentAnimation = animationName;
        //        currentFrameIndex = 0;
        //        frameTimer = 0;
        //        currentSprite = animations[animationName][currentFrameIndex];
        //    }
        //}

        public void setAnimation(string animationName)
        {
            if (animations.ContainsKey(animationName))
            {
                currentAnimation = animationName;
                currentFrameIndex = 0;
                frameTimer = 0;
                currentSprite = animations[currentAnimation][0]; // Ensure this sets the correct sprite
            }
        }

        // Add sprite(s) (either for a single static sprite or for an animation)
        public void addSprite(string assetName, string animationName = "Idle")
        {
            Sprite frame = Bootstrap.getAssetManager().getSprite(assetName);
            if (frame != null)
            {
                if (!animations.ContainsKey(animationName))
                {
                    animations[animationName] = new List<Sprite>();
                }
                animations[animationName].Add(frame);

                // If it's the first sprite for this animation, set it as the current sprite
                if (animations[animationName].Count == 1)
                {
                    currentSprite = frame;
                    currentSprite.setPosition(owner.transform.X, owner.transform.Y);
                    owner.transform.Wid = currentSprite.getWidth();
                    owner.transform.Ht = currentSprite.getHeight();
                    owner.transform.recalculateCentre();
                }
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


    //// Data model for parsing JSON
    //public class SpriteSheetData
    //{
    //    public Dictionary<string, Animation> animations { get; set; }
    //}

    //public class Animation
    //{
    //    public List<Frame> frames { get; set; }
    //}

    //public class Frame
    //{
    //    public int x { get; set; }
    //    public int y { get; set; }
    //    public int width { get; set; }
    //    public int height { get; set; }
    //}

    // Data model for parsing JSON with animations or static frames
    public class SpriteSheetData
    {
        public Dictionary<string, Animation> animations { get; set; } // Animations
        public List<FrameData> frames { get; set; } // Static frames
    }

    public class Animation
    {
        public List<FrameData> frames { get; set; }
    }

    //public class FrameData
    //{
    //    public int x { get; set; }
    //    public int y { get; set; }
    //    public int w { get; set; }
    //    public int h { get; set; }
    //    public int duration { get; set; }
    //    public string filename { get; set; }
    //}

    public class FrameData
    {
        [JsonProperty("filename")]
        public string filename { get; set; }

        [JsonProperty("duration")]
        public int duration { get; set; }

        [JsonProperty("frame")]
        public FramePosition frame { get; set; } // New class to map nested object
    }

    public class FramePosition
    {
        [JsonProperty("x")]
        public int x { get; set; }

        [JsonProperty("y")]
        public int y { get; set; }

        [JsonProperty("w")]
        public int w { get; set; }

        [JsonProperty("h")]
        public int h { get; set; }
    }
}
