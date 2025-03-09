using System;
using System.Collections.Generic;
using static System.Collections.Specialized.BitVector32;

namespace Shard.Shard
{
    class InputAction
    {
        
        public InputType Type { get; private set; }
        public string Name { get; private set; }

        public bool isAxis;

        private Action<object[]> _action;
        private object[] _parameters;

        public InputAction(string name, InputType type, bool isAxis, Action<object[]> action, params object[] parameters)
        {
            Name = name;
            Type = type;
            _action = action;
            _parameters = parameters;
        }

        public void Execute()
        {
            _action?.Invoke(_parameters);
        }
    }

    public class InputAxis
    {
        public string name;
        public float direction;
        private Action<float> action;

        public InputAxis(string name, Action<float> action)
        {
            this.name = name;
            this.action = action;

            direction = 0.0f;
        }

        public void Execute()
        {
            action?.Invoke(direction);
        }
    }
}
