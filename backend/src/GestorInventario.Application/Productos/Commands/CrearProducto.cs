using GestorInventario.Application.Interfaces;
using GestorInventario.Domain.Entities;
using GestorInventario.Domain.Exceptions;
using MediatR;

namespace GestorInventario.Application.Productos.Commands;

public record CrearProductoCommand(
    string Nombre, string? Descripcion, decimal Precio,
    int Stock, int StockMinimo, int CategoriaId) : IRequest<ProductoDto>;

public class CrearProductoCommandHandler(
    IProductoRepository productos,
    ICategoriaRepository categorias) : IRequestHandler<CrearProductoCommand, ProductoDto>
{
    public async Task<ProductoDto> Handle(CrearProductoCommand request, CancellationToken ct)
    {
        if (!await categorias.ExisteActivaAsync(request.CategoriaId, ct))
            throw new DomainException($"La categoría {request.CategoriaId} no existe o está inactiva.");

        var producto = Producto.Crear(request.Nombre, request.Descripcion, request.Precio,
                                      request.Stock, request.StockMinimo, request.CategoriaId);

        await productos.AgregarAsync(producto, ct);

        var creado = await productos.ObtenerPorIdAsync(producto.Id, ct);
        return creado!.ToDto();
    }
}