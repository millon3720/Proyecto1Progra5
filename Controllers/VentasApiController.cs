using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoGrupo5.Models;
using ProyectoProgra5.Data;
using ProyectoProgra5.Models;

namespace ProyectoGrupo5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VentasApiController(AppDbContext context)
        {
            _context = context;
        }
        [Route("CarritoCompras")]
        [HttpGet]
        public ActionResult<IEnumerable<Ventas>> CarritoCompras(String idUsuario)
        {
            UsuariosApiController Usuario = new UsuariosApiController(_context);

            var carritoDeVentas = _context.Ventas
                .Where(v => v.Usuarios.Id == int.Parse(Usuario.Desencriptar(idUsuario)) && v.Pendiente == true)
                .Include(v => v.Productos)
                .ToList();

            return Ok(carritoDeVentas);
        }
        // GET: api/VentasApi
        [Route("ListaVentas")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ventas>>> GetVentas()
        {
          if (_context.Ventas == null)
          {
              return NotFound();
          }
            return await _context.Ventas.ToListAsync();
        }

        // GET: api/VentasApi/5
        [Route("BuscarVenta")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Ventas>> GetVentas(int id)
        {
          if (_context.Ventas == null)
          {
              return NotFound();
          }
            var ventas = await _context.Ventas.FindAsync(id);

            if (ventas == null)
            {
                return NotFound();
            }

            return ventas;
        }

        // PUT: api/VentasApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("ActualizarVenta")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVentas(int id, Ventas ventas)
        {
            if (id != ventas.Id)
            {
                return BadRequest();
            }

            _context.Entry(ventas).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VentasExists(id))
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

        // POST: api/VentasApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("GuardarVenta")]
        [HttpPost]
        public async Task<ActionResult<Ventas>> PostVentas(Ventas ventas)
        {
          if (_context.Ventas == null)
          {
              return Problem("Entity set 'AppDbContext.Ventas'  is null.");
          }
            _context.Ventas.Add(ventas);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVentas", new { id = ventas.Id }, ventas);
        }

        // DELETE: api/VentasApi/5
        [Route("BorrarVenta")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVentas(int id)
        {
            if (_context.Ventas == null)
            {
                return NotFound();
            }
            var ventas = await _context.Ventas.FindAsync(id);
            if (ventas == null)
            {
                return NotFound();
            }

            _context.Ventas.Remove(ventas);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VentasExists(int id)
        {
            return (_context.Ventas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
