using SDL2;
using Shard;
using System.Drawing;
using Shard.Shard.Components;
using Shard.Shard;
using Shard.SpaceInvaders;

namespace SpaceInvaders
{
    class Spaceship : GameObject, InputListener, CollisionHandler
    {
        //bool left, right;
        //float fireCounter, fireDelay;

        //private TransformComponent transform;
        //private SpriteComponent sprite;
        private InputComponent input;
        private WeaponComponent weapon;
        private PhysicsComponent physics;

        private Tags tags;

        public override void initialize()
        {

            this.Transform.X = 100.0f;
            this.Transform.Y = 800.0f;
            
            this.Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("player.png");

            //fireDelay = 2;
            //fireCounter = fireDelay;

            //Bootstrap.getInput().addListener(this);

            //setPhysicsEnabled();

            //MyBody.addRectCollider();

            tags.addTag("Player");
            

            //transform = new TransformComponent(this, 100.0f, 800.0f, 0f, 1f, 1f);
            //sprite = new SpriteComponent(Bootstrap.getAssetManager().getAssetPath("player.png"));
            input = new InputComponent(this);
            weapon = new WeaponComponent(this);
            physics = new PhysicsComponent(this);

            //transform.sprite;

            Bootstrap.getInput().addListener(this);

            physics.MyBody.addRectCollider();

            tags.addTag("Player");


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
            b.setupBullet(Transform.X, Transform.Y);
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

            if (input.Left) Transform.translate(-1 * amount, 0);
            if (input.Right) Transform.translate(1 * amount, 0);
            if (input.Fire && weapon.CanFire())
            {
                fireBullet();
                weapon.Fire();
                input.Fire = false;
            }

            Bootstrap.getDisplay().addToDraw(this);
        }

        public void onCollisionEnter(PhysicsComponent x)
        {

        }

        public void onCollisionExit(PhysicsComponent x)
        {

            physics.MyBody.DebugColor = Color.Green;
        }

        public void onCollisionStay(PhysicsComponent x)
        {
            physics.MyBody.DebugColor = Color.Blue;
        }

        public override string ToString()
        {
            return "Spaceship: [" + Transform.X + ", " + Transform.Y + ", " + Transform.Width + ", " + Transform.Height + "]";
        }

    }
}
