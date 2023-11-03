using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoProgra5.Data;
using ProyectoProgra5.Models;
using System.Net;
using Newtonsoft.Json;


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
            var response = Request.Form["g-recaptcha-response"];
            var secretKey = "6LdNn-4oAAAAANK0M29WuWIcF0I70d5rUzlvWSPU"; // Reemplaza con tu clave secreta reCAPTCHA
            var client = new WebClient();

            // Realiza la verificación del CAPTCHA con Google
            var result = client.DownloadString($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={response}");
            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(result);

            if (captchaResponse.Success)
            {
                List<Usuarios> Login = _db.Usuarios.Where(tp => tp.Correo == Usuario && tp.Contraseña == Clave).Include(v => v.Rol).ToList();

                if (Login != null)
                {
                    foreach (var item in Login)
                    {
                        HttpContext.Session.SetString("Login", "True");
                        HttpContext.Session.SetInt32("IdUsuario", item.Id);
                        HttpContext.Session.SetString("Usuario", item.Nombre);
                        HttpContext.Session.SetString("Correo", item.Correo);
                        HttpContext.Session.SetString("Rol", item.Rol.Nombre);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            else
            {
                ModelState.AddModelError("Captcha", "Por favor, completa el CAPTCHA correctamente.");
                return View("Login");
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
            var response = Request.Form["g-recaptcha-response"];
            var secretKey = "6LdNn-4oAAAAANK0M29WuWIcF0I70d5rUzlvWSPU"; // Reemplaza con tu clave secreta reCAPTCHA
            var client = new WebClient();

            // Realiza la verificación del CAPTCHA con Google
            var result = client.DownloadString($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={response}");
            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(result);

            if (captchaResponse.Success)
            {
                // El CAPTCHA es válido, continúa con el registro del usuario
                // Buscar el rol con Id igual a 2 en la base de datos
                Rol rol = _db.Roles.FirstOrDefault(r => r.Id == 2);
                u.Rol = rol;
                _db.Usuarios.Add(u);
                _db.SaveChanges();
                TempData["Exito"] = "Usuario creado ok";
                return RedirectToAction("Login");
            }
            else
            {
                ModelState.AddModelError("Captcha", "Por favor, completa el CAPTCHA correctamente.");
                return View(u);
            }
          
        }


    }
}
public class CaptchaResponse
{
    public bool Success { get; set; }
}
