using SDL2;
using Shard;
using System.Drawing;
using Shard.Shard.Components;
using Shard.Shard;
using Shard.SpaceInvaders;

namespace SpaceInvaders
{
    class Spaceship : GameObject, CollisionHandler
    {
        //bool left, right;
        //float fireCounter, fireDelay;

        //private TransformComponent transform;
        //private SpriteComponent sprite;
        private InputComponent input;
        //private WeaponComponent weapon;
        private PhysicsComponent physics;

        private Tags tags;
        private double fireCounter, fireDelay = 2.0f;

        public override void initialize()
        {

            this.transform.X = 100.0f;
            this.transform.Y = 800.0f;
            
            this.transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("player.png");

            //fireDelay = 2;
            //fireCounter = fireDelay;

            //Bootstrap.getInput().addListener(this);

            //setPhysicsEnabled();

            //MyBody.addRectCollider();
            tags = new Tags();
            tags.addTag("Player");
            

            //transform = new TransformComponent(this, 100.0f, 800.0f, 0f, 1f, 1f);
            //sprite = new SpriteComponent(Bootstrap.getAssetManager().getAssetPath("player.png"));
            input = new InputComponent(this);
            //weapon = new WeaponComponent(this);
            physics = new PhysicsComponent(this);

            //transform.sprite;

            input.initialize();

            input.bindInputAction("fire", InputAction.InputType.Pressed, (parameters) => fireBullet());

            physics.addRectCollider();

            tags.addTag("Player");


        }

        public void fireBullet()
        {


            Bullet b = new Bullet();
            b.initialize();
            b.setupBullet(transform.X, transform.Y);
            b.transform.rotate(this.transform.Rotz);

            Bootstrap.getSound().playSound("fire.wav");

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

            fireCounter += (float)Bootstrap.getDeltaTime();

            //if (left)
            //{
            //    this.Transform.translate(-1 * amount, 0);
            //}

            //if (right)
            //{
            //    this.Transform.translate(1 * amount, 0);
            //}

            if (input.Left) transform.translate(-1 * amount, 0);
            if (input.Right) transform.translate(1 * amount, 0);
            if (input.Fire)
            {
                fireBullet();
                //weapon.Fire();
                input.Fire = false;
            }

            Bootstrap.getDisplay().addToDraw(this);
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
