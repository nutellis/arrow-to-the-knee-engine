/*
*
*   This manager class makes sure update gets called when it should on all the game objects, 
*       and also handles the pre-physics and post-physics ticks.  It also deals with 
*       transient objects (like bullets) and removing destroyed game objects from the system.
*   @author Michael Heron
*   @version 1.0
*   
*/

using Shard.Shard.Components;
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


        //public void physicsUpdate()
        //{
        //    GameObject gob;
        //    for (int i = 0; i < myObjects.Count; i++)
        //    {
        //        gob = myObjects[i];
        //        gob.physicsUpdate();
        //    }
        //}

        public void physicsUpdate()
        {
            foreach (var gob in myObjects)
            {
                PhysicsComponent physics = gob.getComponent<PhysicsComponent>();
                if (physics != null)
                {
                    physics.physicsUpdate();
                }
            }
        }

        //public void prePhysicsUpdate()
        //{
        //    GameObject gob;
        //    for (int i = 0; i < myObjects.Count; i++)
        //    {
        //        gob = myObjects[i];

        //        gob.prePhysicsUpdate();
        //    }
        //}

        public void prePhysicsUpdate()
        {
            foreach (var gob in myObjects)
            {
                PhysicsComponent physics = gob.getComponent<PhysicsComponent>();
                if (physics != null)
                {
                    physics.prePhysicsUpdate();
                }
            }
        }

        //public void update()
        //{
        //    List<int> toDestroy = new List<int>();
        //    GameObject gob;
        //    for (int i = 0; i < myObjects.Count; i++)
        //    {
        //        gob = myObjects[i];

        //        gob.update();

        //        gob.checkDestroyMe();

        //        if (gob.ToBeDestroyed == true)
        //        {
        //            toDestroy.Add(i);
        //        }
        //    }

        //    if (toDestroy.Count > 0)
        //    {
        //        for (int i = toDestroy.Count - 1; i >= 0; i--)
        //        {
        //            gob = myObjects[toDestroy[i]];
        //            myObjects[toDestroy[i]].killMe();
        //            myObjects.RemoveAt(toDestroy[i]);

        //        }
        //    }

        //    toDestroy.Clear();
        //    //Debug.Log ("NUm Objects is " + myObjects.Count);
        //}

        public void update()
        {
            List<GameObject> toDestroy = new List<GameObject>();

            foreach (var gob in myObjects)
            {
                gob.update();

                // Only check destruction for objects with a PhysicsComponent
                PhysicsComponent physics = gob.getComponent<PhysicsComponent>();
                if (physics != null)
                {
                    physics.checkDestroyMe(gob);
                }

                if (gob.ToBeDestroyed)
                {
                    toDestroy.Add(gob);
                }
            }

            // Destroy marked objects
            foreach (var gob in toDestroy)
            {
                gob.Destroy();
                myObjects.Remove(gob);
            }
        }

    }
}
