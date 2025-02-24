﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Components
{
    internal abstract class Component
    {
        protected GameObject owner;
        private Tags tags;

        private bool enabled = true; // Default: enabled
       
        //public GameObject parent;
        //internal GameObject Parent { get => parent; set => parent = value; }

        // Constructor to initialize the component with a GameObject reference
        public Component(GameObject owner)
        {
            this.owner = owner;
            tags = new Tags();  
        }

        // Optionally override in derived components if needed
        public virtual void initialize() { }

        // Update is called from the GameObject and checks if enabled
        public virtual void update()
        {
            //if (enabled)
            //{
            //    UpdateComponent();
            //}
        }

        // Each component must define its own Update logic
        //protected abstract void UpdateComponent();

        // Accessor for the GameObject owner
        public GameObject Owner => owner;

        // Enable and disable functionality for the component
        public bool IsEnabled => enabled;

        public void Enable() => enabled = true;
        public void Disable() => enabled = false;
    }
}
