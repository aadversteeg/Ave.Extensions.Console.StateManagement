using System.Collections.Generic;

namespace Ave.Extensions.Console.StateManagement
{
    public interface ISessionManager
    {
        IDictionary<string, object> Load();

        void Save(IDictionary<string, object> state);
    }
}
