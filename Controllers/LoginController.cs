using Microsoft.AspNetCore.Mvc;
using ProyectoProgra5.Controllers;
using ProyectoProgra5.Data;
using ProyectoProgra5.Models;
using System.Diagnostics;

namespace ProyectoGrupo5.Controllers
{
    public class LoginController : Controller
    {
        private AppDbContext _db;

        public LoginController(AppDbContext db)
        {
            _db=db;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registrarse(Usuarios u)
        {

            u.IdRol=2;

            if (!ModelState.IsValid) { return View(u); }

            _db.Usuarios.Add(u);
            _db.SaveChanges();

            TempData["Exito"] = "Usuario creado ok";
            return RedirectToAction("Login");
        }
    }
}
