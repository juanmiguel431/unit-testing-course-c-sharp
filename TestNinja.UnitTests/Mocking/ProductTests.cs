using System;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class ProductTests
    {
        [Test]
        [TestCase(0, 0)]
        [TestCase(10, 7)]
        public void GetPrice_WhenCustomerIsGold_ReturnsSeventyPercentOfTheListPriceProperty(int listPrice, int expected)
        {
            var product = new Product
            {
                ListPrice = listPrice
            };

            var result = product.GetPrice(new Customer { IsGold = true });

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(0)]
        [TestCase(100)]
        public void GetPrice_WhenCustomerIsNotGold_ReturnsTheListPriceProperty(int listPrice)
        {
            var product = new Product
            {
                ListPrice = listPrice
            };

            var result = product.GetPrice(new Customer());

            Assert.That(result, Is.EqualTo(listPrice));
        }

        [Test]
        public void GetPrice_WhenCustomerIsNull_ThrowsNullReferenceException()
        {
            var product = new Product();

            Assert.That(() => product.GetPrice(null), Throws.TypeOf<NullReferenceException>());
        }
    }
}