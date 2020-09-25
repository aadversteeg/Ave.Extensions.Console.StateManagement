using System;
using System.Diagnostics;
using System.Management;

namespace Ave.Extensions.Console.StateManagement
{
    public class SessionManager : ISessionManager
    {
        public SessionManager()
        {
            var parentProcessId = GetParentId(Process.GetCurrentProcess());
            Key = parentProcessId.ToString().PadLeft(10, '0');
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
        public string Key { get; }
    }
}
