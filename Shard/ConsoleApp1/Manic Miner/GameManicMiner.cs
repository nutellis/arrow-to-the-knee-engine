// Sprites from https://www.spriters-resource.com/fullview/113060/

using ManicMiner;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Shard
{
    class GameManicMiner : Game, InputListener
    {
        Random rand;
        MinerWilly player;
        List<Collectible> collect;

        public override bool isRunning() {       

            if (player == null || player.ToBeDestroyed) {
                return false;
            }

            foreach (Collectible c in collect)
            {
                if (c != null && !c.ToBeDestroyed)
                {
                    return true;
                }
            }

            return false;

        }

        public override void update()
        {

            if (isRunning() == false)
            {
                Color col = Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256));
                Bootstrap.getDisplay().showText("GAME OVER!", 300, 300, 128, col);
                return;
            }


        }

        public override void initialize()
        {
            Platform p;
            Bootstrap.getInput().addListener(this);
            rand = new Random();
        
            player = new MinerWilly();
            collect = new List<Collectible>();

            p = new Platform();
            p.setPosition(0, 750, 256, 50);

            for (int i = 0; i < 10; i++)
            {
                p = new Platform();

                if (i == 2) {
                    p.setPosition(450, 800, 256, 50);
                    p.MoveDirX = 1;
                }
                else {
                    p.setPosition(0 + (i * 256), 832, 0, 0);
                }

          }


           p = new Platform();
            p.setPosition(30, 800, 600, 150);

            p = new Platform();
            p.setPosition(128, 780, 600, 150);
            p.MoveDirY = -1;

            p = new Platform();
            p.setPosition(400, 605, 600, 100);
            p.MoveDirY = -1;

            p = new Platform();
            p.setPosition(600, 700, 700, 200);
            p.MoveDirY = -1;

            p = new Platform();
            p.setPosition(800, 105, 600, 200);

            p = new Platform();
            p.setPosition(800, 605, 600, 200);



            Collectible c = new Collectible();
            c.Transform.translate(100, 780);
            collect.Add(c);


            c = new Collectible();
            c.Transform.translate (840, 560);
            collect.Add (c);

            c = new Collectible();
            c.Transform.translate(840, 60);
            collect.Add(c);

        }


        public void handleInput(InputEvent inp, string eventType)
        {            
        }
    }
}
