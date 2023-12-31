﻿using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ProductosApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductosApiController(AppDbContext context)
        {
            _context = context;
        }

        [Route("ProductosPrecio")]
        [HttpGet]
        public ActionResult<IEnumerable<Productos>> ObtenerTodosLosProductos()
        {
            var todosLosProductos = _context.TiendaProductos
            .Include(tp => tp.Productos)
            .Include(tp => tp.Tiendas)
            .Select(tp => new
            {
                Id = tp.Productos.Id,
                Nombre = tp.Productos.Nombre,
                Descripcion = tp.Productos.Descripcion,
                Imagen = tp.Productos.Imagen,
                Precio = tp.Precio,
                Cantidad = tp.Cantidad
            })
            .ToList();


            return Ok(todosLosProductos);
        }
        // GET: api/ProductosApi
        [Route("ListaProductos")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Productos>>> GetProductos()
        {
          if (_context.Productos == null)
          {
              return NotFound();
          }
            return await _context.Productos.ToListAsync();
        }

        // GET: api/ProductosApi/5
        [Route("BuscarProducto")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Productos>> GetProductos(int id)
        {
          if (_context.Productos == null)
          {
              return NotFound();
          }
            var productos = await _context.Productos.FindAsync(id);

            if (productos == null)
            {
                return NotFound();
            }

            return productos;
        }

        // PUT: api/ProductosApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("ActualizarProducto")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductos(int id, Productos productos)
        {
            if (id != productos.Id)
            {
                return BadRequest();
            }

            _context.Entry(productos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductosExists(id))
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

        // POST: api/ProductosApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("GuardarProducto")]
        [HttpPost]
        public async Task<ActionResult<Productos>> PostProductos(Productos productos)
        {
          if (_context.Productos == null)
          {
              return Problem("Entity set 'AppDbContext.Productos'  is null.");
          }
            _context.Productos.Add(productos);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductos", new { id = productos.Id }, productos);
        }

        // DELETE: api/ProductosApi/5
        [Route("BorrarProducto")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductos(int id)
        {
            if (_context.Productos == null)
            {
                return NotFound();
            }
            var productos = await _context.Productos.FindAsync(id);
            if (productos == null)
            {
                return NotFound();
            }

            _context.Productos.Remove(productos);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductosExists(int id)
        {
            return (_context.Productos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
