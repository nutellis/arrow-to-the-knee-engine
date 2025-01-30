using GameTest;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Shard
{
    class GameTest : Game, InputListener
    {
        GameObject background;
        List<GameObject> asteroids;
        public override void update()
        {
            
            Bootstrap.getDisplay().showText("FPS: " + Bootstrap.getSecondFPS() + " / " + Bootstrap.getFPS(), 10, 10, 12, 255, 255, 255);


        }

        public override int getTargetFrameRate()
        {
            return 100;

        }
        public void createShip()
        {
            GameObject ship = new Spaceship();
            Random rand = new Random();
            int offsetx = 0, offsety = 0;

            GameObject asteroid;



    
//            asteroid.MyBody.Kinematic = true;
     


            background = new GameObject();
            background.Transform.SpritePath = getAssetManager().getAssetPath ("background2.jpg");
            background.Transform.X = 0;
            background.Transform.Y = 0;


        }

        public override void initialize()
        {
            Bootstrap.getInput().addListener(this);
            createShip();

            asteroids = new List<GameObject>();


        }

        public void handleInput(InputEvent inp, string eventType)
        {

            if (eventType == "MouseDown") {
                Console.WriteLine ("Pressing button " + inp.Button);
            }

            if (eventType == "MouseDown" && inp.Button == 1)
            {
                Asteroid asteroid = new Asteroid();
                asteroid.Transform.X = inp.X;
                asteroid.Transform.Y = inp.Y;
                asteroids.Add (asteroid);
            }

            if (eventType == "MouseDown" && inp.Button == 3)
            {
                foreach (GameObject ast in asteroids) {
                    ast.ToBeDestroyed = true;
                }

                asteroids.Clear();
            }


        }
    }
}
