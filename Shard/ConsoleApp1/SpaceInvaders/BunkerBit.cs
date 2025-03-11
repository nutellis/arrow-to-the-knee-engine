using Shard;
using Shard.Shard;
using Shard.Shard.Components;

namespace SpaceInvaders
{
    class BunkerBit : GameObject, CollisionHandler
    {
        private SpriteComponent sprite;
        private PhysicsComponent physics;
        private SoundComponent sound;

        private string destroyTag;
        public string DestroyTag { get => destroyTag; set => destroyTag = value; }

        public override void initialize()
        {

            //sprite = new SpriteComponent(this,false);
            sprite = new SpriteComponent(this);

            sprite.addSprite("bunkerBit.png");

            physics = new PhysicsComponent(this);

            sound = new SoundComponent(this);

            physics.addRectCollider();

            tags = new Tags();
            tags.addTag("BunkerBit");

            physics.PassThrough = true;

            sound.loadSound("BunkerExplosion", "bunkerexplosion.wav");
            sound.setVolume("BunkerExplosion", 1.0f);

        }

        public void onCollisionEnter(PhysicsComponent other)
        {
            if (other.Owner.Tags != null && (other.Owner.Tags.checkTag(destroyTag) || other.Owner.Tags.checkTag("Bullet")))
            {
                //ToBeDestroyed = true;
                //other.Owner.ToBeDestroyed = true;
                sound.playSound("BunkerExplosion");
            }

            //if (other.Owner.Tags.checkTag("Player"))
            //{
            //    ToBeDestroyed = true;
            //    //other.Owner.ToBeDestroyed = true;
            //    sound.playSound("BunkerExplosion");
            //}
        }

        public void onCollisionExit(PhysicsComponent x)
        {
        }

        public void onCollisionStay(PhysicsComponent other)
        {
            if (other.Owner.Tags != null && other.Owner.Tags.checkTag("Player"))
            {
                //SoundManager.getInstance().stopAllSounds();
            }
        }

        public override void update()
        {

            base.update();
        }
    }
}
