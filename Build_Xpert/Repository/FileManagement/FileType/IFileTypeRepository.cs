using Build_Xpert.Model;

namespace Build_Xpert.Repository
{
    public interface IFileTypeRepository
    {
        Task<bool> CreateFileTypeAsync(FileType fileType);
        Task<bool> DeleteFileTypeAsync(FileType fileType);
        Task<bool> ExistsAsync(FileType fileType);
        Task<IEnumerable<string>> GetAllowedTypesForProfilePicture();
        Task<IEnumerable<string>> GetAllowedTypesForProject();
        Task<FileType> GetFileTypeByExtensionAsync(string extension);
        Task<FileType> GetFileTypeByIdAsync(string id);
        Task<IEnumerable<FileType>> GetFileTypesAsync();
        Task<bool> UpdateFileTypeAsync(FileType fileType);
    }
}