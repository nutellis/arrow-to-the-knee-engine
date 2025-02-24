using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Components
{
    internal class PhysicsComponent : Component
    {
        private bool physicsEnabled;
        private PhysicsBody myBody;

        public PhysicsComponent(GameObject owner) : base(owner)
        {
            physicsEnabled = true;  // Default: Physics enabled
        }

        public override void initialize() { }

        public override void update()
        {
            base.update();
            // Handle physics logic here if physics is enabled
            if (physicsEnabled)
            {
                // Physics-related updates, e.g., movement, collision, etc.
            }
        }

        //// Enable or disable physics for this component
        //public void setPhysicsEnabled(bool enabled)
        //{
        //    physicsEnabled = enabled;
        //    MyBody = new PhysicsBody(this);
        //}

        // Enable or disable physics
        public void setPhysicsEnabled(bool enabled)
        {
            physicsEnabled = enabled;
            if (enabled)
            {
                myBody = new PhysicsBody(this);  // Create physics body
            }
            else
            {             
                myBody = null;  // Remove physics body
            }
        }

        public bool queryPhysicsEnabled()
        {
            if (MyBody == null)
            {
                return false;
            }
            return true;
        }

        internal PhysicsBody MyBody { get => myBody; set => myBody = value; }

        // Check if physics is enabled
        public bool isPhysicsEnabled() => physicsEnabled;

        //protected override void UpdateComponent()
        //{
        //    throw new NotImplementedException();
        //}

        public void checkDestroyMe(GameObject gameObject)
        {
            TransformComponent transform = gameObject.getComponent<TransformComponent>();
            
            if(!gameObject.Transient)
            {
                return;
            }

            if (transform == null) return;

            int screenWidth = Bootstrap.getDisplay().getWidth();
            int screenHeight = Bootstrap.getDisplay().getHeight();

            if (transform.X < 0 || transform.X > screenWidth || transform.Y < 0 || transform.Y > screenHeight)
            {
                gameObject.ToBeDestroyed = true;
            }
        }

        public virtual void physicsUpdate()
        {
        }

        public virtual void prePhysicsUpdate()
        {
        }

        public virtual void killMe()
        {
            PhysicsManager.getInstance().removePhysicsObject(myBody);

            myBody = null;
        }

    }
}
