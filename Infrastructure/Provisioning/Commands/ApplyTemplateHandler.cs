using Application.Commands.ApplyTemplate;
using Application.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.SharePoint.Client;
using PnP.Core.Services;
using PnP.Framework;
using PnP.Framework.Provisioning.ObjectHandlers;

namespace Infrastructure.Provisioning.Commands;

public class ApplyTemplateHandler(ITemplateRepository templateRepository, IPnPContextFactory contextFactory, ILogger<ApplyTemplateHandler> logger) : IApplyTemplateHandler
{
    public async Task<ApplyTemplateResponse> Handle(ApplyTemplateCommand request, CancellationToken cancellationToken)
    {
        var action = request.Action;
        var uri = new Uri($"https://{request.TenantName}.sharepoint.com{action.SiteUrl}");
        
        using var clientContext = await GetClientContextAsync(uri);

        var templates = await templateRepository.GetTemplatesAsync(action.TemplateId);
        
        var applyingInformation = GetProvisioningApplyingInformation();
        
        foreach (var provisioningTemplate in templates)
        {
            foreach (var kvp in action.Required)
            {
                provisioningTemplate.Parameters[kvp.Key] = kvp.Value;
            }
            
            logger.LogInformation("Found template: {TemplateId}", provisioningTemplate.Id);
            clientContext.Web.ApplyProvisioningTemplate(provisioningTemplate, applyingInformation);
        }

        return new ApplyTemplateResponse("Template applied successfully.");
    }
    
    private async Task<ClientContext> GetClientContextAsync(Uri uri)
    {
        var clientContext = await contextFactory.CreateAsync(uri);
        return await PnPCoreSdk.Instance.GetClientContextAsync(clientContext);
    }
    
    private ProvisioningTemplateApplyingInformation GetProvisioningApplyingInformation()
    {
        return new ProvisioningTemplateApplyingInformation
        {
            ProgressDelegate = (message, step, total) =>
            {
                logger.LogInformation("{0:00}/{1:00} - {2}", step, total, message);
            },
            MessagesDelegate = (message, type) =>
            {
                logger.LogTrace($"Provisioning Messages: {message}. Type: {type}");
            }
        };
    }
}