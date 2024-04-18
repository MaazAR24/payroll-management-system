using PayrollManagementSystem.Services.Files;

namespace PayrollManagementSystem.Services.FileUploader
{
    public interface IFileService
    {
        Task<FileModel> Get(int id);
        Task<List<FileModel>> Get();
        Task<List<FileModel>> Get(int moduleId, int recordId);
        Task<List<Models.FileEntity>> Upload(int moduleId, int recordId, List<IFormFile> files);
    }
}