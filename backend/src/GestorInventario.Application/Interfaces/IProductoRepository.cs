using GestorInventario.Domain.Entities;

namespace GestorInventario.Application.Interfaces;

public interface IProductoRepository
{
    Task<Producto?> ObtenerPorIdAsync(int id, CancellationToken ct);
    Task<IReadOnlyList<Producto>> ListarAsync(int? categoriaId, bool? stockBajo, CancellationToken ct);
    Task AgregarAsync(Producto producto, CancellationToken ct);
    Task GuardarCambiosAsync(CancellationToken ct);
    Task EliminarAsync(Producto producto, CancellationToken ct);
}