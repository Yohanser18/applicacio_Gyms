using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMS_TR_Modelos
{
    public class Orden  
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UsuarioapliacacioId { get; set; }
        [ForeignKey("UsuarioapliacacioId")]
        public UsuarioAplicacion UsuarioAplicacion { get; set; }
        public DateTime FechaOrden { get; set; }
        [Required]
        public string Telefono { get; set; }
        [Required]
        public string NombreCompleto { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
