using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoGrupo5.Models;
using Stripe.Checkout;

namespace ProyectoGrupo5.Controllers
{
    public class CheckOutController : Controller
    {
        public ActionResult Index()
        {
            List<ProductEntity> ProductList = new List<ProductEntity>();

            ProductList=new List<ProductEntity>
            {
                new ProductEntity
                { 
                    Product="Prueba",
                    Rate=1500,
                    Quanity=2,
                    ImagePath="img/Salir.png"
                },
                new ProductEntity
                {
                    Product="Prueba 2",
                    Rate=1000,
                    Quanity=1,
                    ImagePath="img/LoginLogo.png"
                }
            };



            return View(ProductList);
        }
        public IActionResult OrderConfirmation()
        {
            var service =new SessionService();
            Session session = service.Get(TempData["Session"].ToString());
            if (session.PaymentStatus=="Paid")
            {
                return View("Succes");
            }
            return View("Login");

        }
        public ActionResult CheckOut()
        {
            List<ProductEntity> ProductList = new List<ProductEntity>();

            ProductList = new List<ProductEntity>
            {
                new ProductEntity
                {
                    Product="Prueba",
                    Rate=1500,
                    Quanity=2,
                    ImagePath="img/Salir.png"
                },
                new ProductEntity
                {
                    Product="Prueba 2",
                    Rate=1000,
                    Quanity=1,
                    ImagePath="img/LoginLogo.png"
                }
            };

            var domain = "http://localhost:7036/";

            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"CheckOut/OrderConfirmation",
                CancelUrl= domain + $"CheckOut/Login",
                LineItems=new List<SessionLineItemOptions>(),
                Mode="payment"
            };

            foreach (var item in ProductList)
            {
                var sessionListItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount=(long)(item.Rate*item.Quanity),
                        Currency="inr",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        { 
                            Name=item.Product.ToString(),

                        }
                    },
                    Quantity=item.Quanity
                };
                options.LineItems.Add(sessionListItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);
            TempData["Session"] = session.Id;
            Response.Headers.Add("Location",session.Url);

            return new StatusCodeResult(303);

        }
    }
}
