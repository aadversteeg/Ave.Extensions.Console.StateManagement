using System.Collections.Generic;

namespace Ave.Extensions.Console.StateManagement
{
    public interface IDirectory
    {
        bool Exists(string path);

        void Create(string path);

        IReadOnlyCollection<string> GetFileNames(string path);
    }
}
