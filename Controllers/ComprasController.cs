using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoGrupo5.Models;
using ProyectoProgra5.Data;
using ProyectoProgra5.Models;
using System.Data;

namespace ProyectoGrupo5.Controllers
{
    public class ComprasController : Controller
    {
        private AppDbContext ConexionBd;
        public ComprasController(AppDbContext db)
        {
            ConexionBd = db;
        }
        public IActionResult MostrarTiendas()
        {
            IEnumerable<Tienda> ListaTiendas = ConexionBd.Tienda;

            var listaTiendas = ConexionBd.Tienda.ToList();
            return View(listaTiendas);
        }
        public IActionResult MostrarArticulos(int? Id)
        {

            IEnumerable<Productos> ListaProductos = ConexionBd.Productos;

            var productosDeTienda = ConexionBd.TiendaProductos
        .Where(tp => tp.Tiendas.Id == Id)
        .Include(tp => tp.Productos).Include(tp => tp.Tiendas)
        .ToList();
            return View(productosDeTienda);
        }
        public IActionResult AgregarAlCarrito(int IdProductos, int Cantidad,decimal Precio, int IdTienda)
        {

            int IdUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            var venta = new Ventas
            {
                Cantidad = Cantidad,
                Fecha = DateTime.Now,  
                Pendiente = true,      
                Total = Precio*Cantidad,
                Productos = ConexionBd.Productos.Find(IdProductos),      
                Usuarios = ConexionBd.Usuarios.Find(IdUsuario)
            };

            ConexionBd.Ventas.Add(venta);
            ConexionBd.SaveChanges();
            return RedirectToAction("MostrarArticulos", new { Id = IdTienda });

        }
        public IActionResult CarritoCompras()
        {
            int IdUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            List<Ventas> carritoDeVentas = ConexionBd.Ventas.Where(tp=>tp.Usuarios.Id== IdUsuario).Include(v => v.Productos).ToList();
            
            return View(carritoDeVentas);
        }
    }
}