﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ave.Extensions.Console.StateManagement
{
    public class FileSessionStorage : ISessionStorage
    {
        private readonly IDirectory _directory;
        private readonly IFile _file;
        private readonly ISessionStateSerializer _sessionStateSerializer;
        private readonly string _path;

        public FileSessionStorage(IDirectory directory, IFile file, ISessionStateSerializer sessionStateSerializer, string path)
        {
            _directory = directory;
            _file = file;
            _sessionStateSerializer = sessionStateSerializer;
            _path = path;
        }

        public IReadOnlyCollection<string> StoredSessions
        {
            get
            {
                if (!_directory.Exists(_path))
                {
                    return new string[0];
                }
                return _directory
                    .GetFileNames(_path)
                    .Select(fn => Path.GetFileName(fn))
                    .ToList();
            }
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
            if (!_directory.Exists(_path))
            {
                _directory.Create(_path);
            }
            var sessionFilename = Path.Combine(_path, sessionKey);

            var bytes = _sessionStateSerializer.Serialize(sessionState);
            _file.WriteAllBytes(sessionFilename, bytes);
        }

        public void Delete(string sessionKey)
        {
            var path = Path.Combine(_path, sessionKey);
            if(_file.Exists(path))
            {
                _file.Delete(path);
            }

        }
    }
}
