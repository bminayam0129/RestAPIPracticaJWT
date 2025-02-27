using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RestAPIPractica.RestAPI.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Correo { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
        public DateTime FechaDeNacimiento { get; set; }
    }
}
