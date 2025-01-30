using Shard;

namespace GameBreakout
{
    class Wall : GameObject, CollisionHandler
    {

        public override void initialize()
        {


            setPhysicsEnabled();

            MyBody.addRectCollider();

            MyBody.Mass = 10;


            addTag("Wall");

            MyBody.Kinematic = true;

        }


        public override void update()
        {


        }

        public void onCollisionEnter(PhysicsBody x)
        {

        }

        public void onCollisionExit(PhysicsBody x)
        {
        }

        public void onCollisionStay(PhysicsBody x)
        {
        }

        public override string ToString()
        {
            return "Wall: [" + Transform.X + ", " + Transform.Y + "]";
        }


    }
}
