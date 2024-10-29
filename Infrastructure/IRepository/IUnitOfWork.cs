using Infrastructure.Entities;

namespace Infrastructure.IRepository
{
    public interface IUnitOfWork : IDisposable
        {
            IGenericRepository<Employee> EmployeeRepository { get; }
            Task Save();
        }
    
}
