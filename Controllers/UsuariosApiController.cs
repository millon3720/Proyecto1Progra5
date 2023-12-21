using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoProgra5.Data;
using ProyectoProgra5.Models;

namespace ProyectoGrupo5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosApiController(AppDbContext context)
        {
            _context = context;
        }
       // GET: api/UsuariosApi/Login
       [Route("Login")]
       [HttpGet]
        public async Task<ActionResult<bool>> Login(string Usuario, string Clave)
        {
            if (_context.Usuarios == null)
            {
                return NotFound();
            }

            var result = await _context.Usuarios
                .Where(tp => tp.Correo == Desencriptar(Usuario) && tp.Contraseña == Desencriptar(Clave))
                .Include(v => v.Rol)
                .ToListAsync();

            return result.Count > 0;
        }
        [Route("GuardarUsuario")]
        [HttpPost]
        public void Registrarse(String Nombre,String Cedula,String Correo,String Contraseña,String Telefono,String Edad)
        {
            Usuarios u = new Usuarios();
            u.Nombre=Desencriptar(Nombre);
            u.Cedula = Desencriptar(Cedula);
            u.Correo= Desencriptar(Correo);
            u.Contraseña = Desencriptar(Contraseña);
            u.Telefono = Desencriptar(Telefono);
            u.Edad=int.Parse(Desencriptar(Edad));
            var client = new WebClient();
            Rol rol = _context.Roles.FirstOrDefault(r => r.Id == 2);
            u.Rol = rol;
            _context.Usuarios.Add(u);
            _context.SaveChanges();

        }


        public string Desencriptar(string Dato)
        {
            string key = "Proyecto2Proyect";
            string iv = "Proyecto2Proyect";
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(Dato)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        // GET: api/UsuariosApi
        [Route("ListaUsuarios")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuarios>>> GetUsuarios()
        {
            if (_context.Usuarios == null)
            {
                return NotFound();
            }
            return await _context.Usuarios.ToListAsync();
        }

        // GET: api/UsuariosApi/5
        [Route("BuscarUsuario")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuarios>> GetUsuarios(int id)
        {
          if (_context.Usuarios == null)
          {
              return NotFound();
          }
            var usuarios = await _context.Usuarios.FindAsync(id);

            if (usuarios == null)
            {
                return NotFound();
            }

            return usuarios;
        }

        // PUT: api/UsuariosApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("ActualizarUsuario")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuarios(int id, Usuarios usuarios)
        {
            if (id != usuarios.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuarios).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuariosExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UsuariosApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("GuardarUsuario1")]
        [HttpPost]
        public async Task<ActionResult<Usuarios>> PostUsuarios(Usuarios usuarios)
        {
          if (_context.Usuarios == null)
          {
              return Problem("Entity set 'AppDbContext.Usuarios'  is null.");
          }
            _context.Usuarios.Add(usuarios);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuarios", new { id = usuarios.Id }, usuarios);
        }

        // DELETE: api/UsuariosApi/5
        [Route("BorrarUsuario")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuarios(int id)
        {
            if (_context.Usuarios == null)
            {
                return NotFound();
            }
            var usuarios = await _context.Usuarios.FindAsync(id);
            if (usuarios == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuarios);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuariosExists(int id)
        {
            return (_context.Usuarios?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
