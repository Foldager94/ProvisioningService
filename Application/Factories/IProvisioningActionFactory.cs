using System.Text.Json;
using Domain.Interfaces;

namespace Application.Factories;

public interface IProvisioningActionFactory
{
    IProvisioningAction Create(JsonElement json);
}