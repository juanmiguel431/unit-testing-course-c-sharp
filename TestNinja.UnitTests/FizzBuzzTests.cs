using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class FizzBuzzTests
    {
        [Test]
        [TestCase(3, "Fizz")]
        [TestCase(5, "Buzz")]
        [TestCase(15, "FizzBuzz")]
        [TestCase(11, "11")]
        public void GetOutput_WhenCalled_ReturnExpected(int number, string expected)
        {
            var result = FizzBuzz.GetOutput(number);
            
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}