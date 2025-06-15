using System.ComponentModel.DataAnnotations;

namespace GYMS_TR.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Este campo es obliatorio")]
        public  string NombreCategoria { get; set; }

        [Required(ErrorMessage ="Esta campo es obligatoro")]
        [Range(1,int.MaxValue, ErrorMessage ="El Orden debe ser mayor a cero")]
        public int MostrasOrden {  get; set; }


    }
}
