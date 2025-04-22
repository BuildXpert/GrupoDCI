using Build_Xpert.Model;

namespace Build_Xpert.Repository
{
    public interface IFileUserRepository
    {
        Task<bool> CreateFileUserAsync(FileUser fileUser);
        Task<bool> DeleteFileUserAsync(FileUser fileUser);
        Task<bool> ExistsAsync(FileUser fileUser);
        Task<FileUser> GetFileUserByIdAsync(string id);
        Task<FileUser> GetFileUserByUserIdAsync(string userId);
        Task<IEnumerable<FileUser>> GetFileUsersAsync();
        Task<bool> UpdateFileUserAsync(FileUser fileUser);
        Task<FileUser> GetFileUserByFileIdAsync(string fileId);
    }
}