using Application.Repositories;
using Application.StorageClients;
using Microsoft.Extensions.Logging;
using PnP.Framework.Provisioning.Connectors;
using PnP.Framework.Provisioning.Model;
using PnP.Framework.Provisioning.Providers.Xml;

namespace Infrastructure.Repositories;

public class TemplateRepository(IBlobStorageClient blobStorageClient, ILogger<TemplateRepository> logger)
    : ITemplateRepository
{
    public async Task<IEnumerable<ProvisioningTemplate>> GetTemplatesAsync(Guid templateId)
    {
        var blobInfo = await blobStorageClient.GetTemplateAsync($"{templateId.ToString()}.pnp");
        var connector = new OpenXMLConnector(blobInfo.Content);
        var provider = new XMLOpenXMLTemplateProvider(connector);
        
        var templates = provider.GetTemplates().ToList();
        
        templates.ForEach(t => t.Connector = connector);
        
        logger.LogInformation("Found {Count} templates for ID: {TemplateId}", templates.Count, templateId);
        
        return templates;
    }
    
}