
using Shard;

namespace GameBreakout
{
    class Brick : GameObject, InputListener, CollisionHandler
    {
        private int health;

        public int Health { get => health; set => health = value; }

        public override void initialize()
        {


            setPhysicsEnabled();

            MyBody.Mass = 1;
            MyBody.Kinematic = true;

            MyBody.addRectCollider();

            addTag("Brick");

        }

        public void handleInput(InputEvent inp, string eventType)
        {




        }


        public override void update()
        {

            this.Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("brick" + Health + ".png");

            Bootstrap.getDisplay().addToDraw(this);
        }

        public void onCollisionEnter(PhysicsBody x)
        {
            Health -= 1;

            if (Health <= 0)
            {
                this.ToBeDestroyed = true;
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
            return "Brick: [" + Transform.X + ", " + Transform.Y + ", " + Transform.Wid + ", " + Transform.Ht + "]";
        }

    }
}
