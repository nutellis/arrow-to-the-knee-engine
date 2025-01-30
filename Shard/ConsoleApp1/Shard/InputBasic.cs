/*
*
*   This is about a simple an input system as you can have, and it's horrible.
*       Only used for illustrative purposes.
*   @author Michael Heron
*   @version 1.0
*   
*/

using System;

namespace Shard
{
    class InputBasic : InputSystem
    {
        public override void getInput()
        {
            InputEvent ie;
            ConsoleKeyInfo cki;
            if (!Console.KeyAvailable)
            {
                return;
            }
            
            cki = Console.ReadKey(true);

            ie = new InputEvent();

            ie.Key = (int)cki.KeyChar;

            informListeners(ie, "KeyDown");
            informListeners(ie, "KeyUp");

            Debug.getInstance().log("Key is " + ie.Key);
        }
    }
}
