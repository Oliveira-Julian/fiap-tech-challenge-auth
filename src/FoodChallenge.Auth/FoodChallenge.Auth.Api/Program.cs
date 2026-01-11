using FoodChallenge.Auth.Api.Data.Postgres;
using FoodChallenge.Auth.Api.Data.Postgres.Seeds;
using FoodChallenge.Auth.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Registrar serviços
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddEfPostgresDependency(configuration.GetConnectionString("PostgreSQL") ?? string.Empty);

// Configurar OpenIddict
builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
            .UseDbContext<AuthDbContext>();
    })
    .AddServer(options =>
    {
        options.SetTokenEndpointUris("/connect/token");
        options.SetIntrospectionEndpointUris("/connect/introspect");

        options.AllowClientCredentialsFlow();

        options.RegisterScopes(
            "orders.read",
            "orders.write",
            "configuration.read",
            "configuration.write");

        options.AddDevelopmentEncryptionCertificate()
               .AddDevelopmentSigningCertificate();

        options.UseAspNetCore()
               .EnableTokenEndpointPassthrough();
    })
    .AddValidation(options =>
    {
        options.UseLocalServer();
        options.UseAspNetCore();
    });

var app = builder.Build();

// Executar migrations e seed
using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
await dbContext.Database.EnsureCreatedAsync();
var seeder = scope.ServiceProvider.GetRequiredService<IOpenIddictSeedService>();
await seeder.SeedAsync();

// Middleware
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Endpoints
app.MapControllers();
app.UseHttpsRedirection();

app.Run();

