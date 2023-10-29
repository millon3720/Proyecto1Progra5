using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoGrupo5.Models;
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
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            HttpContext.Session.Clear();
            return View();
        }
        public IActionResult RevisarLogin(string Usuario,string Clave)
        {
            List<Usuarios> Login = _db.Usuarios.Where(tp => tp.Correo == Usuario && tp.Contraseña == Clave).Include(v => v.Rol).ToList();
            
            if (Login != null)
            {
                foreach (var item in Login)
                {
                    HttpContext.Session.SetString("Login", "True");
                    HttpContext.Session.SetInt32("IdUsuario",item.Id);
                    HttpContext.Session.SetString("Usuario", item.Nombre);
                    HttpContext.Session.SetString("Correo", item.Correo);
                    HttpContext.Session.SetString("Rol", item.Rol.Nombre);
                }


                return RedirectToAction("Index","Home");
            }
            else {
                return RedirectToAction("Login");
            }
        }

        public IActionResult Registrarse()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registrarse(Usuarios u)
        {
            // Buscar el rol con Id igual a 2 en la base de datos
            Rol rol = _db.Roles.FirstOrDefault(r => r.Id == 2);
            u.Rol = rol;
            _db.Usuarios.Add(u);
            _db.SaveChanges();
            TempData["Exito"] = "Usuario creado ok";
            return RedirectToAction("Login");

        }

    }
}
