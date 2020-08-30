using System.Collections.Generic;

namespace Ave.Extensions.Console.StateManagement
{
    public class StateManager : IStateManager
    {
        private readonly IDictionary<string, object> _state = new Dictionary<string, object>();


        public StateManager(string applicationName)
        {
            ApplicationName = applicationName;
        }

        public string ApplicationName { get; }

        public bool HasValueFor(string key)
        {
            return _state.ContainsKey(key);
        }

        public T GetValue<T>(string key, T defaultValue = default)
        {
            if(_state.ContainsKey(key))
            {
                return (T) _state[key];
            }
            return defaultValue;
        }

        public void SetValue<T>(string key, T value)
        {
            if( _state.ContainsKey(key))
            {
                _state[key] = value;
            }
            else
            {
                _state.Add(key, value);
            }
        }
    }
}
