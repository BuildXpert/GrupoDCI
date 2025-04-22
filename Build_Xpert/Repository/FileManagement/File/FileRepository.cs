using Build_Xpert.Model;
using Build_Xpert.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;

namespace Build_Xpert.Repository
{
    public class FileRepository : RepositoryBase<MediaFile, string>, IFileRepository
    {
        #region Dependency Injection
        public FileRepository(ApplicationDbContext _context) : base(_context)
        {
        }
        #endregion
        #region Methods
        public async Task<IEnumerable<MediaFile>> GetMediaFilesAsync()
        {
            return await ReadAsync();
        }
        public async Task<MediaFile> GetMediaFileByIdAsync(string id)
        {
            return await ReadOneAsync(id);
        }
        public async Task<bool> CreateMediaFileAsync(MediaFile file)
        {
            return await CreateAsync(file);
        }
        public async Task<bool> UpdateMediaFileAsync(MediaFile file)
        {
            return await UpdateAsync(file);
        }
        public async Task<bool> DeleteMediaFileAsync(MediaFile file)
        {
            return await DeleteAsync(file);
        }
        public async Task<bool> ExistsAsync(MediaFile file)
        {
            return await BExistsAsync(file);
        }
        #endregion
    }
}
