namespace ProyectoGrupo5.Service
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IConfiguration Configuration;
        public UnitOfWork(IConfiguration config) {
            Configuration=config;
            PaypalServices = new PayPalServices(Configuration);
        }

        public IPaypalServices PaypalServices { get; private set; }
    }
}
