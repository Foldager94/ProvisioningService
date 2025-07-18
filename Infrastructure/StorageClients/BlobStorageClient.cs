using Application.StorageClients;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Infrastructure.StorageClients;

public class BlobStorageClient(BlobServiceClient blobClient, InfrastructureSettings settings) : IBlobStorageClient
{
    public async Task<BlobDownloadInfo> GetProvisioningSchemaAsync(string path)
    {
        var blobContainerClient = blobClient.GetBlobContainerClient(settings.ProvisioningSchemaBlobContainer);
        var provisioningSchemaBlobClient = blobContainerClient.GetBlobClient(path);
        return (await provisioningSchemaBlobClient.DownloadAsync()).Value;
    }
    
    public async Task<BlobDownloadInfo> GetTemplateAsync(string path)
    {
        var blobContainerClient = blobClient.GetBlobContainerClient(settings.TemplateBlobContainer);
        var provisioningSchemaBlobClient = blobContainerClient.GetBlobClient(path);
        return (await provisioningSchemaBlobClient.DownloadAsync()).Value;
    }
}