using GestorInventario.Application.Exceptions;
using GestorInventario.Application.Interfaces;
using MediatR;

namespace GestorInventario.Application.Productos.Queries;

public record ObtenerProductoPorIdQuery(int Id) : IRequest<ProductoDto>;

public class ObtenerProductoPorIdQueryHandler(IProductoRepository productos)
    : IRequestHandler<ObtenerProductoPorIdQuery, ProductoDto>
{
    public async Task<ProductoDto> Handle(ObtenerProductoPorIdQuery request, CancellationToken ct)
    {
        var producto = await productos.ObtenerPorIdAsync(request.Id, ct)
            ?? throw new NotFoundException($"No existe el producto con id {request.Id}.");

        return producto.ToDto();
    }
}