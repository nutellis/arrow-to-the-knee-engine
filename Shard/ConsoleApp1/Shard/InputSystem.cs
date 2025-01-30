/*
*
*   Any game object interested in listening for input events will need to register itself 
*       with this manager.   It handles the informing of all listener objects when an 
*       event is raised.
*   @author Michael Heron
*   @version 1.0
*   
*/

using System.Collections.Generic;

namespace Shard
{

    abstract class InputSystem
    {
        private List<InputListener> myListeners;

        public virtual void initialize()
        {
        }

        public InputSystem()
        {
            myListeners = new List<InputListener>();
        }

        public void addListener(InputListener il)
        {
            if (myListeners.Contains(il) == false)
            {
                myListeners.Add(il);
            }
        }

        public void removeListener(InputListener il)
        {
            myListeners.Remove(il);
        }

        public void informListeners(InputEvent ie, string eventType)
        {
            InputListener il;
            for (int i = 0; i < myListeners.Count; i++)
            {
                il = myListeners[i];

                if (il == null)
                {
                    continue;
                }

                il.handleInput(ie, eventType);
            }
        }
        public abstract void getInput();
    }
}
