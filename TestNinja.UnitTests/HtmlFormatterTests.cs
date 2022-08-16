using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class HtmlFormatterTests
    {
        private HtmlFormatter _htmlFormatter;
        
        [SetUp]
        public void SetUp()
        {
            _htmlFormatter = new HtmlFormatter();
        }
        
        [Test]
        public void FormatAsBold_WhenCalled_ShouldEncloseStringWithsStrongTagElement()
        {
            //AAA
            //Arrange
            var content = "Hello";

            //Act
            var result = _htmlFormatter.FormatAsBold(content);

            //Assert
            //Specific assertion
            Assert.That(result, Is.EqualTo($"<strong>Hello</strong>"));
            
            //More general assertion
            Assert.That(result, Does.StartWith("<strong>").IgnoreCase);
            Assert.That(result, Does.EndWith("</strong>").IgnoreCase);
            Assert.That(result, Does.Contain(content));
        }
    }
}