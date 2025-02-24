using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shard.Shard.Components;

namespace Shard.SpaceInvaders
{
    internal class WeaponComponent : BaseComponent
    {
        private double fireCounter, fireDelay = 2.0f;

        public WeaponComponent(GameObject owner) : base(owner) 
        {
            fireCounter = fireDelay;
        }

        public bool CanFire() => fireCounter >= fireDelay;

        public void Fire()
        {
            fireCounter = 0;
        }

        public void Update(double deltaTime)
        {
            fireCounter += deltaTime;
        }

        //protected override void UpdateComponent()
        //{

        //}
    }
}
