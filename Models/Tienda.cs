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
        [Required]
        public int IdTienda { get; set; }
        [Required]

        public string Ubicacion { get; set; }
        [Required]

        public string Nombre { get; set; }
        [Required]
        public string Imagen { get; set; }
        //Columna para relacionar a la hora de crear el modelo de la Bd
        public ICollection<TiendaProductos> TiendaProductos { get; set; }
      
    }
}
