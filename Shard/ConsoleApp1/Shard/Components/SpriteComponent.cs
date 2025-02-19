using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Components
{
    internal class SpriteComponent : Component
    {
        public string SpritePath { get; set; }

        public SpriteComponent(string spritePath = null)
        {
            SpritePath = spritePath;
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
