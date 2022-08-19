using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class EmployeeControllerTests
    {
        [Test]
        public void DeleteEmployee_EmployeeDeleted_RedirectedToEmployeeList()
        {
            var unitOfWorkMock = new Mock<IEmployeeUnitOfWork>();
            
            unitOfWorkMock.Setup(r => r.EmployeeRepository.Find(It.IsAny<int>())).Returns(new Employee());
            
            var employeeController = new EmployeeController(unitOfWorkMock.Object);

            var result = employeeController.DeleteEmployee(1);
            
            Assert.That(result, Is.TypeOf<RedirectResult>());
        }
        
        [Test]
        public void DeleteEmployee_EmployeeNotFound_ReturnsNotFoundResult()
        {
            var unitOfWorkMock = new Mock<IEmployeeUnitOfWork>();
            
            unitOfWorkMock.Setup(r => r.EmployeeRepository.Find(It.IsAny<int>()));
            
            var employeeController = new EmployeeController(unitOfWorkMock.Object);

            var result = employeeController.DeleteEmployee(1);
            
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
        
        [Test]
        public void DeleteEmployee_WhenCalled_DeleteMethodIsCalled()
        {
            var unitOfWorkMock = new Mock<IEmployeeUnitOfWork>();

            var employee = new Employee();
            unitOfWorkMock.Setup(r => r.EmployeeRepository.Find(It.IsAny<int>())).Returns(employee);
            
            var employeeController = new EmployeeController(unitOfWorkMock.Object);

            employeeController.DeleteEmployee(1);
            
            unitOfWorkMock.Verify(p => p.EmployeeRepository.Remove(employee));
        }
    }
}