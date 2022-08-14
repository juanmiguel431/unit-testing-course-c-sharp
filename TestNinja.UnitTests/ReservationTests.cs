using System;
using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class ReservationTests
    {
        [Test]
        public void CanBeCanceledBy_UserIsAdmin_ReturnsTrue()
        {
            //AAA stands for Arrange, Act, Assert

            //Arrange
            var reservation = new Reservation();

            //Act
            var result = reservation.CanBeCancelledBy(new User() { IsAdmin = true });

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void CanBeCanceledBy_Creator_ReturnsTrue()
        {
            var user = new User();
            var reservation = new Reservation { MadeBy = user };
            
            var result = reservation.CanBeCancelledBy(user);
            
            Assert.IsTrue(result);
        }

        [Test]
        public void CanBeCanceledBy_NotAdminNeitherCreator_ReturnsFalse()
        {
            var reservation = new Reservation() { MadeBy = new User() };
            
            var result = reservation.CanBeCancelledBy(new User());
            
            Assert.IsFalse(result);
        }
    }
}