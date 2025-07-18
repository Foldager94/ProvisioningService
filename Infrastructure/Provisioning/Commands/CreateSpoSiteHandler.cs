using Application.Commands.CreateSpoSite;
using Microsoft.Extensions.Logging;
using Microsoft.SharePoint.Client;
using PnP.Core.Services;
using PnP.Framework;
using PnP.Framework.Sites;

namespace Infrastructure.Provisioning.Commands;

public class CreateSpoSiteHandler(IPnPContextFactory contextFactory, ILogger<CreateSpoSiteHandler> logger)
    : ICreateSpoSiteHandler
{
    public async Task<CreateSpoSiteResponse> Handle(CreateSpoSiteCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Create SharePointOnline site requested");
        
        var action = request.Action;

        var uri = new Uri($"https://{request.TenantName}-admin.sharepoint.com");
        var clientContext = await GetClientContextAsync(uri);

        var commSite = new CommunicationSiteCollectionCreationInformation
        {
            Url = $"https://{request.TenantName}.sharepoint.com" + action.Url,
            Title = action.Title,
            Owner = action.Owner,
        };
        logger.LogInformation(
            $"Creating new site with \nURL: {commSite.Url}, \nTitle: {commSite.Title}, \nOwner: {commSite.Owner}");
        
        await clientContext.CreateSiteAsync(commSite);
        
        logger.LogInformation("Site created successfully: " + commSite.Url);
        
        return new CreateSpoSiteResponse("Site created successfully: " + commSite.Url);
    }

    private async Task<ClientContext> GetClientContextAsync(Uri uri)
    {
        var clientContext = await contextFactory.CreateAsync(uri);
        return await PnPCoreSdk.Instance.GetClientContextAsync(clientContext);
    }
}