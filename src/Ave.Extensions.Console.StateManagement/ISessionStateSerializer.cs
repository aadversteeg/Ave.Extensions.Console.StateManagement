using System.Collections.Generic;

namespace Ave.Extensions.Console.StateManagement
{
    public interface ISessionStateSerializer
    {
        IDictionary<string, object> Deserialize(byte[] bytes);
        byte[] Serialize(IDictionary<string, object> sessionState);
    }
}
