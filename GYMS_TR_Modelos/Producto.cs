using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYMS_TR_Modelos
{
    public class Producto
    {
        //Esto es para que siempre el campo de metrocuadrado aparesca en 1 //
        public Producto()
        {
            TempMetrocuadrado = 1;
        }

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

        public string? ImageneUrl { get; set; }

        //Ahora vamos agregar las relaciones de la tabla//

        public int CategoriaId { get; set; } 

        [ForeignKey("CategoriaId")]
        public virtual Categoria? Categoria { get; set; }// esta es relacion de producto a categoria//

        public int TipoAplicacionId { get; set; }

        [ForeignKey("TipoAplicacionId")]
        public virtual TipoAplicacion? TipoAplicacion { get; set; }// esta es relacion de producto a tipoAplicacion//

        // este campo no se mapea a la base de datos//
        [NotMapped]
        [Range(1, 1000)]
        public int TempMetrocuadrado { get; set; }
    }
}
