/*
*
*   SDL provides an input layer, and we're using that.  This class tracks input, anchors it to the 
*       timing of the game loop, and converts the SDL events into one that is more abstract so games 
*       can be written more interchangeably.
*   @author Michael Heron
*   @version 1.0
*   
*/

using SDL2;
using System.Collections.Generic;

namespace Shard
{
    public enum InputType
    {
        Pressed,
        Released,
        Held,
        MouseMotion,
        MouseDown,
        MouseUp,
        MouseWheel
    }

    // We'll be using SDL2 here to provide our underlying input system.
    class InputFramework : InputSystem
    {

        double tick, timeInterval;
        private Dictionary<int, double> keyHeldSeconds;
       
        public InputFramework()
        {
            keyHeldSeconds = new Dictionary<int, double>();
        }

        public override void getInput()
        {

            SDL.SDL_Event ev;
            int res;
            InputEvent ie;

            tick += Bootstrap.getDeltaTime();

            if (tick < timeInterval)
            {
                return;
            }

            while (tick >= timeInterval)
            {

                res = SDL.SDL_PollEvent(out ev);


                if (res != 1)
                {
                    return;
                }

                ie = new InputEvent();

                if (ev.type == SDL.SDL_EventType.SDL_MOUSEMOTION)
                {
                    SDL.SDL_MouseMotionEvent mot;

                    mot = ev.motion;

                    ie.X = mot.x;
                    ie.Y = mot.y;

                    informListeners(ie, InputType.MouseMotion);
                }

                if (ev.type == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN)
                {
                    SDL.SDL_MouseButtonEvent butt;

                    butt = ev.button;

                    ie.Button = (int)butt.button;
                    ie.X = butt.x;
                    ie.Y = butt.y;

                    informListeners(ie, InputType.MouseDown);
                }

                if (ev.type == SDL.SDL_EventType.SDL_MOUSEBUTTONUP)
                {
                    SDL.SDL_MouseButtonEvent butt;

                    butt = ev.button;

                    ie.Button = (int)butt.button;
                    ie.X = butt.x;
                    ie.Y = butt.y;

                    informListeners(ie, InputType.MouseUp);
                }

                if (ev.type == SDL.SDL_EventType.SDL_MOUSEWHEEL)
                {
                    SDL.SDL_MouseWheelEvent wh;

                    wh = ev.wheel;

                    ie.X = (int)wh.direction * wh.x;
                    ie.Y = (int)wh.direction * wh.y;

                    informListeners(ie, InputType.MouseWheel);
                }


                if (ev.type == SDL.SDL_EventType.SDL_KEYDOWN)
                {
                    ie.Key = (int)ev.key.keysym.scancode;
                    // Record the time the key was pressed
                    if (!keyHeldSeconds.ContainsKey(ie.Key))
                    {
                        Debug.getInstance().log("Keydown: " + ie.Key);
                        keyHeldSeconds[ie.Key] = tick;
                        informListeners(ie, InputType.Pressed);
                    } else
                    {
                        double pressTime = keyHeldSeconds[ie.Key];
                        double duration = tick - pressTime;
                        Debug.getInstance().log("Key " + ie.Key + " is held for: " + duration + "seconds");

                        informListeners(ie, InputType.Held);
                    }
                }

                if (ev.type == SDL.SDL_EventType.SDL_KEYUP)
                {
                    ie.Key = (int)ev.key.keysym.scancode;
                    informListeners(ie, InputType.Released);

                    if (keyHeldSeconds.ContainsKey(ie.Key))
                    {
                        double pressTime = keyHeldSeconds[ie.Key];
                        double duration = tick - pressTime;
                        keyHeldSeconds.Remove(ie.Key);
                        Debug.getInstance().log("Key " + ie.Key + " was held for " + duration + " seconds.");
                    }
                }

                tick -= timeInterval;
            }


        }

        public override void initialize()
        {
            tick = 0;
            timeInterval = 1.0 / 60.0;
        }

    }
}