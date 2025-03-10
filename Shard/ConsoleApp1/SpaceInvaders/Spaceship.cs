using SDL2;
using Shard;
using Shard.Shard.Components;
using System.Drawing;
using Shard.Shard;
using Shard.SpaceInvaders;
using System;

namespace SpaceInvaders
{
    class Spaceship : GameObject, CollisionHandler
    {
        public SpriteComponent sprite;

        private InputComponent input;

        private PhysicsComponent physics;

        private double fireCounter, fireDelay = 0.1f;
        private float moveDistance, moveSpeed = 200;


        public override void initialize()
        {
            this.transform.X = 100.0f;
            this.transform.Y = 800.0f;

            this.sprite = new SpriteComponent(this, false);

            Sprite frame = Bootstrap.getAssetManager().getSprite("SpaceShip_Idle.png");
            this.sprite.sprite = Bootstrap.getAssetManager().extractSprite(frame.surface, 0, 0, 32, 32, "spaceship_small");
            
            
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

        }

        public void moveVertical(float value)
        {
            {
                this.transform.translate(0, value * moveDistance);
            }
        }

        public void moveHorizontal(float value)
        {
            if (value != 0.0)
            {
                this.transform.translate(value * moveDistance, 0);
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
        }


        public void fireHorizontalBullet(float value)
        {
            if (fireCounter < fireDelay || value == 0.0)
            {
                return;
            }

            Bullet b = new Bullet();
            b.initialize();
            b.setupBullet(this.transform.Centre.X, this.transform.Centre.Y, [value,0]);
            b.DestroyTag = "Invader";

            fireCounter = 0;
        }

        public void fireBullet()
        {
            if (fireCounter < fireDelay)
            {
                return;
            }

            Bullet b = new Bullet();
            b.initialize();
            b.setupBullet(this.transform.Centre.X, this.transform.Centre.Y, [0,-1]);
            b.Dir[1] = -1;
            b.DestroyTag = "Invader";

            fireCounter = 0;

        }

        public void handleInput(InputEvent inp, string eventType)
        {

            if (Bootstrap.getRunningGame().isRunning() == false)
            {
                return;
            }

            //if (eventType == "KeyDown")
            //{

            //    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
            //    {
            //        //right = true;
            //        input.Right = true;
            //    }

            //    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
            //    {
            //        //left = true;
            //        input.Left = true;
            //    }

            //}
            //else if (eventType == "KeyUp")
            //{


            //    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
            //    {
            //        //right = false;
            //        input.Right = false;
            //    }

            //    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
            //    {
            //        //left = false;
            //        input.Left = false;
            //    }


            //}



            //if (eventType == "KeyUp")
            //{
            //    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_SPACE)
            //    {
            //        //fireBullet();
            //        input.Fire = true;
            //    }
            //}
        }

        public override void update()
        {
            
            fireCounter += (float)Bootstrap.getDeltaTime();
            moveDistance = (float)(moveSpeed * Bootstrap.getDeltaTime());

            base.update();
        }

        public void onCollisionEnter(PhysicsComponent x)
        {

        }

        public void onCollisionExit(PhysicsComponent x)
        {

            physics.DebugColor = Color.Green;
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
