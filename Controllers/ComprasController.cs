using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoProgra5.Data;
using ProyectoProgra5.Models;

namespace ProyectoGrupo5.Controllers
{
    public class ComprasController : Controller
    {
        private AppDbContext ConexionBd;
        public ComprasController(AppDbContext db) {
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

            var listaProductos = ConexionBd.Productos.ToList();
            return View(listaProductos);
        }
       
    }
}
