using Domain.Interfaces;

namespace Domain.Models.Actions;

public class ApplyTemplateAction(Guid templateId, ProvisioningParameters required, ProvisioningParameters optional)
    : IProvisioningAction
{
    public Guid TemplateId { get; set; } = templateId;
    public string SiteUrl { get; set; } = string.Empty;
    public ProvisioningParameters Required { get; set; } = required;
    public ProvisioningParameters Optional { get; set; } = optional;

    public void Validate()
    {
        throw new NotImplementedException();
    }
}