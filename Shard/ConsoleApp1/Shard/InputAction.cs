using System;

namespace Shard.Shard
{
    class InputAction
    {
        
        public InputType Type { get; private set; }
        public string Name { get; private set; }

        private Action<object[]> _action;
        private object[] _parameters;

        public InputAction(string name, InputType type, Action<object[]> action, params object[] parameters)
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
}
