using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Components
{
    internal class InputComponent : BaseComponent
    {
        public bool Left { get; set; }
        public bool Right { get; set; }
        public bool Up { get; set; }
        public bool Down { get; set; }
        public bool Fire { get; set; }

        public override void initialize()
        {
            Left = Right = Up = Down = Fire = false;
        }

        public override void update()
        {
            // Handle input logic
        }

        //protected override void UpdateComponent()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
