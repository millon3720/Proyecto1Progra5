using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoProgra5.Models
{
    public class Tienda
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("IdTienda")]

        public int IdTienda { get; set; }

        public string Ubicacion { get; set; }
        public string Nombre { get; set; }
        //Columna para relacionar a la hora de crear el modelo de la Bd
        public ICollection<TiendaProductos> TiendaProductos { get; set; }
        //public Tienda (int idTienda, string ubicacion, string nombre, ICollection<TiendaProductos> tiendaProductos)
        //{
        //    IdTienda = idTienda;
        //    Ubicacion = ubicacion;
        //    Nombre = nombre;
        //    TiendaProductos = tiendaProductos;
        //}
    }
}
