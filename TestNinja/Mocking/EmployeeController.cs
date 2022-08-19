using System.Data.Entity;

namespace TestNinja.Mocking
{
    public class EmployeeController
    {
        private readonly IEmployeeUnitOfWork _unitOfWork;

        public EmployeeController(IEmployeeUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult DeleteEmployee(int id)
        {
            var employee = _unitOfWork.EmployeeRepository.Find(id);
            _unitOfWork.EmployeeRepository.Remove(employee);
            _unitOfWork.SaveChanges();
            return RedirectToAction("Employees");
        }

        private ActionResult RedirectToAction(string employees)
        {
            return new RedirectResult();
        }
    }

    public class ActionResult { }
 
    public class RedirectResult : ActionResult { }
    
    public class EmployeeContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
    }

    public class Employee
    {
    }
}