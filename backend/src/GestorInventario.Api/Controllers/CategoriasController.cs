using GestorInventario.Application.Categorias;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestorInventario.Api.Controllers;

[ApiController]
[Route("api/categorias")]
public class CategoriasController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CategoriaDto>>> Listar(CancellationToken ct)
        => Ok(await mediator.Send(new ObtenerCategoriasQuery(), ct));
}