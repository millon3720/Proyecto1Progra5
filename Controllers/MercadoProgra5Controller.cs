using Microsoft.AspNetCore.Mvc;
using ProyectoGrupo5.Models;
using ProyectoProgra5.Data;

namespace ProyectoGrupo5.Controllers
{
    public class MercadoProgra5Controller : Controller
    {
        private AppDbContext _db;
        public MercadoProgra5Controller(AppDbContext db)
        {
            _db = db;
        }
        //public IActionResult Index()
        //{
        //    IEnumerable<MercadoProgra5> cvs = _db.TiendaProductos;
        //    return View(cvs);
        //}
    }
}
