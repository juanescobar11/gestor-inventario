using GestorInventario.Domain.Entities;
using GestorInventario.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestorInventario.Infrastructure.Seeds;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(PersistenceContext context)
    {
        if (!await context.Categorias.AnyAsync())
        {
            context.Categorias.AddRange(
                new Categoria { Nombre = "Electrónica" },
                new Categoria { Nombre = "Oficina" },
                new Categoria { Nombre = "Aseo" });
            await context.SaveChangesAsync();
        }

        if (await context.Productos.AnyAsync()) return;

        var categorias = await context.Categorias.OrderBy(c => c.Id).ToListAsync();
        int Cat(int i) => categorias[i % categorias.Count].Id;

        context.Productos.AddRange(
            Producto.Crear("Mouse inalámbrico", "Mouse óptico 2.4 GHz", 45000m, 25, 5, Cat(0)),
            Producto.Crear("Teclado mecánico", "Teclado retroiluminado", 180000m, 3, 5, Cat(0)),
            Producto.Crear("Audífonos Bluetooth", "Con cancelación de ruido", 120000m, 2, 4, Cat(0)),
            Producto.Crear("Resma de papel carta", "500 hojas, 75 g", 18000m, 40, 10, Cat(1)),
            Producto.Crear("Bolígrafos caja x12", "Tinta azul", 12000m, 4, 8, Cat(1)),
            Producto.Crear("Detergente líquido 5L", null, 32000m, 15, 5, Cat(2)),
            Producto.Crear("Desinfectante multiusos", "Aroma lavanda", 9500m, 6, 5, Cat(2)));

        await context.SaveChangesAsync();
    }
}