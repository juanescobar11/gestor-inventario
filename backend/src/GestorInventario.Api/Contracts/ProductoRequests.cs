using GestorInventario.Domain.Enums;

namespace GestorInventario.Api.Contracts;

public record GuardarProductoRequest(
    string Nombre, string? Descripcion, decimal Precio,
    int Stock, int StockMinimo, int CategoriaId);

public record AjustarStockRequest(TipoMovimiento Tipo, int Cantidad);