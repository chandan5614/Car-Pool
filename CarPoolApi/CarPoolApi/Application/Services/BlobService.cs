using Azure.Storage.Blobs;
using Core.Interfaces;
using Microsoft.Extensions.Options;

namespace Core.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobContainerClient _blobContainerClient;

        public BlobService(IOptions<AzureBlobStorageOptions> options)
        {
            var connectionString = options.Value.ConnectionString;
            var containerName = options.Value.ContainerName;
            _blobContainerClient = new BlobContainerClient(connectionString, containerName);
        }

        public async Task<string> UploadDocumentAsync(Stream fileStream, string fileName)
        {
            var blobClient = _blobContainerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStream, overwrite: true);
            return blobClient.Uri.ToString();
        }

        public async Task<Stream> DownloadDocumentAsync(string fileName)
        {
            var blobClient = _blobContainerClient.GetBlobClient(fileName);
            var downloadInfo = await blobClient.DownloadAsync();
            return downloadInfo.Value.Content;
        }
    }

    public class AzureBlobStorageOptions
    {
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
    }
}

