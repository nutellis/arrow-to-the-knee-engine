using Shard;

namespace SpaceInvaders
{
    class BunkerBit : GameObject, CollisionHandler
    {

        public override void initialize()
        {


            this.Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("bunkerBit.png");

            setPhysicsEnabled();
            MyBody.addRectCollider();

            addTag("BunkerBit");

            MyBody.PassThrough = true;

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

        public override void update()
        {


            Bootstrap.getDisplay().addToDraw(this);
        }
    }
}
