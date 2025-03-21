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
        public SpriteComponent baseSprite;
        public SpriteComponent engineSprite;
        public SpriteComponent engineEffectAnimation;

        private InputComponent input;

        private SoundComponent sound;
  
        private PhysicsComponent physics;

        private HealthComponent health;
        private ShieldComponent shield;
        private WeaponComponent weapon;

        private double fireCounter, fireDelay = 1f;
        private float moveDistance, moveSpeed = 350;

        private (bool horizontal, bool vertical) isMoving = (false, false);

        public override void initialize()
        {
            this.transform.X = 100.0f;
            this.transform.Y = 750.0f;

            this.engineEffectAnimation = new SpriteComponent(this);
            this.engineEffectAnimation.setCurrentAnimation("spaceship_engine");
            this.engineEffectAnimation.setupAnimation("spaceship_engine", 12, 50, 0, 5);

            this.engineSprite = new SpriteComponent(this);
            this.engineSprite.addSprite("baseEngine", "BaseEngine.png", 1.5f, 10, 25, 5);
            this.engineSprite.setSprite("baseEngine");

            this.baseSprite = new SpriteComponent(this);
            this.baseSprite.addSprite("shipFull", "ShipFull.png", 1.5f, 0, 0, 10);
            this.baseSprite.addSprite("shipSlight", "ShipSlightDamaged.png", 1.5f, 0, 0, 10);
            this.baseSprite.addSprite("shipDamaged", "ShipDamaged.png", 1.5f, 0, 0, 10);
            this.baseSprite.addSprite("shipVeryDamaged", "ShipVeryDamaged.png", 1.5f, 0, 0, 10);

            this.baseSprite.setSprite("shipFull");


            fireDelay = 0.5;
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
            sound.loadSound("SpaceShipMove", "spaceshipmove.wav");

            health = new HealthComponent(this);
            shield = new ShieldComponent(this);

            weapon = new WeaponComponent(this);



            sound.loadSound("BackgroundEngine", "spaceshipengine.wav");

            //sound.setVolume("BackgroundMusic", 0.1f);

            sound.setVolume("BackgroundEngine", 1f);
            sound.playSound("BackgroundEngine", true);
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
        }

        public void onCollisionEnter(PhysicsComponent x)
        {

        }

        public void onCollisionExit(PhysicsComponent x)
        {

            physics.DebugColor = Color.Green;

            // Sound Testing
            if (x.Owner.Tags.checkTag("BunkerBit"))
            {
                //Resume the Spaceship engine sound again 
                sound.setVolume("BackgroundEngine", 1f);
                sound.playSound("BackgroundEngine", true);

                //Resume all sounds 
                //SoundManager.getInstance().stopAllSounds(false);
            }
        }

        public void onCollisionStay(PhysicsComponent x)
        {
            // Sound Testing
            physics.DebugColor = Color.Blue;
            if (x.Owner.Tags.checkTag("BunkerBit"))
            {
                //Stop all sounds coming from this component!
                sound.stopAllComponentSounds();

                //Stops Every Sound Playing
                //SoundManager.getInstance().stopAllSounds(true);
            }
        }

        public override string ToString()
        {
            return "Spaceship: [" + transform.X + ", " + transform.Y + ", " + transform.Wid + ", " + transform.Ht + "]";
        }
    }
}