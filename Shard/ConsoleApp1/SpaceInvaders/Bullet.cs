using Shard;
using Shard.Shard;
using Shard.Shard.Components;
using System;
using System.Drawing;

namespace SpaceInvaders
{
    class Bullet : GameObject, CollisionHandler
    {

        private PhysicsComponent physics;

        private string destroyTag;
        private int dir;

        public string DestroyTag { get => destroyTag; set => destroyTag = value; }
        public int Dir { get => dir; set => dir = value; }

        public void setupBullet(float x, float y)
        {
            this.transform.X = x;
            this.transform.Y = y;
            this.transform.Wid = 1;
            this.transform.Ht = 20;

            physics.addRectCollider();

            tags.addTag("Bullet");

            physics.PassThrough = true;

        }

        public override void initialize()
        {
            physics = new PhysicsComponent(this);

            this.Transient = true;

            tags = new Tags();
        }


        public override void update()
        {
            Random r = new Random();
            Color col = Color.FromArgb(r.Next(0, 256), r.Next(0, 256), 0);

            this.transform.translate(0, dir * 400.0 * Bootstrap.getDeltaTime());

            Bootstrap.getDisplay().drawLine(
                (int)transform.X,
                (int)transform.Y,
                (int)transform.X,
                (int)transform.Y + 20,
                col);
        }

         public void onCollisionEnter(PhysicsComponent other)
        {
            GameSpaceInvaders g;

            if (ToBeDestroyed) return;

            if (other.Owner.Tags != null && (other.Owner.Tags.checkTag(destroyTag) || other.Owner.Tags.checkTag("BunkerBit")))
            {
                ToBeDestroyed = true;
                other.Owner.ToBeDestroyed = true;

                if (other.Owner.Tags.checkTag("Player"))
                {
                    g = (GameSpaceInvaders)Bootstrap.getRunningGame();
                    g.Dead = true;
                }
            }
        }

        public void onCollisionExit(PhysicsComponent x)
        {
        }

        public void onCollisionStay(PhysicsComponent x)
        {
        }

        public override string ToString()
        {
            return "Bullet: " + transform.X + ", " + transform.X;
        }
    }
}
