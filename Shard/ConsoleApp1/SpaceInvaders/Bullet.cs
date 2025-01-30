using Shard;
using System;
using System.Drawing;

namespace SpaceInvaders
{
    class Bullet : GameObject, CollisionHandler
    {
        private string destroyTag;
        private int dir;

        public string DestroyTag { get => destroyTag; set => destroyTag = value; }
        public int Dir { get => dir; set => dir = value; }

        public void setupBullet(float x, float y)
        {
            this.Transform.X = x;
            this.Transform.Y = y;
            this.Transform.Wid = 1;
            this.Transform.Ht = 20;


            setPhysicsEnabled();

            MyBody.addRectCollider();

            addTag("Bullet");

            MyBody.PassThrough = true;

        }

        public override void initialize()
        {
            this.Transient = true;
        }


        public override void update()
        {
            Random r = new Random();
            Color col = Color.FromArgb(r.Next(0, 256), r.Next(0, 256), 0);

            this.Transform.translate(0, dir * 400 * Bootstrap.getDeltaTime());

            Bootstrap.getDisplay().drawLine(
                (int)Transform.X,
                (int)Transform.Y,
                (int)Transform.X,
                (int)Transform.Y + 20,
                col);




        }

        public void onCollisionEnter(PhysicsBody x)
        {
            GameSpaceInvaders g;

            if (x.Parent.checkTag(destroyTag) == true || x.Parent.checkTag("BunkerBit"))
            {
                ToBeDestroyed = true;
                x.Parent.ToBeDestroyed = true;

                if (x.Parent.checkTag("Player"))
                {
                    g = (GameSpaceInvaders)Bootstrap.getRunningGame();

                    g.Dead = true;
                }

            }
        }

        public void onCollisionExit(PhysicsBody x)
        {
        }

        public void onCollisionStay(PhysicsBody x)
        {
        }

        public override string ToString()
        {
            return "Bullet: " + Transform.X + ", " + Transform.X;
        }
    }
}
