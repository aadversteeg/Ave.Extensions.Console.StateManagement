using System.Collections.Generic;

namespace Ave.Extensions.Console.StateManagement
{
    public interface IProcessIdProvider
    {
        int ParentProcessId { get; }

        IReadOnlyCollection<int> AllProcessIds { get;  } 
    }
}
