using Build_Xpert.Model;

namespace Build_Xpert.Business
{
    public interface IFileManagerCore
    {
        Task<bool> AddProfilePictureAsync(MediaFile file, string userId);
        Task<bool> AddProjectFileAsync(MediaFile file, string projectId);
        Task<FileType> FindFileExtension(string extension);
        Task<IEnumerable<string>> GetAllowedTypesForProjectAsync();
        Task<MediaFile> GetMediaFileByIdAsync(string fileId);
        Task<MediaFile> GetProfilePictureAsync(string userId);
        Task<IEnumerable<FileProject>> GetProjectFilesAsync(string projectId);
        Task<FileType> GetFileTypeAsync(string id);
        Task DeleteMediaFileAsync(string fileId);
    }
}