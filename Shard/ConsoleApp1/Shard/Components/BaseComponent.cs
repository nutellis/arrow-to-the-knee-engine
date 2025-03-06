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

        protected bool enabled = true;

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
            tags = null;
            enabled = false;
        }

        public virtual void initialize() {

            if(owner != null) {
                registerComponent();
            }
        }
        public virtual void physicsUpdate()
        {
        }

        public virtual void prePhysicsUpdate()
        {
        }

        public virtual void update()
        {

        }

        public void registerComponent()
        {
            GameObjectManager.getInstance().addComponent(owner, this);
        }

        public GameObject Owner => owner;

        public bool IsEnabled => enabled;

        public void Enable() => enabled = true;
        public void Disable() => enabled = false;
    }
}
