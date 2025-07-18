using MediatR;

namespace Application.Commands.ApplyTemplate;

public interface IApplyTemplateHandler : IRequestHandler<ApplyTemplateCommand, ApplyTemplateResponse>
{
    
}