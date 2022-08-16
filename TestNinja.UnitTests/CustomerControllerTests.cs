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
        public void GetCustomer_WithParamZero_ReturnNotFoundType()
        {
            var result = _customerController.GetCustomer(0);
            
            Assert.That(result, Is.TypeOf<NotFound>());
        }
        
        [Test]
        public void GetCustomer_WithParamOne_ReturnsOkType()
        {
            var result = _customerController.GetCustomer(1);
            
            Assert.That(result, Is.TypeOf<Ok>());
        }
    }
}