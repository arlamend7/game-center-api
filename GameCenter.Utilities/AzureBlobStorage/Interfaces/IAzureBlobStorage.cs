using System;
using System.IO;
using System.Threading.Tasks;

namespace GameCenter.Utilities.AzureBlobStorage.Interfaces
{
    public interface IAzureBlobStorage
    {
        Task DeleteFileAsync(string containerName, string path, string fileName);
        Task<Stream> DownloadFileAsync(string containerName, string path, string fileName);
        Task<Uri> UploadFileAsync(string containerName, string path, string fileName, Stream file);
    }
}