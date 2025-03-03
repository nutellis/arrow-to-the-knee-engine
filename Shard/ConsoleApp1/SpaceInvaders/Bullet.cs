﻿using Shard;
using Shard.Shard;
using Shard.Shard.Components;
using System;
using System.Drawing;
using System.IO;

namespace SpaceInvaders
{
    class Bullet : GameObject, CollisionHandler
    {

        private Tags tags;
        private PhysicsComponent physics;

        private string destroyTag;
        private int dir;

        public string DestroyTag { get => destroyTag; set => destroyTag = value; }
        public int Dir { get => dir; set => dir = value; }

        public void setupBullet(float x, float y)
        {
            this.transform.X = x;
            this.transform.Y = y;
            this.transform.Wid = 1;
            this.transform.Ht = 20;

            physics.addRectCollider();
            physics.PassThrough = true;

        }

        public override void initialize()
        {

            physics = new PhysicsComponent(this);

            this.Transient = true;

            tags = new Tags();
            tags.addTag("Bullet");

        }


        public override void update()
        {
            Random r = new Random();
            Color col = Color.FromArgb(r.Next(0, 256), r.Next(0, 256), 0);

            this.transform.translate(0, dir * 400.0 * Bootstrap.getDeltaTime());

            Bootstrap.getDisplay().drawLine(
                (int)transform.X,
                (int)transform.Y,
                (int)transform.X,
                (int)transform.Y + 20,
                col);
        }

        public void onCollisionEnter(PhysicsComponent x)
        {
            GameSpaceInvaders g;

            if (ToBeDestroyed) return;

            //if (tags != null && (tags.checkTag(destroyTag) || tags.checkTag("BunkerBit"))) // This is how it was before
            if (x.Owner.Tags != null && (x.Owner.Tags.checkTag(destroyTag) || x.Owner.Tags.checkTag("BunkerBit")))
            {
                Debug.Log(x.Owner + "Needs to be Destroyed");
                Debug.Log(x.Owner + "ToBeDestroyed: " + x.Owner.ToBeDestroyed);


                ToBeDestroyed = true;
                x.Owner.ToBeDestroyed = true;

                if (x.Owner.Tags.checkTag("Player"))
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
            return "Bullet: " + transform.X + ", " + transform.X;
        }
    }
}
