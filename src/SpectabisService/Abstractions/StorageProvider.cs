using System;
using System.Threading.Tasks;
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

        public StorageProvider(string connectionString)
        {
            storageAccount = CloudStorageAccount.Parse(connectionString);
        }

        public async Task<byte[]> GetFromCache(string serial)
        {
            await InitializeStorage().ConfigureAwait(false);

            var blob = container.GetBlockBlobReference(serial);
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

        public async Task WriteToCache(string serial, byte[] image)
        {
            await InitializeStorage().ConfigureAwait(false);

            var blob = container.GetBlockBlobReference(serial);
            blob.Properties.ContentType = "image/png";
            await blob.UploadFromByteArrayAsync(image, 0, image.Length).ConfigureAwait(false);
        }

        private async Task InitializeStorage()
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