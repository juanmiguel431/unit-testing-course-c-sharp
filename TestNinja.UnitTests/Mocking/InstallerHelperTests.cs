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
        private InstallerHelper _installerHelper;

        [SetUp]
        public void SetUp()
        {
            _webClientWrapperMock = new Mock<IWebClientWrapper>();
            _installerHelper = new InstallerHelper(_webClientWrapperMock.Object);
        }
        
        [Test]
        public void DownloadInstaller_WebException_ReturnsFalse()
        {
            _webClientWrapperMock
                .Setup(c =>
                    c.Download(It.IsAny<string>(), It.IsAny<string>()))
                .Throws<WebException>();

            var result = _installerHelper.DownloadInstaller("customer", "installer", "destination");
            
            Assert.That(result, Is.False);
        }
        
        [Test]
        public void DownloadInstaller_WhenGenericException_ThrowsTheException()
        {
            _webClientWrapperMock
                .Setup(c =>
                    c.Download(It.IsAny<string>(), It.IsAny<string>()))
                .Throws<Exception>();

            Assert.That(() => _installerHelper.DownloadInstaller("customer", "installer", "destination"), Throws.TypeOf<Exception>());
        }

        [Test]
        public void DownloadInstaller_FileDownloaded_ReturnsTrue()
        {
            var result = _installerHelper.DownloadInstaller("customer", "installer", "destination");
            
            Assert.That(result, Is.True);
        }
    }
}