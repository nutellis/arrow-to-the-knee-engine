using SpaceInvaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Shard.PathTracer;

namespace Shard.Shard.Components
{
    internal class NavigationComponent : BaseComponent
    {
        public Transform goalPosition;

        private Queue<Node> pathQueue = new Queue<Node>();
        public bool FollowingPath => pathQueue.Count > 0; // Check if moving along a path
        public bool finishedNavigating = false;
        private float moveDistance;
        private double moveSpeed = 100;

        public NavigationComponent(GameObject owner) : base(owner)
        {
            finishedNavigating = true;
        }

        public override void physicsUpdate()
        {
            base.physicsUpdate();
            if(FollowingPath == false)
                {
                    findGoal(goalPosition);
                }
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
            if(Transform.distance(owner.transform, goalPosition) < 0.1f)
            {
                finishedNavigating = true;
            }

            if (pathQueue.Count == 0)
                return;

            var targetPosition = pathQueue.Peek().transform;

            var newPosition = Transform.lerp(owner.transform, targetPosition, 0.3f);

            // snap if we are close to target
            if (Transform.distance(newPosition.X, targetPosition.X) <= 1.0f)
            {
                newPosition.X = targetPosition.X;
            }
                
            if (Transform.distance(newPosition.Y, targetPosition.Y) <= 1.0f)
            { 
                newPosition.Y = targetPosition.Y;
            }

            // get distance from origin
            var finalMovementX = Transform.distance(newPosition.X, owner.transform.X);
            var finalMovementY = Transform.distance(newPosition.Y, owner.transform.Y);

            owner.transform.moveImmediately(finalMovementX, finalMovementY);

            if (Transform.distance(owner.transform, targetPosition) < 1.0f)
            {
                pathQueue.Dequeue(); // Remove waypoint when reached
            }

        }

        public override void update()
        {
            if (FollowingPath)
            {
                followPath();
            }
                base.update();

        }

        public void findGoal(Transform goalPosition)
        {
            PathTracer.getInstance().findPath(owner, ((int, int))(goalPosition.X, goalPosition.Y));
            setPath(PathTracer.getInstance().getPath());
        }



    }
}
