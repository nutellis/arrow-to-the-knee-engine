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
        private int movementSoundChannel = -1;
        private bool movementSoundIsPlaying = false;
        private float moveValue = 0;
        private float horizontalMoveValue = 0;
        private float verticalMoveValue = 0;

        private PhysicsComponent physics;

        private double fireCounter, fireDelay = 1f;
        private float moveDistance, moveSpeed = 100;


        public override void initialize()
        {
            this.transform.X = 100.0f;
            this.transform.Y = 800.0f;

            this.sprite = new SpriteComponent(this, false);
            this.sprite.addSprite("player.png");

            if (sprite.getSprite() != null)
            {
                sprite.getSprite().setUniformScale(1.0f);
            }

            fireDelay = 1;
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
            sound.loadSound("BackgroundMusic", "background.wav");
            sound.loadSound("SpaceShipMove", "spaceshipmove.wav");
            sound.loadSound("BackgroundEngine", "spaceshipengine.wav");


            sound.setVolume("BackgroundMusic", 0.1f);
            //sound.playSoundOnRepeat("BackgroundMusic");
            //sound.playSoundOnRepeat("BackgroundEngine");

        }


        public void moveVertical(float value)
        {
            if (value != 0.0f)
            {
                this.transform.translate(0, value * moveDistance);
                moveValue = value;  
                verticalMoveValue = value; 
            }
            else
            {
                // Reset only if no movement on vertical axis
                moveValue = horizontalMoveValue; 
                verticalMoveValue = 0.0f; 
            }
        }

        public void moveHorizontal(float value)
        {
            if (value != 0.0f)
            {
                this.transform.translate(value * moveDistance, 0);
                moveValue = value;  
                horizontalMoveValue = value; 
            }
            else
            {
                // Reset only if no movement on horizontal axis
                moveValue = verticalMoveValue; 
                horizontalMoveValue = 0.0f; 
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

            checkMovementSound();
            //Console.WriteLine("Move Value: " + moveValue);
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

        private void checkMovementSound()
        {
            // If we are moving (moveValue != 0), start the sound if not already playing
            if (moveValue != 0.0f)
            {
                if (!movementSoundIsPlaying)
                {
                    startMovementSound();
                }
            }
            else
            {
                // If not moving, stop the sound if it's playing
                if (movementSoundIsPlaying)
                {
                    stopMovementSound();
                }
            }
        }

        private void startMovementSound()
        {
            if (movementSoundChannel != -1)
                return; // Sound is already playing, do nothing

            movementSoundIsPlaying = true;
            movementSoundChannel = SoundManager.getInstance().playSound("SpaceShipMove", true); // Start the looped movement sound
            Console.WriteLine($"Movement sound started on channel {movementSoundChannel}");
        }

        private void stopMovementSound()
        {
            moveValue = 0.0f; // Reset movement value

            if (movementSoundChannel == -1)
                return; // Sound isn't playing, do nothing

            movementSoundIsPlaying = false;
            SoundManager.getInstance().stopSound("SpaceShipMove"); // Stop the movement sound
            movementSoundChannel = -1; // Reset channel to release it
            Console.WriteLine("Stopped movement sound");
        }

    }
}