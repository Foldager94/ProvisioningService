using System.Text.Json;
using Application.Factories;
using Domain.Interfaces;
using Domain.Models;

namespace Infrastructure.Factories;

public class ProvisioningSchemaFactory(IProvisioningActionFactory actionFactory) : IProvisioningSchemaFactory
{
    public IProvisioningSchema Create(JsonElement root)
    {
        var idString = root.GetProperty("id").GetString();
        var id = Guid.Parse(idString);
        
        var flow = root.GetProperty("flow");

        var actions = new List<IProvisioningAction>();

        foreach (var item in flow.EnumerateArray())
        {
            var action = actionFactory.Create(item);
            actions.Add(action);
        }

        return new ProvisioningSchema(id, actions);
    }
}