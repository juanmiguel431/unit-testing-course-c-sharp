using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class BookingHelperTests
    {
        private Mock<IBookingRepository> _bookingRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            BookingHelper.BookingRepository = _bookingRepositoryMock.Object;
        }

        [Test]
        public void OverlappingBookingsExist_StatusIsCanceled_ReturnsEmptyString()
        {
            var result = BookingHelper.OverlappingBookingsExist(new Booking { Status = "Cancelled" });

            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void OverlappingBookingsExist_NoOverlappingRecordFound_ReturnsEmptyString()
        {
            var result = BookingHelper.OverlappingBookingsExist(new Booking());

            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void OverlappingBookingsExist_WhenOverlappingRecordFound_ReturnsReferenceProperty()
        {
            _bookingRepositoryMock.Setup(r => r.GetFirstOrDefaultOverlappingBooking(It.IsAny<Booking>())).Returns(new Booking() { Reference = "a" });

            var result = BookingHelper.OverlappingBookingsExist(new Booking());
            
            Assert.That(result, Is.EqualTo("a"));
        }
    }
}