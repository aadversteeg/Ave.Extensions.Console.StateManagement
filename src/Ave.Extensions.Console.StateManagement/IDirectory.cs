namespace Ave.Extensions.Console.StateManagement
{
    public interface IDirectory
    {
        bool Exists(string path);

        void Create(string path);
    }
}
