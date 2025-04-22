using Build_Xpert.Model;
using Build_Xpert.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace Build_Xpert.Repository
{
    public class FileTypeRepository : RepositoryBase<FileType, string>, IFileTypeRepository
    {
        public FileTypeRepository(ApplicationDbContext _context) : base(_context)
        {
        }


        #region Methods
        public async Task<IEnumerable<FileType>> GetFileTypesAsync()
        {
            return await ReadAsync();
        }
        public async Task<FileType> GetFileTypeByIdAsync(string id)
        {
            return await ReadOneAsync(id);
        }
        public async Task<bool> CreateFileTypeAsync(FileType fileType)
        {
            return await CreateAsync(fileType);
        }
        public async Task<bool> UpdateFileTypeAsync(FileType fileType)
        {
            return await UpdateAsync(fileType);
        }
        public async Task<bool> DeleteFileTypeAsync(FileType fileType)
        {
            return await DeleteAsync(fileType);
        }
        public async Task<bool> ExistsAsync(FileType fileType)
        {
            return await BExistsAsync(fileType);
        }

        public async Task<FileType> GetFileTypeByExtensionAsync(string extension)
        {
            var queriable = ReadQueriableAsync();
            var fileType = await queriable.FirstOrDefaultAsync(x => x.Extension == extension);
            if (fileType == null)
            {
                return null;
            }
            return fileType;
        }

        public async Task<IEnumerable<string>> GetAllowedTypesForProject()
        {
            var fileTypes = await ReadAsync();
            return fileTypes.Where(x => x.AllowedForProject).Select(x => x.MimeType).ToList();
        }

        public async Task<IEnumerable<string>> GetAllowedTypesForProfilePicture()
        {
            var fileTypes = await ReadAsync();
            return fileTypes.Where(x => x.AllowedForProfilePicture).Select(x => x.MimeType).ToList();
        }

        #endregion
    }
}
