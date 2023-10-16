using Microsoft.AspNetCore.Mvc;
using ProyectoProgra5.Controllers;
using ProyectoProgra5.Models;
using System.Diagnostics;

namespace ProyectoGrupo5.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

    }
}
