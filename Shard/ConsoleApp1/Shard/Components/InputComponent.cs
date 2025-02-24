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
        Dictionary<string, int> inputs;


        private List<InputAction> actions;

        public InputComponent(GameObject owner) : base(owner)
        {
            inputs = new Dictionary<string, int>();
            actions = new List<InputAction>();
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
            //match name string to key code
            if (inputs.ContainsKey(name))
            {
                int key = inputs[name];
                InputAction inputAction = new InputAction(name, type, key, action, parameters);
                actions.Add(inputAction);
            }
        }

        public void handleInput(InputEvent inp, InputType eventType)
        {
            if (Bootstrap.getRunningGame().isRunning() == false)
            {
                return;
            }
            // we need to check if the input event is in our list of inputs
            if (inputs.ContainsValue(inp.Key))
            {
                string name = inputs.FirstOrDefault(x => x.Value == inp.Key).Key;

                InputAction action = actions.FirstOrDefault(x => x.Name == name);
                
                if (action == null)
                {
                    return;
                } else
                {
                    switch (action.Type)
                    {
                        case InputType.Pressed:
                            if (eventType == InputType.Pressed)
                            {
                                action.Execute();
                            }
                            break;
                        case InputType.Held:
                            if (eventType == InputType.Held)
                            {
                                action.Execute();
                            }
                            break;
                        case InputType.Released:
                            if (eventType == InputType.Released)
                            {
                                action.Execute();
                            }
                            break;
                    }
                }
            }
        }
    }
}
