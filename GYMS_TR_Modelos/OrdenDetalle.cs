using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMS_TR_Modelos
{
    public class OrdenDetalle
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int OrdenId { get; set; }
        [ForeignKey("OrdenId")]
        public Orden Orden { get; set; }
        [Required]
        public int ProductoId { get; set; }
        [ForeignKey("ProductoId")]
        public Producto  Producto { get; set; }
    }
}
