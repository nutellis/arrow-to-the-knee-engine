using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Components
{
    public abstract class Component
    {
        private bool enabled = true; // Default: enabled

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

        // Enable and disable functionality for the component
        public bool IsEnabled => enabled;

        public void Enable() => enabled = true;
        public void Disable() => enabled = false;
    }
}
