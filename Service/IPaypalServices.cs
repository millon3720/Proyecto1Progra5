using PayPal.Api;

namespace ProyectoGrupo5.Service
{
    public interface IPaypalServices
    {
        Task<Payment> CreateOrderAsync(decimal amount, string returnUrl, string cancelUrl);
    }
}
