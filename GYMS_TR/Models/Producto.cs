using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYMS_TR.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="El nombre del producto es requerido")]
        public  string NombreProducto { get; set; }

        [Required(ErrorMessage ="Descriocion cotar es requerida")]
        public string DescripcionCorta  { get; set; }

        [Required(ErrorMessage ="Descripcion del producto es requrido")]
        public string DescripcionPruducto { get; set; }

        [Required(ErrorMessage ="El precio es requerido")]
        [Range(1,double.MaxValue, ErrorMessage ="El precio de ve de ser mayor a cero")]
        public decimal Precio { get; set; }

        public string ImageneUrl { get; set; }

        //Ahora vamos agregar las relaciones de la tabla//

        public int CategoriaId { get; set; } 

        [ForeignKey("CategoriaId")]
        public virtual Categoria Categoria { get; set; }// esta es relacion de producto a categoria//

        public int TipoAplicacionId { get; set; }

        [ForeignKey("TipoAplicacionId")]
        public virtual TipoAplicacion TipoAplicacion { get; set; }// esta es relacion de producto a tipoAplicacion//
    }
}
