using Azure.Data.Tables;

namespace Application.StorageClients;

public interface ITableStorageClient
{
    Task<TableEntity> GetSolutionMetadata(string solutionId);
}