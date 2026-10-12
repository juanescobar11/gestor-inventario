using GestorInventario.Application.Interfaces;
using GestorInventario.Infrastructure.Persistence;
using GestorInventario.Infrastructure.Repositories;
using GestorInventario.Infrastructure.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GestorInventario.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<PersistenceContext>(o => o.UseSqlServer(connectionString));
        services.AddScoped<IProductoRepository, ProductoRepository>();
        services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        return services;
    }

    public static async Task SeedDatabaseAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PersistenceContext>();
        await DatabaseSeeder.SeedAsync(context);
    }
}