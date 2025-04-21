using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using SGTC.Core.Files.BlobStorages.Interfaces;

namespace SGTC.Core.Files.BlobStorages
{
    public class AzureBlobStorage : IAzureBlobStorage
    {
        private readonly BlobServiceClient blobServiceClient;

        public AzureBlobStorage(BlobServiceClient blobServiceClient)
        {
            this.blobServiceClient = blobServiceClient;
        }

        public async Task<Uri> UploadFileAsync(string containerName, string path, string fileName, Stream file)
        {
            BlobContainerClient containerClient = await GetContainerClientAsync(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(Path.Combine(path, fileName));

            await blobClient.UploadAsync(file, true);
            return blobClient.Uri;
        }

        public async Task<Stream> DownloadFileAsync(string containerName, string path, string fileName)
        {
            BlobContainerClient containerClient = await GetContainerClientAsync(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(Path.Combine(path, fileName));

            BlobDownloadInfo download = await blobClient.DownloadAsync();
            return download.Content;
        }

        public async Task DeleteFileAsync(string containerName, string path, string fileName)
        {
            BlobContainerClient containerClient = await GetContainerClientAsync(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(Path.Combine(path, fileName));

            await blobClient.DeleteAsync();
        }

        private async Task<BlobContainerClient> GetContainerClientAsync(string containerName)
        {
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName.ToLower());

            if (!await containerClient.ExistsAsync())
            {
                await containerClient.CreateAsync();
            }

            return containerClient;
        }
    }
}