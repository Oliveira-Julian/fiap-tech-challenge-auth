using Microsoft.EntityFrameworkCore;
using OpenIddict.EntityFrameworkCore.Models;

namespace FoodChallenge.Auth.Api.Data.Postgres;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    public DbSet<OpenIddictEntityFrameworkCoreApplication> Applications { get; set; } = null!;
    public DbSet<OpenIddictEntityFrameworkCoreAuthorization> Authorizations { get; set; } = null!;
    public DbSet<OpenIddictEntityFrameworkCoreScope> Scopes { get; set; } = null!;
    public DbSet<OpenIddictEntityFrameworkCoreToken> Tokens { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.UseOpenIddict();
    }
}
