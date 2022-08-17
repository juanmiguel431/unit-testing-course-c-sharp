using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class VideoServiceTests
    {
        private VideoService _videoService;
        private Mock<IFileReader> _mockFileReader;

        [SetUp]
        public void SetUp()
        {
            _mockFileReader = new Mock<IFileReader>();
            _videoService = new VideoService(_mockFileReader.Object);
        }
        
        [Test]
        public void ReadVideoTitle_EmptyFile_ReturnErrorMessage()
        {
            _mockFileReader.Setup(fr => fr.Read("video.txt")).Returns("");
            
            var result = _videoService.ReadVideoTitle();
            
            Assert.That(result, Does.Contain("error").IgnoreCase);
        }
    }
}