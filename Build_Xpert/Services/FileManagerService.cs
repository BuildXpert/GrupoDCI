using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using System.Text;
using System.Reflection.Metadata;
using Azure.Storage.Blobs.Models;
using Build_Xpert.Model;
using Build_Xpert.Business;
using Microsoft.Extensions.Caching.Memory;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.CodeAnalysis.Differencing;

namespace Build_Xpert.Services
{
    public class FileManagerService : IFileManagerService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IFileManagerCore _fileManagerCore;
        private readonly IMemoryCache _memoryCache;
        private readonly ProjectService _projectService;

        public FileManagerService(BlobServiceClient blobServiceClient, IFileManagerCore fileManagerCore, IMemoryCache memoryCache, ProjectService projectService)
        {
            _blobServiceClient = blobServiceClient;
            _fileManagerCore = fileManagerCore;
            _memoryCache = memoryCache;
            _projectService = projectService;
        }

        private string GenerateSasUrl(string containerName, string blobName, double validMinutes)
        {
            // Set up the BlobServiceClient
            string azureBlobConnectionString = "DefaultEndpointsProtocol=https;AccountName=builxpert;AccountKey=HEixehIQPIGGC9TbPoraonl5gnt6NaWNSV0T7zdINabRw6cXWVEWaAmiWtWkPlPrCzO5HQX+MoOi+ASteTZ3CA==;EndpointSuffix=core.windows.net";
            var blobServiceClient = new BlobServiceClient(azureBlobConnectionString);

            // Get the container and blob references
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            // Generate the SAS token for the blob
            var blobSasBuilder = new BlobSasBuilder
            {
                BlobContainerName = containerName,
                BlobName = blobName,
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(validMinutes)
            };

            // Set the permissions (read access)
            blobSasBuilder.SetPermissions(BlobSasPermissions.Read);

            // Generate the SAS token
            var sasToken = blobClient.GenerateSasUri(blobSasBuilder).Query;

            // Combine the SAS token with the base URI to get the full URL
            string sasUrl = $"{blobClient.Uri}{sasToken}";

            return sasUrl;
        }
        public async Task<string> GetProfilePictureUrlAsync(string userId)
        {
            var url = _memoryCache.Get<string>("userId");
            if (url != null)
                return url;
            var mediaFile = await _fileManagerCore.GetProfilePictureAsync(userId);
            if (mediaFile == null)
                return "null";
            url = GenerateSasUrl(mediaFile.ContainerName, mediaFile.Path, 50);
            _memoryCache.Set("userId", url, TimeSpan.FromMinutes(1));
            return url;
        }
        public async Task<IEnumerable<MediaFile>> GetProjectFileUrlsAsync(Project project, string requestingUserId, int validHours)
        {
            //Only the owner of the project and the users assigned to the project can access the files
            var validUserId = string.IsNullOrEmpty(requestingUserId) ? false : true;
            //var project = await _projectService.GetProjectByIdAsync(int.Parse(project));
            if (project == null)
                throw new InvalidOperationException("Project not found.");
            if (validUserId)
                if (project.ClientId != requestingUserId && project.AdminId != requestingUserId)
                    throw new InvalidOperationException("You do not have access to the files in this project.");
            var filesPerProject = await _fileManagerCore.GetProjectFilesAsync(project.Id.ToString());
            if (filesPerProject == null)
                throw new InvalidOperationException("No files found for this project.");
            var mediaFilesToBeQueried = new List<MediaFile>();
            var finalFileUrls = new List<MediaFile>();
            await FindFileUrlsInCache(filesPerProject, mediaFilesToBeQueried, finalFileUrls);
            await GenerateUrlsForFieldsToBeQueried(mediaFilesToBeQueried, finalFileUrls, TimeSpan.FromHours(validHours));
            await FillFileTypes(finalFileUrls);
            return finalFileUrls;
        }
        private async Task FillFileTypes(List<MediaFile> mediaFiles)
        {
            foreach (var file in mediaFiles)
            {
                if (file != null)
                {
                    var fileType = await _fileManagerCore.GetFileTypeAsync(file.TypeId);
                    if (fileType != null)
                        file.FileType = fileType;
                }
            }
        }
        private async Task GenerateUrlsForFieldsToBeQueried(List<MediaFile> mediaFilesToBeQueried, List<MediaFile> finalFileUrls, TimeSpan validHours)
        {
            foreach (var file in mediaFilesToBeQueried)
            {
                if (file != null)
                {
                    await GetMediaUrlAsync(file, validHours);
                    finalFileUrls.Add(file);
                }
            }
        }
        public async Task FindFileUrlsInCache(IEnumerable<FileProject> filesPerProject, List<MediaFile> mediaFiles, List<MediaFile> finalFileUrls)
        {
            foreach (var file in filesPerProject)
            {
                var fileUrl = _memoryCache.Get<MediaFile>(file.FileId);
                if (fileUrl == null)
                {
                    mediaFiles.Add(!string.IsNullOrEmpty(file.FileId) ? await _fileManagerCore.GetMediaFileByIdAsync(file.FileId) : null);
                }
                finalFileUrls.Add(fileUrl);
            }
        }
        public async Task<string> UploadFileForUserAsync(IFormFile file, string userId)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            var allowedTypes = await _fileManagerCore.GetAllowedTypesForProjectAsync();
            if (!allowedTypes.Contains(file.ContentType))
                throw new InvalidOperationException("File type not allowed.");
            var sanitizedFileName = SanitizeFileName(file);
            var containerName = FileManagerContainers.PROFILE_PICTURES;
            var path = $"users/{userId}/";

