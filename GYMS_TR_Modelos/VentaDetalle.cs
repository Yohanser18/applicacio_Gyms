using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMS_TR_Modelos
{
    public class VentaDetalle
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int VentaId { get; set; }

        [ForeignKey("VentaId")]
        public Venta Venta { get; set; }
        [Required]
        public int ProductoId { get; set; }
        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; }
        public int MetroCuadrado { get; set; }
        public decimal PrecioPorMetroCuadrado { get; set; }
    }
}
