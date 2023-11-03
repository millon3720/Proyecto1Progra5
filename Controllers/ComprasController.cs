using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoGrupo5.Models;
using ProyectoProgra5.Data;
using ProyectoProgra5.Models;
using System.Data;
using ProyectoGrupo5.Service;



namespace ProyectoGrupo5.Controllers
{
    public class ComprasController : Controller
    {
        private readonly EmailServices EmailService;
        private AppDbContext ConexionBd;
        //private readonly IPaypalServices _paypalServices;
        
        public ComprasController(AppDbContext db, EmailServices _EmailService/*IPaypalServices paypalServices*/)
        {
            ConexionBd = db;
            EmailService = _EmailService; 
            //_paypalServices = paypalServices;
        }
        public IActionResult MostrarTiendas()
        {
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

        public IActionResult EnviarCorreo()
        {
            int IdUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            List<Ventas> carritoDeVentas = ConexionBd.Ventas.Where(tp => tp.Usuarios.Id == IdUsuario && tp.Pendiente == true).Include(v => v.Productos).ToList();

            CrearPdf factura = new CrearPdf();
            var NombreFactura=factura.CrearFactura(HttpContext.Session.GetString("Usuario"), carritoDeVentas);
            EmailService.sendEmail(HttpContext.Session.GetString("Correo"), NombreFactura);
            return RedirectToAction("MostrarTiendas");

        }

        //public async Task<IActionResult> CreateOrder()
        //{
        //    var request = new OrdersCreateRequest();
        //    request.Prefer("return=representation");
        //    request.RequestBody(BuildRequestBody());

        //    var response = await new PayPalHttpClient(_paypalOptions.Value).Execute(request);
        //    var order = response.Result<PayPalCheckoutSdk.Orders.Order>();

        //    return Redirect(order.Links[1].Href); // Redirige al usuario a la página de PayPal para completar el pago
        //}

        //private OrderRequest BuildRequestBody()
        //{
        //    // Crea y devuelve un objeto OrderRequest con los detalles del pedido
        //    // Consulta la documentación de PayPal para obtener detalles específicos
        //    return new OrderRequest();
        //}
    }
}
