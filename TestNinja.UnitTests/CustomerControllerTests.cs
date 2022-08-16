using System;
using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class CustomerControllerTests
    {
        private CustomerController _customerController;
        
        [SetUp]
        public void SetUp()
        {
            _customerController = new CustomerController();
        }

        [Test]
        [TestCase(0, typeof(NotFound))]
        [TestCase(1, typeof(Ok))]
        public void GetCustomer_WhenCalled_ReturnsType(int a, Type type)
        {
            var result = _customerController.GetCustomer(a);
            
            // NotFound
            Assert.That(result, Is.TypeOf(type));
            
            // NotFound or one of its derivatives
            // Assert.That(result, Is.InstanceOf(type));
        }
    }
}