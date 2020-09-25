using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ave.Extensions.Console.StateManagement
{
    public class SystemDirectory : IDirectory
    {
        public bool Exists(string path)
        {
            return Directory.Exists(path);
        }

        public void Create(string path)
        {
            Directory.CreateDirectory(path);
        }

        public IReadOnlyCollection<string> GetFileNames(string path)
        {
            return Directory.EnumerateFiles(path).ToList();
        }
    }
}
