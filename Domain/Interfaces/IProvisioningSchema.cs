namespace Domain.Interfaces;

public interface IProvisioningSchema
{
    Guid Id { get; }
    IEnumerable<IProvisioningAction> Actions { get; }
}