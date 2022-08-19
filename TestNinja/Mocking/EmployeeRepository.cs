namespace TestNinja.Mocking
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
    }

    public class EmployeeRepository : Repository<EmployeeContext, Employee>, IEmployeeRepository
    {
        public EmployeeRepository(EmployeeContext context) : base(context)
        {
        }
        
        public override void Remove(Employee item)
        {
            Context.Employees.Remove(item);
        }
    }
}