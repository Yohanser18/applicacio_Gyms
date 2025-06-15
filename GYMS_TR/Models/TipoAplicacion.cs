using System.ComponentModel.DataAnnotations;

namespace GYMS_TR.Models
{
    public class TipoAplicacion
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Esta campo es obligatorio")]
        public string Nombre { get; set; }
    }
}
