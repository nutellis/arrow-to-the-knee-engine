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
        private string[] sprites;
        private int xdir;
        private GameSpaceInvaders game;
        private Random rand;

        public int Xdir { get => xdir; set => xdir = value; }

        //private SpriteComponent sprite;
        private Tags tags;
        private PhysicsComponent physics;

        public override void initialize()
        {
            //sprite = new SpriteComponent(Bootstrap.getAssetManager().getAssetPath("bunkerBit.png"));
            physics = new PhysicsComponent(this);

            sprites = new string[2];

            game = (GameSpaceInvaders)Bootstrap.getRunningGame();

            sprites[0] = "invader1.png";
            sprites[1] = "invader2.png";

            spriteToUse = 0;

            this.Transform.X = 200.0f;
            this.Transform.Y = 100.0f;
            this.Transform.SpritePath = sprites[0];

            physics.MyBody.addRectCollider();

            rand = new Random();

            tags.addTag("Invader");

            physics.MyBody.PassThrough = true;

        }


        public void changeSprite()
        {
            spriteToUse += 1;

            if (spriteToUse >= sprites.Length)
            {
                spriteToUse = 0;
            }

            this.Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath(sprites[spriteToUse]);

        }

        public override void update()
        {


            Bootstrap.getDisplay().addToDraw(this);
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
            return "Asteroid: [" + Transform.X + ", " + Transform.Y + ", " + Transform.Width + ", " + Transform.Height + "]";
        }

        public void fire()
        {
            Bullet b = new Bullet();

            b.setupBullet(this.Transform.Transform.Centre.X, this.Transform.Transform.Centre.Y);
            b.Dir = 1;
            b.DestroyTag = "Player";
        }
    }
}
