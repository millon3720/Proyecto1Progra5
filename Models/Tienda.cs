using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoProgra5.Models
{
    public class Tienda
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Id")]
        [Required]
        public int Id { get; set; }
        [Required]

        public string Ubicacion { get; set; }
        [Required]

        public string Nombre { get; set; }
        [Required]
        public string Imagen { get; set; }

        [InverseProperty("Tiendas")]
        public List<TiendaProductos> TiendaProductos { get; set; }
    }
}
