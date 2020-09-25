using System;
using System.Diagnostics;
using System.Linq;
using System.Management;

namespace Ave.Extensions.Console.StateManagement
{
    public class SessionManager : ISessionManager
    {
        ISessionStorage _sessionStorage;


        public SessionManager(ISessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;

            var parentProcessId = GetParentId(Process.GetCurrentProcess());
            Key = parentProcessId.ToString().PadLeft(10, '0');

            PurgeSessions();
        }

        private int GetParentId(Process process)
        {
            // query the management system objects
            string queryText = string.Format("select parentprocessid from win32_process where processid = {0}", process.Id);
            using (var searcher = new ManagementObjectSearcher(queryText))
            {
                foreach (var obj in searcher.Get())
                {
                    object data = obj.Properties["parentprocessid"].Value;
                    if (data != null)
                        return Convert.ToInt32(data);
                }
            }
            return 0;
        }

        private void PurgeSessions()
        {
            var runningSessionKeys = Process.GetProcesses().Select(p => p.Id.ToString().PadLeft(10, '0')).ToList();

            var storedSessionKeys = _sessionStorage.StoredSessions;
            foreach(var storedSessionKey in storedSessionKeys)
            {
                if( !runningSessionKeys.Contains(storedSessionKey))
                {
                    _sessionStorage.Delete(storedSessionKey);
                }
            }
        }

        public string Key { get; }
    }
}
