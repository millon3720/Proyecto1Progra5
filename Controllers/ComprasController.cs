using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoGrupo5.Models;
using ProyectoProgra5.Data;
using ProyectoProgra5.Models;
using System.Data;
using ProyectoGrupo5.Service;
using Stripe.Checkout;
using Microsoft.CodeAnalysis.CSharp;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Stripe;
using System.Globalization;

namespace ProyectoGrupo5.Controllers
{
    public class ComprasController : Controller
    {
        private readonly EmailServices EmailService;
        private AppDbContext ConexionBd;
        
        public ComprasController(AppDbContext db, EmailServices _EmailService)
        {
            ConexionBd = db;
            EmailService = _EmailService; 
        }
        public IActionResult MostrarTiendas(string Pago)
        {
            if (Pago== "Realizado")
            {
                PagoRealizado();
            }
            IEnumerable<Tienda> ListaTiendas = ConexionBd.Tienda;

            var listaTiendas = ConexionBd.Tienda.ToList();
            return View(listaTiendas);
        }
        public IActionResult MostrarArticulos(int? Id)
        {

            IEnumerable<Productos> ListaProductos = ConexionBd.Productos;

            var productosDeTienda = ConexionBd.TiendaProductos
        .Where(tp => tp.Tiendas.Id == Id)
        .Include(tp => tp.Productos).Include(tp => tp.Tiendas)
        .ToList();
            return View(productosDeTienda);
        }
        public IActionResult AgregarAlCarrito(int IdProductos, int Cantidad, decimal Precio, int IdTienda)
        {

            int IdUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            var venta = new Ventas
            {
                Cantidad = Cantidad,
                Fecha = DateTime.Now,
                Pendiente = true,
                Total = Precio * Cantidad,
                Productos = ConexionBd.Productos.Find(IdProductos),
                Usuarios = ConexionBd.Usuarios.Find(IdUsuario)
            };

            ConexionBd.Ventas.Add(venta);
            ConexionBd.SaveChanges();
            return RedirectToAction("MostrarArticulos", new { Id = IdTienda });

        }
        public IActionResult CarritoCompras()
        {
            int IdUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            List<Ventas> carritoDeVentas = ConexionBd.Ventas.Where(tp => tp.Usuarios.Id == IdUsuario && tp.Pendiente == true).Include(v => v.Productos).ToList();

            return View(carritoDeVentas);
        }
        public IActionResult BorrarCarrito(int Id)
        {
            var Eliminar = ConexionBd.Ventas.FirstOrDefault(v => v.Id == Id);

            ConexionBd.Ventas.Remove(Eliminar);
            ConexionBd.SaveChanges();
            return RedirectToAction("CarritoCompras");
        }
        public void EnviarCorreo()
        {
            int IdUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            List<Ventas> carritoDeVentas = ConexionBd.Ventas.Where(tp => tp.Usuarios.Id == IdUsuario && tp.Pendiente == true).Include(v => v.Productos).ToList();

            CrearPdf factura = new CrearPdf();
            var NombreFactura=factura.CrearFactura(HttpContext.Session.GetString("Usuario"), carritoDeVentas);
            EmailService.sendEmail(HttpContext.Session.GetString("Correo"), NombreFactura);
        }
        public ActionResult RealizarPago()
        {
            int IdUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            List<Ventas> carritoDeVentas = ConexionBd.Ventas.Where(tp => tp.Usuarios.Id == IdUsuario && tp.Pendiente == true).Include(v => v.Productos).ToList();
            if (carritoDeVentas.Count > 0)
            {
                List<ProductEntity> ProductList = new List<ProductEntity>();

                foreach (var item in carritoDeVentas)
                {
                    ProductEntity product = new ProductEntity
                    {
                        Producto = item.Productos.Nombre,
                        Precio = (item.Total / item.Cantidad),
                        Cantidad = item.Cantidad,
                        Total = item.Total
                    };

                    ProductList.Add(product);
                }

                var options = new SessionCreateOptions
                {
                    SuccessUrl = Url.Action("MostrarTiendas", "Compras", new { Pago = "Realizado" }, "https"),
                    CancelUrl = Url.Action("CarritoCompras", "Compras", null, "https"),
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment"
                };

                foreach (var item in ProductList)
                {
                    var sessionListItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Precio * 100),
                            Currency = "crc",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Producto.ToString(),

                            }
                        },
                        Quantity = (long)item.Cantidad
                    };
                    options.LineItems.Add(sessionListItem);
                }

                var service = new SessionService();
                Session session = service.Create(options);
                TempData["Session"] = session.Id;
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }
            return RedirectToAction("MostrarTiendas");

        }
        public void PagoRealizado()
        {
            int IdUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            EnviarCorreo();
            var ventasPorActualizar = ConexionBd.Ventas.Where(v => v.UsuariosId == IdUsuario && v.Pendiente == true).ToList();

            foreach (var venta in ventasPorActualizar)
            {
                TiendaProductos tiendaProducto = ConexionBd.TiendaProductos.FirstOrDefault(tp => tp.Productos.Id == venta.Productos.Id);

                if (tiendaProducto != null)
                {
                    if (tiendaProducto.Cantidad - venta.Cantidad < 0)
                    {
                        tiendaProducto.Cantidad = 0;
                    }
                    else
                    {
                        tiendaProducto.Cantidad = tiendaProducto.Cantidad - venta.Cantidad;
                    }
                    ConexionBd.SaveChanges();
                }
                venta.Pendiente = false;

            }
            ConexionBd.SaveChanges();
        }
    }
}
