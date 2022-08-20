using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class BookingHelperOverlappingBookingsExistV2Tests
    {
        private Mock<IBookingRepository> _bookingRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            BookingHelper.BookingRepository = _bookingRepositoryMock.Object;
        }

        [Test]
        public void StatusIsCanceled_ReturnsEmptyString()
        {
            var result = BookingHelper.OverlappingBookingsExistV2(new Booking { Status = "Cancelled" });

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void NoOverlappingRecordFound_ReturnsEmptyString()
        {
            var result = BookingHelper.OverlappingBookingsExistV2(new Booking());

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void WhenOverlappingRecordFound_ReturnsReferenceProperty()
        {
            _bookingRepositoryMock.Setup(r => r.GetFirstOrDefaultOverlappingBooking(It.IsAny<Booking>())).Returns(new Booking() { Reference = "a" });

            var result = BookingHelper.OverlappingBookingsExistV2(new Booking());

            Assert.That(result, Is.EqualTo("a"));
        }
    }
}