namespace PayrollManagementSystem.Models
{
    public class FileEntity
    {
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public int RecordId { get; set; }
        public string FileName { get; set; }
        public string StoredFileName { get; set; }
    }
}
