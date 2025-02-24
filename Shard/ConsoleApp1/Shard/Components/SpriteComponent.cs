using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Components
{
    internal class SpriteComponent : BaseComponent
    {
        public string SpritePath { get; set; }

        public SpriteComponent(GameObject owner) : base(owner)
        {
            //SpritePath = spritePath;
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
