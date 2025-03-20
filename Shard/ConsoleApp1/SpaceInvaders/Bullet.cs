using Shard;
using Shard.Shard;
using Shard.Shard.Components;
using System;
using System.Drawing;

namespace SpaceInvaders
{
    class Bullet : GameObject, CollisionHandler
    {

        private PhysicsComponent physics;
        private SoundComponent sound;
        private SpriteComponent sprite;

        private string destroyTag;
        private float[] direction = { 0, 0 };

        public string DestroyTag { get => destroyTag; set => destroyTag = value; }
        public float[] Dir { get => direction; set => direction = value; }

        public void setupBullet(float x, float y, float[] direction)
        {
            this.direction = direction;
            this.transform.X = x;
            this.transform.Y = y;
            
            if (direction[0] > 0)
            {
                this.sprite.setupAnimation("gun_projectile", 0, 0, 90, 20);
            }
            else if (direction[0] < 0)
            {
                this.sprite.setupAnimation("gun_projectile", 0, 0, -90, 20);
            }
            else if (direction[1] > 0)
            {
                this.sprite.setupAnimation("gun_projectile", 0, 0, 180, 20);
            }
            else
            {
                this.sprite.setupAnimation("gun_projectile", 0, 0, 0, 20);
            }

            physics.addRectCollider();

            tags.addTag("Bullet");

            physics.PassThrough = true;

        }

        public override void initialize()
        {
            physics = new PhysicsComponent(this);
            sound = new SoundComponent(this);

            this.sprite = new SpriteComponent(this);
            this.sprite.setCurrentAnimation("gun_projectile");

            this.Transient = true;

            tags = new Tags();

            sound.loadSound("GotHit", "hit.wav");
            sound.loadSound("BunkerExplosion", "bunkerexplosion.wav");
            sound.setVolume("BunkerExplosion", 0.1f);
        }


        public override void update()
        {
           
            this.transform.moveImidiately((float)(direction[0] * 450.0 * Bootstrap.getDeltaTime()), (float)(direction[1] * 400.0 * Bootstrap.getDeltaTime()));

            //drawBullet();
        }

        private void drawBullet()
        {
            Random r = new Random();
            Color col = Color.FromArgb(r.Next(0, 256), r.Next(0, 256), 0);

            Bootstrap.getDisplay().drawLine(
                (int)transform.X ,
                (int)transform.Y ,
                (int)(transform.X + (20 * direction[0])),
                (int)(transform.Y + (20 * direction[1])),
                col);
        }

         public void onCollisionEnter(PhysicsComponent other)
        {
            GameSpaceInvaders g;

            if (ToBeDestroyed) return;

            if (other.Owner.Tags != null && (other.Owner.Tags.checkTag(destroyTag) || other.Owner.Tags.checkTag("BunkerBit")))
            {
                ToBeDestroyed = true;
                other.Owner.ToBeDestroyed = true;

                sound.playSound("BunkerExplosion");

                if (other.Owner.Tags.checkTag("Player"))
                {
                    g = (GameSpaceInvaders)Bootstrap.getRunningGame();
                    g.Dead = true;
                }
            }
        }

        public void onCollisionExit(PhysicsComponent x)
        {
        }

        public void onCollisionStay(PhysicsComponent x)
        {
        }

        public override string ToString()
        {
            return "Bullet: " + transform.X + ", " + transform.Y;
        }
    }
}
