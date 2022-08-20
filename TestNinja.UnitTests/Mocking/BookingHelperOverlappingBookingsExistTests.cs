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
            var result = BookingHelper.OverlappingBookingsExist(new Booking { Status = "Cancelled" });

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void WhenNoOverlapping_ReturnsEmptyString()
        {
            _bookingRepositoryMock.Setup(r => r.GetActiveBookings(It.IsAny<int>()))
                .Returns(new List<Booking> { }.AsQueryable());
            var result = BookingHelper.OverlappingBookingsExist(new Booking());

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void WhenOverlappingCase1_ReturnsReferenceProperty()
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
        public void WhenOverlappingCase2_ReturnsReferenceProperty()
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
        public void WhenOverlappingCase3_ReturnsReferenceProperty()
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
        public void WhenOverlappingCase4_ReturnsReferenceProperty()
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