using Application;
using Application.Commands.ApplyTemplate;
using Application.Commands.CreateSpoSite;
using Domain.Interfaces;
using Domain.Models.Actions;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Infrastructure.Provisioning;

public class ProvisioningCommandHandler(IMediator mediator, InfrastructureSettings settings, ILogger<ProvisioningCommandHandler> logger)
    : IProvisioningHandler
{
    public async Task RunAsync(IProvisioningSchema schema)
    {
        await ExecuteActionsAsync(schema.Actions);
    }

    private async Task ExecuteActionsAsync(IEnumerable<IProvisioningAction> actions)
    {
        foreach (var action in actions)
        {
            switch (action)
            {
                case CreateSpoSiteAction createAction:
                    await mediator.Send(new CreateSpoSiteCommand(createAction, settings.TenantName));
                    break;
                case ApplyTemplateAction templateAction:
                    await mediator.Send(new ApplyTemplateCommand(templateAction, settings.TenantName));
                    break;
                default:
                    throw new NotSupportedException($"Action type {action.GetType().Name} is not supported");
            }
        }
    }

}