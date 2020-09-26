using System.Collections.Generic;

namespace Ave.Extensions.Console.StateManagement
{
    public class StateManager : IStateManager
    {
        private readonly IDictionary<string, object> _state = new Dictionary<string, object>();
        private readonly ISessionManager _sessionManager;

        public StateManager(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
            _state = _sessionManager.Load();
        }

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

        public void Save()
        {
            _sessionManager.Save(_state);
        }
    }
}
