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
        private readonly Carrito _carrito;

        public ComprasController(AppDbContext db, Carrito carrito)
        {
            ConexionBd = db;
            _carrito = carrito;
        }
        public IActionResult MostrarTiendas()
        {
            IEnumerable<Tienda> ListaTiendas = ConexionBd.Tienda;

            var listaTiendas = ConexionBd.Tienda.ToList();
            return View(listaTiendas);
        }
        public IActionResult MostrarArticulos(int? Id)
        {

            //var productosDeTienda = ConexionBd.TiendaProductos
            //.Where(tp => tp.IdTienda == Id)
            //.Join(
            //    ConexionBd.Productos,
            //    tp => tp.IdProductos,
            //    p => p.IdProducto,
            //    (tp, p) => new { TiendaProducto = tp, Producto = p }
            //)
            //.Select(joined => joined.Producto)
            //.ToList();

            //return productosDeTienda;

            IEnumerable<Productos> ListaProductos = ConexionBd.Productos;

            var listaProductos = ConexionBd.Productos.ToList();
            return View(listaProductos);
        }
        public IActionResult AgregarAlCarrito(int? Id)
        {
            var Articulo = ConexionBd.Productos.Find(Id);
            _carrito.AgregarProducto(Articulo);
            return RedirectToAction("MostrarArticulos");
        }
        public IActionResult CarritoCompras()
        {

            return View(_carrito.ObtenerProductos());
        }
    }
}