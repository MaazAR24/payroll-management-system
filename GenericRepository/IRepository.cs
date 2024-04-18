namespace GenericRepository
{
    public interface IRepository<T>
        where T : class
    {
        Task<List<T>> ListAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task<List<T>> AddRangeAsync(List<T> entities);
        Task PersistChanges();
    }
}
