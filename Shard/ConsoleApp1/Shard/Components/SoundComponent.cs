using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Components
{
    internal class SoundComponent : BaseComponent
    {
        public string SoundPath { get; set; }   
        public bool IsPlaying { get; set; }      

        public SoundComponent(string soundPath)
        {
            SoundPath = soundPath;
            IsPlaying = false;
        }

        public override void initialize()
        {

        }

        public override void update()
        {

        }

        //protected override void UpdateComponent()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
