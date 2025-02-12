using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Components
{
    class SoundComponent
    {
        public string SoundPath { get; set; }   
        public bool IsPlaying { get; set; }      

        public SoundComponent(string soundPath)
        {
            SoundPath = soundPath;
            IsPlaying = false;
        }
    }
}
