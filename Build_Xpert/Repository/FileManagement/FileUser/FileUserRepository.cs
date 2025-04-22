using Build_Xpert.Model;
using Build_Xpert.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace Build_Xpert.Repository
{
    public class FileUserRepository : RepositoryBase<FileUser, string>, IFileUserRepository
    {
        public FileUserRepository(ApplicationDbContext _context) : base(_context)
        {
        }

        #region Methods
        public async Task<IEnumerable<FileUser>> GetFileUsersAsync()
        {
            return await ReadAsync();
        }
        public async Task<FileUser> GetFileUserByIdAsync(string id)
        {
            return await ReadOneAsync(id);
        }

        public async Task<FileUser> GetFileUserByFileIdAsync(string fileId)
        {
            var queriable = ReadQueriableAsync();
            return await queriable.FirstOrDefaultAsync(x => x.FileId == fileId);
        }
        public async Task<bool> CreateFileUserAsync(FileUser fileUser)
        {
            return await CreateAsync(fileUser);
        }
        public async Task<bool> UpdateFileUserAsync(FileUser fileUser)
        {
            return await UpdateAsync(fileUser);
        }
        public async Task<bool> DeleteFileUserAsync(FileUser fileUser)
        {
            return await DeleteAsync(fileUser);
        }
        public async Task<bool> ExistsAsync(FileUser fileUser)
        {
            return await BExistsAsync(fileUser);
        }
        public async Task<FileUser> GetFileUserByUserIdAsync(string userId)
        {
            return await DbContext.Set<FileUser>().FirstOrDefaultAsync(x => x.UserId == userId);
        }
        #endregion
    }
}
