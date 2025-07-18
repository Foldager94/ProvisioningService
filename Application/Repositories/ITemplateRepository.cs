using PnP.Framework.Provisioning.Model;

namespace Application.Repositories;

public interface ITemplateRepository
{
    Task<IEnumerable<ProvisioningTemplate>> GetTemplatesAsync(Guid templateId);
}