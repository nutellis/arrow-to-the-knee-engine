//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Shard.Shard
//{
//   public class SpriteSheetData
//    {
//        public Dictionary<string, Animation> animations { get; set; } // Animations
//        public List<FrameData> frames { get; set; } // Static frames
//    }

//    public class Animation
//    {
//        public List<FrameData> frames { get; set; }
//    }

//    //public class FrameData
//    //{
//    //    public int x { get; set; }
//    //    public int y { get; set; }
//    //    public int w { get; set; }
//    //    public int h { get; set; }
//    //    public int duration { get; set; }
//    //    public string filename { get; set; }
//    //}

//    public class FrameData
//    {
//        [JsonProperty("filename")]
//        public string filename { get; set; }

//        [JsonProperty("duration")]
//        public int duration { get; set; }

//        [JsonProperty("frame")]
//        public FramePosition frame { get; set; } // New class to map nested object
//    }

//    public class FramePosition
//    {
//        [JsonProperty("x")]
//        public int x { get; set; }

//        [JsonProperty("y")]
//        public int y { get; set; }

//        [JsonProperty("w")]
//        public int w { get; set; }

//        [JsonProperty("h")]
//        public int h { get; set; }
//    }
//}
