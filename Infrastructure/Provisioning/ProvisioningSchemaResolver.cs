using System.Text.Json;
using Application;
using Application.Binder;
using Application.Factories;
using Application.Repositories;
using Domain.Interfaces;
using Infrastructure.Extensions;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Provisioning;

public class ProvisioningSchemaResolver(
    ILogger<ProvisioningSchemaResolver> logger,
    IProvisioningSchemaBinder binder,
    IProvisioningSchemaFactory schemaFactory,
    IProvisioningSchemaRepository schemaRepository): IProvisioningSchemaResolver
{
    public async Task<IProvisioningSchema> ProcessAsync(JsonElement jsonData)
    {
        var id = ValidateAndGetId(jsonData);
        
        using var docSchema = await schemaRepository.GetSchemaAsync(id);
        
        var schema = schemaFactory.Create(docSchema.RootElement);
        
        return binder.Bind(schema, jsonData.ToDictionary());
    }
    
    private string ValidateAndGetId(JsonElement jsonData)
    {
        var id = jsonData.GetProperty("id").GetString();
        
        if (!string.IsNullOrEmpty(id)) return id;
        
        logger.LogError("Provisioning schema ID is missing in the JSON data.");
        throw new ArgumentException("Provisioning schema ID is required.", nameof(jsonData));
    }
}