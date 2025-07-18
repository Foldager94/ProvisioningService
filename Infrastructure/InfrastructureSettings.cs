using System.Security.Cryptography.X509Certificates;

namespace Infrastructure;

public class InfrastructureSettings
{
    public required string TenantName { get; init; }
    public required string ClientId { get; init; }
    public required string SiteUrl { get; init; }
    public required string TenantId { get; init; }
    public required string CertificateThumbprint { get; init; }
    public StoreName CertificateStoreName { get; init; } = StoreName.My;
    public StoreLocation CertificateStoreLocation { get; init; } = StoreLocation.CurrentUser;
    public required string AzureWebJobsStorage { get; init; }
    public required string DefaultOwner { get; set; }
    public required string TemplateBlobContainer { get; set; }
    public required string ProvisioningSchemaBlobContainer { get; set; }
    public required string FormSchemaBlobContainer { get; set; }
    public required string SolutionDataTable { get; set; }
    public required string ProvisioningQueue { get; set; }
    
    public override string ToString()
    {
        return $"TenantName: {TenantName}, " +
               $"ClientId: {ClientId}, " +
               $"TenantId: {TenantId}, " +
               $"CertificateThumbprint: {CertificateThumbprint}, " +
               $"CertificateStoreName: {CertificateStoreName}, " +
               $"CertificateStoreLocation: {CertificateStoreLocation} ";
    }
}