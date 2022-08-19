using System.Net;

namespace TestNinja.Mocking
{
    public interface IWebClientWrapper
    {
        void Download(string address, string fileName);
    }

    public class WebClientWrapper : IWebClientWrapper
    {
        public void Download(string address, string fileName)
        {
            var client = new WebClient();
            client.DownloadFile(address, fileName);
        }
    }
}