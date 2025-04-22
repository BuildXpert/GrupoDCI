
using Build_Xpert.Model;

namespace Build_Xpert.Services
{
    public interface IFileManagerService
    {
        Task<string> GetProfilePictureUrlAsync(string userId);
        Task<IEnumerable<MediaFile>> GetProjectFileUrlsAsync(Project project, string requestingUser, int validHours);
        Task<string> UploadFileForProjectAsync(IFormFile file, string projectId, bool? isPrivate = true);
        Task<string> UploadFileForUserAsync(IFormFile file, string userId);
        Task GetMediaUrlAsync(MediaFile file, TimeSpan validHours);
        Task DeleteFileAsync(string fileId, string userId);
    }
}