using System.Text.Json;
using Application.Repositories;
using Application.StorageClients;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class ProvisioningSchemaRepository(
    ITableStorageClient tableStorageClient,
    IBlobStorageClient blobStorageClient,
    ILogger<ProvisioningSchemaRepository> logger)
    : IProvisioningSchemaRepository
{
    public async Task<JsonDocument> GetSchemaAsync(string id)
    {
        var metadata = await tableStorageClient.GetSolutionMetadata(id);
        var schemaPath = metadata["ProvisioningSchemaPath"].ToString();
        
        if (string.IsNullOrEmpty(schemaPath))
        {
            logger.LogError("Provisioning schema path is empty for solution {SolutionId}", id);
            throw new InvalidOperationException("Provisioning schema path is not set.");
        }
        
        var schemaBlob = await blobStorageClient.GetProvisioningSchemaAsync(schemaPath);

        using var reader = new StreamReader(schemaBlob.Content);
        var schemaJson = await reader.ReadToEndAsync();
        
        return JsonDocument.Parse(schemaJson);
    }
}