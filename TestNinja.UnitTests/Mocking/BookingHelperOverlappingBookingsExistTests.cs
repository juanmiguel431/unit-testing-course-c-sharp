using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class BookingHelperOverlappingBookingsExistTests
    {
        private Mock<IBookingRepository> _repository;
        private Booking _existingBooking;

        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<IBookingRepository>();
            BookingHelper.BookingRepository = _repository.Object;

            _existingBooking = new Booking
            {
                Id = 2,
                Status = "Reserved",
                Reference = "b",
                ArrivalDate = new DateTime(2022, 02, 01),
                DepartureDate = new DateTime(2022, 02, 05)
            };
        }

        [Test]
        public void StatusIsCanceled_ReturnsEmptyString()
        {
            var result = BookingHelper.OverlappingBookingsExist(new Booking { Status = "Cancelled" });

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void WhenNoOverlapping_ReturnsEmptyString()
        {
            _repository.Setup(r => r.GetActiveBookings(It.IsAny<int>()))
                .Returns(new List<Booking> { }.AsQueryable());
            var result = BookingHelper.OverlappingBookingsExist(new Booking());

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void BeforeArrivalDateAndCollapseExistingBooking_ReturnsReferenceProperty()
        {
            _repository.Setup(r => r.GetActiveBookings(1)).Returns(new List<Booking> { _existingBooking }.AsQueryable());

            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 1,
                Status = "Requested",
                Reference = "a",
                ArrivalDate = Before(_existingBooking.ArrivalDate),
                DepartureDate = Before(_existingBooking.DepartureDate),
            });

            Assert.That(result, Is.EqualTo("b"));
        }

        [Test]
        public void CollapseExistingBooking_ReturnsReferenceProperty()
        {
            _repository.Setup(r => r.GetActiveBookings(1)).Returns(new List<Booking> { _existingBooking }.AsQueryable());

            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 1,
                Status = "Requested",
                Reference = "a",
                ArrivalDate = After(_existingBooking.ArrivalDate),
                DepartureDate = Before(_existingBooking.DepartureDate),
            });

            Assert.That(result, Is.EqualTo("b"));
        }
        
        [Test]
        public void CollapseExistingBookingAndAfterDepartureDate_ReturnsReferenceProperty()
        {
            _repository.Setup(r => r.GetActiveBookings(1)).Returns(new List<Booking> { _existingBooking }.AsQueryable());

            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 1,
                Status = "Requested",
                Reference = "a",
                ArrivalDate = Before(_existingBooking.DepartureDate),
                DepartureDate = After(_existingBooking.DepartureDate),
            });

            Assert.That(result, Is.EqualTo("b"));
        }
        
        [Test]
        public void BeforeArrivalDateAndCollapseExistingBookingAndAfterDepartureDate_ReturnsReferenceProperty()
        {
            _repository.Setup(r => r.GetActiveBookings(1)).Returns(new List<Booking> { _existingBooking }.AsQueryable());

            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 1,
                Status = "Requested",
                Reference = "a",
                ArrivalDate = Before(_existingBooking.ArrivalDate),
                DepartureDate = After(_existingBooking.DepartureDate),
            });

            Assert.That(result, Is.EqualTo("b"));
        }
        
        private static DateTime After(DateTime date, int days = 1)
        {
            return date.AddDays(days);
        }

        private static DateTime Before(DateTime date, int days = 1)
        {
            return date.AddDays(-days);
        }
    }
}