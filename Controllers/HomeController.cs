using Microsoft.AspNetCore.Mvc;
using ProyectoProgra5.Models;
using System.Diagnostics;

namespace ProyectoProgra5.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        #region Login
        public IActionResult Login()
        {
            return View();
        }
        #endregion Login

        #region Indice-Privacy
        public IActionResult Index()
        {
            return View(); 
        }

      
        #endregion Indice-Privacy

       

        #region Manejo de errores
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion Manejo de errores
    }
}