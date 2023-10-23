using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using ProyectoGrupo5.Models;
using System.Collections.Generic; 

namespace ProyectoProgra5.Models
{
    public class Usuarios
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Id")]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Contraseña { get; set; }
        public string Cedula { get; set; }
        [MaxLength(10)]
        public string Telefono { get; set; }
        public int Edad { get; set; }

        // Columnas para relacionar a la hora de crear el modelo de la Bd
        [ForeignKey("Rol")]
        public int RolId { get; set; }
        public Rol Rol { get; set; }

        [InverseProperty("Usuarios")]
        public List<Ventas> Ventas { get; set; }
    }
}

