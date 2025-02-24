using Shard;
using Shard.Shard;
using Shard.Shard.Components;
using System;
using System.Drawing;

namespace SpaceInvaders
{
    class Bullet : GameObject, CollisionHandler
    {

        private Tags tags;
        private PhysicsComponent physics;

        private string destroyTag;
        private int dir;

        public string DestroyTag { get => destroyTag; set => destroyTag = value; }
        public int Dir { get => dir; set => dir = value; }

        public void setupBullet(float x, float y)
        {
            this.Transform.X = x;
            this.Transform.Y = y;
            this.Transform.Transform.Wid = 1;
            this.Transform.Transform.Ht = 20;

            physics.MyBody.addRectCollider();

            tags.addTag("Bullet");

            physics.MyBody.PassThrough = true;

        }

        public override void initialize()
        {

            physics = new PhysicsComponent(this);

            this.Transient = true;
        }


        public override void update()
        {
            Random r = new Random();
            Color col = Color.FromArgb(r.Next(0, 256), r.Next(0, 256), 0);

            this.Transform.translate(0, dir * 400.0 * Bootstrap.getDeltaTime());

            Bootstrap.getDisplay().drawLine(
                (int)Transform.X,
                (int)Transform.Y,
                (int)Transform.X,
                (int)Transform.Y + 20,
                col);
        }

        public void onCollisionEnter(PhysicsComponent x)
        {
            GameSpaceInvaders g;

            // Ensure the object has a TagComponent before checking tags
            if (tags != null && (tags.checkTag(destroyTag) || tags.checkTag("BunkerBit")))
            {
                ToBeDestroyed = true;
                x.Owner.ToBeDestroyed = true;

                if (tags.checkTag("Player"))
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
            return "Bullet: " + Transform.X + ", " + Transform.X;
        }
    }
}
