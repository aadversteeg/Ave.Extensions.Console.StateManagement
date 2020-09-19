using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Ave.Extensions.Console.StateManagement
{
    public class BinarySessionStateSerializer : ISessionStateSerializer
    {
        public IDictionary<string, object> Deserialize(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();

                memoryStream.Write(bytes, 0, bytes.Length);
                memoryStream.Seek(0, SeekOrigin.Begin);

                return (IDictionary<string, object>) binaryFormatter.Deserialize(memoryStream);
            }
        }

        public byte[] Serialize(IDictionary<string, object> sessionState)
        {
            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();

                binaryFormatter.Serialize(memoryStream, sessionState);

                return memoryStream.ToArray();
            }
        }
    }
}
