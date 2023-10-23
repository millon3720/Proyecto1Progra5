using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ProyectoProgra5.Models;
using System.Collections.Generic;

namespace ProyectoGrupo5.Models
{
    public class Ventas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Id")]
        public int Id { get; set; }
        [Required]
        public int Cantidad { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
        [Required]
        public bool Pendiente { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Total { get; set; }

        // Relaciones
        [ForeignKey("Usuarios")]
        public int UsuariosId { get; set; }
        public Usuarios Usuarios { get; set; }

        [ForeignKey("Productos")]
        public int ProductosId { get; set; }
        public Productos Productos { get; set; }
    }
}
