using GestorInventario.Domain.Enums;
using GestorInventario.Domain.Exceptions;

namespace GestorInventario.Domain.Entities;

public class Producto
{
    public int Id { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public string? Descripcion { get; private set; }
    public decimal Precio { get; private set; }
    public int Stock { get; private set; }
    public int StockMinimo { get; private set; }
    public int CategoriaId { get; private set; }
    public Categoria? Categoria { get; private set; }
    public DateTime FechaCreacion { get; private set; }

    // Regla de negocio: stock bajo cuando Stock < StockMinimo
    public bool EstaEnStockBajo => Stock < StockMinimo;

    private Producto() { } // requerido por EF Core

    public static Producto Crear(string nombre, string? descripcion, decimal precio,
                                 int stock, int stockMinimo, int categoriaId)
    {
        var producto = new Producto { FechaCreacion = DateTime.Now };
        producto.Actualizar(nombre, descripcion, precio, stock, stockMinimo, categoriaId);
        return producto;
    }

    public void Actualizar(string nombre, string? descripcion, decimal precio,
                           int stock, int stockMinimo, int categoriaId)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new DomainException("El nombre es obligatorio.");
        if (nombre.Trim().Length > 150)
            throw new DomainException("El nombre no puede superar 150 caracteres.");
        if (descripcion is { Length: > 500 })
            throw new DomainException("La descripción no puede superar 500 caracteres.");
        if (precio <= 0)
            throw new DomainException("El precio debe ser mayor a 0.");
        if (precio > 99999999.99m)
            throw new DomainException("El precio supera el máximo permitido.");
        if (stock < 0)
            throw new DomainException("El stock no puede ser negativo.");
        if (stockMinimo < 0)
            throw new DomainException("El stock mínimo no puede ser negativo.");

        Nombre = nombre.Trim();
        Descripcion = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion.Trim();
        Precio = precio;
        Stock = stock;
        StockMinimo = stockMinimo;
        CategoriaId = categoriaId;
    }

    public void AjustarStock(TipoMovimiento tipo, int cantidad)
    {
        if (cantidad <= 0)
            throw new DomainException("La cantidad debe ser mayor a 0.");

        if (tipo == TipoMovimiento.Salida)
        {
            if (cantidad > Stock)
                throw new DomainException($"Stock insuficiente: hay {Stock} y se intentan sacar {cantidad}.");
            Stock -= cantidad;
        }
        else
        {
            Stock += cantidad;
        }
    }
}