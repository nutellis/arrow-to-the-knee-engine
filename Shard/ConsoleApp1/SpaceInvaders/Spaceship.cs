using SDL2;
using Shard;
using Shard.Shard.Components;
using System.Drawing;
using Shard.Shard;
using Shard.SpaceInvaders;
using System;
using System.Text.Json;

namespace SpaceInvaders
{
    class Spaceship : GameObject, CollisionHandler
    {
        public SpriteComponent sprite;

        private InputComponent input;

        private SoundComponent sound;
  
        private PhysicsComponent physics;

        private double fireCounter, fireDelay = 1f;
        private float moveDistance, moveSpeed = 200;

        private (bool horizontal, bool vertical) isMoving = (false, false);


        public override void initialize()
        {
            this.transform.X = 100.0f;
            this.transform.Y = 800.0f;

            //this.sprite = new SpriteComponent(this, false);
            this.sprite = new SpriteComponent(this);

            // store animations and pass them to the frames list 
            this.sprite.setCurrentAnimation("spaceship_animation");

            fireDelay = 0.1;
            fireCounter = fireDelay;

            tags = new Tags();
            tags.addTag("Player");

            input = new InputComponent(this);
            input.initialize();

            input.bindInputAction("Fire", InputType.Pressed, (parameters) => fireBullet());

            input.bindAxisAction("Horizontal", moveHorizontal);
            input.bindAxisAction("Vertical", moveVertical);

            input.bindAxisAction("FireHorizontal", fireHorizontalBullet);
            input.bindAxisAction("FireVertical", fireVerticalBullet);

            physics = new PhysicsComponent(this);
            physics.addRectCollider();

            tags.addTag("Player");

            sound = new SoundComponent(this);
            sound.loadSound("SpaceShipAttack", "fire.wav");
            //sound.loadSound("BackgroundMusic", "background.wav");
            sound.loadSound("SpaceShipMove", "spaceshipmove.wav");
            //sound.loadSound("BackgroundEngine", "spaceshipengine.wav");


            //sound.setVolume("BackgroundMusic", 0.1f);

        }


        public void moveVertical(float value)
        {
            if (value != 0.0)
            {
                this.transform.translate(0, value * moveDistance);
                isMoving.vertical = true;
            }
            else
            {
                isMoving.vertical = false;
            }
        }

        public void moveHorizontal(float value)
        {
            if (value != 0.0)
            {
                this.transform.translate(value * moveDistance, 0);
                isMoving.horizontal = true;
            }
            else
            {
                isMoving.horizontal = false;
            }
        }

        public void fireVerticalBullet(float value)
        {
            if (fireCounter < fireDelay || value == 0.0)
            {
                return;
            }

            Bullet b = new Bullet();
            b.initialize();
            b.setupBullet(this.transform.Centre.X, this.transform.Centre.Y, [0, value]);
            b.DestroyTag = "Invader";

            fireCounter = 0;

            sound.playSound("SpaceShipAttack");
        }


        public void fireHorizontalBullet(float value)
        {
            if (fireCounter < fireDelay || value == 0.0)
            {
                return;
            }

            Bullet b = new Bullet();
            b.initialize();
            b.setupBullet(this.transform.Centre.X, this.transform.Centre.Y, [value, 0]);
            b.DestroyTag = "Invader";

            fireCounter = 0;

            sound.playSound("SpaceShipAttack");
        }

        public void fireBullet()
        {
            if (fireCounter < fireDelay)
            {
                return;
            }

            Bullet b = new Bullet();
            b.initialize();
            b.setupBullet(this.transform.Centre.X, this.transform.Centre.Y, [0, -1]);
            b.Dir[1] = -1;
            b.DestroyTag = "Invader";

            fireCounter = 0;

            sound.playSound("SpaceShipAttack");

        }

        public override void update()
        {
            base.update();

            fireCounter += (float)Bootstrap.getDeltaTime();
            moveDistance = (float)(moveSpeed * Bootstrap.getDeltaTime());


            if (isMoving.Equals((false, false)))
            {
                sound.stopSound("SpaceShipMove");
            } else {
                sound.playSound("SpaceShipMove");
            }
            //Console.WriteLine("Move Value: " + moveValue);
        }

        public void onCollisionEnter(PhysicsComponent x)
        {
        }

        public void onCollisionExit(PhysicsComponent x)
        {

            physics.DebugColor = Color.Green;

            if (x.Owner.Tags.checkTag("BunkerBit"))
            {
                sound.stopAllSounds();
            }
        }

        public void onCollisionStay(PhysicsComponent x)
        {
            physics.DebugColor = Color.Blue;
        }

        public override string ToString()
        {
            return "Spaceship: [" + transform.X + ", " + transform.Y + ", " + transform.Wid + ", " + transform.Ht + "]";
        }
    }
}