using System;
using System.Net;
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class InstallerHelperTests
    {
        private Mock<IWebClientWrapper> _webClientWrapperMock;
        
        [SetUp]
        public void SetUp()
        {
            _webClientWrapperMock = new Mock<IWebClientWrapper>();
        }
        
        [Test]
        public void DownloadInstaller_WebException_ReturnsFalse()
        {
            _webClientWrapperMock.Setup(c => c.Download(It.IsAny<string>(), It.IsAny<string>())).Throws<WebException>();
                
            var installerHelper = new InstallerHelper(_webClientWrapperMock.Object);

            var result = installerHelper.DownloadInstaller("", "", "");
            
            Assert.That(result, Is.EqualTo(false));
        }
        
        [Test]
        public void DownloadInstaller_WhenGenericException_ThrowsTheException()
        {
            _webClientWrapperMock.Setup(c => c.Download(It.IsAny<string>(), It.IsAny<string>())).Throws<Exception>();
                
            var installerHelper = new InstallerHelper(_webClientWrapperMock.Object);
            
            Assert.That(() => installerHelper.DownloadInstaller("", "", ""), Throws.TypeOf<Exception>());
        }

        [Test]
        public void DownloadInstaller_FileDownloaded_ReturnsTrue()
        {
            var installerHelper = new InstallerHelper(_webClientWrapperMock.Object);

            var result = installerHelper.DownloadInstaller("", "", "");
            
            Assert.That(result, Is.EqualTo(true));
        }
    }
}