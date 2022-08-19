using NUnit.Framework;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class InstallerHelperTests
    {
        [Test]
        public void DownloadInstaller_WebException_ReturnsFalse()
        {
            
        }
        
        [Test]
        public void DownloadInstaller_WebGenericException_ThrowsTheException()
        {
            
        }
        
        [Test]
        public void DownloadInstaller_FileDownloaded_ReturnsTrue()
        {
            
        }
        
        [Test]
        public void DownloadInstaller_FileDownloaded_SetupDestinationFilePropertyIsNotNullOrEmpty()
        {
            
        }
        
        [Test]
        [TestCase(null, null)]
        [TestCase("a", null)]
        [TestCase(null, "a")]
        [TestCase("", "")]
        [TestCase("a", "")]
        [TestCase("", "b")]
        public void DownloadInstaller_AnyParameterNullOrEmpty_ReturnsFalse(string customerName, string installerName)
        {
            
        }
    }
}