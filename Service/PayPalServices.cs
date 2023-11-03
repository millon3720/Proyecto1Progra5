using PayPal.Api;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProyectoGrupo5.Service;

namespace TuProyecto.Services
{
    public class PaypalServices : IPaypalServices
    {
        private readonly IConfiguration _configuration;

        public PaypalServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Payment> CreateOrderAsync(decimal amount, string returnUrl, string cancelUrl)
        {
            var clientId = _configuration["Paypal:ClientId"];
            var clientSecret = _configuration["Paypal:ClientSecret"];

            var config = new Dictionary<string, string>
    {
        { "mode", "sandbox" },
        { "clientId", clientId },
        { "clientSecret", clientSecret }
    };

            var accessToken = new OAuthTokenCredential(clientId, clientSecret, config).GetAccessToken();
            var apiContext = new APIContext(accessToken);

            var itemList = new ItemList
            {
                items = new List<Item>
        {
            new Item
            {
                name = "Product Name",
                currency = "USD",
                price = amount.ToString("0.00"),
                quantity = "1"
            }
        }
            };

            var transaction = new Transaction
            {
                amount = new Amount
                {
                    currency = "USD",
                    total = amount.ToString("0.00")
                },
                item_list = itemList,
                description = "Purchase Description"
            };

            var payment = new Payment
            {
                intent = "sale",
                payer = new Payer { payment_method = "paypal" },
                redirect_urls = new RedirectUrls
                {
                    return_url = returnUrl,
                    cancel_url = cancelUrl
                },
                transactions = new List<Transaction> { transaction }
            };

            return await Task.Run(() => payment.Create(apiContext));
        }

    }
}
