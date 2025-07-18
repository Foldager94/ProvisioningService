using Azure.Storage.Blobs.Models;

namespace Application.StorageClients;

public interface IBlobStorageClient
{
    Task<BlobDownloadInfo> GetProvisioningSchemaAsync(string path);

    Task<BlobDownloadInfo> GetTemplateAsync(string path);

}