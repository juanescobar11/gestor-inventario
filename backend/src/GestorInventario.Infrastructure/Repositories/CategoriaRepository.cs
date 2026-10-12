using GestorInventario.Application.Interfaces;
using GestorInventario.Domain.Entities;
using GestorInventario.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestorInventario.Infrastructure.Repositories;

public class CategoriaRepository(PersistenceContext context) : ICategoriaRepository
{
    public async Task<IReadOnlyList<Categoria>> ListarActivasAsync(CancellationToken ct) =>
        await context.Categorias.AsNoTracking()
            .Where(c => c.Activo)
            .OrderBy(c => c.Nombre)
            .ToListAsync(ct);

    public Task<bool> ExisteActivaAsync(int id, CancellationToken ct) =>
        context.Categorias.AnyAsync(c => c.Id == id && c.Activo, ct);
}