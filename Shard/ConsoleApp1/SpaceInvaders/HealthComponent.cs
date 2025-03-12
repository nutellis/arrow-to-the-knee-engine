using Shard.Shard.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.SpaceInvaders
{
    public class HealthComponent: BaseComponent
    {
        private int health;
        private int maxHealth;

        public HealthComponent(GameObject owner) : base(owner)
        {
            health = 100;
            maxHealth = 100;
        }

        public override void initialize()
        {
        }

        public override void update()
        {
            if (health <= 0)
            {
                owner.ToBeDestroyed = true;
            }
        }

        public void takeDamage(int damage)
        {
            health -= damage;
        }

        public void heal(int amount)
        {
            health += amount;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }
    }
}
