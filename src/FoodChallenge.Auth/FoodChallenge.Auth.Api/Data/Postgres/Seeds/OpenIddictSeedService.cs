using OpenIddict.Abstractions;

namespace FoodChallenge.Auth.Api.Data.Postgres.Seeds;

public class OpenIddictSeedService(
    ILogger<OpenIddictSeedService> logger,
    IOpenIddictApplicationManager applicationManager) : IOpenIddictSeedService
{
    public async Task SeedAsync()
    {
        await SeedOrdersApiClientAsync();
        await SeedConfigurationApiClientAsync();
    }

    private async Task SeedOrdersApiClientAsync()
    {
        const string clientId = "190abe6d-8d8a-4cc5-9d9c-22e599525c9f";
        const string clientSecret = "948a0a2e-5072-4739-818c-6f53ddc2a192";
        const string displayName = "Orders API";

        var scopes = new[]
        {
            "openid",
            "orders.read",
            "orders.write",
        };

        await CreateClientIfNotExistsAsync(clientId, clientSecret, displayName, scopes);
    }

    private async Task SeedConfigurationApiClientAsync()
    {
        const string clientId = "f2b46aaa-084c-4ce1-8361-27a472cfe320";
        const string clientSecret = "948a0a2e-5072-4739-818c-6f53ddc2a192";
        const string displayName = "Configuration API";

        var scopes = new[]
        {
            "openid",
            "configuration.read",
            "configuration.write",
        };

        await CreateClientIfNotExistsAsync(clientId, clientSecret, displayName, scopes);
    }

    private async Task CreateClientIfNotExistsAsync(
        string clientId,
        string clientSecret,
        string displayName,
        string[] scopes)
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

            await applicationManager.CreateAsync(descriptor);

            logger.LogInformation("Cliente OpenIddict '{DisplayName}' ({ClientId}) criado com sucesso.", displayName, clientId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao criar seed do cliente OpenIddict '{DisplayName}'.", displayName);
        }
    }
}
