using Shard;
using System;

namespace ManicMiner
{
    class Platform : GameObject, CollisionHandler
    {
        private int moveDirX, moveDirY;
        private int maxY, minY;
        private int maxX, minX;
        private int moveDist;
        private int moveSpeed;

        private int origX, origY;
        public int MoveDist { get => moveDist; set => moveDist = value; }
        public int MoveDirX { get => moveDirX; set => moveDirX = value; }
        public int MoveDirY { get => moveDirY; set => moveDirY = value; }
        public int MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

        public override void initialize()
        {
            setPhysicsEnabled();
            MyBody.addRectCollider();
            MyBody.Mass = 10;
            MyBody.Kinematic = true;
            

            this.Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("platform.png");



        }

        public void setPosition(int x, int y, int dist, int speed) {
            origX = x;
            origY = y;

            MoveDist = dist;

            minY = origY - MoveDist;
            maxY = origY;

            maxX = origX + MoveDist;
            minX = origX;

            MoveSpeed = speed;

            Transform.translate (x, y);
        }

        public void onCollisionEnter(PhysicsBody x)
        {
        }

        public void onCollisionExit(PhysicsBody x)
        {
        }

        public void onCollisionStay(PhysicsBody x)
        {
        }

        public override void update()
        {

            if (moveDirY != 0)
            {
                Transform.translate(0, moveSpeed * moveDirY * Bootstrap.getDeltaTime());

                if (Transform.Y > maxY) {
                    MoveDirY = -1;
                }
            
                if (Transform.Y < minY) {
                    MoveDirY = 1;

                }
            }


            if (moveDirX != 0)
            {
                Transform.translate(moveSpeed * moveDirX * Bootstrap.getDeltaTime(), 0);

                if (Transform.X > maxX)
                {
                    MoveDirX = -1;
                }

                if (Transform.X < minX)
                {
                    MoveDirX = 1;

                }
            }



            Bootstrap.getDisplay().addToDraw(this);
        }

    }
}
