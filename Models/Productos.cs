using ProyectoGrupo5.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace ProyectoProgra5.Models
{
    public class Productos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Id")]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public string Imagen { get; set; }

        [InverseProperty("Productos")]
        public List<TiendaProductos> TiendaProductos { get; set; }

        [InverseProperty("Productos")]
        public List<Ventas> Ventas { get; set; }
    }
}
