using System;
using System.Collections.Generic;

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
        public Axis axis;
        public float direction;
        public Action<float> action;
        public float currentValue;

        public InputAxis(Axis type, float direction, Action<float> action)
        {
            this.axis = type;
            this.direction = direction;
            this.action = action;

            currentValue = 0.0f;
        }
    }
}
