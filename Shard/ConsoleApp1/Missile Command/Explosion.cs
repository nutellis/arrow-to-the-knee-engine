using Shard;
using System;
using System.Drawing;

namespace MissileCommand
{
    class Explosion : GameObject, CollisionHandler
    {
        private int maxRadius;
        private float currentRadius;
        private float radDir;
        private Random rand;
        private ColliderCircle c;
        private string targetTag;

        public string TargetTag { get => targetTag; set => targetTag = value; }
        public override void initialize()
        {
            currentRadius = 0;
            radDir = 1f;
            maxRadius = 50;
            rand = new Random();

            setPhysicsEnabled();

            c = MyBody.addCircleCollider(0, 0, (int)currentRadius);

            MyBody.PassThrough = true;

        }

        public override void prePhysicsUpdate()
        {


        }

        public override void update()
        {
            Color col = Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256));
            int explosionSpeed = maxRadius / 4;

            if (currentRadius >= maxRadius)
            {
                radDir *= -1;
            }

            currentRadius += (float)(radDir * Bootstrap.getDeltaTime() * explosionSpeed);

            c.X = (float)Transform.Centre.X;
            c.Y = (float)Transform.Centre.Y;
            c.Rad = currentRadius;
            c.recalculate();


            if (currentRadius <= -1)
            {
                ToBeDestroyed = true;
            }


            Bootstrap.getDisplay().drawFilledCircle((int)Transform.X, (int)Transform.Y, (int)currentRadius, col);

        }

        public void onCollisionEnter(PhysicsBody x)
        {

            if (x.Parent.checkTag(TargetTag))
            {
                Debug.Log("Collided!");
                x.Parent.ToBeDestroyed = true;
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
            return "Explosion: [" + c.Left + ", " + c.Right + ", " + c.Top + ", " + c.Bottom + "]";
        }


    }
}
