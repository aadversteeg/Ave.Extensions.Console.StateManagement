using System.Linq;

namespace Ave.Extensions.Console.StateManagement
{
    public class SessionManager : ISessionManager
    {
        IProcessIdProvider _processIdProvider;
        ISessionStorage _sessionStorage;

        public SessionManager(ISessionStorage sessionStorage, IProcessIdProvider processIdProvider)
        {
            _sessionStorage = sessionStorage;
            _processIdProvider = processIdProvider;

            Key = ToSessionKey(_processIdProvider.ParentProcessId);

            PurgeSessions();
        }

        private void PurgeSessions()
        {
            var runningSessionKeys = _processIdProvider.AllProcessIds
                .Select(p => ToSessionKey(p)).ToList();

            var storedSessionKeys = _sessionStorage.StoredSessions;
            foreach(var storedSessionKey in storedSessionKeys)
            {
                if( !runningSessionKeys.Contains(storedSessionKey))
                {
                    _sessionStorage.Delete(storedSessionKey);
                }
            }
        }

        private string ToSessionKey(int processId)
        {
            return processId.ToString().PadLeft(10, '0');
        }

        public string Key { get; }
    }
}
