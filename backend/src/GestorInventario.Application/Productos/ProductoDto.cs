using GestorInventario.Domain.Entities;

namespace GestorInventario.Application.Productos;

public record ProductoDto(
    int Id,
    string Nombre,
    string? Descripcion,
    decimal Precio,
    int Stock,
    int StockMinimo,
    int CategoriaId,
    string CategoriaNombre,
    bool StockBajo,
    DateTime FechaCreacion);

public static class ProductoMapper
{
    public static ProductoDto ToDto(this Producto p) => new(
        p.Id, p.Nombre, p.Descripcion, p.Precio, p.Stock, p.StockMinimo,
        p.CategoriaId, p.Categoria?.Nombre ?? string.Empty,
        p.EstaEnStockBajo, p.FechaCreacion);
}