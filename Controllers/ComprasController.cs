﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoProgra5.Data;
using ProyectoProgra5.Models;

namespace ProyectoGrupo5.Controllers
{
    public class ComprasController : Controller
    {
        private AppDbContext ConexionBd;
        public ComprasController(AppDbContext db) {
            ConexionBd = db;
        }  
        public IActionResult MostrarArticulos() 
        {
            IEnumerable<Productos> ListaProductos = ConexionBd.Productos;

            var listaProductos = ConexionBd.Productos.ToList();
            return View(listaProductos);
        }
        
    }
}