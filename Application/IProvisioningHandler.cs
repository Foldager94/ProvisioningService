using System.Text.Json;
using Domain.Interfaces;

namespace Application;

public interface IProvisioningHandler
{
    Task RunAsync(IProvisioningSchema schema);
}