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
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Shard
{
    class GameObjectManager
    {
        private static GameObjectManager me;
        List<GameObject> myObjects;

        Dictionary<GameObject, List<BaseComponent>> components;

        private GameObjectManager()
        {
            myObjects = new List<GameObject>();
            components = new Dictionary<GameObject, List<BaseComponent>>();
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

        public void addComponent(GameObject owner, BaseComponent component)
        {
            if (components.ContainsKey(owner))
            {
                components[owner].Add(component);
            }
            else
            {
                List<BaseComponent> newListComponent = new List<BaseComponent>();
                newListComponent.Add(component);
                components.Add(owner, newListComponent);
            }
        }

        public void removeAllComponents(GameObject owner)
        {
            if (components.ContainsKey(owner))
            {
                foreach (BaseComponent component in components[owner])
                {
                    component.dispose();
                }
                components.Remove(owner);
            }
        }

        public void removeComponent(GameObject owner, BaseComponent component)
        {
            if (components.ContainsKey(owner))
            {
                components[owner].Remove(component);
            }
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
                if (components.TryGetValue(gob, out List<BaseComponent> value))
                {
                    PhysicsComponent physics = (PhysicsComponent)value.Find(
                    delegate (BaseComponent physics)
                    {
                        return physics is PhysicsComponent;
                    }
                    );
                    physics?.physicsUpdate();
                }


                //PhysicsComponent physics = gob.getComponent<PhysicsComponent>();
                //if (physics != null)
                //{
                //    physics.physicsUpdate();
                //}
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
                //PhysicsComponent physics = gob.getComponent<PhysicsComponent>();
                //if (physics != null)
                //{
                //    physics.prePhysicsUpdate();
                //}
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
            //Debug.Log ("NUm Objects is " + myObjects.Count);
        }

        public void tickComponents(GameObject owner)
        {
            if(components.ContainsKey(owner))
            {
                foreach (BaseComponent component in components[owner])
                {
                    component.update();
                }
            }
        }

    }
}
