using Build_Xpert.Services;
using Build_Xpert.Model;
using Build_Xpert.Repository;

namespace Build_Xpert.Business
{
    public class FileManagerCore : IFileManagerCore
    {
        private readonly UserService _userService;
        private readonly IFileRepository _fileRepository;
        private readonly IFileTypeRepository _fileTypeRepository;
        private readonly IFileUserRepository _fileUserRepository;
        private readonly IFileProjectRepository _fileProjectRepository;
        public FileManagerCore(
            IFileRepository fileRepository,
            IFileTypeRepository fileTypeRepository,
            IFileUserRepository fileUserRepository,
            UserService userService,
            IFileProjectRepository fileProjectRepository)
        {
            _fileRepository = fileRepository;
            _fileTypeRepository = fileTypeRepository;
            _fileUserRepository = fileUserRepository;
            _userService = userService;
            _fileProjectRepository = fileProjectRepository;
        }

        #region User profile picture methods

        public async Task<bool> AddProfilePictureAsync(MediaFile file, string userId)
        {
            await _fileRepository.CreateMediaFileAsync(file);
            FileUser fileUser = new FileUser
            {
                FileId = file.MediaId,
                UserId = userId
            };
            return await _fileUserRepository.CreateFileUserAsync(fileUser);
        }
        public async Task<MediaFile> GetProfilePictureAsync(string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("Usuario no encontrado");
            }
            var fileUser = await _fileUserRepository.GetFileUserByUserIdAsync(userId);
            if (fileUser == null)
            {
                return null;
            }
            var file = await _fileRepository.GetMediaFileByIdAsync(fileUser.FileId);
            if (file == null)
            {
                return null;
            }
            return file;
        }
        public async Task<IEnumerable<string>> GetAllowedTypesForProfilePicture()
        {
            return await _fileTypeRepository.GetAllowedTypesForProfilePicture();
        }
        public async Task<MediaFile> GetMediaFileByIdAsync(string fileId)
        {
            var file = await _fileRepository.GetMediaFileByIdAsync(fileId);
            if (file == null)
            {
                throw new Exception("Archivo no encontrado");
            }
            return file;
        }
        public async Task<IEnumerable<FileProject>> GetProjectFilesAsync(string projectId)
        {
            var files = await _fileProjectRepository.GetFilesByProjectIdAsync(projectId);
            if (files == null)
            {
                throw new Exception("No se encontraron archivos para el proyecto");
            }
            return files;
        }
        #endregion
        #region Project Files
        public async Task<bool> AddProjectFileAsync(MediaFile file, string projectId)
        {
            file.FileType = await _fileTypeRepository.GetFileTypeByIdAsync(file.TypeId);
            if (file.FileType == null)
            {
                return false;
            }
            await _fileRepository.CreateMediaFileAsync(file);
            FileProject fileProject = new FileProject
            {
                FileId = file.MediaId,
                ProjectId = int.Parse(projectId)
            };
            return await _fileProjectRepository.CreateFileProjectAsync(fileProject);
        }
        public async Task<IEnumerable<string>> GetAllowedTypesForProjectAsync()
        {
            return await _fileTypeRepository.GetAllowedTypesForProject();
        }
        #endregion
        public async Task<FileType> FindFileExtension(string extension)
        {
            return await _fileTypeRepository.GetFileTypeByExtensionAsync(extension);
        }

        public async Task<FileType> GetFileTypeAsync(string id)
        {
            var fileType = await _fileTypeRepository.GetFileTypeByIdAsync(id);
            if (fileType == null)
            {
                throw new Exception("Tipo de archivo no encontrado");
            }
            return fileType;
        }

        public async Task DeleteMediaFileAsync(string fileId)
        {
            var file = await _fileRepository.GetMediaFileByIdAsync(fileId);
            if (file == null)
            {
                throw new Exception("Archivo no encontrado");
            }
            var fileUser = await _fileUserRepository.GetFileUserByFileIdAsync(fileId);
            if (fileUser != null)
            {
                await _fileUserRepository.DeleteFileUserAsync(fileUser);
            }
            var fileProject = await _fileProjectRepository.GetFileProjectByFileIdAsync(fileId);
            if (fileProject != null)
            {
                await _fileProjectRepository.DeleteFileProjectAsync(fileProject);
            }
            await _fileRepository.DeleteMediaFileAsync(file);
        }
    }
}
