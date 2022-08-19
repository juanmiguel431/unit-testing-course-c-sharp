using System.Net;

namespace TestNinja.Mocking
{
    public class InstallerHelper
    {
        private readonly IWebClientWrapper _webClientWrapper;
        
        public InstallerHelper(IWebClientWrapper webClientWrapper)
        {
            _webClientWrapper = webClientWrapper;
        }
        
        public bool DownloadInstaller(string customerName, string installerName, string destinationFileName)
        {
            try
            {
                var address = $"http://example.com/{customerName}/{installerName}";
                
                _webClientWrapper.Download(address, destinationFileName);

                return true;
            }
            catch (WebException)
            {
                return false; 
            }
        }
    }
}