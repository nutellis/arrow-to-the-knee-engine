/*
*
*   Any game object that is going to react to collisions will need to implement this interface.
*   @author Michael Heron
*   @version 1.0
*   
*/

namespace Shard
{
    interface CollisionHandler
    {
        public abstract void onCollisionEnter(PhysicsBody x);
        public abstract void onCollisionExit(PhysicsBody x);
        public abstract void onCollisionStay(PhysicsBody x);
    }
}
