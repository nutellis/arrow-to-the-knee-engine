using Newtonsoft.Json;
using System.Collections.Generic;


namespace Shard.Shard
{
    public class AnimationData
    {
        public List<Animation> animations { get; set; } = new List<Animation>();
    }

    public class Animation
    {
        public string animationName { get; set; } = "";
        public List<Frame> animationFrames { get; set; } = new List<Frame>();
    }

    public class Frame
    {
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}
