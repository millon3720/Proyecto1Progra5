using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoProgra5.Models
{
    public class TiendaProductos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("IdTiendaProducto")]

        public int IdTiendaProducto { get; set; }

        [Required]
        public int IdProducto { get; set; }

        [Required]
        public int IdTienda { get; set; }

        public int Cantidad { get; set; }

        public decimal Precio { get; set; }
        //Columnas para relacionar a la hora de crear el modelo de la Bd
        public Tienda IdTiendas { get; set; }
        public Productos IdProductos { get; set; }
    }
}
