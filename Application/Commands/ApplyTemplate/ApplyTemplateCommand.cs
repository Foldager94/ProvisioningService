using Domain.Models.Actions;
using MediatR;

namespace Application.Commands.ApplyTemplate;

public record ApplyTemplateCommand(ApplyTemplateAction Action, string TenantName) : IRequest<ApplyTemplateResponse>
{
    
}