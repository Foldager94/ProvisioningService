using Domain.Interfaces;
using Domain.Models;

namespace Application.Binder;

public interface IProvisioningSchemaBinder
{
    IProvisioningSchema Bind(IProvisioningSchema schema, Dictionary<string, string?> userData);
}