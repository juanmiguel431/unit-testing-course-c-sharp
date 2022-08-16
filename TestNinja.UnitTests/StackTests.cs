using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class StackTests
    {
        [Test]
        public void Push_AddingOneElement_CountPropertyIsOne()
        {
            var stack = new Stack<object>();
            
            stack.Push(new object());
            
            Assert.That(stack.Count, Is.EqualTo(1));
        }
        
        [Test]
        public void Count_EmptyStack_ReturnsZero()
        {
            var stack = new Stack<object>();

            Assert.That(stack.Count, Is.EqualTo(0));
        }

        [Test]
        public void Push_ParamIsNull_ThrowsArgumentNullException()
        {
            var stack = new Stack<object>();
            
            Assert.That(() => stack.Push(null), Throws.ArgumentNullException);
        }

        [Test]
        public void Pop_TheStackIsEmpty_ThrowsInvalidOperationException()
        {
            var stack = new Stack<object>();
            
            Assert.That(() => stack.Pop(), Throws.InvalidOperationException);
        }

        [Test]
        public void Pop_WhenCalledWithOneElement_CountPropertyIsZero()
        {
            var stack = new Stack<object>();
            stack.Push(new object());

            stack.Pop();
            
            Assert.That(stack.Count, Is.EqualTo(0));
        }

        [Test]
        public void Pop_WhenCalled_ReturnsTheElementDeleted()
        {
            var stack = new Stack<string>();
            stack.Push("a");
            var element = "b";
            stack.Push(element);

            var result = stack.Pop();
            
            Assert.That(result, Is.EqualTo(element));
        }

        [Test]
        public void Pick_WhenStackIsEmpty_ThrowsInvalidOperationException()
        {
            var stack = new Stack<object>();
            
            Assert.That(() => stack.Peek(), Throws.InvalidOperationException);
        }

        [Test]
        public void Pick_StackWithTwoElement_ReturnsLastPushedElement()
        {
            var stack = new Stack<object>();
            stack.Push(new object());
            var last = new object();
            stack.Push(last);

            var result = stack.Peek();
            
            Assert.That(result, Is.EqualTo(last));
        }

        [Test]
        public void Pick_WhenCalled_DoesNotRemoveObjectsOnTheStack()
        {
            var stack = new Stack<object>();
            stack.Push(new object());
            stack.Push(new object());
            stack.Push(new object());

            stack.Peek();
            
            Assert.That(stack.Count, Is.EqualTo(3));
        }
    }
}