using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoProgra5.Data;
using ProyectoProgra5.Models;

namespace ProyectoGrupo5.Controllers
{
    public class TiendaProductosController : Controller
    {
        private readonly AppDbContext _context;

        public TiendaProductosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: TiendaProductos
        public async Task<IActionResult> Index()
        {
              return _context.TiendaProductos != null ? 
                          View(await _context.TiendaProductos.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.TiendaProductos'  is null.");
        }

        // GET: TiendaProductos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TiendaProductos == null)
            {
                return NotFound();
            }

            var tiendaProductos = await _context.TiendaProductos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tiendaProductos == null)
            {
                return NotFound();
            }

            return View(tiendaProductos);
        }

        // GET: TiendaProductos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TiendaProductos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Cantidad,Precio")] TiendaProductos tiendaProductos)
        {
            try
            {
                _context.Add(tiendaProductos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                return View(tiendaProductos);
            }

   
           
        }

        // GET: TiendaProductos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TiendaProductos == null)
            {
                return NotFound();
            }

            var tiendaProductos = await _context.TiendaProductos.FindAsync(id);
            
            
            if (tiendaProductos == null)
            {
                return NotFound();
            }
            return View(tiendaProductos);
        }

        // POST: TiendaProductos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cantidad,Precio")] TiendaProductos tiendaProductos)
        {
            if (id != tiendaProductos.Id)
            {
                return NotFound();
            }

            
                try
                {
                    _context.Update(tiendaProductos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TiendaProductosExists(tiendaProductos.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            
        }

        // GET: TiendaProductos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TiendaProductos == null)
            {
                return NotFound();
            }

            var tiendaProductos = await _context.TiendaProductos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tiendaProductos == null)
            {
                return NotFound();
            }

            return View(tiendaProductos);
        }

        // POST: TiendaProductos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TiendaProductos == null)
            {
                return Problem("Entity set 'AppDbContext.TiendaProductos'  is null.");
            }
            var tiendaProductos = await _context.TiendaProductos.FindAsync(id);
            if (tiendaProductos != null)
            {
                _context.TiendaProductos.Remove(tiendaProductos);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TiendaProductosExists(int id)
        {
          return (_context.TiendaProductos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