            var fullPath = path + sanitizedFileName;
            MediaFile mediaFile = new MediaFile();
            try
            {
                mediaFile.ContainerName = containerName;
                mediaFile.FileName = Path.GetFileNameWithoutExtension(file.FileName);
                mediaFile.Path = fullPath;
                mediaFile.CreatedAt = DateTime.UtcNow;
                mediaFile.TypeId = (await _fileManagerCore.FindFileExtension(Path.GetExtension(file.FileName))).Id;
                mediaFile.IsPrivate = false;
            }
            catch
            {
                throw new InvalidOperationException("File type not allowed.");
            }
            var fileUrl = await UploadFileAsync(file, containerName, path, sanitizedFileName);
            await _fileManagerCore.AddProfilePictureAsync(mediaFile, userId);
            return fileUrl;
        }
        public async Task<string> UploadFileForProjectAsync(IFormFile file, string projectId, bool? isPrivate = true)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");
            var allowedTypes = await _fileManagerCore.GetAllowedTypesForProjectAsync();
            if (!allowedTypes.Contains(file.ContentType))
                throw new InvalidOperationException("File type not allowed.");
            var sanitizedFileName = SanitizeFileName(file);
            var containerName = FileManagerContainers.PROJECT_FILES;
            var path = $"projects/{projectId}/";
            var fullPath = path + sanitizedFileName;
            MediaFile mediaFile = new MediaFile();
            try
            {
                mediaFile.ContainerName = containerName;
                mediaFile.FileName = Path.GetFileNameWithoutExtension(file.FileName);
                mediaFile.Path = fullPath;
                mediaFile.CreatedAt = DateTime.UtcNow;
                mediaFile.TypeId = (await _fileManagerCore.FindFileExtension(Path.GetExtension(file.FileName))).Id;
                mediaFile.IsPrivate = isPrivate ?? true || false;
            }
            catch
            {
                throw new InvalidOperationException("File type not allowed.");
            }
            var fileUrl = await UploadFileAsync(file, containerName, path, sanitizedFileName);
            await _fileManagerCore.AddProjectFileAsync(mediaFile, projectId);
            return fileUrl;
        }
        private async Task<string> UploadFileAsync(IFormFile file, string containerName, string path, string sanitizedFileName)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                await containerClient.CreateIfNotExistsAsync();

                var blobClient = containerClient.GetBlobClient(path + sanitizedFileName);

                var blobHttpHeaders = new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                };

                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, new BlobUploadOptions
                    {
                        HttpHeaders = blobHttpHeaders
                    });
                }

                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("File upload failed.");
            }
        }
        private string SanitizeFileName(IFormFile fileName)
        {
            var originalFileName = Path.GetFileNameWithoutExtension(fileName.FileName);
            var extension = Path.GetExtension(fileName.FileName);
            var sanitizedFileName = string.Concat(originalFileName.Where(c => !Path.GetInvalidFileNameChars().Contains(c)));
            return $"{sanitizedFileName}_{Guid.NewGuid()}{extension}";
        }
        public async Task GetMediaUrlAsync(MediaFile file, TimeSpan validHours)
        {
            file.PublicUrl = GenerateSasUrl(file.ContainerName, file.Path, validHours.TotalMinutes);
            _memoryCache.Set(file.MediaId, file, validHours);
        }

        public async Task DeleteFileAsync(string fileId,string userId)
        {
            var file = await _fileManagerCore.GetMediaFileByIdAsync(fileId);
            if (file == null)
                return;
            var containerClient = _blobServiceClient.GetBlobContainerClient(file.ContainerName);
            var blobClient = containerClient.GetBlobClient(file.Path);
            await blobClient.DeleteIfExistsAsync();
            await _fileManagerCore.DeleteMediaFileAsync(fileId);
        }
    }

    public static class FileManagerContainers
    {
        public const string PROFILE_PICTURES = "profile-pictures";
        public const string PROJECT_FILES = "project-files";
    }

    public static class BrowserFileExtensions
    {
        public static async Task<IFormFile> ToFormFileAsync(this IBrowserFile browserFile)
        {
            var stream = browserFile.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024); // 10MB limit
            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            return new FormFile(memoryStream, 0, memoryStream.Length, "file", browserFile.Name)
            {
                Headers = new HeaderDictionary(),
                ContentType = browserFile.ContentType
            };
        }
    }

}
