using GestorInventario.Domain.Entities;

namespace GestorInventario.Application.Interfaces;

public interface ICategoriaRepository
{
    Task<IReadOnlyList<Categoria>> ListarActivasAsync(CancellationToken ct);
    Task<bool> ExisteActivaAsync(int id, CancellationToken ct);
}