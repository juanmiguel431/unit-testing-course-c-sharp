using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class MathTests
    {
        [Test]
        public void Add_WhenCalled_ReturnsTheSumOfArguments()
        {
            var math = new Math();

            var result = math.Add(3, 4);
            
            Assert.That(result, Is.EqualTo(7));
        }

        [Test]
        public void Max_WhenFirstArgumentIsGreater_ReturnsTheFirstArgument()
        {
            var math = new Math();

            var result = math.Max(2, 1);
            
            Assert.That(result, Is.EqualTo(2));
        }
        
        [Test]
        public void Max_WhenSecondArgumentIsGreater_ReturnsTheSecondArgument()
        {
            var math = new Math();

            var result = math.Max(1, 2);
            
            Assert.That(result, Is.EqualTo(2));
        }
        
        [Test]
        public void Max_ArgumentsAreEqual_ReturnsTheSecondArgument()
        {
            var math = new Math();

            var result = math.Max(1, 1);
            
            Assert.That(result, Is.EqualTo(1));
        }
    }
}