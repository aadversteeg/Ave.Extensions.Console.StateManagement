using System.Collections.Generic;
using System.IO;

namespace Ave.Extensions.Console.StateManagement
{
    public class FileSessionStorage : ISessionStorage
    {
        private IFile _file;
        private ISessionStateSerializer _sessionStateSerializer;
        private string _path;

        public FileSessionStorage(IFile file, ISessionStateSerializer sessionStateSerializer, string path)
        {
            _file = file;
            _sessionStateSerializer = sessionStateSerializer;
            _path = path;
        }

        public IDictionary<string, object> Load(string sessionKey)
        {
            var sessionFilename = Path.Combine(_path, sessionKey);

            if (_file.Exists(sessionFilename))
            {
                var bytes = _file.ReadAllBytes(sessionFilename);
                return _sessionStateSerializer.Deserialize(bytes);
            }

            return new Dictionary<string, object>();
        }

        public void Save(string sessionKey, IDictionary<string, object> sessionState)
        {
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
            var sessionFilename = Path.Combine(_path, sessionKey);

            var bytes = _sessionStateSerializer.Serialize(sessionState);
            _file.WriteAllBytes(sessionFilename, bytes);
        }
    }
}
