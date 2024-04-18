using PayrollManagementSystem.Repositories;
using PayrollManagementSystem.Services.Files;

namespace PayrollManagementSystem.Services.FileUploader
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        public FileService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task<FileModel> Get(int id)
        {
            var fileObj = await _fileRepository.GetByIdAsync(id);
            if (fileObj == null) return null;

            var file = new FileModel { 
                Id = fileObj.Id,
                FileName = fileObj.FileName,
                FilePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileObj.StoredFileName),
                ContentType = GetContentType(fileObj.FileName)
            };

            return file;
        }

        public async Task<List<FileModel>> Get()
        {
            var fileObjs = await _fileRepository.ListAsync();
            if (fileObjs == null) return null;

            return fileObjs.Select(fileObj => new FileModel {
                Id = fileObj.Id,
                FileName = fileObj.FileName,
                FilePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileObj.StoredFileName),
                ContentType = GetContentType(fileObj.FileName)
            }).ToList();
        }

        public async Task<List<FileModel>> Get(int moduleId, int recordId)
        {
            var fileObjs = await _fileRepository.ListAsync(moduleId, recordId);
            if (fileObjs == null) return null;

            return fileObjs.Select(fileObj => new FileModel
            {
                Id = fileObj.Id,
                FileName = fileObj.FileName,
                FilePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileObj.StoredFileName),
                ContentType = GetContentType(fileObj.FileName)
            }).ToList();
        }

        private string GetContentType(string fileName)
        {
            string[] parts = fileName.Split('.');
            string extension = parts[parts.Length - 1].ToLower();

            switch (extension)
            {
                case "pdf": return "application/pdf";
                case "txt": return "text/plain";
                case "doc": return "application/vnd.ms-word";
                // Add more MIME types as needed
                default: return "application/octet-stream";
            }
        }

        public async Task<List<Models.FileEntity>> Upload(int moduleId, int recordId, List<IFormFile> files)
        {
            List<Models.FileEntity> savedFiles = new List<Models.FileEntity>();

            foreach (var file in files)
            {
                if (file == null || file.Length == 0)
                    continue;

                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var fileModel = new Models.FileEntity
                {
                    ModuleId = moduleId,
                    RecordId = recordId,
                    FileName = file.FileName,
                    StoredFileName = fileName,
                };

                savedFiles.Add(fileModel);
            }

            await _fileRepository.AddRangeAsync(savedFiles);

            return savedFiles;
        }
    }
}
