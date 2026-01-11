using FoodChallenge.Auth.Api.Constants;
using OpenIddict.Abstractions;

namespace FoodChallenge.Auth.Api.Data.Postgres.Seeds;

public class OpenIddictSeedService(
    ILogger<OpenIddictSeedService> logger,
    IOpenIddictApplicationManager applicationManager) : IOpenIddictSeedService
{
    public async Task SeedAsync()
    {
        await SeedOrdersApiClientAsync();
        await SeedConfigurationsApiClientAsync();
        await SeedPaymentsApiClientAsync();
        await SeedKitchensApiClientAsync();
    }

    private async Task SeedOrdersApiClientAsync()
    {
        const string clientId = "190abe6d-8d8a-4cc5-9d9c-22e599525c9f";
        const string clientSecret = "948a0a2e-5072-4739-818c-6f53ddc2a192";
        const string displayName = "Orders API";

        var scopes = new[]
        {
            OpenIddictConstants.Scopes.OpenId,
            AuthConstants.Scopes.OrdersRead,
            AuthConstants.Scopes.OrdersWrite,
        };

        var audiences = new[]
        {
            AuthConstants.Audiences.OrdersApi
        };

        await CreateClientIfNotExistsAsync(clientId, clientSecret, displayName, scopes, audiences);
    }

    private async Task SeedConfigurationsApiClientAsync()
    {
        const string clientId = "f2b46aaa-084c-4ce1-8361-27a472cfe320";
        const string clientSecret = "948a0a2e-5072-4739-818c-6f53ddc2a192";
        const string displayName = "Configurations API";

        var scopes = new[]
        {
            OpenIddictConstants.Scopes.OpenId,
            AuthConstants.Scopes.ConfigurationsRead,
            AuthConstants.Scopes.ConfigurationsWrite,
        };

        var audiences = new[]
        {
            AuthConstants.Audiences.ConfigurationsApi
        };

        await CreateClientIfNotExistsAsync(clientId, clientSecret, displayName, scopes, audiences);
    }

    private async Task SeedPaymentsApiClientAsync()
    {
        const string clientId = "cd23f25d-6ebb-44dd-8458-b353bea49820";
        const string clientSecret = "cdb90fc1-a972-446f-9cd5-fa45bec7a60f";
        const string displayName = "Payments API";

        var scopes = new[]
        {
            OpenIddictConstants.Scopes.OpenId,
            AuthConstants.Scopes.PaymentsRead,
            AuthConstants.Scopes.PaymentsWrite,
        };

        var audiences = new[]
        {
            AuthConstants.Audiences.PaymentsApi
        };

        await CreateClientIfNotExistsAsync(clientId, clientSecret, displayName, scopes, audiences);
    }

    private async Task SeedKitchensApiClientAsync()
    {
        const string clientId = "42d9f099-651a-4a85-b250-50c6e7c57e25";
        const string clientSecret = "a38fbf18-fd77-4cb6-b905-e15a40348bab";
        const string displayName = "Kitchens API";
        var scopes = new[]
        {
            OpenIddictConstants.Scopes.OpenId,
            AuthConstants.Scopes.KitchensRead,
            AuthConstants.Scopes.KitchensWrite,
        };

        var audiences = new[]
        {
            AuthConstants.Audiences.KitchensApi
        };

        await CreateClientIfNotExistsAsync(clientId, clientSecret, displayName, scopes, audiences);
    }

    private async Task CreateClientIfNotExistsAsync(
        string clientId,
        string clientSecret,
        string displayName,
        string[] scopes,
        string[] audiences)
    {
        try
        {
            var existingClient = await applicationManager.FindByClientIdAsync(clientId);
            if (existingClient is not null)
                return;

            var descriptor = new OpenIddictApplicationDescriptor
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                DisplayName = displayName
            };

            descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token);
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Introspection);
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.ClientCredentials);
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Token);

            foreach (var scope in scopes)
            {
                descriptor.Permissions.Add($"{OpenIddictConstants.Permissions.Prefixes.Scope}{scope}");
            }

            foreach (var audience in audiences)
            {
                descriptor.Permissions.Add($"{OpenIddictConstants.Permissions.Prefixes.Audience}{audience}");
            }

            await applicationManager.CreateAsync(descriptor);

            logger.LogInformation("Cliente OpenIddict '{DisplayName}' ({ClientId}) criado com sucesso.", displayName, clientId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao criar seed do cliente OpenIddict '{DisplayName}'.", displayName);
        }
    }
}
