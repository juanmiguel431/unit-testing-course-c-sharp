using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class BookingHelperTests
    {
        [Test]
        public void OverlappingBookingsExist_StatusIsCanceled_ReturnsEmptyString()
        {
            var result = BookingHelper.OverlappingBookingsExist(new Booking() { Status = "Cancelled" });
            
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void OverlappingBookingsExist_NoOverlappingRecordFound_ReturnsEmptyString()
        {
            var bookingRepositoryMock = new Mock<IBookingRepository>();
            BookingHelper.BookingRepository = bookingRepositoryMock.Object;
            
            var result = BookingHelper.OverlappingBookingsExist(new Booking());
            
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void OverlappingBookingsExist_WhenOverlappingRecordFound_ReturnsReferenceProperty()
        {
        }

        [Test]
        public void OverlappingBookingsExist_WhenMoreThanOneOverlappingRecordFound_ReturnsReferencePropertyOfTheFirstOccurrence()
        {
        }
    }
}