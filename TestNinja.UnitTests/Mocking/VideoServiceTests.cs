using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class VideoServiceTests
    {
        [Test]
        public void ReadVideoTitle_EmptyFile_ReturnErrorMessage()
        {
            var videoService = new VideoService(new FakeFileReader());

            var result = videoService.ReadVideoTitle();
            
            Assert.That(result, Does.Contain("error").IgnoreCase);
        }
    }
}