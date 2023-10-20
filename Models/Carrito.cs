using ProyectoProgra5.Models;

namespace ProyectoGrupo5.Models
{
    public class Carrito
    {
        public List<Productos> Productos { get; } = new List<Productos>();

        public void AgregarProducto(Productos producto)
        {
            Productos.Add(producto);
        }

        public void EliminarProducto(int productoId)
        {
            var producto = Productos.FirstOrDefault(p => p.IdProducto == productoId);
            if (producto != null)
            {
                Productos.Remove(producto);
            }
        }
        public List<Productos> ObtenerProductos()
        {
            return Productos;
        }
    }
}
