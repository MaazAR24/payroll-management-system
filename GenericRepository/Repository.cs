using Microsoft.EntityFrameworkCore;

namespace GenericRepository
{
    public class Repository<T> : IRepository<T>
        where T : class
    {
        private readonly DbContext _dbContext;
        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public virtual async Task<T> CreateAsync(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var existingRecord = await _dbContext.Set<T>().FindAsync(id);
            if (existingRecord == null)
                return false;

            _dbContext.Set<T>().Remove(existingRecord);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public virtual async Task<List<T>> ListAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<List<T>> AddRangeAsync(List<T> entities)
        {
            await _dbContext.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
            return entities;
        }

        public async Task PersistChanges()
        {
            await _dbContext.SaveChangesAsync();
        }

        
    }
}