using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class MathTests
    {
        private Math _math;

        [SetUp]
        public void SetUp()
        {
            _math = new Math();
        }
        
        [Test]
        public void Add_WhenCalled_ReturnsTheSumOfArguments()
        {
            var result = _math.Add(3, 4);
            
            Assert.That(result, Is.EqualTo(7));
        }

        [Test]
        [TestCase(2, 1, 2)]
        [TestCase(1, 2, 2)]
        [TestCase(2, 2, 2)]
        public void Add_WhenCalled_ReturnsTheGreatestArgument(int a, int b, int expectedResult)
        {
            var result = _math.Max(a, b);
            
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}