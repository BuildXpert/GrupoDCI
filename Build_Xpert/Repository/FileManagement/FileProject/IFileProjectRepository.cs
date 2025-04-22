using Build_Xpert.Model;

namespace Build_Xpert.Repository
{
    public interface IFileProjectRepository
    {
        Task<bool> CreateFileProjectAsync(FileProject fileProject);
        Task<bool> DeleteFileProjectAsync(FileProject fileProject);
        Task<bool> ExistsAsync(FileProject fileProject);
        Task<FileProject> GetFileProjectByFileIdAsync(string id);
        Task<IEnumerable<FileProject>> GetFilesByProjectIdAsync(string projectId);
        Task<IEnumerable<FileProject>> GetFileProjectAsync();
        Task<bool> UpdateFileProjectAsync(FileProject fileProject);
    }
}