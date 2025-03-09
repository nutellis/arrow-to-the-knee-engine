/*
*
*   Anything that is going to be an interactable object in your game should extend from GameObject.  
*       It handles the life-cycle of the objects, some useful general features (such as tags), and serves 
*       as the convenient facade to making the object work with the physics system.  It's a good class, Bront.
*   @author Michael Heron
*   @version 1.0
*   
*/

using Shard.Shard;
using System;
using System.Collections.Generic;
using Shard.Shard.Components;
using System.Linq;

namespace Shard
{
     class GameObject
    {
        public Guid uuid;
        protected Tags tags;
        protected bool visible = true;
        protected bool transient = false;
        protected bool toBeDestroyed = false;

        public Transform transform { get; private set; }


        public bool Visible
        {
            get => visible;
            set => visible = value;
        }

        public bool Transient
        {
            get => transient;
            set => transient = value;
        }

        public bool ToBeDestroyed
        {
            get => toBeDestroyed;
            set => toBeDestroyed = value;
        }
        public Tags Tags
        {
            get => tags;
            set => tags = value;
        }


        public GameObject()
        {
            uuid = Guid.NewGuid();

            GameObjectManager.getInstance().addGameObject(this);  // Manage game object
            transform = new Transform();
            tags = new Tags();

            visible = false;
            toBeDestroyed = false;

            this.initialize();
        }

        public virtual void initialize() { }

        public virtual void physicsUpdate()
        {
        }

        public virtual void prePhysicsUpdate()
        {
        }

        // Called each frame to update all enabled components
        public virtual void update()
        {
            transform.consumeMovement();
        }

        public void checkDestroyMe()
        {

            if (!transient)
            {
                return;
            }

            if (transform.X > 0 && transform.X < Bootstrap.getDisplay().getWidth())
            {
                if (transform.Y > 0 && transform.Y < Bootstrap.getDisplay().getHeight())
                {
                    return;
                }
            }

            ToBeDestroyed = true;
        }

        public virtual void killMe()
        {
            transform = null;
            tags = null;

            GameObjectManager.getInstance().removeAllComponents(this);
        }
    }
}
