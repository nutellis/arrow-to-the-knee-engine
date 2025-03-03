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

        private Tags tags;
        private double fireCounter, fireDelay = 2.0f;
        private float moveDistance;


        public override void initialize()
        {
            this.transform.X = 100.0f;
            this.transform.Y = 800.0f;

            this.sprite = new SpriteComponent(this, false);
            this.sprite.initialize();
            this.sprite.addSprite("player.png");
            
            fireDelay = 2;
            fireCounter = fireDelay;

            tags = new Tags();
            tags.addTag("Player");

            input = new InputComponent(this);
            input.initialize();

            input.bindInputAction("Fire", InputType.Pressed, (parameters) => fireBullet());
            input.bindInputAction("Left", InputType.Pressed, (parameters) => moveLeft());
            input.bindInputAction("Right", InputType.Pressed, (parameters) => moveRight());

            physics = new PhysicsComponent(this);
            physics.addRectCollider();

            tags.addTag("Player");

        }

        // Again a very naive way to implement axis movement
        public void moveLeft()
        {
            moveDistance = (float)(2500 * Bootstrap.getDeltaTime());
            this.transform.translate(-1 * moveDistance, 0);
        }
        public void moveRight()
        {
            moveDistance = (float)(2500 * Bootstrap.getDeltaTime());
            this.transform.translate(1 * moveDistance, 0);
        }

        public void fireBullet()
        {
            if (fireCounter < fireDelay)
            {
                return;
            }

            Bullet b = new Bullet();
            b.initialize();
            b.setupBullet(this.transform.Centre.X, this.transform.Centre.Y);
            b.Dir = -1;
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

            base.update();
            //if (left)
            //{
            //    this.Transform.translate(-1 * amount, 0);
            //}

            //if (right)
            //{
            //    this.Transform.translate(1 * amount, 0);
            //}

            //if (input.Left) transform.translate(-1 * amount, 0);
            //if (input.Right) transform.translate(1 * amount, 0);
            //if (input.Fire)
            //{
            //    fireBullet();
            //    //weapon.Fire();
            //    input.Fire = false;
            //}

            // Bootstrap.getDisplay().addToDraw(this);
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
