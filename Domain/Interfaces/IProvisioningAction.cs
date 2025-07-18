using Domain.Models;

namespace Domain.Interfaces;

public interface IProvisioningAction
{
    string ActionType => GetType().Name;
    void Validate();
}