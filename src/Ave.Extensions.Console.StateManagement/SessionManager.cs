using System.Collections.Generic;
using System.Linq;

namespace Ave.Extensions.Console.StateManagement
{
    public class SessionManager : ISessionManager
    {
        private const string UserKey = "user";

        private readonly IProcessIdProvider _processIdProvider;
        private readonly ISessionStorage _sessionStorage;
        private readonly string _sessionKey;

        public SessionManager(ISessionStorage sessionStorage, IProcessIdProvider processIdProvider)
        {
            _sessionStorage = sessionStorage;
            _processIdProvider = processIdProvider;

            _sessionKey = ToSessionKey(_processIdProvider.ParentProcessId);

            PurgeSessions();
        }

        private void PurgeSessions()
        {
            var runningSessionKeys = _processIdProvider.AllProcessIds
                .Select(p => ToSessionKey(p)).ToList();

            var storedSessionKeys = _sessionStorage.StoredSessions;
            foreach(var storedSessionKey in storedSessionKeys)
            {
                if( !runningSessionKeys.Contains(storedSessionKey) && (storedSessionKey != UserKey))
                {
                    _sessionStorage.Delete(storedSessionKey);
                }
            }
        }

        private string ToSessionKey(int processId)
        {
            return processId.ToString().PadLeft(10, '0');
        }

        public IDictionary<string, object> Load(StateScope scope)
        {
            if (scope == StateScope.Session)
            {
                return _sessionStorage.Load(_sessionKey);
            }
            return _sessionStorage.Load(UserKey);
        }

        public void Save(StateScope scope, IDictionary<string, object> state)
        {
            if (scope == StateScope.Session)
            {
                _sessionStorage.Save(_sessionKey, state);
            }
            else
            {
                _sessionStorage.Save(UserKey, state);
            }
        }
    }
}
