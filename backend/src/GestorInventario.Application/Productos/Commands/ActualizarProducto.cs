using GestorInventario.Application.Exceptions;
using GestorInventario.Application.Interfaces;
using GestorInventario.Domain.Exceptions;
using MediatR;

namespace GestorInventario.Application.Productos.Commands;

public record ActualizarProductoCommand(
    int Id, string Nombre, string? Descripcion, decimal Precio,
    int Stock, int StockMinimo, int CategoriaId) : IRequest<ProductoDto>;

public class ActualizarProductoCommandHandler(
    IProductoRepository productos,
    ICategoriaRepository categorias) : IRequestHandler<ActualizarProductoCommand, ProductoDto>
{
    public async Task<ProductoDto> Handle(ActualizarProductoCommand request, CancellationToken ct)
    {
        var producto = await productos.ObtenerPorIdAsync(request.Id, ct)
            ?? throw new NotFoundException($"No existe el producto con id {request.Id}.");

        if (!await categorias.ExisteActivaAsync(request.CategoriaId, ct))
            throw new DomainException($"La categoría {request.CategoriaId} no existe o está inactiva.");

        producto.Actualizar(request.Nombre, request.Descripcion, request.Precio,
                            request.Stock, request.StockMinimo, request.CategoriaId);

        await productos.GuardarCambiosAsync(ct);

        var actualizado = await productos.ObtenerPorIdAsync(producto.Id, ct);
        return actualizado!.ToDto();
    }
}