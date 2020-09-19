using System.Collections.Generic;

namespace Ave.Extensions.Console.StateManagement
{
    public class StateManager : IStateManager
    {
        private readonly IDictionary<string, object> _state = new Dictionary<string, object>();
        private readonly string _sessionKey;
        private readonly ISessionStorage _sessionStorage;

        public StateManager(string applicationName, ISession session, ISessionStorage sessionStorage)
        {
            ApplicationName = applicationName;
            _sessionKey = session.Key;
            _sessionStorage = sessionStorage;

            _state = _sessionStorage.Load(_sessionKey);
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

        public void Save()
        {
            _sessionStorage.Save(_sessionKey, _state);
        }
    }
}
