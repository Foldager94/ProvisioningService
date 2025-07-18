using System.Text.Json;
using Application.Factories;
using Domain.Interfaces;
using Domain.Models;
using Domain.Models.Actions;

namespace Infrastructure.Factories;

public class ProvisioningActionFactory : IProvisioningActionFactory
{
    public IProvisioningAction Create(JsonElement root)
    {
        var actionType = root.GetProperty("action").GetString();

        switch (actionType)
        {
            case "CreateSpoSite":
                return BuildCreateSpoSiteAction(root);
            case "ApplyTemplate":
                return BuildApplyTemplateAction(root);
            default:
                throw new NotSupportedException($"Action type '{actionType}' is not supported.");
        }
    }

    private IProvisioningAction BuildApplyTemplateAction(JsonElement root)
    {
        var requiredParams = root.GetProperty("required").EnumerateArray();
        IEnumerable<string> requiredListParams = requiredParams
            .Select(e => e.GetString()!)
            .ToList();       
        var optionalParams = root.GetProperty("optional").EnumerateArray();
        IEnumerable<string> optionalListParams = optionalParams
            .Select(e => e.GetString()!)
            .ToList();   
        
        var templateId = root.GetProperty("templateId").GetGuid();
        
        return new ApplyTemplateAction(templateId, new ProvisioningParameters(requiredListParams), new ProvisioningParameters(optionalListParams));
    }

    private IProvisioningAction BuildCreateSpoSiteAction(JsonElement root)
    {
        var siteType = root.GetProperty("type").GetString()!;
        return new CreateSpoSiteAction()
        {
            Type = siteType,
        };
    }
}