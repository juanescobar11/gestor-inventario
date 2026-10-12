using GestorInventario.Application.Exceptions;
using GestorInventario.Application.Interfaces;
using MediatR;

namespace GestorInventario.Application.Productos.Commands;

public record EliminarProductoCommand(int Id) : IRequest;

public class EliminarProductoCommandHandler(IProductoRepository productos)
    : IRequestHandler<EliminarProductoCommand>
{
    public async Task Handle(EliminarProductoCommand request, CancellationToken ct)
    {
        var producto = await productos.ObtenerPorIdAsync(request.Id, ct)
            ?? throw new NotFoundException($"No existe el producto con id {request.Id}.");

        await productos.EliminarAsync(producto, ct);
    }
}