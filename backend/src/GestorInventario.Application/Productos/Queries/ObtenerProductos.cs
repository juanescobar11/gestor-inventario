using GestorInventario.Application.Interfaces;
using MediatR;

namespace GestorInventario.Application.Productos.Queries;

public record ObtenerProductosQuery(int? CategoriaId, bool? StockBajo)
    : IRequest<IReadOnlyList<ProductoDto>>;

public class ObtenerProductosQueryHandler(IProductoRepository productos)
    : IRequestHandler<ObtenerProductosQuery, IReadOnlyList<ProductoDto>>
{
    public async Task<IReadOnlyList<ProductoDto>> Handle(ObtenerProductosQuery request, CancellationToken ct)
    {
        var lista = await productos.ListarAsync(request.CategoriaId, request.StockBajo, ct);
        return lista.Select(p => p.ToDto()).ToList();
    }
}