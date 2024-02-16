using Azure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace MRA.Jobs.Web.AzureKeyVault;

public static class WebApplicationBuilderExtension
{
    public static void ConfigureAzureKeyVault(this WebApplicationBuilder builder)
    {
        if (!builder.Environment.IsDevelopment())
        {
            using var x509Store = new X509Store(StoreLocation.CurrentUser);
            x509Store.Open(OpenFlags.ReadOnly);

            var thumbprint = builder.Configuration["AzureKeyVault:AzureADCertThumbprint"];

            var certificate = x509Store.Certificates
                .Find(
                    X509FindType.FindByThumbprint,
                    thumbprint,
                    validOnly: false)
                .OfType<X509Certificate2>()
                .Single();

            builder.Configuration.AddAzureKeyVault(
                new Uri($"https://{builder.Configuration["AzureKeyVault:KeyVaultName"]}.vault.azure.net/"),
                new ClientCertificateCredential(
                    builder.Configuration["AzureKeyVault:AzureADDirectoryId"],
                    builder.Configuration["AzureKeyVault:AzureADApplicationId"],
                    certificate),
                new PrefixKeyVaultSecretManager());
        }
    }
}
