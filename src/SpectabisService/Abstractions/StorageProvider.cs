using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using SpectabisService.Abstractions.Interfaces;

namespace SpectabisService.Abstractions
{
    public class StorageProvider : IStorageProvider
    {
        private const string ContainerName = "service-cache";

        private readonly CloudStorageAccount storageAccount;
        private CloudBlobClient client;
        private CloudBlobContainer container;

        public StorageProvider(IConfigurationRoot config)
        {
            var connectionString = config.GetValue<string>("AzureWebJobsStorage");
            storageAccount = CloudStorageAccount.Parse(connectionString);
        }

        public async Task<DateTimeOffset?> GetLastModified(string fileName)
        {
            await InitializeStorage().ConfigureAwait(false);

            var blob = container.GetBlockBlobReference(fileName);
            var exists = await blob.ExistsAsync().ConfigureAwait(false);

            if(!exists)
            {
                return null;
            }

            await blob.FetchAttributesAsync().ConfigureAwait(false);

            return blob.Properties.LastModified;
        }

        public async Task<byte[]> ReadBytesFromStorage(string fileName)
        {
            await InitializeStorage().ConfigureAwait(false);

            var blob = container.GetBlockBlobReference(fileName);
            var exists = await blob.ExistsAsync().ConfigureAwait(false);

            if(!exists)
            {
                return null;
            }

            await blob.FetchAttributesAsync().ConfigureAwait(false);

            byte[] imageBuffer = new byte[blob.Properties.Length];
            await blob.DownloadToByteArrayAsync(imageBuffer, 0).ConfigureAwait(false);

            return imageBuffer;
        }

        public async Task WriteImageToStorage(string fileName, byte[] buffer)
        {
            await InitializeStorage().ConfigureAwait(false);

            var blob = container.GetBlockBlobReference(fileName);
            blob.Properties.ContentType = "image/png";
            await blob.UploadFromByteArrayAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
        }

        public async Task WriteDataToStorage(string fileName, byte[] buffer)
        {
            await InitializeStorage().ConfigureAwait(false);

            var blob = container.GetBlockBlobReference(fileName);
            blob.Properties.ContentType = "application/octet-stream";
            await blob.UploadFromByteArrayAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
        }

        public async Task InitializeStorage()
        {
            if(client != null)
            {
                return;
            }

            client = storageAccount.CreateCloudBlobClient();
            container = client.GetContainerReference(ContainerName);
            await container.CreateIfNotExistsAsync().ConfigureAwait(false);
        }
    }
}