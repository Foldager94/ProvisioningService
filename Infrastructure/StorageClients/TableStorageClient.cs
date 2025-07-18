using Application.StorageClients;
using Azure;
using Azure.Data.Tables;

namespace Infrastructure.StorageClients;

public class TableStorageClient : ITableStorageClient
{
    private readonly TableClient _tableClient;

    public TableStorageClient(TableServiceClient tableServiceClient, InfrastructureSettings settings)
    {
        _tableClient = tableServiceClient.GetTableClient(settings.SolutionDataTable);
        _tableClient.CreateIfNotExistsAsync();
    }
    
    public async Task<TableEntity> GetSolutionMetadata(string solutionId)
    {
        return (await _tableClient.GetEntityAsync<TableEntity>("solutions", solutionId)).Value;
    }
    
    
}