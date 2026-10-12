using GestorInventario.Application.Exceptions;
using GestorInventario.Application.Interfaces;
using GestorInventario.Domain.Enums;
using MediatR;

namespace GestorInventario.Application.Productos.Commands;

public record AjustarStockCommand(int Id, TipoMovimiento Tipo, int Cantidad) : IRequest<ProductoDto>;

public class AjustarStockCommandHandler(IProductoRepository productos)
    : IRequestHandler<AjustarStockCommand, ProductoDto>
{
    public async Task<ProductoDto> Handle(AjustarStockCommand request, CancellationToken ct)
    {
        var producto = await productos.ObtenerPorIdAsync(request.Id, ct)
            ?? throw new NotFoundException($"No existe el producto con id {request.Id}.");

        producto.AjustarStock(request.Tipo, request.Cantidad);
        await productos.GuardarCambiosAsync(ct);

        return producto.ToDto();
    }
}