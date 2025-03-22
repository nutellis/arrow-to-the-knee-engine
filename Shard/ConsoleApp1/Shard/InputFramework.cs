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
        double tick, timeInterval;
        private Dictionary<int, double> keyHeldSeconds;
        private Dictionary<int, string> registeredInputActions;

        private Dictionary<int, (string,float)> axisBindings;
        private Dictionary<string,float> axisValue;

        const double HOLD_THRESHOLD = 0.25;

        private InputFramework()
        {
            keyHeldSeconds = [];
            registeredInputActions = [];
            axisBindings = [];
            axisValue = [];

            timeInterval = 1 / 50.0;
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

        public void setAxisMapping(string inputActionName, SDL.SDL_Scancode key, float direction)
        {
            axisBindings[(int)key] = (inputActionName,direction);
            if (!axisValue.ContainsKey(inputActionName))
            {
                axisValue[inputActionName] = 0.0f;
            }
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

                //updateHeldButtons();

                tick -= timeInterval;
            }
        }

        private void handleKeyPress(InputEvent inputEvent)
        {
            int keyCode = inputEvent.Key;
            if (axisBindings.ContainsKey(keyCode))
            {
                if (!keyHeldSeconds.ContainsKey(keyCode))
                {
                    keyHeldSeconds[keyCode] = Bootstrap.getCurrentMillis(); // Start tracking hold duration

                    processAxisInput(keyCode, true);
                }
            } 
            else if (registeredInputActions.TryGetValue(keyCode, out string inputName)) {

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
                if (keyHeldSeconds.TryGetValue(keyCode, out double value))
                {
                    keyHeldSeconds.Remove(keyCode); // Stop tracking
                    processAxisInput(keyCode, false);
                }
            }
            else if (registeredInputActions.ContainsKey(keyCode))
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

        // Will not use for now.
        //private void updateHeldButtons()
        //{
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
        //}
        
        private void processAxisInput(int keyCode, bool toggle)
        {

            string axisName = axisBindings[keyCode].Item1;

            if(toggle) { 
                axisValue[axisName] += axisBindings[keyCode].Item2;
            } else
            {
                axisValue[axisName] -= axisBindings[keyCode].Item2;
            }
            InputEvent axisEvent = new InputEvent();
            axisEvent.InputActionName = axisBindings[keyCode].Item1;

            axisEvent.MoveAmount = (float)Math.Clamp(axisValue[axisName], -1.0, 1.0);

            informListeners(axisEvent, InputType.Held);
        }
    }
    
}