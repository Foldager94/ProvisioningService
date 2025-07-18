using MediatR;

namespace Application.Commands.CreateSpoSite;

public interface ICreateSpoSiteHandler : IRequestHandler<CreateSpoSiteCommand, CreateSpoSiteResponse>
{
    
}