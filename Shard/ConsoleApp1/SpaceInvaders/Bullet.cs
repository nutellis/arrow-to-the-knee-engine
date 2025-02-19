using Shard;
using Shard.Shard;
using Shard.Shard.Components;
using System;
using System.Drawing;

namespace SpaceInvaders
{
    class Bullet : GameObject, CollisionHandler
    {

        private TagComponent tag;
        private PhysicsComponent physics;

        private string destroyTag;
        private int dir;

        public string DestroyTag { get => destroyTag; set => destroyTag = value; }
        public int Dir { get => dir; set => dir = value; }

        public void setupBullet(float x, float y)
        {
            this.Transform.X = x;
            this.Transform.Y = y;
            this.Transform.Wid = 1;
            this.Transform.Ht = 20;


            physics.setPhysicsEnabled(true);

            physics.MyBody.addRectCollider();

            tag.addTag("Bullet");

            physics.MyBody.PassThrough = true;

        }

        public override void initialize()
        {

            physics = new PhysicsComponent();
            tag = new TagComponent();

            this.Transient = true;
        }


        public override void update()
        {
            Random r = new Random();
            Color col = Color.FromArgb(r.Next(0, 256), r.Next(0, 256), 0);

            this.Transform.translate(0, dir * 400 * Bootstrap.getDeltaTime());

            Bootstrap.getDisplay().drawLine(
                (int)Transform.X,
                (int)Transform.Y,
                (int)Transform.X,
                (int)Transform.Y + 20,
                col);
        }

        public void onCollisionEnter(PhysicsBody x)
        {
            GameSpaceInvaders g;

            // Get the TagComponent from x.Parent
            TagComponent tagComp = x.Parent.getComponent<TagComponent>();

            // Ensure the object has a TagComponent before checking tags
            if (tagComp != null && (tagComp.checkTag(destroyTag) || tagComp.checkTag("BunkerBit")))
            {
                ToBeDestroyed = true;
                x.Parent.ToBeDestroyed = true;

                if (tagComp.checkTag("Player"))
                {
                    g = (GameSpaceInvaders)Bootstrap.getRunningGame();
                    g.Dead = true;
                }
            }
        }

        public void onCollisionExit(PhysicsBody x)
        {
        }

        public void onCollisionStay(PhysicsBody x)
        {
        }

        public override string ToString()
        {
            return "Bullet: " + Transform.X + ", " + Transform.X;
        }
    }
}
