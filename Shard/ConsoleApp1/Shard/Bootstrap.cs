/*
*
*   The Bootstrap - this loads the config file, processes it and then starts the game loop
*   @author Michael Heron
*   @version 1.0
*   
*/

using Shard.Shard;
using SpaceInvaders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Shard
{
    class Bootstrap
    {
        public static string DEFAULT_CONFIG = "config.cfg";


        private static Game runningGame;
        private static Display displayEngine;
        private static SoundManager soundEngine;
        private static InputSystem input;
        private static PhysicsManager phys;
        private static AssetManagerBase asset;

        private static int targetFrameRate;
        private static int millisPerFrame;
        private static double deltaTime;
        private static double timeElapsed;
        private static int frames;
        private static List<long> frameTimes;
        private static long startTime;
        private static string baseDir;
        private static Dictionary<string,string> enVars;


        
        public static bool checkEnvironmentalVariable (string id) {
            return enVars.ContainsKey (id);
        }

        
        public static string getEnvironmentalVariable (string id) {
            if (checkEnvironmentalVariable (id)) {
                return enVars[id];
            }

            return null;
        }


        public static double TimeElapsed { get => timeElapsed; set => timeElapsed = value; }

        public static string getBaseDir() {
            return baseDir;
        }

        public static void setup()
        {
            string workDir = Environment.CurrentDirectory;
            baseDir = Directory.GetParent(workDir).Parent.Parent.Parent.FullName;

            setupEnvironmentalVariables(baseDir + "\\" + "envar.cfg");
            setup(baseDir + "\\" + DEFAULT_CONFIG);
        }

        public static void setupEnvironmentalVariables (String path) {
                Console.WriteLine("Path is " + path);

                Dictionary<string, string> config = BaseFunctionality.getInstance().readConfigFile(path);

                enVars = new Dictionary<string,string>();

                foreach (KeyValuePair<string, string> kvp in config)
                {
                    enVars[kvp.Key] = kvp.Value;
                }
        }
        public static double getDeltaTime()
        {
            return deltaTime;
        }

        public static Display getDisplay()
        {
            return displayEngine;
        }

        public static SoundManager getSound()
        {
            return soundEngine;
        }

        public static InputSystem getInput()
        {
            return input;
        }

        public static AssetManagerBase getAssetManager() {
            return asset;
        }

        public static Game getRunningGame()
        {
            return runningGame;
        }

        public static void setup(string path)
        {
            Console.WriteLine ("Path is " + path);

            Dictionary<string, string> config = BaseFunctionality.getInstance().readConfigFile(path);
            Type t;
            object ob;
            bool bailOut = false;

            phys = PhysicsManager.getInstance();

            //made input a singleton
            input = InputFramework.getInstance();

            //same with the sound
            soundEngine = SoundManager.getInstance(); 

            foreach (KeyValuePair<string, string> kvp in config)
            {
                t = Type.GetType("Shard." + kvp.Value);

                if (t == null)
                {
                    Debug.getInstance().log("Missing Class Definition: " + kvp.Value + " in " + kvp.Key, Debug.DEBUG_LEVEL_ERROR);
                    Environment.Exit(0);
                }

                ob = Activator.CreateInstance(t);


                switch (kvp.Key)
                {
                    case "display":
                        displayEngine = (Display)ob;
                        displayEngine.initialize();
                        break;
                    case "asset":
                        asset = (AssetManagerBase)ob;
                        asset.registerAssets();
                        break;
                    case "game":
                        runningGame = (Game)ob;
                        targetFrameRate = runningGame.getTargetFrameRate();
                        millisPerFrame = 1000 / targetFrameRate;
                        break;
                }

                Debug.getInstance().log("Config file... setting " + kvp.Key + " to " + kvp.Value);
            }

            if (runningGame == null)
            {
                Debug.getInstance().log("No game set", Debug.DEBUG_LEVEL_ERROR);
                bailOut = true;
            }

            if (displayEngine == null)
            {
                Debug.getInstance().log("No display engine set", Debug.DEBUG_LEVEL_ERROR);
                bailOut = true;
            }

            if (soundEngine == null)
            {
                Debug.getInstance().log("No sound engine set", Debug.DEBUG_LEVEL_ERROR);
                bailOut = true;
            }

            if (bailOut)
            {
                Environment.Exit(0);
            }
        }

        public static long getCurrentMillis()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public static int getFPS()
        {
            int fps;
            double seconds;

            seconds = (getCurrentMillis() - startTime) / 1000.0;

            fps = (int)(frames / seconds);

            return fps;
        }

        public static int getSecondFPS()
        {
            int count = 0;
            long now = getCurrentMillis();
            int lastEntry;



            Debug.Log ("Frametimes is " + frameTimes.Count);

            if (frameTimes.Count == 0) {
                return -1;
            }

            lastEntry = frameTimes.Count - 1;

            while (frameTimes[lastEntry] > (now - 1000) && lastEntry > 0) {
                lastEntry -= 1;
                count += 1;
            }

            if (lastEntry > 0) {
                frameTimes.RemoveRange (0, lastEntry);
            }

            return count;
        }

        public static int getCurrentFrame()
        {
            return frames;
        }

        static void Main(string[] args)
        {
            long timeInMillisecondsStart, lastTick, timeInMillisecondsEnd;
            long interval;
            int sleep;
            int tfro = 1;
            bool physUpdate = false;
            bool physDebug = false;



            // Setup the engine.
            setup();

            // When we start the program running.
            startTime = getCurrentMillis();
            frames = 0;
            frameTimes = new List<long>();
            // Start the game running.
            runningGame.initialize();

            timeInMillisecondsStart = startTime;
            lastTick = startTime;

            phys.GravityModifier = 0.1f;
            // This is our game loop.

            if (getEnvironmentalVariable("physics_debug") == "1")
            {
                physDebug = true;
            }

            while (true)
            {
               
                
                frames += 1;

                timeInMillisecondsStart = getCurrentMillis();
                
                // Clear the screen.
                Bootstrap.getDisplay().clearDisplay();


                // Debuging PathTracer
                PathTracer.getInstance.initialize(16, 16);
                PathTracer.getInstance.findPath((100, 100), (720, 600));   

                
                // Update 
                runningGame.update();
                // Input
                // Get input, which works at 50 FPS to make sure it doesn't interfere with the 
                // variable frame rates.

                input.getInput();

                if (runningGame.isRunning() == true)
                {

                    // Update runs as fast as the system lets it.  Any kind of movement or counter 
                    // increment should be based then on the deltaTime variable.
                    GameObjectManager.getInstance().update();

                    // This will update every 20 milliseconds or thereabouts.  Our physics system aims 
                    // at a 50 FPS cycle.
                    if (phys.willTick())
                    {
                        GameObjectManager.getInstance().prePhysicsUpdate();
                    }

                    // Update the physics.  If it's too soon, it'll return false.   Otherwise 
                    // it'll return true.
                    physUpdate = phys.update();

                    if (physUpdate)
                    {
                        // If it did tick, give every object an update
                        // that is pinned to the timing of the physics system.
                        GameObjectManager.getInstance().physicsUpdate();
                    }

                    if (physDebug) {
                        phys.drawDebugColliders();
                    }

                }
                
                // Render the screen.
                Bootstrap.getDisplay().display();
                
                timeInMillisecondsEnd = getCurrentMillis();

                frameTimes.Add (timeInMillisecondsEnd);

                interval = timeInMillisecondsEnd - timeInMillisecondsStart;

                sleep = (int)(millisPerFrame - interval);


                TimeElapsed += deltaTime;


                ///
                if (sleep >= 0)
                {
                    // Frame rate regulator.  Bear in mind since this is millisecond precision, and we 
                    // only get whole numbers from our interval, it will only rarely match a target 
                    // FPS.  Milliseconds just aren't precise enough.
                    //
                    //  (I'm hinting if this bothers you, you might have found an engine modification to make...)
                    Thread.Sleep(sleep);
                }

                timeInMillisecondsEnd = getCurrentMillis();
                deltaTime = (timeInMillisecondsEnd - timeInMillisecondsStart) / 1000.0f;

                millisPerFrame = 1000 / targetFrameRate;

                lastTick = timeInMillisecondsStart;

                

            }


        }
    }
}
