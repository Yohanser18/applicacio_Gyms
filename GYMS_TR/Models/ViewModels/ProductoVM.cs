using Microsoft.AspNetCore.Mvc.Rendering;

namespace GYMS_TR.Models.ViewModels
{
    public class ProductoVM  // Aqui este viewmodels lo que estams haciendo es empaquetar de lo de producto 
    {                         // para no utilizar la entidad completa de producto, y tambie traemos sus relaciones 
                              // de categoria y tipoaplicacion las en paquetamos es este viewmodels, esto vendria 
                              //haciendo como los Dto .
        public Producto Producto { get; set; }
        
        public IEnumerable<SelectListItem> CategoriaLista { get; set; }

        public IEnumerable<SelectListItem> TipoAplicacionLista { get; set; }
    }
}
