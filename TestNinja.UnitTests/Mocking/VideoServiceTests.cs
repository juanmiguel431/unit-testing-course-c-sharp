using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
        private Mock<IVideoRepository> _mockVideoRepository;
        private Mock<IVideoContext> _mockVideoContext;

        [Test]
        public void ReadVideoTitle_EmptyFile_ReturnErrorMessage()
        {
            _mockFileReader.Setup(fr => fr.Read("video.txt")).Returns("");

            var result = _videoService.ReadVideoTitle();

            Assert.That(result, Does.Contain("error").IgnoreCase);
        }

        [Test]
        public void GetUnprocessedVideosAsCsv_NoVideosFound_ReturnsEmptyString()
        {
            _mockFileReader = new Mock<IFileReader>();
            _mockVideoRepository = new Mock<IVideoRepository>();

            _mockVideoRepository.Setup(r => r.GetNotProcessed()).Returns(new List<Video>());

            _videoService = new VideoService(_mockFileReader.Object, _mockVideoRepository.Object);

            var result = _videoService.GetUnprocessedVideosAsCsv();

            Assert.That(result, Is.EqualTo(""));
        }

        [Test]
        public void GetUnprocessedVideosAsCsv_WhenThereAreElements_IdPropertiesAreSeparatedByComma()
        {
            _mockFileReader = new Mock<IFileReader>();
            _mockVideoRepository = new Mock<IVideoRepository>();

            var elements = new List<Video>()
            {
                new Video() { Id = 1 },
                new Video() { Id = 2 },
            };
            _mockVideoRepository.Setup(r => r.GetNotProcessed()).Returns(elements);

            _videoService = new VideoService(_mockFileReader.Object, _mockVideoRepository.Object);

            var result = _videoService.GetUnprocessedVideosAsCsv();

            Assert.That(result, Is.EqualTo("1,2"));
        }

        [Test]
        [Ignore("Instead of Mock DbContext lets Mock the repository")]
        public void GetUnprocessedVideosAsCsv_WhenThereAreElements_IdPropertiesAreSeparatedByCommaV2()
        {
            _mockFileReader = new Mock<IFileReader>();
            _mockVideoRepository = new Mock<IVideoRepository>();
            _videoService = new VideoService(_mockFileReader.Object, _mockVideoRepository.Object);
            _mockVideoContext = new Mock<IVideoContext>();

            var elements = new List<Video>()
            {
                new Video() { Id = 1 },
                new Video() { Id = 2 },
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Video>>();

            mockSet.As<IQueryable<Video>>().Setup(e => e.Provider).Returns(elements.Provider);
            mockSet.As<IQueryable<Video>>().Setup(e => e.Expression).Returns(elements.Expression);
            mockSet.As<IQueryable<Video>>().Setup(e => e.ElementType).Returns(elements.ElementType);
            mockSet.As<IQueryable<Video>>().Setup(e => e.GetEnumerator()).Returns(elements.GetEnumerator());

            _mockVideoContext.Setup(c => c.Videos).Returns(mockSet.Object);

            var result = _videoService.GetUnprocessedVideosAsCsv();

            Assert.That(result, Is.EqualTo("1,2"));
        }
    }
}