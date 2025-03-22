/*
*
*   Any game object that is going to react to collisions will need to implement this interface.
*   @author Michael Heron
*   @version 1.0
*   
*/

using Shard.Shard;
using Shard.Shard.Components;

namespace Shard
{
    interface CollisionHandler
    {
        public abstract void onCollisionEnter(PhysicsComponent other);
        public abstract void onCollisionExit(PhysicsComponent other);
        public abstract void onCollisionStay(PhysicsComponent other);
    }
}