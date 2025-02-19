using Shard;
using Shard.Shard;
using Shard.Shard.Components;

namespace SpaceInvaders
{
    class BunkerBit : GameObject, CollisionHandler
    {
        //private SpriteComponent sprite;
        private TagComponent tag;
        private PhysicsComponent physics;

        public override void initialize()
        {

            //sprite = new SpriteComponent(Bootstrap.getAssetManager().getAssetPath("bunkerBit.png"));
            physics = new PhysicsComponent();
            tag = new TagComponent();

            this.Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("bunkerBit.png");

            physics.setPhysicsEnabled(true);
            physics.MyBody.addRectCollider();

            tag.addTag("BunkerBit");

            physics.MyBody.PassThrough = true;

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
