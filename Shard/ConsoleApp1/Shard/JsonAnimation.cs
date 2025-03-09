using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard
{
    public class JsonAnimationFrame
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class JsonAnimation
    {
        public List<JsonAnimationFrame> Frames { get; set; } = new();
    }
    public class JsonAnimationData
    {
        public Dictionary<string, JsonAnimationFrame> Animations { get; set; } = new();
    }
}
