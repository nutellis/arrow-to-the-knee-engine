using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shard.Shard.Components;

namespace Shard.SpaceInvaders
{
    internal class WeaponComponent : SpriteComponent
    {
        private double fireCounter, fireDelay = 2.0f;

        public WeaponComponent(GameObject owner) : base(owner) 
        {
            fireCounter = fireDelay;
        }

        public bool canFire() => fireCounter >= fireDelay;

        public void fire()
        {
            fireCounter = 0;
        }

        public void update(double deltaTime)
        {
            fireCounter += deltaTime;
        }
    }
}
