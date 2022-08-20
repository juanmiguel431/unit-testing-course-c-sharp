using System.IO;

namespace TestNinja.Mocking
{
    public interface IFileManager
    {
        void Delete(string filename);
    }

    public class FileManager : IFileManager
    {
        public void Delete(string filename)
        {
            File.Delete(filename);
        }
    }
}