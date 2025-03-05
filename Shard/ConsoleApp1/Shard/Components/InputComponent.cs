using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shard.Shard.Components
{
    internal class InputComponent : BaseComponent, InputListener
    {
        Dictionary<string, InputAction> actions;



        public InputComponent(GameObject owner) : base(owner)
        {
            actions = new Dictionary<string, InputAction>();
        }

        public override void initialize()
        {
            Bootstrap.getInput().addListener(this);
            
        }

        public override void update()
        {
            // Handle input logic
        }

        public void bindInputAction(string name, InputType type, Action<object[]> action, params object[] parameters)
        {
            if (!actions.ContainsKey(name))
            {
                InputAction inputAction = new InputAction(name, type, false, action, parameters);
                actions[name] = inputAction;
            }
        }

        //public void bindAxisAction(AxisType type, Action<object[]> action, params object[] parameters)
        //{
        //    if (!actions.ContainsKey(name))
        //    {
        //        InputAction inputAction = new InputAction(name, type, true, action, parameters);
        //        actions[name] = inputAction;
        //    }
        //}

        public void handleInput(InputEvent inp, InputType eventType)
        {
            if (Bootstrap.getRunningGame().isRunning() == false || inp.InputActionName == null)
            {
                return;
            }
            // we need to check if the input event is in our list of inputs
            if (actions.TryGetValue(inp.InputActionName, out InputAction action)){
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
