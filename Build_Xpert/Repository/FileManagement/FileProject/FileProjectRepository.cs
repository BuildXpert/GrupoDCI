using Build_Xpert.Model;
using Build_Xpert.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace Build_Xpert.Repository
{
    public class FileProjectRepository : RepositoryBase<FileProject, string>, IFileProjectRepository
    {
        public FileProjectRepository(ApplicationDbContext _context) : base(_context)
        {
        }

        #region Methods
        public async Task<IEnumerable<FileProject>> GetFileProjectAsync()
        {
            return await ReadAsync();
        }
        public async Task<FileProject> GetFileProjectByFileIdAsync(string fileId)
        {
            var queriable = ReadQueriableAsync();
            return await queriable.FirstOrDefaultAsync(x => x.FileId == fileId);
        }
        public async Task<bool> CreateFileProjectAsync(FileProject fileProject)
        {
            return await CreateAsync(fileProject);
        }
        public async Task<bool> UpdateFileProjectAsync(FileProject fileProject)
        {
            return await UpdateAsync(fileProject);
        }
        public async Task<bool> DeleteFileProjectAsync(FileProject fileProject)
        {
            return await DeleteAsync(fileProject);
        }
        public async Task<bool> ExistsAsync(FileProject fileProject)
        {
            return await BExistsAsync(fileProject);
        }
        public async Task<IEnumerable<FileProject>> GetFilesByProjectIdAsync(string projectId)
        {
            var query = await ReadAsync();
            return query.Where(x => x.ProjectId == int.Parse(projectId)).ToList();
        }
        #endregion
    }
}
