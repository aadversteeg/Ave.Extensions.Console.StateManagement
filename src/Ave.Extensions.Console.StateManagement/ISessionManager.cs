using System.Collections.Generic;

namespace Ave.Extensions.Console.StateManagement
{
    public interface ISessionManager
    {
        IDictionary<string, object> Load(StateScope scope);

        void Save(StateScope scope, IDictionary<string, object> state);
    }
}
