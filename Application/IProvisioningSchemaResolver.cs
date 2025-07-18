using System.Text.Json;
using Domain.Interfaces;

namespace Application;

public interface IProvisioningSchemaResolver
{
    Task<IProvisioningSchema> ProcessAsync(JsonElement jsonData);
}