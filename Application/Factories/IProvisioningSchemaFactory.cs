using System.Text.Json;
using Domain.Interfaces;

namespace Application.Factories;

public interface IProvisioningSchemaFactory
{
    IProvisioningSchema Create(JsonElement json);
}