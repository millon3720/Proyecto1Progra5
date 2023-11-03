using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PayPalHttp;
using ProyectoGrupo5.Service;

namespace ProyectoGrupo5.Controllers
{
    public class PayPalController : Controller
    {
        private readonly IOptions<PayPalSettings> _paypalSettings;

        public PayPalController(IOptions<PayPalSettings> paypalSettings)
        {
            _paypalSettings = paypalSettings;
        }

        public async Task<IActionResult> CreatePayment()
        {
            // Lógica para crear un pedido en PayPal

            var environment = new SandboxEnvironment(_paypalSettings.Value.ClientId, _paypalSettings.Value.ClientSecret);
            var client = new PayPalHttpClient(environment);

            var orderRequest = new OrdersCreateRequest();
            orderRequest.Prefer("return=representation");
            orderRequest.RequestBody(new OrderRequest()
            {
                Intent = "CAPTURE", // Establece el Intent en el OrderRequest
                PurchaseUnits = new List<PurchaseUnitRequest>()
                {
                    new PurchaseUnitRequest()
                    {
                        AmountWithBreakdown = new AmountWithBreakdown()
                        {
                            CurrencyCode = "USD",
                            Value = "100.00"
                        }
                    }
                }
            });

            try
            {
                var response = await client.Execute(orderRequest);
                var result = response.Result<Order>();
                var approvalUrl = result.Links.Find(link => link.Rel == "approve");

                return Redirect(approvalUrl.Href);
            }
            catch (Exception ex)
            {
                // Manejar errores aquí
                return View("Error");
            }
        }

        public async Task<IActionResult> ExecutePayment(string orderId, string token, string PayerID)
        {
            // Lógica para ejecutar el pago en PayPal después de la aprobación del usuario

            var environment = new SandboxEnvironment(_paypalSettings.Value.ClientId, _paypalSettings.Value.ClientSecret);
            var client = new PayPalHttpClient(environment);

            var orderExecuteRequest = new OrdersCaptureRequest(orderId);
            orderExecuteRequest.RequestBody(new OrderActionRequest());

            try
            {
                var response = await client.Execute(orderExecuteRequest);
                var result = response.Result<Order>();
                // Aquí puedes manejar la confirmación del pago
                return View("PaymentConfirmation", result);
            }
            catch (Exception ex)
            {
                // Manejar errores aquí
                return View("Error");
            }
        }
    }
}
