using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Components
{
    internal abstract class BaseComponent
    {
        protected GameObject owner;
        protected Tags tags;

        protected bool enabled = true; // Default: enabled
       
        // Constructor to initialize the component with a GameObject reference
        public BaseComponent(GameObject owner)
        {
            this.owner = owner;
            tags = new Tags();
            if (owner != null)
            {
                registerComponent();
            }
        }

        public virtual void dispose()
        {
            owner = null;
            tags = null;
            enabled = false;
        }

        // Optionally override in derived components if needed
        public virtual void initialize() {

            if(owner != null) {
                registerComponent();
            }
        }

        // Update is called from the GameObject and checks if enabled
        public virtual void update()
        {
            //if (enabled)
            //{
            //    UpdateComponent();
            //}
        }

        public void registerComponent()
        {
            GameObjectManager.getInstance().addComponent(owner, this);
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
