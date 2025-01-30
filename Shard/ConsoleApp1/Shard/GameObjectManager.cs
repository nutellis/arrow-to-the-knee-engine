/*
*
*   This manager class makes sure update gets called when it should on all the game objects, 
*       and also handles the pre-physics and post-physics ticks.  It also deals with 
*       transient objects (like bullets) and removing destroyed game objects from the system.
*   @author Michael Heron
*   @version 1.0
*   
*/

using System.Collections.Generic;

namespace Shard
{
    class GameObjectManager
    {
        private static GameObjectManager me;
        List<GameObject> myObjects;

        private GameObjectManager()
        {
            myObjects = new List<GameObject>();
        }

        public static GameObjectManager getInstance()
        {
            if (me == null)
            {
                me = new GameObjectManager();
            }

            return me;
        }

        public void addGameObject(GameObject gob)
        {
            myObjects.Add(gob);

        }

        public void removeGameObject(GameObject gob)
        {
            myObjects.Remove(gob);
        }


        public void physicsUpdate()
        {
            GameObject gob;
            for (int i = 0; i < myObjects.Count; i++)
            {
                gob = myObjects[i];
                gob.physicsUpdate();
            }
        }

        public void prePhysicsUpdate()
        {
            GameObject gob;
            for (int i = 0; i < myObjects.Count; i++)
            {
                gob = myObjects[i];

                gob.prePhysicsUpdate();
            }
        }

        public void update()
        {
            List<int> toDestroy = new List<int>();
            GameObject gob;
            for (int i = 0; i < myObjects.Count; i++)
            {
                gob = myObjects[i];

                gob.update();

                gob.checkDestroyMe();

                if (gob.ToBeDestroyed == true)
                {
                    toDestroy.Add(i);
                }
            }

            if (toDestroy.Count > 0)
            {
                for (int i = toDestroy.Count - 1; i >= 0; i--)
                {
                    gob = myObjects[toDestroy[i]];
                    myObjects[toDestroy[i]].killMe();
                    myObjects.RemoveAt(toDestroy[i]);

                }
            }

            toDestroy.Clear();

            //            Debug.Log ("NUm Objects is " + myObjects.Count);
        }

    }
}
