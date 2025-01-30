/*
*
*   Any object that will want to listen for input events should register that interest, for 
*       which they will need to have this interface implemented.
*   @author Michael Heron
*   @version 1.0
*   
*/

namespace Shard
{
    interface InputListener
    {


        public void handleInput(InputEvent inp, string eventType);
    }
}
