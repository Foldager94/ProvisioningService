using Application.Binder;
using Domain.Interfaces;
using Domain.Models;
using Domain.Models.Actions;

namespace Infrastructure.Binder;

public class ProvisioningDataBinder : IProvisioningSchemaBinder
{
    public IProvisioningSchema Bind(IProvisioningSchema schema, Dictionary<string, string?> userData)
    {
        foreach (var action in schema.Actions)
        {

            switch (action)
            {
                case CreateSpoSiteAction a:
                    BindCreateSpoSiteAction(a, userData);
                    break;
                case ApplyTemplateAction a:
                    BindApplyTemplateAction(a, userData);
                    break;
                default:
                    throw new NotSupportedException($"Action type '{action}' is not supported.");
            }
            
        }
        return schema;
    }

    private void BindCreateSpoSiteAction(CreateSpoSiteAction action, Dictionary<string, string?> userData)
    {
        action.Url = userData["siteUrl"] ?? throw new NullReferenceException("'siteUrl' is required.");
        action.Title = userData["title"] ?? throw new NullReferenceException("'title' is required.");
        action.Owner = userData["owner"] ?? throw new NullReferenceException("'owner' is required.");
    }

    private void BindApplyTemplateAction(ApplyTemplateAction action, Dictionary<string, string?> userData)
    {
        action.SiteUrl = userData["siteUrl"] ?? throw new NullReferenceException("'siteUrl' is required.");
        
        foreach (var kvp in action.Required.ToList())
        {
            if (userData.TryGetValue(kvp.Key, out var val))
            {
                action.Required.Set(kvp.Key, val);
            }
        }
        foreach (var kvp in action.Optional.ToList())
        {
            if (userData.TryGetValue(kvp.Key, out var val))
            {
                if (val == null) continue;
                action.Optional.Set(kvp.Key, val);
            }
        }
    }
}
