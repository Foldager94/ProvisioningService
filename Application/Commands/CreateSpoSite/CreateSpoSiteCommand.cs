using Domain.Models.Actions;
using MediatR;

namespace Application.Commands.CreateSpoSite;

public record CreateSpoSiteCommand(CreateSpoSiteAction Action, string TenantName) : IRequest<CreateSpoSiteResponse>
{
    
}