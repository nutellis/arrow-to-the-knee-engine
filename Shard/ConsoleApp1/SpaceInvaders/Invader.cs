using Shard;
using Shard.Shard;
using Shard.Shard.Components;
using System;

namespace SpaceInvaders
{
    class Invader : GameObject, CollisionHandler
    {

        // https://github.com/sausheong/invaders

        private int spriteToUse;
        private int xdir;
        private GameSpaceInvaders game;
        private Random rand;

        public int Xdir { get => xdir; set => xdir = value; }

        private SpriteComponent sprite;
        private PhysicsComponent physics;

        public override void initialize()
        {

            physics = new PhysicsComponent(this);

            sprite = new SpriteComponent(this, true);
            sprite.addSprite("invader1.png");
            sprite.addSprite("invader2.png");

            game = (GameSpaceInvaders)Bootstrap.getRunningGame();

            this.transform.X = 200.0f;
            this.transform.Y = 100.0f;

            physics.addRectCollider();

            rand = new Random();

            tags = new Tags();
            tags.addTag("Invader");

            physics.PassThrough = true;

        }

        public override void physicsUpdate()
        {
            base.physicsUpdate();
            if (tags.checkTag("AI"))
            {
                PathTracer.getInstance.findPath(((int)transform.X, (int)transform.Y), (400, 800));
            }
        }


        public void changeSprite()
        {
            spriteToUse += 1;

            if (spriteToUse >= 2)
            {
                spriteToUse = 0;
            }

            this.sprite.setSprite(spriteToUse);

        }

        public override void update()
        {
            base.update();
            if (tags.checkTag("AI"))
            {
                if(PathTracer.getInstance.getPath() != null)
                {
                    PathTracer.getInstance.debugPrintPathVisual(PathTracer.getInstance.getPath());
                }
            }

        }

        //public void onCollisionEnter(PhysicsBody x)
        //{
        //    if (x.Parent.checkTag("Player"))
        //    {
        //        x.Parent.ToBeDestroyed = true;
        //    }

        //    if (x.Parent.checkTag("BunkerBit"))
        //    {
        //        x.Parent.ToBeDestroyed = true;
        //    }
        //}

        public void onCollisionEnter(PhysicsComponent x)
        {
            // Get the TagComponent from the collided object
            Tags tagComp = x.Owner.Tags;

            // Check if the object has a TagComponent before using checkTag()
            if (tagComp != null)
            {
                if (tagComp.checkTag("Player"))
                {
                    x.Owner.ToBeDestroyed = true;
                }

                if (tagComp.checkTag("BunkerBit"))
                {
                    x.Owner.ToBeDestroyed = true;
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
            return "Asteroid: [" + transform.X + ", " + transform.Y + ", " + transform.Wid + ", " + transform.Ht + "]";
        }

        public void fire()
        {
            Bullet b = new Bullet();
            b.initialize();
            b.setupBullet(this.transform.Centre.X, this.transform.Centre.Y, [0,1]);
            b.DestroyTag = "Player";
        }
    }
}
