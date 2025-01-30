using Shard;
using System.Numerics;

namespace GameTest
{
    class Asteroid : GameObject, CollisionHandler, InputListener
    {
        int torqueCounter = 0;
        public void handleInput(InputEvent inp, string eventType)
        {

            if (eventType == "MouseDown" && inp.Button == 2)
            {
                if (MyBody.checkCollisions(new Vector2(inp.X, inp.Y)) != null)
                {
                    torqueCounter += 10;
                }
            }


        }

        public override void initialize()
        {
            this.Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("asteroid.png");

            setPhysicsEnabled();

            MyBody.MaxTorque = 100;
            MyBody.Mass = 1;
            MyBody.AngularDrag = 0.0f;
            MyBody.MaxForce = 100;
            MyBody.UsesGravity = true;
            MyBody.StopOnCollision = false;
            MyBody.ReflectOnCollision = true;
            //            MyBody.Kinematic = true;


            MyBody.addForce(this.Transform.Right, 20.5f);
            //            MyBody.addCircleCollider(32, 32, 30);
            MyBody.addRectCollider();
            Bootstrap.getInput().addListener(this);

            addTag("Asteroid");

        }


        public override void physicsUpdate()
        {
            for (int i = 0; i < torqueCounter; i++)
            {
                MyBody.addTorque(0.1f);
            }

            if (torqueCounter > 0)
            {
                torqueCounter -= 1;
            }
            


        }

        public override void update()
        {
            Bootstrap.getDisplay().addToDraw(this);
        }

        public void onCollisionEnter(PhysicsBody x)
        {
            if (x.Parent.checkTag("Bullet") == true)
            {
                ToBeDestroyed = true;
                Debug.getInstance().log("Boom");
            }

            Debug.getInstance().log("Bang");

        }

        public void onCollisionExit(PhysicsBody x)
        {
            Debug.getInstance().log("Anti Bang");
        }

        public void onCollisionStay(PhysicsBody x)
        {
        }

        public override string ToString()
        {
            return "Asteroid: [" + Transform.X + ", " + Transform.Y + ", " + Transform.Wid + ", " + Transform.Ht + "]";
        }
    }
}
