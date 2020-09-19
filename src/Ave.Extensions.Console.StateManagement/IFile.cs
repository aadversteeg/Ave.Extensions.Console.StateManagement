namespace Ave.Extensions.Console.StateManagement
{
    public interface IFile
    {
        bool Exists(string path);
        byte[] ReadAllBytes(string path);

        void WriteAllBytes(string path, byte[] bytes);
    }
}
