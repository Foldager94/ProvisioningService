using Domain.Interfaces;

namespace Domain.Models.Actions;

public class CreateSpoSiteAction : IProvisioningAction
{
    public string Url { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Owner { get; set; } = null!;
    public string? Description { get; set; }
    public string? PageDesign { get; set; }

    public ProvisioningParameters Required { get; set; }
    public ProvisioningParameters Optional { get; set; }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Url))
            throw new InvalidOperationException("Url is required");

        if (string.IsNullOrWhiteSpace(Title))
            throw new InvalidOperationException("Title is required");

        if (string.IsNullOrWhiteSpace(Owner))
            throw new InvalidOperationException("Owner is required");
    }
}