using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Application;
using Application.Binder;
using Application.Factories;
using Application.Repositories;
using Application.StorageClients;
using Azure.Identity;
using Domain.Interfaces;
using Infrastructure.Binder;
using Infrastructure.Factories;
using Infrastructure.Provisioning;
using Infrastructure.Provisioning.Commands;
using Infrastructure.Repositories;
using Infrastructure.StorageClients;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using PnP.Core.Auth;

namespace Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var loggerFactory = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger(nameof(ConfigureServices));
        var settings = configuration.Get<InfrastructureSettings>();
        logger.LogInformation($"settings: {settings}");
        if(settings == null)
            throw new ArgumentNullException(nameof(settings));
        
        
        services.AddSingleton(settings!);
        services.AddGraphServices(settings);
        services.AddPnPServices(settings);
        services.AddAzureClientServices(settings);

        services.AddMediatR(cfg =>    {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        
        services.AddScoped<IProvisioningSchemaRepository, ProvisioningSchemaRepository>();
        services.AddScoped<ITemplateRepository, TemplateRepository>();
        services.AddTransient<IProvisioningActionFactory,ProvisioningActionFactory>();
        services.AddTransient<IProvisioningSchemaFactory,ProvisioningSchemaFactory>();
        services.AddTransient<IProvisioningSchemaBinder,ProvisioningDataBinder>();
        
        services.AddTransient<IProvisioningHandler, ProvisioningCommandHandler>();
        
        services.AddTransient<IProvisioningSchemaResolver, ProvisioningSchemaResolver>();
        
        return services;
    }
        
    private static void AddPnPServices(this IServiceCollection services, InfrastructureSettings settings)
    {
        services.AddPnPCore(options =>
        {
            var authProvider = new X509CertificateAuthenticationProvider(
                settings.ClientId,
                settings.TenantId,
                settings.CertificateStoreName,
                settings.CertificateStoreLocation,
                settings.CertificateThumbprint
            );
            
            options.DefaultAuthenticationProvider = authProvider;
        });
    }
    private static void AddGraphServices(this IServiceCollection services, InfrastructureSettings settings)
    {
        services.AddScoped<GraphServiceClient>(sp =>
        {
            using var store = new X509Store(settings.CertificateStoreName, settings.CertificateStoreLocation);
            store.Open(OpenFlags.ReadOnly);
            var certificate = store.Certificates
                .Find(X509FindType.FindByThumbprint, settings.CertificateThumbprint, validOnly: false)
                .OfType<X509Certificate2>()
                .FirstOrDefault();

            if (certificate == null)
            {
                throw new InvalidOperationException($"Certificate with thumbprint {settings.CertificateThumbprint} not found.");
            }
            
            var credential = new ClientCertificateCredential(
                settings.TenantId,
                settings.ClientId,
                certificate
            );
            
            var authProvider = new DelegateAuthenticationProvider(async (requestMessage) =>
            {
                var token = await credential.GetTokenAsync(
                    new Azure.Core.TokenRequestContext(new[] { "https://graph.microsoft.com/.default" }));
            
                requestMessage.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Token);
            });

            return new GraphServiceClient(authProvider);
        });
    }
    
    private static void AddAzureClientServices(this IServiceCollection services,
        InfrastructureSettings settings)
    {
        services.AddAzureClients(clientBuilder =>
        {
            clientBuilder.AddQueueServiceClient(settings.AzureWebJobsStorage);
            clientBuilder.AddTableServiceClient(settings.AzureWebJobsStorage);
            clientBuilder.AddBlobServiceClient(settings.AzureWebJobsStorage);
        });
        
        services.AddScoped<IBlobStorageClient, BlobStorageClient>();
        services.AddScoped<ITableStorageClient, TableStorageClient>();

    }
}
