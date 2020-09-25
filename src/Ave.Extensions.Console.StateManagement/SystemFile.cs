namespace Ave.Extensions.Console.StateManagement
{
    public class SystemFile : IFile
    {
        public void Delete(string path)
        {
            System.IO.File.Delete(path);
        }

        public bool Exists(string path)
        {
            return System.IO.File.Exists(path);
        }

        public byte[] ReadAllBytes(string path)
        {
            return System.IO.File.ReadAllBytes(path);
        }

        public void WriteAllBytes(string path, byte[] bytes)
        {
            System.IO.File.WriteAllBytes(path, bytes);
        }
    }
}
