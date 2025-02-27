using Shard;
using Shard.Shard;
using Shard.Shard.Components;

namespace SpaceInvaders
{
    class BunkerBit : GameObject, CollisionHandler
    {
        //private SpriteComponent sprite;
        private Tags tags;
        private PhysicsComponent physics;

        public override void initialize()
        {

            //sprite = new SpriteComponent(Bootstrap.getAssetManager().getAssetPath("bunkerBit.png"));
            physics = new PhysicsComponent(this);

            this.transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("bunkerBit.png");

            physics.addRectCollider();

            tags = new Tags();
            tags.addTag("BunkerBit");

            physics.PassThrough = true;

        }

        public void onCollisionEnter(PhysicsComponent x)
        {
        }

        public void onCollisionExit(PhysicsComponent x)
        {
        }

        public void onCollisionStay(PhysicsComponent x)
        {
        }

        public override void update()
        {


            Bootstrap.getDisplay().addToDraw(this);
        }
    }
}
