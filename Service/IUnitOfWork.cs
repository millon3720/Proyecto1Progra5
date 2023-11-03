namespace ProyectoGrupo5.Service
{
    public interface IUnitOfWork
    {
        IPaypalServices PaypalServices { get; }

    }
}
