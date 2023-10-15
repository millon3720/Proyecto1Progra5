using Microsoft.EntityFrameworkCore;
using ProyectoProgra5.Models;
using System.Collections.Generic;

namespace ProyectoProgra5.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Productos> Productos { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Tienda> Tienda { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<TiendaProductos> TiendaProductos { get; set; }

        //se crean las relaciones a la hora de ejecutar el modelado de la Bd
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuarios>()
            .HasOne(u => u.Rol)
            .WithMany(r => r.Usuarios)
            .HasForeignKey(u => u.IdRol);

            modelBuilder.Entity<TiendaProductos>()
            .HasOne(tp => tp.IdProductos)
            .WithMany(p => p.ProductosTienda)
            .HasForeignKey(tp => tp.IdProducto);

            modelBuilder.Entity<TiendaProductos>()
            .HasOne(tp => tp.IdTiendas)
            .WithMany(t => t.TiendaProductos)
            .HasForeignKey(tp => tp.IdTienda);
        }
    }
}
