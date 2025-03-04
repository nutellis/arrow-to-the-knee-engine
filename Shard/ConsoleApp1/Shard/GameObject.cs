﻿/*
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
    //class GameObject
    //{
    //    private Transform3D transform;
    //    private bool transient;
    //    private bool toBeDestroyed;
    //    private bool visible;
    //    private PhysicsBody myBody;
    //    private List<string> tags;

    //    public void addTag(string str)
    //    {
    //        if (tags.Contains(str))
    //        {
    //            return;
    //        }

    //        tags.Add(str);
    //    }

    //    public void removeTag(string str)
    //    {
    //        tags.Remove(str);
    //    }

    //    public bool checkTag(string tag)
    //    {
    //        return tags.Contains(tag);
    //    }

    //    public String getTags()
    //    {
    //        string str = "";

    //        foreach (string s in tags)
    //        {
    //            str += s;
    //            str += ";";
    //        }

    //        return str;
    //    }

    //    public void setPhysicsEnabled()
    //    {
    //        MyBody = new PhysicsBody(this);
    //    }


    //    public bool queryPhysicsEnabled()
    //    {
    //        if (MyBody == null)
    //        {
    //            return false;
    //        }
    //        return true;
    //    }

    //    internal Transform3D Transform
    //    {
    //        get => transform;
    //    }

    //    internal Transform Transform2D
    //    {
    //        get => (Transform)transform;
    //    }


    //    public bool Visible
    //    {
    //        get => visible;
    //        set => visible = value;
    //    }
    //    public bool Transient { get => transient; set => transient = value; }
    //    public bool ToBeDestroyed { get => toBeDestroyed; set => toBeDestroyed = value; }
    //    internal PhysicsBody MyBody { get => myBody; set => myBody = value; }

    //    public virtual void initialize()
    //    {
    //    }

    //    public virtual void update()
    //    {
    //    }

    //    public virtual void physicsUpdate()
    //    {
    //    }

    //    public virtual void prePhysicsUpdate()
    //    {
    //    }

    //    public GameObject()
    //    {
    //        GameObjectManager.getInstance().addGameObject(this);

    //        transform = new Transform3D(this);
    //        visible = false;

    //        ToBeDestroyed = false;
    //        tags = new List<string>();

    //        this.initialize();

    //    }

    //}

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
            transform = new Transform(this);
            tags = new Tags();

            visible = false;
            toBeDestroyed = false;

            this.initialize();
        }

        // Optional: Can be overridden in derived classes to add more specific logic
        public virtual void initialize() { }

        // Called each frame to update all enabled components
        public virtual void update()
        {

            GameObjectManager.getInstance().tickComponents(this);
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

        //TODO: check if this should be on physics compponent only
        public virtual void killMe()
        {
            transform = null;
            tags = null;

            GameObjectManager.getInstance().removeAllComponents(this);
        }
    }
}
