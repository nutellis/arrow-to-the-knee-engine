using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Components
{
    internal class TransformComponent : BaseComponent
    {

        public Transform3D transform;

        public TransformComponent(GameObject owner) : base(owner)
        {
            //transform = new Transform3D()
        }

        // set transform or whatever

        public void initialize(float x, float y)
        {
            transform.X = x;
            transform.Y = y;
        }

        public override void update()
        {

        }
    }
}
