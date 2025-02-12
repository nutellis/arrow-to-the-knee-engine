/*
*
*   Any game object that is going to react to collisions will need to implement this interface.
*   @author Michael Heron
*   @version 1.0
*   
*/

using System;

namespace Shard
{
    public class PhysicsComponent
    {
        public object Parent { get; internal set; }

        internal void AddCollider()
        {
            throw new NotImplementedException();
        }
    }
}