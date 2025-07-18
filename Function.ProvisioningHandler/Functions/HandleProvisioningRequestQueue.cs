using System.Text.Json;
using Application;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Function.ProvisioningHandler.Functions;

public class HandleProvisioningRequestQueue(
    ILogger<HandleProvisioningRequestQueue> logger,
    IProvisioningSchemaResolver schemaResolver,
    IProvisioningHandler handler)
{
    [Function(nameof(HandleProvisioningRequestQueue))]
    public async Task Run(
        [QueueTrigger("provisioningrequests", Connection = "AzureWebJobsStorage")] QueueMessage message)
    {
        logger.LogInformation("Processing message from queue: {MessageId}", message.MessageId);

        using var jsonDoc = ProcessProvisioningRequestMessage(message);
        var jsonData = jsonDoc.RootElement;
        var schema = await schemaResolver.ProcessAsync(jsonData);

        await handler.RunAsync(schema);
    }

    private JsonDocument ProcessProvisioningRequestMessage(QueueMessage message)
    {
        var docData = JsonDocument.Parse(message.MessageText);
        var jsonData = docData.RootElement;

        if (jsonData.TryGetProperty("id", out var idProperty) && idProperty.ValueKind == JsonValueKind.String)
            return docData;

        logger.LogError("Invalid input: 'id' property is missing or not a string.");
        throw new ArgumentException("Invalid input: 'id' property is required and must be a string.");
    }
}