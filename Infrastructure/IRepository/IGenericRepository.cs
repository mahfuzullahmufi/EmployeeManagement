namespace Infrastructure.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IList<T>> GetAll( );
        Task<T> GetById(int id);
        Task Insert(T entity);
        
        Task Delete(int id);
        void Update(T entity);
        
    }
}
