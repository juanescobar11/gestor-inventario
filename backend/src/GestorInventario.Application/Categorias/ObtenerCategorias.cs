using GestorInventario.Application.Interfaces;
using MediatR;

namespace GestorInventario.Application.Categorias;

public record CategoriaDto(int Id, string Nombre);

public record ObtenerCategoriasQuery : IRequest<IReadOnlyList<CategoriaDto>>;

public class ObtenerCategoriasQueryHandler(ICategoriaRepository categorias)
    : IRequestHandler<ObtenerCategoriasQuery, IReadOnlyList<CategoriaDto>>
{
    public async Task<IReadOnlyList<CategoriaDto>> Handle(ObtenerCategoriasQuery request, CancellationToken ct)
    {
        var lista = await categorias.ListarActivasAsync(ct);
        return lista.Select(c => new CategoriaDto(c.Id, c.Nombre)).ToList();
    }
}