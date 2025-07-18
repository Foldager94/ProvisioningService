using System.Text.Json;

namespace Application.Repositories;

public interface IProvisioningSchemaRepository
{
    Task<JsonDocument> GetSchemaAsync(string id);
}