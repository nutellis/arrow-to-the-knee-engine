﻿using SDL2;
using Shard;
using System.Drawing;
using Shard.Shard.Components;
using Shard.Shard;

namespace SpaceInvaders
{
    class Spaceship : GameObject, InputListener, CollisionHandler
    {
        //bool left, right;
        //float fireCounter, fireDelay;

        private TransformComponent transform;
        private InputComponent input;
        private WeaponComponent weapon;


        public override void initialize()
        {

            //this.Transform.X = 100.0f;
            //this.Transform.Y = 800.0f;
            //this.Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("player.png");

            //fireDelay = 2;
            //fireCounter = fireDelay;

            //Bootstrap.getInput().addListener(this);

            //setPhysicsEnabled();

            //MyBody.addRectCollider();

            //addTag("Player");

            transform = new TransformComponent(100.0f, 800.0f, 0f, 1f, 1f, Bootstrap.getAssetManager().getAssetPath("player.png"));
            input = new InputComponent();
            weapon = new WeaponComponent();

            Bootstrap.getInput().addListener(this);

            setPhysicsEnabled();
            MyBody.addRectCollider();

            addTag("Player");

        }

        public void fireBullet()
        {
            //if (fireCounter < fireDelay)
            //{
            //    return;
            //}

            //Bullet b = new Bullet();

            //b.setupBullet(this.Transform.Centre.X, this.Transform.Centre.Y);
            //b.Dir = -1;
            //b.DestroyTag = "Invader";

            //fireCounter = 0;

            Bullet b = new Bullet();
            b.setupBullet(transform.X, transform.Y);
            b.Dir = -1;
            b.DestroyTag = "Invader";

        }

        public void handleInput(InputEvent inp, string eventType)
        {

            if (Bootstrap.getRunningGame().isRunning() == false)
            {
                return;
            }

            if (eventType == "KeyDown")
            {

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
                {
                    //right = true;
                    input.Right = true;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
                {
                    //left = true;
                    input.Left = true;
                }

            }
            else if (eventType == "KeyUp")
            {


                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
                {
                    //right = false;
                    input.Right = false;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
                {
                    //left = false;
                    input.Left = false;
                }


            }



            if (eventType == "KeyUp")
            {
                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_SPACE)
                {
                    //fireBullet();
                    input.Fire = true;
                }
            }
        }

        public override void update()
        {
            float amount = (float)(100 * Bootstrap.getDeltaTime());

            //fireCounter += (float)Bootstrap.getDeltaTime();

            //if (left)
            //{
            //    this.Transform.translate(-1 * amount, 0);
            //}

            //if (right)
            //{
            //    this.Transform.translate(1 * amount, 0);
            //}

            weapon.Update(Bootstrap.getDeltaTime());

            if (input.Left) transform.Translate(-1 * amount, 0);
            if (input.Right) transform.Translate(1 * amount, 0);
            if (input.Fire && weapon.CanFire())
            {
                fireBullet();
                weapon.Fire();
                input.Fire = false;
            }

            Bootstrap.getDisplay().addToDraw(this);
        }

        public void onCollisionEnter(PhysicsBody x)
        {

        }

        public void onCollisionExit(PhysicsBody x)
        {

            MyBody.DebugColor = Color.Green;
        }

        public void onCollisionStay(PhysicsBody x)
        {
            MyBody.DebugColor = Color.Blue;
        }

        public override string ToString()
        {
            return "Spaceship: [" + Transform.X + ", " + Transform.Y + ", " + Transform.Wid + ", " + Transform.Ht + "]";
        }

    }
}
