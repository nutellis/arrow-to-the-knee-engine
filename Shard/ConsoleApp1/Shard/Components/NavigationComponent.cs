using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Shard.PathTracer;

namespace Shard.Shard.Components
{
    internal class NavigationComponent : BaseComponent
    {
        private (int x, int y) goalPosition;
        private (int x, float y) currentPosition;

        private Queue<Node> pathQueue = new Queue<Node>();
        public bool FollowingPath => pathQueue.Count > 0; // Check if moving along a path


        public NavigationComponent(GameObject owner) : base(owner)
        {
            
        }

        public override void update()
        {
            base.update();
            followPath();
        }

        public void setPath(List<Node> path)
        {
            pathQueue.Clear();
            foreach (var node in path)
            {
                pathQueue.Enqueue(node);
            }
        }

        public void followPath()
        {
            if (pathQueue.Count == 0)
                return; 

            Node nextNode = pathQueue.Peek();
            Vector2 nextPosition = new Vector2(nextNode.posX * 16, nextNode.posY * 16);

            // Move towards next node
            owner.transform.moveImidiately(nextPosition.X - owner.transform.X, nextPosition.Y - owner.transform.Y);

            // Check if reached the node
            if (Math.Abs(owner.transform.X - nextPosition.X) < 1 && Math.Abs(owner.transform.Y - nextPosition.Y) < 1)
            {
                pathQueue.Dequeue(); // Move to the next node
            }
        }


        public void moveTowardsGoal((int x, int y) goalPosition)
        {

            PathTracer.getInstance.findPath(owner, goalPosition);
            setPath(PathTracer.getInstance.getPath());


        }



    }
}
