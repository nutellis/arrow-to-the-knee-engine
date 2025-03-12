using Shard.Shard.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.SpaceInvaders
{
    public class ShieldComponent : SpriteComponent
    {
        public int shieldAmount;

        public ShieldComponent(GameObject owner) : base(owner)
        {
            shieldAmount = 100;
            IsVisible = false;

        }

        public override void update()
        {
            if (shieldAmount <= 0)
            {
                //remove object from game
            }
        }

        public void takeDamage(int damage)
        {
            shieldAmount -= damage;
        }

        public void recharge(int amount)
        {
            shieldAmount += amount;
            if (shieldAmount > 100)
            {
                shieldAmount = 100;
            }
        }

    }

}
