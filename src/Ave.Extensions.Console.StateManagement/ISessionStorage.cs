using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ave.Extensions.Console.StateManagement
{
    public interface ISessionStorage
    {
        IDictionary<string, object> Load(string sessionKey);

        void Save(string sessionKey, IDictionary<string, object> sessionState);

        IReadOnlyCollection<string> StoredSessions { get; }

        void Delete(string sessionKey);
    }
}
