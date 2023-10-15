using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoProgra5.Models
{
    public class Rol
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("IdRol")]
        public int IdRol { get; set; }

        [Required]
        public string Nombre { get; set; }

        //Columnas para relacionar a la hora de crear el modelo de la Bd
        public ICollection<Usuarios> Usuarios { get; set; }
    }
}
