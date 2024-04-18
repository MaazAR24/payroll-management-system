using GenericRepository;
using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Data;
using PayrollManagementSystem.Models;
using PayrollManagementSystem.Services.Files;

namespace PayrollManagementSystem.Repositories
{
    public interface IFileRepository : IRepository<FileEntity>
    {
        Task<List<FileEntity>> ListAsync(int moduleId, int recordId);
    }
    public class FileRepository : Repository<FileEntity>, IFileRepository
    {
        private readonly PayrollContext _dbContext;
        public FileRepository(PayrollContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<FileEntity>> ListAsync(int moduleId, int recordId)
        {
            return await _dbContext.Files
                .Where( f =>
                    f.ModuleId == moduleId 
                    && f.RecordId == recordId
                )
                .ToListAsync();
        }
    }
}
