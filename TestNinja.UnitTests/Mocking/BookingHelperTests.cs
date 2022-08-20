using System;
using System.Collections.Generic;
using System.Linq;
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
        public void OverlappingBookingsExistV2_StatusIsCanceled_ReturnsEmptyString()
        {
            var result = BookingHelper.OverlappingBookingsExistV2(new Booking { Status = "Cancelled" });

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void OverlappingBookingsExistV2_NoOverlappingRecordFound_ReturnsEmptyString()
        {
            var result = BookingHelper.OverlappingBookingsExistV2(new Booking());

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void OverlappingBookingsExistV2_WhenOverlappingRecordFound_ReturnsReferenceProperty()
        {
            _bookingRepositoryMock.Setup(r => r.GetFirstOrDefaultOverlappingBooking(It.IsAny<Booking>())).Returns(new Booking() { Reference = "a" });

            var result = BookingHelper.OverlappingBookingsExistV2(new Booking());

            Assert.That(result, Is.EqualTo("a"));
        }

        [Test]
        public void OverlappingBookingsExist_StatusIsCanceled_ReturnsEmptyString()
        {
            var result = BookingHelper.OverlappingBookingsExist(new Booking { Status = "Cancelled" });

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void OverlappingBookingsExist_WhenNoOverlapping_ReturnsEmptyString()
        {
            _bookingRepositoryMock.Setup(r => r.GetActiveBookings(It.IsAny<int>()))
                .Returns(new List<Booking> { }.AsQueryable());
            var result = BookingHelper.OverlappingBookingsExist(new Booking());

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void OverlappingBookingsExist_WhenOverlappingCase1_ReturnsReferenceProperty()
        {
            var booking = new Booking
            {
                Id = 1,
                Status = "Requested",
                Reference = "a",
                ArrivalDate = new DateTime(2022, 01, 31),
                DepartureDate = new DateTime(2022, 02, 02),
            };
            
            _bookingRepositoryMock.Setup(r => r.GetActiveBookings(booking.Id))
                .Returns(new List<Booking>
                {
                    new Booking
                    {
                        Id = 2,
                        Status = "Reserved",
                        Reference = "b",
                        ArrivalDate = new DateTime(2022, 02, 01),
                        DepartureDate = new DateTime(2022, 02 , 05)
                    }
                }.AsQueryable());


            var result = BookingHelper.OverlappingBookingsExist(booking);

            Assert.That(result, Is.EqualTo("b"));
        }
        
        [Test]
        public void OverlappingBookingsExist_WhenOverlappingCase2_ReturnsReferenceProperty()
        {
            var booking = new Booking
            {
                Id = 1,
                Status = "Requested",
                Reference = "a",
                ArrivalDate = new DateTime(2022, 02, 02),
                DepartureDate = new DateTime(2022, 02, 02),
            };
            
            _bookingRepositoryMock.Setup(r => r.GetActiveBookings(booking.Id))
                .Returns(new List<Booking>
                {
                    new Booking
                    {
                        Id = 2,
                        Status = "Reserved",
                        Reference = "b",
                        ArrivalDate = new DateTime(2022, 02, 01),
                        DepartureDate = new DateTime(2022, 02 , 05)
                    }
                }.AsQueryable());


            var result = BookingHelper.OverlappingBookingsExist(booking);

            Assert.That(result, Is.EqualTo("b"));
        }
        
        [Test]
        public void OverlappingBookingsExist_WhenOverlappingCase3_ReturnsReferenceProperty()
        {
            var booking = new Booking
            {
                Id = 1,
                Status = "Requested",
                Reference = "a",
                ArrivalDate = new DateTime(2022, 02, 02),
                DepartureDate = new DateTime(2022, 02, 15),
            };
            
            _bookingRepositoryMock.Setup(r => r.GetActiveBookings(booking.Id))
                .Returns(new List<Booking>
                {
                    new Booking
                    {
                        Id = 2,
                        Status = "Reserved",
                        Reference = "b",
                        ArrivalDate = new DateTime(2022, 02, 01),
                        DepartureDate = new DateTime(2022, 02 , 05)
                    }
                }.AsQueryable());


            var result = BookingHelper.OverlappingBookingsExist(booking);

            Assert.That(result, Is.EqualTo("b"));
        }
        
        [Test]
        public void OverlappingBookingsExist_WhenOverlappingCase4_ReturnsReferenceProperty()
        {
            var booking = new Booking
            {
                Id = 1,
                Status = "Requested",
                Reference = "a",
                ArrivalDate = new DateTime(2022, 01, 31),
                DepartureDate = new DateTime(2022, 02, 15),
            };
            
            _bookingRepositoryMock.Setup(r => r.GetActiveBookings(booking.Id))
                .Returns(new List<Booking>
                {
                    new Booking
                    {
                        Id = 2,
                        Status = "Reserved",
                        Reference = "b",
                        ArrivalDate = new DateTime(2022, 02, 01),
                        DepartureDate = new DateTime(2022, 02 , 05)
                    }
                }.AsQueryable());


            var result = BookingHelper.OverlappingBookingsExist(booking);

            Assert.That(result, Is.EqualTo("b"));
        }
    }
}