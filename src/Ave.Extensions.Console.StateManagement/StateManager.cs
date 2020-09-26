using System.Collections.Generic;

namespace Ave.Extensions.Console.StateManagement
{
    public class StateManager : IStateManager
    {
        private readonly IDictionary<string, object> _sessionState = new Dictionary<string, object>();
        private readonly IDictionary<string, object> _userState = new Dictionary<string, object>();

        private readonly ISessionManager _sessionManager;

        public StateManager(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;

            _sessionState = _sessionManager.Load(StateScope.Session);
            _userState = _sessionManager.Load(StateScope.User);
        }

        public bool HasValueFor(StateScope scope, string key)
        {
            var state = scope == StateScope.Session ? _sessionState : _userState;
            return state.ContainsKey(key);
        }

        public T GetValue<T>(StateScope scope, string key, T defaultValue = default)
        {
            var state = scope == StateScope.Session ? _sessionState : _userState;
            if (state.ContainsKey(key))
            {
                return (T) state[key];
            }
            return defaultValue;
        }

        public void SetValue<T>(StateScope scope, string key, T value)
        {
            var state = scope == StateScope.Session ? _sessionState : _userState;
            if (state.ContainsKey(key))
            {
                state[key] = value;
            }
            else
            {
                state.Add(key, value);
            }
        }

        public void Save()
        {
            _sessionManager.Save(StateScope.Session, _sessionState);
            _sessionManager.Save(StateScope.User, _userState);
        }
    }
}
