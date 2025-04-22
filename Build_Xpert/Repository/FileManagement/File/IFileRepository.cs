using Build_Xpert.Model;

namespace Build_Xpert.Repository
{
    public interface IFileRepository
    {
        Task<bool> CreateMediaFileAsync(MediaFile file);
        Task<bool> DeleteMediaFileAsync(MediaFile file);
        Task<bool> ExistsAsync(MediaFile file);
        Task<MediaFile> GetMediaFileByIdAsync(string id);
        Task<IEnumerable<MediaFile>> GetMediaFilesAsync();
        Task<bool> UpdateMediaFileAsync(MediaFile file);
    }
}