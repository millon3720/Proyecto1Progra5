using Microsoft.EntityFrameworkCore;
using ProyectoGrupo5.Models;
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
        public DbSet<Ventas> Ventas { get; set; }
        public DbSet<TiendaProductos> TiendaProductos { get; set; }      
        
    }
}
