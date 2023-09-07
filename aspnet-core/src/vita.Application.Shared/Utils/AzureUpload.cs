using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace vita.Utils
{
    public static class AzureUpload
    {

        public static void CopyFolderToAzureStorage(string localFolderPath, int tenantId, string uniqueIdentifier)
        {
            string containerName = "invoicefiles";
            string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=unicoresandbox;AccountKey=WLgA21LCb+hWWEpUC5qVZ2o+wp521UHRINLsBiAd5oshneapYtHq1zBpjuGJ2ZLm54D14oLg81C/+ASt2pjxuw==;EndpointSuffix=core.windows.net";
            
            BlobServiceClient blobServiceClient = new BlobServiceClient(storageConnectionString);

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            foreach (string filePath in Directory.GetFiles(localFolderPath, "*", SearchOption.AllDirectories))
            {
                string relativePath = filePath.Replace(localFolderPath, string.Empty).TrimStart(Path.DirectorySeparatorChar);

                BlobClient blobClient = containerClient.GetBlobClient(Path.Combine(tenantId.ToString(), uniqueIdentifier, relativePath));

                using (FileStream fileStream = File.OpenRead(filePath))
                {
                    blobClient.Upload(fileStream, true);
                }
            }
        }
        public static string GetBlobUrl(string blobName)
        {
            string containerName = "invoicefiles";
            string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=unicoresandbox;AccountKey=WLgA21LCb+hWWEpUC5qVZ2o+wp521UHRINLsBiAd5oshneapYtHq1zBpjuGJ2ZLm54D14oLg81C/+ASt2pjxuw==;EndpointSuffix=core.windows.net";

            BlobServiceClient blobServiceClient = new BlobServiceClient(storageConnectionString);

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            return blobClient.Uri.ToString();
        }
    }
}
