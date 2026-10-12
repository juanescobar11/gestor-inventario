using GestorInventario.Application.Interfaces;
using GestorInventario.Domain.Entities;
using GestorInventario.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestorInventario.Infrastructure.Repositories;

public class ProductoRepository(PersistenceContext context) : IProductoRepository
{
    public Task<Producto?> ObtenerPorIdAsync(int id, CancellationToken ct) =>
        context.Productos.Include(p => p.Categoria).FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<IReadOnlyList<Producto>> ListarAsync(int? categoriaId, bool? stockBajo, CancellationToken ct)
    {
        var query = context.Productos.Include(p => p.Categoria).AsQueryable();

        if (categoriaId.HasValue)
            query = query.Where(p => p.CategoriaId == categoriaId.Value);

        if (stockBajo == true)
            query = query.Where(p => p.Stock < p.StockMinimo);
        else if (stockBajo == false)
            query = query.Where(p => p.Stock >= p.StockMinimo);

        return await query.OrderBy(p => p.Nombre).ToListAsync(ct);
    }

    public async Task AgregarAsync(Producto producto, CancellationToken ct)
    {
        context.Productos.Add(producto);
        await context.SaveChangesAsync(ct);
    }

    public Task GuardarCambiosAsync(CancellationToken ct) => context.SaveChangesAsync(ct);

    public async Task EliminarAsync(Producto producto, CancellationToken ct)
    {
        context.Productos.Remove(producto);
        await context.SaveChangesAsync(ct);
    }
}