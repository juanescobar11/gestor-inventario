using GestorInventario.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestorInventario.Infrastructure.Persistence;

public class PersistenceContext(DbContextOptions<PersistenceContext> options) : DbContext(options)
{
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Producto> Productos => Set<Producto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(e =>
        {
            e.ToTable("Categorias");
            e.HasKey(c => c.Id);
            e.Property(c => c.Nombre).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<Producto>(e =>
        {
            e.ToTable("Productos");
            e.HasKey(p => p.Id);
            e.Property(p => p.Nombre).IsRequired().HasMaxLength(150);
            e.Property(p => p.Descripcion).HasMaxLength(500);
            e.Property(p => p.Precio).HasPrecision(10, 2);
            e.HasOne(p => p.Categoria)
             .WithMany()
             .HasForeignKey(p => p.CategoriaId)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }
}