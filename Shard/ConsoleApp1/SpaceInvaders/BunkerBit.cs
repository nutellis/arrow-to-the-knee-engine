using Shard;
using Shard.Shard;
using Shard.Shard.Components;

namespace SpaceInvaders
{
    class BunkerBit : GameObject, CollisionHandler
    {
        private SpriteComponent sprite;
        private PhysicsComponent physics;

        public override void initialize()
        {

            //sprite = new SpriteComponent(this,false);
            sprite = new SpriteComponent(this);

            sprite.addSprite("bunkerBit.png");

            physics = new PhysicsComponent(this);

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

            base.update();
        }
    }
}
