using System;
using System.Collections.Generic;

namespace Shard.Shard.Components
{
    internal class InputComponent : BaseComponent, InputListener
    {
        Dictionary<string, InputAction> actions;
        Dictionary<string, InputAxis> axisActions;



        public InputComponent(GameObject owner) : base(owner)
        {
            actions = new Dictionary<string, InputAction>();
            axisActions = new Dictionary<string, InputAxis>();

            initialize();
        }

        public override void initialize()
        {
            Bootstrap.getInput().addListener(this);
            
        }

        public override void update()
        {
            // Handle input logic
            foreach(InputAxis inputAxis in axisActions.Values)
            {
                inputAxis.Execute();
            }
        }

        public void bindInputAction(string name, InputType type, Action<object[]> action, params object[] parameters)
        {
            if (!actions.ContainsKey(name))
            {
                InputAction inputAction = new InputAction(name, type, false, action, parameters);
                actions[name] = inputAction;
            }
        }

        public void bindAxisAction(string name, Action<float> action)
        {
            if (!actions.ContainsKey(name))
            {
                InputAxis inputAxis = new InputAxis(name, action);
                axisActions[name] = inputAxis;
            }
        }

        public void handleInput(InputEvent inp, InputType eventType)
        {
            if (Bootstrap.getRunningGame().isRunning() == false || inp.InputActionName == null)
            {
                return;
            }
            if (axisActions.TryGetValue(inp.InputActionName, out InputAxis axisAction))
            {
                if (axisAction == null)
                {
                    return;
                }
                axisAction.direction = inp.MoveAmount;
                axisAction.Execute();
            }
                // we need to check if the input event is in our list of inputs
             else if (actions.TryGetValue(inp.InputActionName, out InputAction action))
             {
                if (action == null)
                {
                    return;
                }

                if (eventType == action.Type)
                {
                    switch (eventType)
                    {
                        case InputType.Pressed:
                            action.Execute();
                            break;
                        case InputType.Held:
                            action.Execute();
                            break;
                        case InputType.Released:
                            action.Execute();
                            break;
                    }
                }
            }
        }
    }
}
