namespace TestNinja.Mocking
{
    public interface IEmployeeUnitOfWork
    {
        IEmployeeRepository EmployeeRepository { get; }
        void SaveChanges();
    }

    public class EmployeeUnitOfWork : IEmployeeUnitOfWork
    {
        private readonly EmployeeContext _context;
        private IEmployeeRepository _employeeRepository;

        public IEmployeeRepository EmployeeRepository => _employeeRepository ?? (_employeeRepository = new EmployeeRepository(_context));

        public EmployeeUnitOfWork(EmployeeContext context)
        {
            _context = context;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}