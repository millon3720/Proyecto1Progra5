using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoProgra5.Models
{
    public class Productos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("IdProducto")]

        public int IdProducto { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public string Imagen { get; set; }

        //Columnas para relacionar a la hora de crear el modelo de la Bd
        public ICollection<TiendaProductos> ProductosTienda { get; set; }

       
    }
}
