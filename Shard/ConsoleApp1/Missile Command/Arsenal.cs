using Shard;
using System.Collections.Generic;

namespace MissileCommand
{
    class Arsenal : GameObject
    {
        int numMissiles;
        List<ArsenalSprite> myMissiles;

        public bool canFireMissile()
        {
            if (numMissiles > 0)
            {
                return true;
            }

            return false;
        }

        public void resetMissiles()
        {
            int xmod = 0, ymod = 0;
            for (int i = 0; i < numMissiles; i++)
            {


                ArsenalSprite ar = new ArsenalSprite();
                ar.Transform.X = this.Transform.X + xmod;
                ar.Transform.Y = this.Transform.Y + ymod;

                if (i > 0 && i % 5 == 0)
                {
                    ymod += 18;
                    xmod = 0;
                }

                xmod += 10;

                myMissiles.Add(ar);

            }
        }

        public void fireMissile()
        {
            if (canFireMissile() == false)
            {
                return;
            }

            myMissiles[numMissiles - 1].ToBeDestroyed = true;

            numMissiles -= 1;


        }

        public override void initialize()
        {
            myMissiles = new List<ArsenalSprite>();


            numMissiles = 10;
            Transform.Wid = 64;
            Transform.Ht = 32;


        }

        public override void update()
        {

        }


    }
}
