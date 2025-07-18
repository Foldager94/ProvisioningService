using Domain.Interfaces;

namespace Domain.Models;

public class ProvisioningSchema(Guid id, IEnumerable<IProvisioningAction> actions) : IProvisioningSchema
{
    public Guid Id { get; } = id;
    public IEnumerable<IProvisioningAction> Actions { get; } = actions;
}