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
using Shard.Shard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Runtime.CompilerServices.RuntimeHelpers;

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

    public enum Axis
    {
        Horizontal,
        Vertical
    }

    // We'll be using SDL2 here to provide our underlying input system.
    class InputFramework : InputSystem
    {
        private static InputFramework me;
        double tick, timeInterval, lastUpdate;
        private Dictionary<int, double> keyHeldSeconds;
        private Dictionary<int, string> registeredInputActions;
        private Dictionary<int, InputAxis> axisBindings;

        const double HOLD_THRESHOLD = 0.25;

        private InputFramework()
        {
            keyHeldSeconds = [];
            registeredInputActions = [];
            axisBindings = [];

            lastUpdate = Bootstrap.getCurrentMillis();
            timeInterval = 1 / 60.0;
        }

        public static InputFramework getInstance()
        {
            if (me == null)
            {
                me = new InputFramework();
            }

            return me;
        }

        public void setInputMapping(string inputActionName, SDL.SDL_Scancode key)
        {
            registeredInputActions[(int)key] = inputActionName;
        }

        public void setAxisMapping(Axis axis, SDL.SDL_Scancode key, float direction)
        {
            axisBindings[(int)key] = new InputAxis(axis,direction, null);
        }



        // slow!
        public double getActionTimeHeld(InputAction action)
        {
            var keyLookup = registeredInputActions
                .Where(pair => pair.Value == action.Name)
                .Select(pair => pair.Key);

            foreach (var key in keyLookup)
            {
                if(keyHeldSeconds.TryGetValue(key, out double start))
                {
                    return (Bootstrap.getCurrentMillis() - start) / 1000.0;
                }
            }

            return 0.0;
        }

        public bool willTick()
        {
            if (Bootstrap.getCurrentMillis() - lastUpdate > timeInterval)
            {
                return true;
            }

            return false;
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

                // lastUpdate = Bootstrap.getCurrentMillis();
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
                    

                    if (registeredInputActions.TryGetValue((int)ev.type, out string inputName))
                    {
                        ie.InputActionName = registeredInputActions[(int)ev.type];
                        informListeners(ie, InputType.MouseMotion);
                    }
                }

                if (ev.type == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN)
                {
                    SDL.SDL_MouseButtonEvent butt;

                    butt = ev.button;

                    ie.Button = (int)butt.button;
                    ie.X = butt.x;
                    ie.Y = butt.y;
                    

                    if (registeredInputActions.TryGetValue((int)ev.type, out string inputName))
                    {
                        ie.InputActionName = registeredInputActions[(int)ev.type];
                        informListeners(ie, InputType.MouseDown);
                    }
                }

                if (ev.type == SDL.SDL_EventType.SDL_MOUSEBUTTONUP)
                {
                    SDL.SDL_MouseButtonEvent butt;

                    butt = ev.button;

                    ie.Button = (int)butt.button;
                    ie.X = butt.x;
                    ie.Y = butt.y;
                    

                    if (registeredInputActions.TryGetValue((int)ev.type, out string inputName))
                    {
                        ie.InputActionName = registeredInputActions[(int)ev.type];
                        informListeners(ie, InputType.MouseUp);
                    }
                }

                if (ev.type == SDL.SDL_EventType.SDL_MOUSEWHEEL)
                {
                    SDL.SDL_MouseWheelEvent wh;

                    wh = ev.wheel;

                    ie.X = (int)wh.direction * wh.x;
                    ie.Y = (int)wh.direction * wh.y;
                    

                    if (registeredInputActions.TryGetValue((int)ev.type, out string inputName))
                    {
                        ie.InputActionName = registeredInputActions[(int)ev.type];
                        informListeners(ie, InputType.MouseWheel);
                    }
                }


                if (ev.type == SDL.SDL_EventType.SDL_KEYDOWN)
                {
                    ie.Key = (int)ev.key.keysym.scancode;
                    handleKeyPress(ie);
                }

                if (ev.type == SDL.SDL_EventType.SDL_KEYUP)
                {
                    ie.Key = (int)ev.key.keysym.scancode;
                    handleKeyRelease(ie);
                }

                updateHeldButtons();
                tick -= timeInterval;
            }
        }

        private void handleKeyPress(InputEvent inputEvent)
        {
            int keyCode = inputEvent.Key;
            if (axisBindings.TryGetValue(keyCode, out InputAxis value))
            {
                Console.WriteLine($"Key {keyCode} PRESSED with axis movement on " + value.axis);
                informListeners(inputEvent, InputType.Pressed);
            }
            else if(registeredInputActions.TryGetValue(keyCode, out string inputName)) {

                inputEvent.InputActionName = registeredInputActions[keyCode];

                if (!keyHeldSeconds.ContainsKey(keyCode))
                {
                    keyHeldSeconds[keyCode] = Bootstrap.getCurrentMillis(); // Start tracking hold duration
                }
                Console.WriteLine($"Key {keyCode} PRESSED with Action of " + inputName);
                informListeners(inputEvent, InputType.Pressed);
            } 

        }

        void handleKeyRelease(InputEvent inputEvent)
        {
            int keyCode = inputEvent.Key;
            if (axisBindings.ContainsKey(keyCode))
            {
                Console.WriteLine($"Key {keyCode} RELEASED with axis movement on " + axisBindings[keyCode].axis);
                informListeners(inputEvent, InputType.Released);
            }
            if (registeredInputActions.ContainsKey(keyCode))
            {
                inputEvent.InputActionName = registeredInputActions[keyCode];
                if (keyHeldSeconds.TryGetValue(keyCode, out double value))
                {
                    double duration = (Bootstrap.getCurrentMillis() - value) / 1000.0;
                    Console.WriteLine($"Key {keyCode} RELEASED after {duration:F2} seconds");
                    keyHeldSeconds.Remove(keyCode); // Stop tracking
                    informListeners(inputEvent, InputType.Released);
                }
            }
        }

        private void updateHeldButtons()
        {
            //long currentTime = Bootstrap.getCurrentMillis();
            //foreach (var key in new List<int>(keyHeldSeconds.Keys))
            //{
            //    double duration = (currentTime - keyHeldSeconds[key]) / 1000.0;

            //    if (duration >= HOLD_THRESHOLD)
            //    {
            //        InputEvent inputEvent = new InputEvent();
            //        inputEvent.Key = key;
            //        inputEvent.InputActionName = registeredInputActions[key];

            //        informListeners(inputEvent, InputType.Held);
            //    }
            //}
        }
        //private void processAxisInput()
        //{
        //    foreach (var axis in axisBindings.Values)
        //    {
        //        float axisValue = 0.0f;

        //        foreach (var keyPair in axis.KeyMappings)
        //        {
        //            if (keyHeldSeconds.ContainsKey(keyPair.Key))
        //            {
        //                axisValue += keyPair.Value;
        //            }
        //        }

        //        axisValue = Math.Clamp(axisValue, -1.0f, 1.0f);

        //        if (Math.Abs(axis.CurrentValue - axisValue) > 0.001f)
        //        {
        //            axis.CurrentValue = axisValue;
        //            axis.Callback(axisValue);
        //        }
        //    }
        //}
    }
    
}