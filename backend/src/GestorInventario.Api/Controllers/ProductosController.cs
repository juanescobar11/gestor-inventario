using GestorInventario.Api.Contracts;
using GestorInventario.Application.Productos;
using GestorInventario.Application.Productos.Commands;
using GestorInventario.Application.Productos.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestorInventario.Api.Controllers;

[ApiController]
[Route("api/productos")]
public class ProductosController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductoDto>>> Listar(
        [FromQuery] int? categoriaId, [FromQuery] bool? stockBajo, CancellationToken ct)
        => Ok(await mediator.Send(new ObtenerProductosQuery(categoriaId, stockBajo), ct));

    [HttpGet("stock-bajo")]
    public async Task<ActionResult<IReadOnlyList<ProductoDto>>> ListarStockBajo(CancellationToken ct)
        => Ok(await mediator.Send(new ObtenerProductosQuery(null, true), ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductoDto>> ObtenerPorId(int id, CancellationToken ct)
        => Ok(await mediator.Send(new ObtenerProductoPorIdQuery(id), ct));

    [HttpPost]
    public async Task<ActionResult<ProductoDto>> Crear(GuardarProductoRequest request, CancellationToken ct)
    {
        var creado = await mediator.Send(new CrearProductoCommand(
            request.Nombre, request.Descripcion, request.Precio,
            request.Stock, request.StockMinimo, request.CategoriaId), ct);

        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductoDto>> Editar(int id, GuardarProductoRequest request, CancellationToken ct)
        => Ok(await mediator.Send(new ActualizarProductoCommand(
            id, request.Nombre, request.Descripcion, request.Precio,
            request.Stock, request.StockMinimo, request.CategoriaId), ct));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id, CancellationToken ct)
    {
        await mediator.Send(new EliminarProductoCommand(id), ct);
        return NoContent();
    }

    [HttpPatch("{id:int}/stock")]
    public async Task<ActionResult<ProductoDto>> AjustarStock(int id, AjustarStockRequest request, CancellationToken ct)
        => Ok(await mediator.Send(new AjustarStockCommand(id, request.Tipo, request.Cantidad), ct));
}