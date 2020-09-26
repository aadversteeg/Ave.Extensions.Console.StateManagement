using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;

namespace Ave.Extensions.Console.StateManagement
{
    public class SystemProcessIdProvider : IProcessIdProvider
    {
        public int ParentProcessId
        {
            get
            {
                var currentProcessId = Process.GetCurrentProcess().Id;

                string queryText = string.Format("select parentprocessid from win32_process where processid = {0}", currentProcessId);
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
        }

        public IReadOnlyCollection<int> AllProcessIds
        {
            get
            {
                return Process
                    .GetProcesses()
                    .Select(p => p.Id)
                    .ToList();
            }
        }
    }
}
